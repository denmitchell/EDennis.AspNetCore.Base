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
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'GetListFromJsonStoredProcedure'
declare @TestScenario varchar(255) = 'WriteableEndpointTests|RgbJsonByColorNameContains'
declare @TestCase varchar(255) = 'B'

declare @ControllerPath varchar(255) = 'api/Rgb'
declare @SpName varchar(255) = 'RgbJsonByColorNameContains'
declare @ColorNameContains varchar(255) = 'Red'

declare @ParamValues varchar(max) =
(
	select @ColorNameContains ColorNameContains
	for json path, without_array_wrapper
);

select * into #SpResults 
    from openrowset('SQLNCLI', 
	  'Server=(localdb)\MSSQLLocalDb;Database=Color2Db;Trusted_Connection=yes;',
      'EXEC RgbJsonByColorNameContains ''Red''')

/*
declare 
	@ExpectedJsonColumn varchar(max) = 
(
	select [Json]
	from #SpResults
	for json path
);
*/

--use OPENJSON to convert Json column result to regular table
--and then convert back to simplified Json
declare 
	@Expected varchar(max) = 
(
select * 
    from openjson((select json from #SpResults))
    WITH( 
      Id int,  
      Name varchar(255),  
      Red int,  
      Green int,  
      Blue int
     ) as recs
 for json path
)


exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ColorNameContains', @ColorNameContains
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ParamValues', @ParamValues
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

if object_id('tempdb..#SpResults') is not null drop table #SpResults
go
