Add-Type -path "C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.Smo\14.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Smo.dll"

# #######################################################
#
# Author: Dennis Mitchell
# Last Modified: 2018-12-24
# Functionality: Executes all .SQL files in test projects
#
# #######################################################
function ExecuteTestSql($solution,$database,$showSuccesses){    
 
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

    #$smoSvrConnection.Connect();

    Write-Host $smoSvrConnection.BatchSeparator
    
    Set-Location -Path c:\Users\$ENV:UserName/source/repos/$solution

    # get all .sql files in projects having the word "Test" in them
    Get-ChildItem -Path *.sql -Recurse -File |
        Where-Object {
            $_.PSParentPath -like "*Test*" -and $_.FullName `
                -notmatch "\\bin\\" -and !$_.PSISContainer
        } |
        Select FullName |
        ForEach-Object {         
            $file = $_.FullName;         
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
                        Write-Host "    Successfully executed:  $($file)";
                    }           
                }           
                $cntPrev = $cnt;         
            } catch {            
                try {
                   $db.ExecuteNonQuery($sqlFile); #use SMO
                    if($showSuccesses){                    
                        Write-Host "    Successfully executed:  $($file)";
                    }           
                } catch {
                #write the error message to the console
                   Write-Host "****ERROR: COULD NOT EXECUTE:  $($file): $($_.Exception.Message)";         
                }
            }     
        } | Out-Null;
 
    #write all duplicate test parameters to the console             
    foreach($key in $dict.Keys){        
        if(([string]$dict[$key]).Contains("`n`t")) {            
            Write-Host "Duplicate Test Params: $([string]$key) : `n`t$([string]$dict[$key])";          
        }     
    }             
}

#ExecuteTestSql -solution 'EDennis.AspNetCore.Base' -database Colors2 -showSuccesses true