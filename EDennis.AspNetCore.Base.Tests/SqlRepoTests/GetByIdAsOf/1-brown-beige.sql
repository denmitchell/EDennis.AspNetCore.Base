use colordb;

exec _maintenance.Temporal_DisableSystemTime
go

declare @Id int = 1;
declare @AsOf datetime2 = '2017-07-01';
declare @Color1 varchar(30) = 'brown'
declare @SysStart1 datetime2 = '2016-01-01'
declare @Color2 varchar(30) = 'beige'
declare @SysStart2 datetime2 = '2017-01-01'
declare @TestCase varchar(100) = convert(varchar(1),@Id) + '-' + @Color1 + '-' + @Color2

insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select @Id,@Color2,@SysStart2,dateadd(millisecond,-1,SysStart)
		from dbo.Colors
		where Id = @Id
	
insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select @Id,@Color1,@SysStart1,dateadd(millisecond,-1,SysStart)
		from dbo_history.Colors
		where Id = @Id



exec _maintenance.Temporal_EnableSystemTime
go

declare @Id int = 1;
declare @AsOf datetime2 = '2017-07-01';
declare @Color1 varchar(30) = 'brown'
declare @SysStart1 datetime2 = '2016-01-01'
declare @Color2 varchar(30) = 'beige'
declare @SysStart2 datetime2 = '2017-01-01'
declare @TestCase varchar(100) = convert(varchar(1),@Id) + '-' + @Color1 + '-' + @Color2


select * from Colors 
	for system_time all
	where Id = @id



declare @Expected varchar(max) = (
	select * from (
		select * from Colors 
			for system_time as of @asOf
			where Id = @id
	) a
	for json path, without_array_wrapper, include_null_values
);


exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@TestCase,'Id', @Id
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@TestCase,'AsOf', @AsOf
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@TestCase,'Color1', @Color1
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@TestCase,'Color2', @Color2
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@TestCase,'SysStart1', @SysStart1
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@TestCase,'SysStart2', @SysStart2
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@TestCase,'Expected', @Expected

exec  _maintenance.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@TestCase


exec _maintenance.Temporal_DisableSystemTime
go
exec _maintenance.Temporal_DisableSystemTime
go
truncate table dbo_history.Colors
go
exec _maintenance.Temporal_EnableSystemTime
go