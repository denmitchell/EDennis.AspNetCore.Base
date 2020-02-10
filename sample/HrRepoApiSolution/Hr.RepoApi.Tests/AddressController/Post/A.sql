use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi.Lib'
declare @ClassName varchar(255) = 'AddressController'
declare @MethodName varchar(255) = 'Post'
declare @TestScenario varchar(255) = 'Success'
declare @TestCase varchar(255) = 'A'

declare @ControllerPath varchar(255) = 'api/Address'
declare @Id int = -999301
declare @StreetAddress varchar(255) = '123 Main Street'
declare @City varchar(255) = 'Xenia'
declare @State varchar(2) = 'OH'
declare @PostalCode varchar(255) = '45385'
declare @PersonId int = -999005
declare @SysUser varchar(255) = 'tester@example.org'

declare @LinqWhere varchar(255) = 'Id ge -999301 and Id le -999299'

declare @ExpectedStatusCode int = 200 --Success

begin transaction
insert into Address (Id, StreetAddress, City, State, PostalCode, PersonId, SysUser) 
    values 
        (@Id, @StreetAddress, @City, @State, @PostalCode, @PersonId, @SysUser)

declare @Input varchar(max) = 
(
	select * from Address where Id = @Id
	for json path, without_array_wrapper
)

declare @Expected varchar(max) = 
(
	select * from Address where Id between -999301 and -999299
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
