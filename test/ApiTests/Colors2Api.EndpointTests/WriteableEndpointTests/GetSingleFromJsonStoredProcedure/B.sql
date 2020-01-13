﻿use Color2Db;
go
sp_configure 'Show Advanced Options', 1
go
reconfigure
go
sp_configure 'Ad Hoc Distributed Queries', 1
go
reconfigure
go
if object_id('tempdb..#SpResults') is not null drop table #SpResults
go

declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'GetSingleFromJsonStoredProcedure'
declare @TestScenario varchar(255) = 'WriteableEndpointTests|RgbJsonByColorName'
declare @TestCase varchar(255) = 'B'

declare @ControllerPath varchar(255) = 'api/Rgb'
declare @SpName varchar(255) = 'RgbJsonByColorName'
declare @ColorName varchar(255) = 'DarkKhaki'


select * into #SpResults 
    from openrowset('SQLNCLI', 
	  'Server=(localdb)\MSSQLLocalDb;Database=Color2Db;Trusted_Connection=yes;',
      'EXEC RgbJsonByColorName ''DarkKhaki''')

declare @ParamValues varchar(max) =
(
	select @ColorName ColorName
	for json path, without_array_wrapper
);

declare 
	@Expected varchar(max) = 
(
	select [Json] from #SpResults
	for json path, without_array_wrapper
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ColorName', @ColorName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ParamValues', @ParamValues
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

if object_id('tempdb..#SpResults') is not null drop table #SpResults
go