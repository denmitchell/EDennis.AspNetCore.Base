use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'Create'
declare @TestScenario varchar(255) = 'Verifying with Dynamic Linq, Success'
declare @TestCase varchar(255) = 'A'

declare @Name varchar(255) = 'Lucite'
declare @Red int = 125
declare @Green int = 208
declare @Blue int = 182

declare @LinqWhere varchar(255) = 'Id ge -999149 and Id le -999143'

declare @TargetId int = -999149
declare @Exception varchar(255) = null --Success

begin transaction
insert into Rgb (Id, Name, Red, Green, Blue, SysUser, DateAdded) 
    values 
        (@TargetId, @Name, @Red, @Green, @Blue, 'tester@example.org', GETDATE())

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
