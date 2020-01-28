use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'GetWithDevExtreme'
declare @TestScenario varchar(255) = 'FilterSortSelectTake'
declare @TestCase varchar(255) = 'A'

declare @ControllerPath varchar(255) = 'api/Rgb'
declare @Filter varchar(255) = '["Name","Contains","Blue"]'
declare @Select varchar(255) = '["Name","SysUser"]'
declare @Sort varchar(255) = '[{selector:"Name",desc:true}]'
declare @Skip int = 0
declare @Take int = 10

declare @ExpectedStatusCode int = 200 --Success

declare 
	@Expected varchar(max) = 
(
	select Name, SysUser from Rgb
	where Name like '%Blue%'
	order by Name desc
	offset @Skip rows fetch next @Take row only
	for json path, include_null_values
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Filter', @Filter
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Sort', @Sort
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ExpectedStatusCode', @ExpectedStatusCode
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase