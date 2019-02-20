# CreateClones.ps1

# #######################################################
#
# Author: Dennis Mitchell
# Last Modified: 2019-02-12
# Functionality: Generates clones of database for use by 
#                ParallelDatabaseServer instance of LocalDb
#                for testing purposes.
# #######################################################


class DbFileInfo{
    [string]$Database
    [string]$Name
    [string]$Path
    [int]$Type
    [string]$Directory
    [string]$Extension

    [string] GetOnFilesString(){
        $attachPath = $this.Path;
        return "(FILENAME = '$attachPath')";
    }
    [string] GetOnFilesString([int]$cloneIndex, [string]$folder){
        $attachPath = $folder + "\" + $this.Name + $cloneIndex.ToString() + $this.Extension;
        return "(FILENAME = '$attachPath')";
    }
}

class CloneGenerator{

    [string]$DestinationServer = "ParallelDatabaseServer"
    [string]$DestinationFolder = "$env:USERPROFILE\AppData\Local\ParallelDatabaseServer"
    [string]$SourceConnectionString = "Server=(LocalDB)\MSSQLLocalDB;Database=master;Trusted_Connection=True;MultipleActiveResultSets=True;"
    [string]$DestinationConnectionString = "Server=(LocalDB)\ParallelDatabaseServer;Database=master;Trusted_Connection=True;MultipleActiveResultSets=True;"

    [string]$SqlUpdateCloneStage =     
@"
declare @CloneStage int = convert(int,session_context(N'CloneStage'));
set @CloneStage = isnull(@CloneStage,0) + 1;
exec sp_set_session_context 'CloneStage', @CloneStage;
select convert(int,session_context(N'CloneStage')) CloneStage; 
"@



    [string]$Database
    [int]$Count
    [system.data.sqlclient.sqlconnection]$SourceConnection
    [system.data.sqlclient.sqlconnection]$DestinationConnection
    [string]$MssqlBackup
    [string]$BackupForParallel

    CloneGenerator(){}

    CloneGenerator([string]$database,[int]$count){
        $this.Count = $count;
        $this.Database = $database;
        $this.SourceConnection = new-object system.data.sqlclient.sqlconnection($this.SourceConnectionString); 
        $this.DestinationConnection = new-object system.data.sqlclient.sqlconnection($this.DestinationConnectionString); 
        $this.MssqlBackup = "$($this.DestinationFolder)\$database-mssql.bak";
        $this.BackupForParallel = "$($this.DestinationFolder)\$database-parallel.bak";
    }

