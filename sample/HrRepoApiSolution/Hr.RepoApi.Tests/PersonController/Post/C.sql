use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi.Lib'
declare @ClassName varchar(255) = 'PersonRepo'
declare @MethodName varchar(255) = 'Post'
declare @TestScenario varchar(255) = 'Conflict'
declare @TestCase varchar(255) = 'C'

declare @ControllerPath varchar(255) = 'api/Person'
declare @FirstName varchar(255) = 'Chandler'
declare @LastName varchar(255) = 'Bing'
declare @DateOfBirth datetime = '1970-05-01'
declare @SysUser varchar(255) = 'tester@example.org'

declare @LinqWhere varchar(255) = 'Id ge -999006 and Id le -999004'

declare @TargetId int = -999005
declare @ExpectedStatusCode int = 409 --Conflict

begin transaction

declare @Input varchar(max) = 
(
	select * from Person where Id = @TargetId
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
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ExpectedStatusCode', @ExpectedStatusCode
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'LinqWhere', @LinqWhere

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
