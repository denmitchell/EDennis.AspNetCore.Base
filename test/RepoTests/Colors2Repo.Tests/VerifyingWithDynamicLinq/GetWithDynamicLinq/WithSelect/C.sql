use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'GetWithDynamicLinq'
declare @TestScenario varchar(255) = 'Verifying with Dynamic Linq, ParseException'
declare @TestCase varchar(255) = 'C'

declare @Where varchar(255) = '$fjiou#jdfah'
declare @Select varchar(255) = '$jaj84a@!'
declare @OrderBy varchar(255) = 'ldfa*73D!jd'
declare @Skip int = 0
declare @Take int = 10

declare @currentPage int;
declare @pageCount int;
declare @pageSize int;
declare @rowCount int;

select @rowCount = count(*) from Rgb where Name like '%Green%'
set @currentPage = 1 + ceiling(convert(decimal(10,2),@Skip)/@Take)
set @pageCount = ceiling(convert(decimal(10,2),@rowCount)/@Take)
set @pageSize = @Take

declare @Exception varchar(255) = 'ParseException'

declare 
	@Expected varchar(max) = 
(
	select null
);


exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Where', @Where
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'OrderBy', @OrderBy
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Skip', @Skip
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Take', @Take
--exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Exception', @Exception
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase