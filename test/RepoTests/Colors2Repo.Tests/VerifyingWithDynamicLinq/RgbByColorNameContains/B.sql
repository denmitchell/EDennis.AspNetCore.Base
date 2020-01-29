﻿go
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
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'RgbByColorNameContains'
declare @TestScenario varchar(255) = 'Verifying with Dynamic Linq, Params'
declare @TestCase varchar(255) = 'B'

declare @SpName varchar(255) = 'RgbByColorNameContains'
declare @ColorNameContains varchar(255) = 'Green'

select * into #SpResults from Rgb where 1=0

declare @sql nvarchar(max) =
   N'insert into #SpResults 
      select *
        from openrowset(
            ''SQLNCLI'',
            ''Server=(localdb)\MSSQLLocalDb;Database=Color2Db;Trusted_Connection=yes'',
            ''EXEC ' + @SpName + ' @ColorNameContains =''''' + @ColorNameContains + ''''''')'
exec(@sql)

declare @Params varchar(max) = 
(
    select @ColorNameContains ColorNameContains
	for json path, without_array_wrapper
)

declare @Exception varchar(255) = null
declare 
	@Expected varchar(max) = 
(
	select * from #SpResults
	for json path, include_null_values
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Params', @Params
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Exception', @Exception
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

if object_id('tempdb..#SpResults') is not null drop table #SpResults
go