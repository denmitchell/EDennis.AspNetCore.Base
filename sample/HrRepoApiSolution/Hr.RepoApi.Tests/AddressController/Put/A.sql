use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi'
declare @ClassName varchar(255) = 'AddressController'
declare @MethodName varchar(255) = 'Put'
declare @TestScenario varchar(255) = 'Success'
declare @TestCase varchar(255) = 'A'

declare @ControllerPath varchar(255) = 'api/Address'
declare @StreetAddress varchar(255) = '123 Main Street'
declare @SysUser varchar(255) = 'tester@example.org'

declare @LinqWhere varchar(255) = 'Id ge -999006 and Id le -999004'

declare @Id int = -999005
declare @ExpectedStatusCode int = 200 --Success


begin transaction
declare @Input varchar(max) = 
(
	select
		@Id Id,
		@StreetAddress StreetAddress,
		City,State,PostalCode,
		@SysUser SysUser
		from Address where Id = @Id
	for json path, without_array_wrapper
)

update Address set StreetAddress=@StreetAddress, SysUser=@SysUser
	where Id = @Id

declare @Expected varchar(max) = 
(
	select * from Address where Id between -999006 and -999004
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @Id
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ExpectedStatusCode', @ExpectedStatusCode
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'LinqWhere', @LinqWhere

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
