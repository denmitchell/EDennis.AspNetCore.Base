declare @any varchar(10) = '{ANY}'
declare @ReadSeedMax int = -999142
declare @WriteSeedMax int = -999146

begin transaction
insert into Rgb (Id, Name, Red, Green, Blue, SysUser, DateAdded) 
    values 
        (@WriteSeedMax,'WhiteSmoke', 245, 245, 245, 'jill@hill.org', CAST('2019-01-24T12:29:50.0000000' AS DateTime2)),
        (@WriteSeedMax-1,'Yellow', 255, 255, 0, 'jack@hill.org', CAST('2019-01-23T12:29:50.0000000' AS DateTime2)),
        (@WriteSeedMax-2,'YellowGreen', 154, 205, 50, 'jack@hill.org', CAST('2019-01-22T12:29:50.0000000' AS DateTime2))

declare @ReadSeed varchar(max) = 
(
	select * from Rgb where Id <= @ReadSeedMax and Id > @WriteSeedMax
	for json path, include_null_values
);
declare @WriteSeed varchar(max) = 
(
	select * from Rgb where Id <= @WriteSeedMax
	for json path, include_null_values
);
declare @Seed varchar(max) = 
(
	select * from Rgb where Id <= @ReadSeedMax
	for json path, include_null_values
);

rollback transaction
--exec _.ResetSequences

exec _.SaveTestJson 'Colors2Repo', @any, @any, @any, @any, 'WriteSeed', @WriteSeed
exec _.SaveTestJson 'Colors2Repo', @any, @any, @any, @any, 'ReadSeed', @ReadSeed
exec _.SaveTestJson 'Colors2Repo', @any, @any, @any, @any, 'Seed', @Seed

exec _.GetTestJson 'Colors2Repo', @any, @any, @any, @any
