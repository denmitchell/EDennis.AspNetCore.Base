use Colors2;
declare @ProjectName varchar(255) = 'EDennis.Samples.Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'PostAsync'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = '1'
declare @Id int = convert(int,@TestCase)

declare 
	@Input varchar(max) = 
(
	select 0 Id, Name, @Id Red, @Id Green, @Id Blue, SysUser, DateAdded
		from Rgb
		where Id = @Id
		for json path, without_array_wrapper
);

begin transaction
insert into Rgb(Name,Red,Green,Blue,SysUser,DateAdded)
	select Name, @Id, @Id, @Id, SysUser, DateAdded
	from Rgb
	where Id = @Id

declare 
	@Expected varchar(max) = 
(
	select * from Rgb
	for json path
);

rollback transaction
exec _.ResetIdentities

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

