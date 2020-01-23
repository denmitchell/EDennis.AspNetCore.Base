use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbController'
declare @MethodName varchar(255) = 'Post'
declare @TestScenario varchar(255) = 'WriteableEndpointTests'
declare @TestCase varchar(255) = 'B'
declare @WindowStart int = -999148
declare @WindowEnd int = -999143
declare @ControllerPath varchar(255) = 'api/Rgb'

declare @Id int = -999202
declare @Red int = 202
declare @Green int = 202
declare @Blue int = 202

declare @recordCount int;
declare @pageCount int;
declare @pageNumber int;
declare @pageSize int;

select @recordCount = count(*) from Rgb where Id between @WindowStart and @WindowEnd or Id = @Id
select @pageSize = @recordCount
select @pageCount = 1
select @pageNumber = 1 

declare 
	@Input varchar(max) = 
(
	select @Id Id, @TestCase Name, @Red Red, @Green Green, @Blue Blue, 'tester@example.org' SysUser, '2020-01-01' DateAdded
		for json path, without_array_wrapper
);

begin transaction
insert into Rgb(Id,Name,Red,Green,Blue,SysUser,DateAdded)
	select @Id Id, @TestCase Name, @Red Red, @Green Green, @Blue Blue, 'tester@example.org' SysUser, '2020-01-01' DateAdded

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
				where Id between @WindowStart and @WindowEnd or Id = @Id
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


rollback transaction
exec _.ResetIdentities

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Id', @Id
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'WindowStart', @WindowStart
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'WindowEnd', @WindowEnd
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ControllerPath', @ControllerPath
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