    [void] ForceDeleteFile([string]$filePath){
        if([System.IO.File]::Exists($filePath)){
            $allProcesses = Get-Process
            foreach ($process in $allProcesses) { 
                $process.Modules | where {$_.FileName -eq $filePath} `
                | Stop-Process -Force -ErrorAction SilentlyContinue
            }
            Remove-Item $filePath
        }
    }


    [void] CloseSqlConnection([system.data.sqlclient.sqlconnection]$connection){
        if ($connection.State -eq [System.Data.ConnectionState]::Open){
           $connection.Close();
        }
    }


    [void] ExecuteSqlCommand([system.data.sqlclient.sqlconnection]$connection, [string]$cmdText){
        if ($connection.State -eq [System.Data.ConnectionState]::Closed){
            $connection.Open();
        }
        $cmd = $connection.CreateCommand();      
        $cmd.CommandText = $cmdText;      
        $cmd.ExecuteNonQuery() | Out-Null;     
    }

    [int] GetIntScalar([system.data.sqlclient.sqlconnection]$connection, [string]$cmdText){
        if ($connection.State -eq [System.Data.ConnectionState]::Closed){
            $connection.Open();
        }
        $cmd = $connection.CreateCommand();      
        $cmd.CommandText = $cmdText;      
        $result = $cmd.ExecuteScalar();     
        if(([DBNull]::Value).Equals($result)){
            return [int]::MinValue;
        }
        return $result;
    }

    [string] GetStringScalar([system.data.sqlclient.sqlconnection]$connection, [string]$cmdText){
        if ($connection.State -eq [System.Data.ConnectionState]::Closed){
            $connection.Open();
        }
        $cmd = $connection.CreateCommand();      
        $cmd.CommandText = $cmdText;      
        $result = $cmd.ExecuteScalar();     
        return $result.ToString();
    }


    [void] WaitForSourceStage([int]$stage){
        $sql = "select session_context(N'CloneStage')"
        $existingStage = -1;
        while($true){
            $existingStage = $this.GetIntScalar($this.SourceConnection,$sql);
            if ($existingStage -eq $stage){
                break;
            } else {
                Write-Host "Waiting for source stage $stage";
                Start-Sleep -Milliseconds 500
            }
        }
    }


    [void] WaitForDestinationStage([int]$stage){
        $sql = "select session_context(N'CloneStage')"
        $existingStage = -1;
        while($true){
            $existingStage = $this.GetIntScalar($this.DestinationConnection,$sql);
            if ($existingStage -eq $stage){
                break;
            } else {
                Write-Host "Waiting for destination stage $stage";
                Start-Sleep -Milliseconds 500
            }
        }
    }


    [void] WaitForSourceDatabase(){
        $sql = "select count from sys.databases where name='$($this.Database)'"
        $existingCount = 0;
        while($true){
            $existingCount = $this.GetIntScalar($this.SourceConnection,$sql);
            if ($existingCount -eq 1){
                break;
            } else {
                Write-Host "Waiting for source database $($this.Database)";
                Start-Sleep -Milliseconds 500
            }
        }
    }


    [void] WaitForSourceTable([string]$table){
        $sql = "select count from [$($this.Database)].sys.tables where name='$($table)'"
        $existingCount = 0;
        while($true){
            $existingCount = $this.GetIntScalar($this.SourceConnection,$sql);
            if ($existingCount -eq 1){
                break;
            } else {
                Write-Host "Waiting for source table $table";
                Start-Sleep -Milliseconds 500
            }
        }
    }

    [void] WaitForPath([string]$path){
        while(-Not(Test-Path ($path))){    
            Start-Sleep -Milliseconds 500;   
        } 
    }

    [void] WaitForNotPath([string]$path){
        while(Test-Path ($path)){    
            Start-Sleep -Milliseconds 500;   
        } 
    }


    [void] CreateDbBackup([string]$destination){
$sql=@"
backup database [$($this.Database)] to disk = '$destination' with copy_only;
$($this.SqlUpdateCloneStage)
"@;

        $this.ExecuteSqlCommand($this.SourceConnection,$sql);
    }


    [void] RestoreDbBackup([string]$source){
$sql=@"
restore database [$($this.Database)] from disk = '$source'
$($this.SqlUpdateCloneStage)
"@;
        $this.ExecuteSqlCommand($this.SourceConnection,$sql);
    }


    [DbFileInfo] GetDbFileInfo(){
        $sql= "select name,physical_name from [$($this.Database)].sys.database_files where type = 0;"

        if ($this.SourceConnection.State -eq [System.Data.ConnectionState]::Closed){
            $this.SourceConnection.Open();
        }

        $adapter = new-object system.data.sqlclient.sqldataadapter ($sql, $this.SourceConnection)
        $table = new-object system.data.datatable
        $adapter.Fill($table)
    
        $row = $table.Rows[0]; #get the first row -- should just be one row.
            
        $info = [DbFileInfo]::new();
        $info.Database = $($this.Database);
        $info.Name = $row['name'];
        $info.Path = $row['physical_name'];
        $info.Type = 0;#[int]::Parse($row['type']);
        $info.Extension = [System.IO.Path]::GetExtension($info.Path);           
        $info.Directory = [System.IO.Path]::GetDirectoryName($info.Path);           

        return $info;
    }



    [void] DetachDatabase(){

$sql=@"
alter database [$($this.Database)] set single_user with rollback immediate; 
exec sp_detach_db '$($this.Database)', 'true';
$($this.SqlUpdateCloneStage)
"@;

        $this.ExecuteSqlCommand($this.SourceConnection,$sql);
    }



    [void] CloneMdf([DbFileInfo]$info){
        $destPath = "$($this.DestinationFolder)\$($this.Database)" + $info.Extension;
        $this.ForceDeleteFile("$destPath");
        Copy-Item "$($info.Path)" "$destPath"; 
        for($i=0;$i -lt $($this.Count); $i++){
            $newDb = $info.Database + $i.ToString();
            $destPath = "$($this.DestinationFolder)\$newDb" + $info.Extension;
            $this.ForceDeleteFile("$destPath");
            $this.ForceDeleteFile("$($destPath.Replace('.mdf','_log.ldf'))");
            Copy-Item "$($info.Path)" "$destPath"; 
        }
    }


    [void] ReattachMdf([DbFileInfo]$info){
            $newDb = $($info.Database)
$sql = @"
if DB_ID('$($info.Database)') is not null
begin
    alter database [$($info.Database)] set single_user with rollback immediate;
    drop database [$($info.Database)];
end
create database [$($info.Database)] on $($info.GetOnFilesString()) for attach_rebuild_log;
$($this.SqlUpdateCloneStage)
"@;      
            Write-Host "Reattaching $($info.Database)";
            $this.ExecuteSqlCommand($this.SourceConnection,$sql);
                
    }



    [void] AttachCloneMdfs([DbFileInfo]$info){
$sql = "";
        for($i=0; $i -lt $($this.Count); $i++){
            $newDb = $($info.Database + $i.ToString())
$sql = $sql + @"
if DB_ID('$newDb') is not null
begin
    alter database [$newDb] set single_user with rollback immediate;
    drop database [$newDb];
end
create database [$newDb] on $($info.GetOnFilesString($i,$this.DestinationFolder)) for attach_rebuild_log;
"@;      
        }
$sql = $sql + $($this.SqlUpdateCloneStage);

        $this.ExecuteSqlCommand($this.DestinationConnection,$sql);
    }


    [void] PurgeExistingClones(){

$sql = @"
declare @dbPrefix varchar(255) = '$($this.Database)'
declare @name varchar(255)
declare @sql nvarchar(500)
declare crsrDb cursor for
	select d.name
		from sys.databases d
		where d.database_id not in 
			(select dbid from sys.sysprocesses)
			and d.name like @dbPrefix + '%'
open crsrDb
fetch next from crsrDb into @name
while @@fetch_status = 0
begin
  if @name is not null
  begin
    set @sql = 'use master; alter database [' + @name + '] set single_user with rollback immediate; drop database [' + @name + ']'     
	exec sp_executesql @sql
  end
  fetch next from crsrDb into @name
end
close crsrDb
deallocate crsrDb;
$($this.SqlUpdateCloneStage)
"@;
        $this.ExecuteSqlCommand($this.DestinationConnection,$sql);
    }




    [void] RemoveMaintenanceObjectsAndShrink(){
$sql=
@"

-- get all _maintenance objects

use [$($this.Database)];
declare @name varchar(200)
declare @type varchar(20)
declare @temporal_type tinyint
declare @sql nvarchar(500)

declare crsr cursor for
   select name, 'procedure' type, 0 temporal_type from sys.objects where type = 'P' and schema_id in
      (select schema_id from sys.schemas where name = '_maintenance')
   union
   select name, 'table' type, temporal_type from sys.tables where type = 'U' and schema_id in 
      (select schema_id from sys.schemas where name = '_maintenance')
   order by temporal_type desc,name 


-- drop all _maintenance objects

open crsr
fetch next from crsr into @name, @type, @temporal_type
while @@fetch_status = 0
  begin
    if @name is not null
      begin
        set @sql =  'DROP ' + @type +  ' _maintenance.[' + @name +  ']'

        if @temporal_type = 2 set @sql = 'ALTER TABLE _maintenance.[' + @name +  '] SET (system_versioning = off); ' + @sql
          execute sp_executesql @sql
      end   
    fetch next from crsr into @name, @type, @temporal_type
  end
close crsr
deallocate crsr

-- shrink the files

use master;

declare @file varchar(255);
declare @db varchar(255) = '$($this.Database)'
declare @sqlFile nvarchar(500)
declare crsrFile cursor for
  select name from [$($this.Database)].sys.sysfiles;

open crsrFile
fetch next from crsrFile into @file
  while @@fetch_status = 0
    begin
      if @file != null
        begin
          set @sqlFile = N'use [$($this.Database)]; checkpoint; dbcc shrinkfile (' + @file + ', 1)'
          exec sp_executesql @sql
          set @sqlFile = N'use master; alter database [$($this.Database)] modify file (name=' + @file + ',filegrowth=1MB);'
          exec sp_executesql @sql
        end
      fetch next from crsrFile into @file 
    end
close crsrFile
deallocate crsrFile
$($this.SqlUpdateCloneStage)
"@;
        $this.ExecuteSqlCommand($this.SourceConnection,$sql);
    }


} 




# -----------------------------------------------
# ENTRY POINT for CloneGenerator
# -----------------------------------------------

function CreateClones([string]$database,[int]$cloneCount){     


    $cg = [CloneGenerator]::new($database,$cloneCount);



    # 1. Create destination folder, if it doesn't exist
    if ( -Not(Test-Path -Path ($cg.DestinationFolder)) ){
        New-Item -ItemType directory -Path ($cg.DestinationFolder)
    }


    # 2. Ensure that the parallel database server has been created
    $cmd = "sqllocaldb.exe create $($cg.DestinationServer)"
    Invoke-Expression -Command:$cmd | Out-Null


    # 3. Ensure that the parallel database server is running
    $cmd = "sqllocaldb.exe start $($cg.DestinationServer)"
    Invoke-Expression -Command:$cmd | Out-Null

    Write-Host "$($cg.DestinationServer) ready"


    # 4. Purge existing clones
    Write-Host "Purging existing clones of $database"
    $cg.PurgeExistingClones();
    $cg.WaitForDestinationStage(1);


    # 5. Get file info
    Write-Host "Getting file info for $($cg.Database)"
    $info = $cg.GetDbFileInfo()


    # 6. Detach unaltered database
    Write-Host "Detaching unaltered $($cg.Database)"
    $cg.DetachDatabase()
    $cg.WaitForSourceStage(1);


    # 7. Create copy of unaltered database
    Write-Host "Creating a copy of unaltered $database"
    Copy-Item "$($info.Path)" "$($info.Path.Replace('.mdf','-unaltered.mdf'))"; 
    $cg.WaitForPath($info.Path.Replace('.mdf','-unaltered.mdf'));


    # 8. Reattach unaltered database
    Write-Host "Reattaching $database"
    $cg.ReattachMdf($info);
    $cg.WaitForSourceStage(2);


    # 9. Remove maintenance objects and shrink files
    Write-Host "Removing maintenance objects and shrinking $database"
    $cg.RemoveMaintenanceObjectsAndShrink();    
    $cg.WaitForSourceStage(3);


    # 10. Detach minimized database
    Write-Host "Detaching minimized $($cg.Database)"
    $cg.DetachDatabase()
    $cg.WaitForSourceStage(4);


    # 11. Clone minimized database
    Write-Host "Cloning minimized $database"
    $cg.CloneMdf($info);
    $cg.WaitForPath("$($cg.DestinationFolder)\$($database)$($cloneCount - 1).mdf");


    # 12. Replace minimized database mdf with unaltered database mdf
    Write-Host "Replacing minimized database MDF with unaltered database MDF"
    $cg.ForceDeleteFile($info.Path);
    $cg.WaitForNotPath($info.Path);
    Copy-Item "$($info.Path.Replace('.mdf','-unaltered.mdf'))" "$($info.Path)"; 
    $cg.WaitForPath($info.Path.Replace('.mdf','-unaltered.mdf'));

 
    # 13. Reattach unaltered database
    Write-Host "Reattaching unaltered $database"
    $cg.ReattachMdf($info);
    $cg.WaitForSourceStage(5);


    # 14. Attach clones
    Write-Host "Attaching clones of $database"
    $cg.AttachCloneMdfs($info);
    $cg.WaitForDestinationStage(2);


    # 15. Close connections
    Write-Host "Closing connections"
    $cg.CloseSqlConnection($cg.DestinationConnection);
    $cg.CloseSqlConnection($cg.SourceConnection);


}

#sample call
CreateClones StateBackgroundCheck 5
