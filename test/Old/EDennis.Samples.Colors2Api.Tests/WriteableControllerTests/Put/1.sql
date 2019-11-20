use Colors2;
declare @ProjectName varchar(255) = 'EDennis.Samples.Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'Put'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = '1'
declare @Id int = convert(int,@TestCase)

declare 
	@Input varchar(max) = 
(
	select Id, Name, @Id Red, @Id Green, @Id Blue, SysUser, DateAdded
		from Rgb
		where Id = @Id
		for json path, without_array_wrapper
);

begin transaction
update Rgb set Red = @Id, Green = @Id, Blue = @Id where Id = @Id

declare 
	@Expected varchar(max) = 
(
	select * from Rgb
	for json path
);

rollback transaction

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Id', @Id
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

