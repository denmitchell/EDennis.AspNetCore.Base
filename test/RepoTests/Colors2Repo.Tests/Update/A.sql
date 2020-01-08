use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'Update'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = 'A'

declare @Name varchar(255) = 'GreyA'
declare @Red int = 128
declare @Green int = 128
declare @Blue int = 128
declare @SysUser varchar(255) = 'tester@example.org'
declare @TargetId int = -999145

-- Limit the window of records inspected for testing purposes
--  -999148		
--  -999147		
--  -999146		
--  -999145		
--  -999144		
--  -999143		@WindowStart		Start of test window		
declare @WindowStart int = -999143

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
	select * from Rgb where Id <= @WindowStart
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @Id
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'WindowStart', @WindowStart

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
