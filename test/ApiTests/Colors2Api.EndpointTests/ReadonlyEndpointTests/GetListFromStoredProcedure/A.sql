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
declare @MethodName varchar(255) = 'GetListFromStoredProcedure'
declare @TestScenario varchar(255) = 'ReadonlyEndpointTests|HslByColorNameContains'
declare @TestCase varchar(255) = 'A'

declare @ControllerPath varchar(255) = 'api/Hsl'
declare @SpName varchar(255) = 'HslByColorNameContains'
declare @ColorNameContains varchar(255) = 'Blue'

declare @ParamValues varchar(max) =
(
	select @ColorNameContains ColorNameContains
	for json path
);

select * into #SpResults 
    from openrowset('SQLNCLI', 
	  'Server=(localdb)\MSSQLLocalDb;Database=Color2Db;Trusted_Connection=yes;',
      'EXEC HslByColorNameContains ''Blue''')

declare 
	@Expected varchar(max) = 
(
	select * from #SpResults
	for json path
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ColorNameContains', @ColorNameContains
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ParamValues', @ParamValues
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

if object_id('tempdb..#SpResults') is not null drop table #SpResults
go
