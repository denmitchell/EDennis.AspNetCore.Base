use Color2Db;
declare @TestCase varchar(255) = 'Lucite'
declare @Red int = 125
declare @Green int = 208
declare @Blue int = 182
declare @ReadSeedMax int = -999142
declare @WriteSeedMax int = -999146

begin transaction
insert into Rgb (Id, Name, Red, Green, Blue, SysUser, DateAdded) 
    values 
        (@WriteSeedMax,'WhiteSmoke', 245, 245, 245, 'jill@hill.org', CAST('2019-01-24T12:29:50.0000000' AS DateTime2)),
        (@WriteSeedMax-1,'Yellow', 255, 255, 0, 'jack@hill.org', CAST('2019-01-23T12:29:50.0000000' AS DateTime2)),
        (@WriteSeedMax-2,'YellowGreen', 154, 205, 50, 'jack@hill.org', CAST('2019-01-22T12:29:50.0000000' AS DateTime2)),
        -- target
        (@WriteSeedMax-3,@TestCase, @Red, @Green, @Blue, 'testuser@example.org', GETDATE())

declare @Input varchar(max) = 
(
	select * from Rgb where Id = @WriteSeedMax-3
	for json path, without_array_wrapper
)

declare @Expected varchar(max) = 
(
	select * from Rgb where Id <= @ReadSeedMax
	for json path
);

rollback transaction
--exec _.ResetSequences

exec _.SaveTestJson 'Colors2Repo', 'RgbRepo', 'Create', '', @TestCase, 'Input', @Input
exec _.SaveTestJson 'Colors2Repo', 'RgbRepo', 'Create', '', @TestCase, 'Expected', @Expected

exec _.GetTestJson 'Colors2Repo', 'RgbRepo', 'Create', '', @TestCase
