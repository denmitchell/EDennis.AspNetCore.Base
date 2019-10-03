use colordb;
begin transaction
declare @Color varchar(30) = 'orange'
declare @SysUser varchar(255) = 'moe@stooges.org'
declare @Input varchar(max) = (
select @Color Name
	for json path, without_array_wrapper
);
insert into Color(name, SysStart, SysEnd, SysUser) 
	values (@Color, getdate(), _.MaxDateTime2(), @SysUser);
declare @Expected varchar(max) = (
	select Id, Name
		from Color
		for json path
);
rollback transaction
exec _.ResetIdentities;
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Post','HttpClientExtensions',@Color,'Input',@Input
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Post','HttpClientExtensions',@Color,'Expected',@Expected
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Post','HttpClientExtensions',@Color
