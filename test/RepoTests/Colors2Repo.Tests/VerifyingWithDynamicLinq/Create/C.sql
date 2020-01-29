use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'Create'
declare @TestScenario varchar(255) = 'Verifying with Dynamic Linq, DbUpdateException'
declare @TestCase varchar(255) = 'C'

declare @Name varchar(255) = 'Marsala'
declare @Red int = 150
declare @Green int = 82
declare @Blue int = 81

declare @LinqWhere varchar(255) = 'Id ge -999149 and Id le -999143'

declare @TargetId int = -999145
declare @Exception varchar(255) = 'DbUpdateException' --Conflict

begin transaction

declare @Input varchar(max) = 
(
	select * from Rgb where Id = @TargetId
	for json path, without_array_wrapper
)

declare @Expected varchar(max) = 
(
	select * from Rgb where Id between -999149 and -999143
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Exception', @Exception
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'LinqWhere', @LinqWhere
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
