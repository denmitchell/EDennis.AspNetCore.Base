use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'GetWithDevExtreme'
declare @TestScenario varchar(255) = 'Bad Request'
declare @TestCase varchar(255) = 'B'

declare @ControllerPath varchar(255) = 'api/Rgb'
declare @Filter varchar(255) = 'h438p;hh62#@$!jfha'
declare @Select varchar(255) = '8q34#@FGjfieru'
declare @Sort varchar(255) = 'Aadj$DSF9!'
declare @Skip int = 0
declare @Take int = 10

declare @ExpectedStatusCode int = 400 --Bad Request

declare 
	@Expected varchar(max) = 
(
	select null 
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Filter', @Filter
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Sort', @Sort
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ExpectedStatusCode', @ExpectedStatusCode
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase