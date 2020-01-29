use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'Patch'
declare @TestScenario varchar(255) = 'Verifying with Dynamic Linq, JsonException'
declare @TestCase varchar(255) = 'D'

declare @DateAdded varchar(255) = 'AnInvalidDate'
declare @Red int = 55
declare @Green int = 55
declare @Blue int = 225
declare @SysUser varchar(255) = 'tester@example.org'

declare @LinqWhere varchar(255) = 'Id ge -999148 and Id le -999143'

declare @TargetId int = -999146
declare @Exception varchar(255) = 'JsonException'

begin transaction
declare @Input varchar(max) = 
(
	select @DateAdded DateAdded, 'ABC' AnotherBadField
		for json path, without_array_wrapper
)

declare @Expected varchar(max) = 
(
	select * from Rgb where Id between -999148 and -999143
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Exception', @Exception
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'LinqWhere', @LinqWhere

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
