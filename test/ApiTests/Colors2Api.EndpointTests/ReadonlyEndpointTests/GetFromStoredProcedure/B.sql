use Color2Db;
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
declare @ClassName varchar(255) = 'HslController'
declare @MethodName varchar(255) = 'GetFromStoredProcedure'
declare @TestScenario varchar(255) = 'ReadonlyEndpointTests|HslByColorName'
declare @TestCase varchar(255) = 'B'

declare @ControllerPath varchar(255) = 'api/Hsl'
declare @SpName varchar(255) = 'HslByColorName'
declare @ColorName varchar(255) = 'DarkKhaki'

declare @ParamValues varchar(max) =
(
	select @ColorName ColorName
	for json path, without_array_wrapper
);

select * into #SpResults 
    from openrowset('SQLNCLI', 
	  'Server=(localdb)\MSSQLLocalDb;Database=Colors2;Trusted_Connection=yes;',
      'EXEC HslByColorName ''DarkKhaki''')

declare 
	@Expected varchar(max) = 
(
	select Hue, Saturation, Luminance from #SpResults
	for json path, include_null_values
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ColorName', @ColorName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ParamValues', @ParamValues
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

if object_id('tempdb..#SpResults') is not null drop table #SpResults
go
