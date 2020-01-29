use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'Exists'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = 'B'

declare @TargetId int = -111111111
declare @Expected bit = 0 

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase