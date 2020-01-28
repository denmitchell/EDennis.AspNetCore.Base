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
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'RgbByColorName'
declare @TestScenario varchar(255) = 'Success'
declare @TestCase varchar(255) = 'A'

declare @ControllerPath varchar(255) = 'api/Rgb'
declare @SpName varchar(255) = 'RgbByColorName'
declare @ColorName varchar(255) = 'AliceBlue'

select Red,Green,Blue into #SpResults from Rgb where 1=0

declare @sql nvarchar(max) =
   N'insert into #SpResults 
      select *
        from openrowset(
            ''SQLNCLI'',
            ''Server=(localdb)\MSSQLLocalDb;Database=Color2Db;Trusted_Connection=yes'',
            ''EXEC ' + @SpName + ' @ColorName =''''' + @ColorName + ''''''')'
exec(@sql)

declare @ExpectedStatusCode int = 200
declare 
	@Expected varchar(max) = 
(
	select * from #SpResults
	for json path, without_array_wrapper
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ColorName', @ColorName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ExpectedStatusCode', @ExpectedStatusCode
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

if object_id('tempdb..#SpResults') is not null drop table #SpResults
go
