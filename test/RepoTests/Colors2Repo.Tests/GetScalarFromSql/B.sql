use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'GetScalarFromSql'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = 'B'
declare @Sql varchar(255) = 'select count(*) from Rgb where Green > ((Red + Blue)*2)'
declare @scalarSql nvarchar(max) = 'select @scalar = (' + @Sql + ')';

DECLARE @Expected int   
DECLARE @ParmDefinition nvarchar(500);

SET @ParmDefinition = N'@scalar int OUTPUT';

EXEC sp_executesql @scalarSql, @ParmDefinition, @scalar=@Expected OUTPUT;

--select @Expected Expected;

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Sql', @Sql
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
