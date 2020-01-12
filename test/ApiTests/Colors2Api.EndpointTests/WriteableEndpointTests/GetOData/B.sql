use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'GetOData'
declare @TestScenario varchar(255) = 'WriteableEndpointTests|FilterSkipTop'
declare @TestCase varchar(255) = 'B'

declare @Filter varchar(255) = 'Red gt 200'
declare @Skip int = 2
declare @Top int = 5

declare 
	@Expected varchar(max) = 
(
	select * from Rgb
	where Red > 200 
	order by Id
	offset @Skip ROWS FETCH NEXT @Top ROWS ONLY
	for json path, include_null_values
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Filter', @Filter
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Skip', @Skip
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Top', @Top
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase