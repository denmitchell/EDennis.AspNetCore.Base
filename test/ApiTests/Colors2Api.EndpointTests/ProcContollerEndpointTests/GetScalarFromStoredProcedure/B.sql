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
declare @MethodName varchar(255) = 'GetScalarFromStoredProcedure'
declare @TestScenario varchar(255) = 'Success'
declare @TestCase varchar(255) = 'B'

declare @ControllerPath varchar(255) = 'api/Proc'
declare @SpName varchar(255) = 'RgbInt'
declare @ColorName varchar(255) = 'DarkKhaki'
declare @ReturnType varchar(255) = 'int'

select 0 intVal into #SpResults from Rgb where 1=0

declare @sql nvarchar(max) =
   N'insert into #SpResults 
      select *
        from openrowset(
            ''SQLNCLI'',
            ''Server=(localdb)\MSSQLLocalDb;Database=Color2Db;Trusted_Connection=yes'',
            ''EXEC ' + @SpName + ' @ColorName =''''' + @ColorName + ''''''')'
exec(@sql)

declare @Params varchar(max) = 
(
    select @ColorName ColorName
	for json path, without_array_wrapper
)


declare @ExpectedStatusCode int = 200
declare @Expected int;
select @Expected = intVal from #spResults

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ReturnType', @ReturnType
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Params', @Params
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ExpectedStatusCode', @ExpectedStatusCode
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

if object_id('tempdb..#SpResults') is not null drop table #SpResults
go
