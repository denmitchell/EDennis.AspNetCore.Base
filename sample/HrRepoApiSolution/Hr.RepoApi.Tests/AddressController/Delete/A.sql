use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi'
declare @ClassName varchar(255) = 'AddressController'
declare @MethodName varchar(255) = 'Delete'
declare @TestScenario varchar(255) = 'No Content'
declare @TestCase varchar(255) = 'A'
declare @LinqWhere varchar(255) = 'Id ge -999006 and Id le -999004'

declare @ControllerPath varchar(255) = 'api/Address'
declare @TargetId int = -999005
declare @ExpectedStatusCode int = 204 --No Content

begin transaction

declare @Expected varchar(max) = 
(
	select * from Address 
		where Id between -999006 and -999004
			and Id <> @TargetId
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ExpectedStatusCode', @ExpectedStatusCode
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'LinqWhere', @LinqWhere

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
