use Color2Db;
declare @TestCase varchar(255) = 'B'
declare @Name varchar(255) = 'Marsala'
declare @Red int = 150
declare @Green int = 82
declare @Blue int = 81

-- Limit the window of records inspected for testing purposes
--	-9990149	@ReadOnlyEnd - 1	@NewRecordId
--  -9990148	@ReadOnlyEnd
--  -9990147    @ReadOnlyEnd + 1
--  -9990146    @ReadOnlyEnd + 2
--  -9990145    @ReadOnlyStart		Start of test records
declare @ReadOnlyCount int = 4
declare @ReadOnlyStart int
declare @ReadOnlyEnd int
declare @NewRecordId int

select @ReadOnlyEnd = min(Id) from Rgb;
set @ReadOnlyStart = @ReadOnlyEnd + @ReadOnlyCount - 1
set @NewRecordId = @ReadOnlyEnd - 1

begin transaction
insert into Rgb (Id, Name, Red, Green, Blue, SysUser, DateAdded) 
    values 
        (@NewRecordId, @Name, @Red, @Green, @Blue, 'tester@example.org', GETDATE())

declare @Input varchar(max) = 
(
	select * from Rgb where Id = @NewRecordId
	for json path, without_array_wrapper
)

declare @Expected varchar(max) = 
(
	select * from Rgb where Id <= @ReadOnlyStart
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson 'Colors2Repo', 'RgbRepo', 'Create', '', @TestCase, 'Input', @Input
exec _.SaveTestJson 'Colors2Repo', 'RgbRepo', 'Create', '', @TestCase, 'Expected', @Expected
exec _.SaveTestJson 'Colors2Repo', 'RgbRepo', 'Create', '', @TestCase, 'ReadOnlyStart', @ReadOnlyStart

exec _.GetTestJson 'Colors2Repo', 'RgbRepo', 'Create', '', @TestCase
