use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'Get'
declare @TestScenario varchar(255) = 'Verifying with Dynamic Linq, Success'
declare @TestCase varchar(255) = 'B'

declare @TargetId int = -999147
declare @Exception varchar(255) = null

begin transaction

declare @Expected varchar(max) = 
(
	select * from Rgb 
		where Id = @TargetId
	for json path, without_array_wrapper
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Exception', @Exception
exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
