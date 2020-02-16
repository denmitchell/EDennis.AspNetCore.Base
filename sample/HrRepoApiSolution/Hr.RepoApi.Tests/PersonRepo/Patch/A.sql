use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi'
declare @ClassName varchar(255) = 'PersonRepo'
declare @MethodName varchar(255) = 'Patch'
declare @TestScenario varchar(255) = 'Success'
declare @TestCase varchar(255) = 'A'

declare @Id int = -999005
declare @FirstName varchar(255) = 'Chandler'
declare @SysUser varchar(255) = 'tester@example.org'

declare @LinqWhere varchar(255) = 'Id ge -999006 and Id le -999004'

declare @Exception varchar(255) = null --Success


begin transaction
declare @Input varchar(max) = 
(
	select
		@Id Id,
		@FirstName FirstName,
		@SysUser SysUser
	for json path, without_array_wrapper
)

update Person set FirstName=@FirstName, SysUser=@SysUser
	where Id = @Id

declare @Expected varchar(max) = 
(
	select * from Person where Id between -999006 and -999004
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @Id
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Exception', @Exception
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'LinqWhere', @LinqWhere

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
