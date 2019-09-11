use Colors2;
declare @ProjectName varchar(255) = 'EDennis.Samples.Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'Delete'
declare @TestScenario varchar(255) = ''
declare @TestCase varchar(255) = '2'

declare @Id int = convert(int,@TestCase)

declare 
	@Expected varchar(max) = 
(
	select * from Rgb
	where Id <> @Id
	for json path
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Id', @Id
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase