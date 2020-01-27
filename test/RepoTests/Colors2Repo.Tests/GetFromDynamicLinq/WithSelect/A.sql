﻿use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'GetFromDynamicLinq'
declare @TestScenario varchar(255) = 'WithSelect'
declare @TestCase varchar(255) = 'A'

declare @Where varchar(255) = 'Name.Contains("Blue")'
declare @Select varchar(255) = 'new(Name,SysUser)'
declare @OrderBy varchar(255) = 'Name desc'
declare @Skip int = 0
declare @Take int = 10

declare @currentPage int;
declare @pageCount int;
declare @pageSize int;
declare @rowCount int;

select @rowCount = count(*) from Rgb where Name like '%Blue%'
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
			select Name, SysUser 
				from Rgb
				where Name like '%Blue%'
				order by Name desc
				offset @Skip rows fetch next @Take rows only
				for json path, include_null_values
		) Data
		
		for json path, without_array_wrapper
);


exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Select', @Select
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Where', @Where
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'OrderBy', @OrderBy
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Skip', @Skip
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase