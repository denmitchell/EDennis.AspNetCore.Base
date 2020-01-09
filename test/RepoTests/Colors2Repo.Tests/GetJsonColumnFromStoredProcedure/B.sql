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

declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'GetJsonColumnFromStoredProcedure'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = 'B'

declare @SpName varchar(255) = 'RgbJsonByColorName'
declare @ColorName varchar(255) = 'DarkKhaki'


select * into #SpResults 
    from openrowset('SQLNCLI', 
	  'Server=(localdb)\MSSQLLocalDb;Database=Color2Db;Trusted_Connection=yes;',
      'EXEC RgbJsonByColorName ''DarkKhaki''')

declare 
	@ExpectedJsonColumn varchar(max) = 
(
	select [Json]
	from #SpResults
	for json path, without_array_wrapper
);

--use OPENJSON to convert Json column result to regular table
--and then convert back to simplified Json
declare 
	@Expected varchar(max) = 
(
	select cast(Red as int) Red, cast(Green as int) Green, cast(Blue as int) Blue from
		(select *
			from 
			openjson(
				(select value
					from openjson(@ExpectedJsonColumn)
				)
			)
		) t	
		pivot(
			min([value])
			for [key] in ([Red],[Green],[Blue])
		) as result
	for json path, without_array_wrapper
)

--select @Expected;

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ColorName', @ColorName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

if object_id('tempdb..#SpResults') is not null drop table #SpResults
go
