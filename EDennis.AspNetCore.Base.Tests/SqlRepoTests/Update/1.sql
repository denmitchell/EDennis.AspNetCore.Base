use colordb;
declare @Id int = 1;
declare @Color varchar(30) = 'brown'
declare @SysUser varchar(255) = 'moe@stooges.net'
begin transaction
declare @Input varchar(max) = (
select @id Id, @Color Name, getdate() SysStart, 
		_.MaxDateTime2() SysEnd, @SysUser SysUser
	for json path, without_array_wrapper
);
if object_id('tempdb..#color') is not null
	drop table #color;
select * into #color from color where Id = @Id
update #color set SysUserNext = @SysUser;

update Color set 
		Name = @Color,
		SysStart = getdate(),
		SysEnd = _.MaxDateTime2(),
		SysUser = @SysUser
	where Id = @id;
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
rollback transaction;

exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Update','SqlRepo',@Id,'Id',@Id
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Update','SqlRepo',@Id,'Input',@Input
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Update','SqlRepo',@Id,'Expected',@Expected
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Update','SqlRepo',@Id,'ExpectedHistory',@ExpectedHistory
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Update','SqlRepo',@Id
