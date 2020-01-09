use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'GetFromSql'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = 'A'
declare @Sql varchar(255) = 'select Id, Name, Red, Green, Blue from Rgb where Blue > ((Red + Green)*2)'
declare @jsonSql nvarchar(max) = 'select @j = (' + @Sql + ' for json path )';

DECLARE @Expected varchar(max)   
DECLARE @ParmDefinition nvarchar(500);

SET @ParmDefinition = N'@j varchar(max) OUTPUT';

EXEC sp_executesql @jsonSql, @ParmDefinition, @j=@Expected OUTPUT;

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Sql', @Sql
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
