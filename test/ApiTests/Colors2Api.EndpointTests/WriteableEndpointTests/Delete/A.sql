use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'Delete'
declare @TestScenario varchar(255) = 'WriteableEndpointTests'
declare @TestCase varchar(255) = 'A'
declare @WindowStart int = -999148
declare @WindowEnd int = -999143

declare @ControllerPath varchar(255) = 'api/Rgb'
declare @Id int = -999145

declare @recordCount int;
declare @pageCount int;
declare @pageNumber int;
declare @pageSize int;

select @recordCount = count(*) from Rgb where Id between @WindowStart and @WindowEnd and Id <> @Id
select @pageSize = @recordCount
select @pageCount = 1
select @pageNumber = 1 

declare 
	@Expected varchar(max) = 
(
	select
		RecordCount [PagingData.RecordCount], 
		PageSize [PagingData.PageSize], 
		PageNumber [PagingData.PageNumber], 
		PageCount [PagingData.PageCount],
		(
			select * from Rgb
			where Id between @WindowStart and @WindowEnd and Id <> @Id
			for json path
		) Data
		
		from (
			select
				@recordCount RecordCount,
				@pageSize PageSize,
				@pageNumber PageNumber,
				@pageCount PageCount
		) PagingData
		for json path, without_array_wrapper
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Id', @Id
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'WindowStart', @WindowStart
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'WindowEnd', @WindowEnd
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase