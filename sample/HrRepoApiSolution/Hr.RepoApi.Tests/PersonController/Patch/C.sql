use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi'
declare @ClassName varchar(255) = 'PersonController'
declare @MethodName varchar(255) = 'Patch'
declare @TestScenario varchar(255) = 'Bad Request - Bad Id'
declare @TestCase varchar(255) = 'C'

declare @ControllerPath varchar(255) = 'api/Person'
declare @FirstName varchar(255) = 'Chandler'
declare @SysUser varchar(255) = 'tester@example.org'

declare @LinqWhere varchar(255) = 'Id ge -999006 and Id le -999004'

declare @TargetId int = -999005
declare @InvalidId int = -999299
declare @ExpectedStatusCode int = 400 --Bad Request


begin transaction
declare @Input varchar(max) = 
(
	select
		@InvalidId Id,
		@FirstName FirstName, @SysUser SysUser
	for json path, without_array_wrapper
)


declare @Expected varchar(max) = 
(
	select * from Person where Id between -999006 and -999004
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ExpectedStatusCode', @ExpectedStatusCode
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'LinqWhere', @LinqWhere

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
