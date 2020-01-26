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
declare @MethodName varchar(255) = 'GetScalarFromStoredProcedure'
declare @TestScenario varchar(255) = 'StringResult'
declare @TestCase varchar(255) = 'A'

declare @SpName varchar(255) = 'RgbHex'
declare @ColorName varchar(255) = 'AliceBlue'

create table #SpResults(RgbHex varchar(6))

declare @sql nvarchar(max) =
   N'insert into #SpResults(RgbHex) 
      select RgbHex
        from openrowset(
            ''SQLNCLI'',
            ''Server=(localdb)\MSSQLLocalDb;Database=Color2Db;Trusted_Connection=yes'',
            ''EXEC ' + @SpName + ' @ColorName =''''' + @ColorName + ''''''')'
exec(@sql)
declare @Expected varchar(6) = (select * from #SpResults)

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ColorName', @ColorName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase


if object_id('tempdb..#SpResults') is not null drop table #SpResults
go