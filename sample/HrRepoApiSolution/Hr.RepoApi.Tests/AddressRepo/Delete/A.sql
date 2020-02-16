use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi'
declare @ClassName varchar(255) = 'AddressRepo'
declare @MethodName varchar(255) = 'Delete'
declare @TestScenario varchar(255) = 'Success'
declare @TestCase varchar(255) = 'A'
declare @LinqWhere varchar(255) = 'Id ge -999005 and Id le -999003'

declare @TargetId int = -999004
declare @Exception varchar(255) = null

begin transaction

declare @Expected varchar(max) = 
(
	select * from Address 
		where Id between -999005 and -999003
			and Id <> @TargetId
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Exception', @Exception
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'LinqWhere', @LinqWhere
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
