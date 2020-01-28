use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'GetWithDynamicLinq'
declare @TestScenario varchar(255) = 'Bad Request'
declare @TestCase varchar(255) = 'C'

declare @ControllerPath varchar(255) = 'api/Rgb'
declare @OrderBy varchar(255) = 'fjakl#rn!*&'
declare @Where varchar(255) = '$DF#hadi@*(98'
declare @Skip int = 2
declare @Take int = 5

declare @currentPage int;
declare @pageCount int;
declare @pageSize int;
declare @rowCount int;

select @rowCount = count(*) from Rgb where Red > 200
set @currentPage = 1 + ceiling(convert(decimal(10,2),@Skip)/@Take)
set @pageCount = ceiling(convert(decimal(10,2),@rowCount)/@Take)
set @pageSize = @Take

declare @ExpectedStatusCode int = 400 --Bad Request
declare 
	@Expected varchar(max) = 
(
	select null
);


exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ControllerPath', @ControllerPath
--exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Where', @Where
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'OrderBy', @OrderBy
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Skip', @Skip
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ExpectedStatusCode', @ExpectedStatusCode
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase