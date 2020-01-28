use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'GetWithDevExtreme'
declare @TestScenario varchar(255) = 'FilterSkipTake'
declare @TestCase varchar(255) = 'B'

declare @ControllerPath varchar(255) = 'api/Rgb'
declare @Filter varchar(255) = '["Red",">","200"]'
declare @Skip int = 2
declare @Take int = 5

declare @ExpectedStatusCode int = 200 --Success

declare 
	@Expected varchar(max) = 
(
	select * from Rgb
	where Red > 200 
	order by Id
	offset @Skip ROWS FETCH NEXT @Take ROWS ONLY
	for json path, include_null_values
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Filter', @Filter
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Skip', @Skip
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ExpectedStatusCode', @ExpectedStatusCode
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase