use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'GetDynamicLinq'
declare @TestScenario varchar(255) = 'WriteableControllerTests|WhereOrderBySelectTake'
declare @TestCase varchar(255) = 'B'

declare @Where varchar(255) = 'Name.Contains("Blue")'
declare @Select varchar(255) = 'new(Name,SysUser)'
declare @OrderBy varchar(255) = 'Name desc'
declare @Skip int = 0
declare @Take int = 10


declare @recordCount int;
declare @pageCount int;
declare @pageNumber int;

select @recordCount = count(*) from Rgb where name like '%Blue%'
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
			select Name, SysUser 
				from Rgb
				where Name like '%Blue%'
				order by Name desc
				offset @Skip rows fetch next @Take row only
				for json path
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
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase