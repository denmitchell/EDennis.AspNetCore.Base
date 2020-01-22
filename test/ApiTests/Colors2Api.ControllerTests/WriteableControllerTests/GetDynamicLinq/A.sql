use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'GetDynamicLinq'
declare @TestScenario varchar(255) = 'WriteableControllerTests|WhereSkipTake'
declare @TestCase varchar(255) = 'A'

declare @Select varchar(255) = null -- don't use
declare @OrderBy varchar(255) = 'Id asc'
declare @Where varchar(255) = 'Red gt 200'
declare @Skip int = 2
declare @Take int = 5



declare @recordCount int;
declare @pageCount int;
declare @pageNumber int;

select @recordCount = count(*) from Rgb where Red > 200
select @pageCount = ceiling(convert(decimal(10,2),@recordCount)/@Take)
select @pageNumber = 1 + ceiling(convert(decimal(10,2),@Skip)/@Take)
declare 
	@Expected varchar(max) = 
(
	select
		RecordCount [PagingData.RecordCount], 
		PageSize [PagingData.PageSize], 
		PageNumber [PagingData.PageNumber], 
		PageCount [PagingData.PageCount],
		(
			select * 
				from Rgb
				where Red > 200
				order by Name desc
				offset @Skip rows fetch next @Take rows only
				for json path, include_null_values
		) Data
		
		from (
			select
				@recordCount RecordCount,
				@Take PageSize,
				@pageNumber PageNumber,
				@pageCount PageCount
		) PagingData
		for json path, without_array_wrapper
);


exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Where', @Where
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'OrderBy', @OrderBy
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Skip', @Skip
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase