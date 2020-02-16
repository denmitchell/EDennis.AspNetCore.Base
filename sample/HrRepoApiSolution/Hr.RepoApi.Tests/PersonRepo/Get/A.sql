use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi'
declare @ClassName varchar(255) = 'PersonRepo'
declare @MethodName varchar(255) = 'Get'
declare @TestScenario varchar(255) = 'Success'
declare @TestCase varchar(255) = 'A'

declare @TargetId int = -999005
declare @Exception varchar(255) = null

declare @Expected varchar(max) = 
(
	select * from Person 
		where Id = @TargetId
	for json path, without_array_wrapper
);

--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Exception', @Exception
exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
