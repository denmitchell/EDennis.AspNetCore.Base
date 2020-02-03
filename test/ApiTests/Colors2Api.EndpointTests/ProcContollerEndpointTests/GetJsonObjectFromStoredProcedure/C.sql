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

use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'ProcController'
declare @MethodName varchar(255) = 'GetJsonObjectFromStoredProcedure'
declare @TestScenario varchar(255) = 'Bad Request'
declare @TestCase varchar(255) = 'C'

declare @ControllerPath varchar(255) = 'api/Proc'
declare @SpName varchar(255) = '$fjs3i3ls'
declare @ColorName varchar(255) = 'DarkKhaki'

select Red,Green,Blue into #SpResults from Rgb where 1=0

declare @ExpectedStatusCode int = 400 --Bad Request
declare 
	@Expected varchar(max) = 
(
	select * from #SpResults
	for json path, without_array_wrapper
);
declare @Params varchar(max) = 
(
    select @ColorName ColorName
	for json path, without_array_wrapper
)

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Params', @Params
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ExpectedStatusCode', @ExpectedStatusCode
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

if object_id('tempdb..#SpResults') is not null drop table #SpResults
go
