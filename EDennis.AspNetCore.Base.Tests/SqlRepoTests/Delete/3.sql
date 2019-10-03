use colordb;
declare @id int = 3;
declare @SysUser varchar(255) = 'moe@stooges.org'
begin transaction
declare @Input int = @id;

if object_id('tempdb..#color') is not null
	drop table #color;
select * into #color from color where Id = @Id
update #color set SysUserNext = @SysUser;


delete from Color where Id = @id;
declare @Expected varchar(max) = (
	select *
		from Color
		for json path
);

declare @ExpectedHistory varchar(max) = (
	select * from (
		select * from dbo_history.Color
		union select * from #color			
	) a	for json path
);

drop table #color;
rollback transaction
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Delete','SqlRepo',@Id,'Input',@Input
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Delete','SqlRepo',@Id,'Expected',@Expected
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Delete','SqlRepo',@Id,'ExpectedHistory',@ExpectedHistory
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Delete','SqlRepo',@Id
