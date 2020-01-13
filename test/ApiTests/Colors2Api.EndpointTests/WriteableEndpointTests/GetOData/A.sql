﻿use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'GetOData'
declare @TestScenario varchar(255) = 'WriteableEndpointTests|FilterOrderBySelectTop'
declare @TestCase varchar(255) = 'A'
declare @ControllerPath varchar(255) = 'api/Rgb'

declare @Filter varchar(255) = 'contains(Name, ''Blue'')'
declare @Select varchar(255) = 'Name,SysUser'
declare @OrderBy varchar(255) = 'Name desc'
declare @Skip int = 0
declare @Top int = 10

declare 
	@Expected varchar(max) = 
(
	select Name, SysUser from Rgb
	where Name like '%Blue%'
	order by Name desc
	offset @Skip rows fetch next @Top row only
	for json path, include_null_values
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Filter', @Filter
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'OrderBy', @OrderBy
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Top', @Top
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase