use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'Exists'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = 'A'

declare @TargetId int = -999146
declare @Expected bit = 1 

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
