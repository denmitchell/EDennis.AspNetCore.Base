use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi'
declare @ClassName varchar(255) = 'AddressController'
declare @MethodName varchar(255) = 'GetWithDynamicLinq'
declare @TestScenario varchar(255) = 'Without Select'
declare @TestCase varchar(255) = 'A'

declare @ControllerPath varchar(255) = 'api/Address'
--declare @Select varchar(255) = null -- don't use
declare @OrderBy varchar(255) = 'City asc, StreetAddress asc'
declare @Where varchar(255) = 'State eq "CA"'
declare @Skip int = 2
declare @Take int = 5

declare @currentPage int;
declare @pageCount int;
declare @pageSize int;
declare @rowCount int;

select @rowCount = count(*) from Address where State = 'CA'
set @currentPage = 1 + ceiling(convert(decimal(10,2),@Skip)/@Take)
set @pageCount = ceiling(convert(decimal(10,2),@rowCount)/@Take)
set @pageSize = @Take

declare @ExpectedStatusCode int = 200 --Success
declare 
	@Expected varchar(max) = 
(
	select
		@currentPage [CurrentPage],
		@pageCount [PageCount],
		@pageSize [PageSize],
		@rowCount [RowCount],
		(
			select * 
				from Address
				where State = 'CA'
				order by City asc, StreetAddress asc
				offset @Skip rows fetch next @Take rows only
				for json path, include_null_values
		) Data
		
		for json path, without_array_wrapper
);


exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ControllerPath', @ControllerPath
--exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Where', @Where
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'OrderBy', @OrderBy
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Skip', @Skip
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'ExpectedStatusCode', @ExpectedStatusCode
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase