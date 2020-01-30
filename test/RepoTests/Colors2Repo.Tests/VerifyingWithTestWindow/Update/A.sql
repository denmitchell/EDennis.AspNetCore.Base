use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'Patch'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = 'A'

declare @Name varchar(255) = 'GreyA'
declare @Red int = 128
declare @Green int = 128
declare @Blue int = 128
declare @SysUser varchar(255) = 'tester@example.org'

declare @WindowStart int
select @WindowStart = min(Id) from Rgb;
declare @WindowEnd int = @WindowStart + 5 

declare @TargetId int
select @TargetId = @WindowStart + 1 from Rgb;


begin transaction
declare @Input varchar(max) = 
(
	select
		@TargetId Id,
		@Name Name,
		@Red Red, @Green Green, @Blue Blue
	for json path, without_array_wrapper
)

update Rgb set Name=@Name, Red=@Red, Blue=@Blue, Green=@Green, SysUser=@SysUser
	where Id = @TargetId

declare @Expected varchar(max) = 
(
	select * from Rgb where Id between @WindowStart and @WindowEnd
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'WindowStart', @WindowStart
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'WindowEnd', @WindowEnd

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
