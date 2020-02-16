﻿use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi'
declare @ClassName varchar(255) = 'PersonController'
declare @MethodName varchar(255) = 'Get'
declare @TestScenario varchar(255) = 'Success'
declare @TestCase varchar(255) = 'A'

declare @ControllerPath varchar(255) = 'api/Person'
declare @TargetId int = -999005
declare @ExpectedStatusCode int = 200 --Success

begin transaction

declare @Expected varchar(max) = 
(
	select * from Person 
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
