use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'Delete'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = 'B'

declare @WindowStart int
select @WindowStart = min(Id) from Rgb;
declare @WindowEnd int = @WindowStart + 5 

declare @TargetId int
select @TargetId = @WindowStart + 3 from Rgb;


begin transaction

declare @Expected varchar(max) = 
(
	select * from Rgb 
		where Id between @WindowStart and @WindowEnd
			and Id <> @TargetId
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'WindowStart', @WindowStart
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'WindowEnd', @WindowEnd

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
