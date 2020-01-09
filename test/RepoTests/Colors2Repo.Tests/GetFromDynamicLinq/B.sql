use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'GetFromDynamicLinq'
declare @TestScenario varchar(255) = 'WhereOrderBySkipTake'
declare @TestCase varchar(255) = 'B'

declare @Select varchar(255) = '' -- don't use
declare @OrderBy varchar(255) = 'Id asc'
declare @Where varchar(255) = 'Red gt 200'
declare @Skip int = 2
declare @Take int = 5

declare 
	@Expected varchar(max) = 
(
	select * from Rgb
	where Red > 200 
	order by Id
	offset @Skip ROWS FETCH NEXT @Take ROWS ONLY
	for json path, include_null_values
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Where', @Where
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'OrderBy', @OrderBy
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Skip', @Skip
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase