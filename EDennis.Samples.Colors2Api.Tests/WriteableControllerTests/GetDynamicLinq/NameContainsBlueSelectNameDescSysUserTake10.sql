use Colors2;
declare @ProjectName varchar(255) = 'EDennis.Samples.Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'GetDynamicLinq'
declare @TestScenario varchar(255) = 'WhereSelectOrderByTake'
declare @TestCase varchar(255) = 'NameContainsBlueSelectNameDescSysUserTake10'

declare @Where varchar(255) = '["Name","Contains","Blue"]'
declare @Select varchar(255) = '["Name","SysUser"]'
declare @OrderBy varchar(255) = '["Name desc"]'
declare @Take int = 10

declare 
	@Expected varchar(max) = 
(
	select Name from Rgb
	where Name like '%Blue%'
	order by Name desc
	for json path, include_null_values
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Where', @Where
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'OrderBy', @OrderBy
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase