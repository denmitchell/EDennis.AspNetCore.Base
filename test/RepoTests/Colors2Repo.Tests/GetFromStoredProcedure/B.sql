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
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'HslRepo'
declare @MethodName varchar(255) = 'GetFromStoredProcedure'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = 'B'

declare @SpName varchar(255) = 'HslByColorName'
declare @ColorName varchar(255) = 'DarkKhaki'


select * into #SpResults 
    from openrowset('SQLNCLI', 
	  'Server=(localdb)\MSSQLLocalDb;Database=Color2Db;Trusted_Connection=yes;',
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
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

if object_id('tempdb..#SpResults') is not null drop table #SpResults
go
