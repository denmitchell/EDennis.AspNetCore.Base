use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'GetWithDynamicLinq'
declare @TestScenario varchar(255) = 'WithoutSelect'
declare @TestCase varchar(255) = 'B'

--declare @Select varchar(255) = null -- don't use
declare @OrderBy varchar(255) = 'Id asc'
declare @Where varchar(255) = 'Red gt 200'
declare @Skip int = 2
declare @Take int = 5

declare @currentPage int;
declare @pageCount int;
declare @pageSize int;
declare @rowCount int;

select @rowCount = count(*) from Rgb where Red > 200
set @currentPage = 1 + ceiling(convert(decimal(10,2),@Skip)/@Take)
set @pageCount = ceiling(convert(decimal(10,2),@rowCount)/@Take)
set @pageSize = @Take

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
				from Rgb
				where Red > 200
				order by Id asc
				offset @Skip rows fetch next @Take rows only
				for json path, include_null_values
		) Data
		
		for json path, without_array_wrapper
);


--exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Where', @Where
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'OrderBy', @OrderBy
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Skip', @Skip
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase