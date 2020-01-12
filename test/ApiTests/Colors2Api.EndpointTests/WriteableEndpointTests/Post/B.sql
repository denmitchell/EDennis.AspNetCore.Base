﻿use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'Post'
declare @TestScenario varchar(255) = 'WriteableEndpointTests'
declare @TestCase varchar(255) = 'B'
declare @WindowStart int = -999143

declare @Id int = -999202
declare @Red int = 202
declare @Green int = 202
declare @Blue int = 202

declare 
	@Input varchar(max) = 
(
	select @Id Id, @TestCase Name, @Red Red, @Green Green, @Blue Blue, 'tester@example.org' SysUser, '2020-01-01' DateAdded
		for json path, without_array_wrapper
);

begin transaction
insert into Rgb(Id,Name,Red,Green,Blue,SysUser,DateAdded)
	select @Id Id, @TestCase Name, @Red Red, @Green Green, @Blue Blue, 'tester@example.org' SysUser, '2020-01-01' DateAdded

declare 
	@Expected varchar(max) = 
(
	select * from Rgb
	where Id <= @WindowStart
	for json path
);

rollback transaction
exec _.ResetIdentities

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'WindowStart', @WindowStart
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

