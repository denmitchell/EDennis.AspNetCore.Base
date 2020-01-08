Add-Type -path "C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.Smo\14.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Smo.dll"
## 
## NOTE: If necessary, adjust the above to the path to where Microsoft.SqlServer.Smo is installed in the GAC
##

##
## NOTE: You can save this file to the location returned by 
## PM> $profile
## 
## This allows you to execute the functions from
##    the Package Manager Console in Visual Studio.
##

# #######################################################
#
# Author: Dennis Mitchell
# Last Modified: 2020-01-08
# Functionality: Executes all test .sql files in a solution.
#                The .sql files must have a lower-case
#                .sql extension and must have the word
#                SaveTestJson in the .sql file.
#
# Example Calls:
#
# PM> Invoke-TestSql -solution 'EDennis.AspNetCore.RepoTests' -database Color2Db -showSuccesses $false
# PM> Invoke-TestSql EDennis.AspNetCore.RepoTests Color2Db $false
# PM> Invoke-TestSql EDennis.AspNetCore.RepoTests Color2Db $true
# PM> Invoke-TestSql EDennis.AspNetCore.RepoTests Color2Db
#
# #######################################################
function Invoke-TestSql($solution,$database,$showSuccesses){    
 
    #declare a dictionary for holding the SQL strings
    $dict = new-object "System.Collections.Generic.Dictionary``2[System.String,System.String]";
    $cntPrev = 0;    
    $cxn = new-object system.data.sqlclient.sqlconnection("Server=(LocalDB)\MSSQLLocalDB;Database=$database;Trusted_Connection=True;MultipleActiveResultSets=True;");     
    $cxn.Open();     
 
    #purge the TestJson table
    $cmdTruncate = $cxn.CreateCommand();     
    $cmdTruncate.CommandText = "exec _.TruncateTestJson";     
    $cmdTruncate.ExecuteNonQuery() | Out-Null;    
 
    #define SQL query for counting all records in TestJson
    $sqlCount =
@'
    select sum(cnt)
        from (
            select count(*) cnt
                from _.TestJson
            union select count(*) cnt
                from _.TestJsonHistory
         ) a;            
'@   
 
    #define SQL query for getting the most recently added key
    $sqlKey =
@'
    select top 1 ProjectName
            + '|' + ClassName + '|' + MethodName
            + '|' + TestScenario + '|' + TestCase
            + '|' + TestFile KeyVal
        from _.TestJson
        order by SysStart desc;                
'@   

    # get SMO Server object
    $smoSvrConnection = new-object Microsoft.SqlServer.Management.Common.ServerConnection $cxn;
    $smoSvr = new-object Microsoft.SqlServer.Management.Smo.Server $smoSvrConnection;
    $db = $smoSvr.Databases[$database]

    
    $solution = $solution + ".sln"
    $solutionPath = "C:\Users\$ENV:UserName\source\repos\*$solution"

    Write-Host "Executing Test SQL for $solutionPath"    

    $sourcePath = (Get-ChildItem -Path "$solutionPath" -Recurse -Force | 
        Select-Object -First 1).Directory.FullName


    $fileCount

    # get all .sql files in projects having the word "SaveTestJson" in them
    Get-ChildItem -Path "$sourcePath\*.sql" -Recurse |
        Where-Object {
            $_.FullName -notmatch "\\bin\\" `
                -and !$_.PSISContainer
        } |
        Select-String "SaveTestJson" |
        Group Path |
        Select Name |
        ForEach-Object {         
            $file = $_.Name;         
            $abbrvFile = $file.Replace("$sourcePath","..");
            $fileCount = $fileCount + 1;
            try {      
 
                #execute the SQL file                  
                $sqlFile = [IO.File]::ReadAllText($file) ;


                $cmdFile = new-object system.data.sqlclient.sqlcommand($sqlFile, $cxn);            
                $cmdFile.ExecuteNonQuery() | Out-Null;            

                
                #get the total number of records in TestJson and history table
                $cmdCount = new-object system.data.sqlclient.sqlcommand($sqlCount, $cxn);            
                $cnt = [int]$cmdCount.ExecuteScalar();            
 
                #if the count increased, then ...
                if($cnt -gt $cntPrev){                
 
                    #get the key for the last inserted record            
                    $cmdKey = new-object system.data.sqlclient.sqlcommand($sqlKey, $cxn);                
                    $keyVal = [string]$cmdKey.ExecuteScalar();
            
                    #if the key already exists, append the additional
                    #  file name to the existing file name
                    if ($dict.ContainsKey($keyVal)){                    
                        $dict[$keyVal] = $dict[$keyVal] + "`n`t" + $($file);                
                    #otherwise, just add the new file name
                    } else {                    
                        $dict.Add($keyVal,$file);                 
                    }                
                    if($showSuccesses){                    
                        Write-Host "    Successfully executed:  $($abbrvFile)";
                    }           
                }           
                $cntPrev = $cnt;         
            } catch {            
                try {
                   $db.ExecuteNonQuery($sqlFile); #use SMO
                    if($showSuccesses){                    
                        Write-Host "    Successfully executed:  $($abbrvFile)";
                    }           
                } catch {
                #write the error message to the console
                   Write-Host "****ERROR: COULD NOT EXECUTE:  $($abbrvFile): $($_.Exception.Message)";         
                }
            }     
            #Write-Progress -Activity "Executing" -Status "SQL Files Executed : $fileCount"
        } | Out-Null;
 
    #write all duplicate test parameters to the console             
    foreach($key in $dict.Keys){        
        if(([string]$dict[$key]).Contains("`n`t")) {            
            Write-Host "Duplicate Test Params: $([string]$key) : `n`t$([string]$dict[$key])";          
        }     
    }             
}

# #######################################################
#
# Author: Dennis Mitchell
# Last Modified: 2020-01-08
# Functionality: Lists all test .sql files in a solution.
#                The .sql files must have a lower-case
#                .sql extension and must have the word
#                SaveTestJson in the .sql file.
#
# Example Calls:
#
# PM> Get-TestSql -solution 'EDennis.AspNetCore.RepoTests'
# PM> Get-TestSql EDennis.AspNetCore.RepoTests
#
# #######################################################
function Get-TestSql($solution){    
 
    $solution = $solution + ".sln"
    $solutionPath = "C:\Users\$ENV:UserName\source\repos\*$solution"

    Write-Host "Listing Test SQL for $solutionPath"    

    $sourcePath = (Get-ChildItem -Path "$solutionPath" -Recurse -Force | 
        Select-Object -First 1).Directory.FullName

    $j = 0
    # get all .sql files in projects having the word "SaveTestJson" in them
    Get-ChildItem -Path "$sourcePath\*.sql" -Recurse |
        Where-Object {
            $_.FullName -notmatch "\\bin\\" `
                -and !$_.PSISContainer
        } |
        Select-String "SaveTestJson" |
        Group Path |
        Select Name |
        ForEach-Object {         
            $file = $_.Name.Replace("$sourcePath", "..") ;         
            Write-Host $file
        } | Out-Null;
 
}

#Get-TestSql -solution 'EDennis.AspNetCore.RepoTests'
#Invoke-TestSql -solution 'EDennis.AspNetCore.RepoTests' -database Color2Db -showSuccesses $false
