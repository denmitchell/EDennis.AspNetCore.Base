use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi'
declare @ClassName varchar(255) = 'AddressController'
declare @MethodName varchar(255) = 'Get'
declare @TestScenario varchar(255) = 'Not Found'
declare @TestCase varchar(255) = 'C'

declare @ControllerPath varchar(255) = 'api/Address'
declare @TargetId int = -999399
declare @ExpectedStatusCode int = 404 --Not Found

begin transaction

declare @Expected varchar(max) = 
(
	select * from Address 
		where Id = @TargetId
	for json path, without_array_wrapper
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ExpectedStatusCode', @ExpectedStatusCode

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
