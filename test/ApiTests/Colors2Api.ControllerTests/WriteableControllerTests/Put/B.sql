use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'Put'
declare @TestScenario varchar(255) = 'WriteableControllerTests'
declare @TestCase varchar(255) = 'B'
declare @WindowStart int = -999143

declare @Id int = -999146
declare @Red int = 146
declare @Green int = 146
declare @Blue int = 146

declare 
	@Input varchar(max) = 
(
	select @ID Id, @TestCase Name, @Red Red, @Green Green, @Blue Blue, 'tester@example.org' SysUser, '2020-01-01' DateAdded
		for json path, without_array_wrapper
);

begin transaction
update Rgb set Red = @Red, Green = @Green, Blue = @Blue where Id = @Id

declare 
	@Expected varchar(max) = 
(
	select * from Rgb
	where Id <= @WindowStart
	for json path
);

rollback transaction

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Id', @Id
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'WindowStart', @WindowStart
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

