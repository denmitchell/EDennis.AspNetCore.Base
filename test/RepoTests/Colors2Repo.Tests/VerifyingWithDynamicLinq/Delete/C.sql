use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'Delete'
declare @TestScenario varchar(255) = 'Verifying with Dynamic Linq, Exception'
declare @TestCase varchar(255) = 'C'
declare @LinqWhere varchar(255) = 'Id ge -999148 and Id le -999143'

declare @TargetId int = -999299
declare @ThrowsException bit = 1

begin transaction

declare @Expected varchar(max) = 
(
	select * from Rgb 
		where Id between -999148 and -999143
			and Id <> @TargetId
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ThrowsException', @ThrowsException
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'LinqWhere', @LinqWhere
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
