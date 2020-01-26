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
declare @MethodName varchar(255) = 'GetSingleFromJsonStoredProcedure'
declare @TestScenario varchar(255) = 'ReadonlyEndpointTests|HslJsonByColorName'
declare @TestCase varchar(255) = 'B'

declare @ControllerPath varchar(255) = 'api/Hsl'
declare @SpName varchar(255) = 'HslJsonByColorName'
declare @ColorName varchar(255) = 'DarkKhaki'


select * into #SpResults 
    from openrowset('SQLNCLI', 
	  'Server=(localdb)\MSSQLLocalDb;Database=Color2Db;Trusted_Connection=yes;',
      'EXEC HslJsonByColorName ''DarkKhaki''')

declare @ParamValues varchar(max) =
(
	select @ColorName ColorName
	for json path, without_array_wrapper
);

/*
declare 
	@ExpectedJsonColumn varchar(max) = 
(
	select [Json]
	from #SpResults
	for json path, without_array_wrapper
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
      Hue int,  
      Saturation int,  
      Luminance int
     ) as recs
 for json path, without_array_wrapper
)


exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ColorName', @ColorName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ParamValues', @ParamValues
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

if object_id('tempdb..#SpResults') is not null drop table #SpResults
go
