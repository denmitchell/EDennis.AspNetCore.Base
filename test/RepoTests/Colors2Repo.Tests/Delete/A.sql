use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'Delete'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = 'A'

declare @TargetId int = -999146; --must be in Window range below

-- Limit the window of records inspected for testing purposes
--  -999148		
--  -999147		
--  -999146		@TargetId
--  -999145		
--  -999144		
--  -999143		@WindowStart		Start of test window		
declare @WindowStart int = -999143

begin transaction

declare @Expected varchar(max) = 
(
	select * from Rgb 
		where Id <= @WindowStart
			and Id <> @TargetId
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'WindowStart', @WindowStart

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
