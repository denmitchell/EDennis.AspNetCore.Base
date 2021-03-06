﻿use Colors2;
declare @ProjectName varchar(255) = 'EDennis.Samples.Colors2Api'
declare @ClassName varchar(255) = 'HslController'
declare @MethodName varchar(255) = 'GetDynamicLinqAsync'
declare @TestScenario varchar(255) = 'WhereOrderBySelectTake'
declare @TestCase varchar(255) = 'NameContainsBlueSelectNameDescSysUserTake10'

declare @Where varchar(255) = 'Name.Contains("Blue")'
declare @Select varchar(255) = 'new(Name,SysUser)'
declare @OrderBy varchar(255) = 'Name desc'
declare @Skip int = 0
declare @Take int = 10

declare 
	@Expected varchar(max) = 
(
	select Name, SysUser from vwHsl
	where Name like '%Blue%'
	order by Name desc
	offset @Skip rows fetch next @Take row only
	for json path, include_null_values
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Where', @Where
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'OrderBy', @OrderBy
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase