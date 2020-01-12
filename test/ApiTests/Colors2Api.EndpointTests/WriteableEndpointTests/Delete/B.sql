use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'Delete'
declare @TestScenario varchar(255) = 'WriteableEndpointTests'
declare @TestCase varchar(255) = 'B'
declare @WindowStart int = -999143

declare @Id int = -999146

declare 
	@Expected varchar(max) = 
(
	select * from Rgb
	where Id <= @WindowStart and Id <> @Id
	for json path
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Id', @Id
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase