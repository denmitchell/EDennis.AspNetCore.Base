use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi'
declare @ClassName varchar(255) = 'AddressController'
declare @MethodName varchar(255) = 'GetWithDevExtreme'
declare @TestScenario varchar(255) = 'FilterSortSelectTake'
declare @TestCase varchar(255) = 'A'

declare @ControllerPath varchar(255) = 'api/Address'
declare @Filter varchar(255) = '["State","=","CA"]'
declare @Select varchar(255) = '["StreetAddress","City"]'
declare @Sort varchar(255) = '[{selector:"City",asc:true},{selector:"StreetAddress",asc:true}]'
declare @Skip int = 0
declare @Take int = 10

declare @ExpectedStatusCode int = 200 --Success

declare 
	@Expected varchar(max) = 
(
	select StreetAddress,City from Address
	where State = 'CA'
	order by City, StreetAddress
	offset @Skip rows fetch next @Take row only
	for json path, include_null_values
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Filter', @Filter
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Sort', @Sort
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ExpectedStatusCode', @ExpectedStatusCode
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase