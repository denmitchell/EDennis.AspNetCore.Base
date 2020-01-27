use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'Post'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = 'A'

declare @Name varchar(255) = 'Lucite'
declare @Red int = 125
declare @Green int = 208
declare @Blue int = 182

declare @TargetId int
select @TargetId = min(Id) - 1 from Rgb;

declare @WindowStart int
select @WindowStart = @TargetId;
declare @WindowEnd int = @WindowStart + 5 

begin transaction
insert into Rgb (Id, Name, Red, Green, Blue, SysUser, DateAdded) 
    values 
        (@TargetId, @Name, @Red, @Green, @Blue, 'tester@example.org', GETDATE())

declare @Input varchar(max) = 
(
	select * from Rgb where Id = @TargetId
	for json path, without_array_wrapper
)

declare @Expected varchar(max) = 
(
	select * from Rgb where Id between @WindowStart and @WindowEnd
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'WindowStart', @WindowStart
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'WindowEnd', @WindowEnd

exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
