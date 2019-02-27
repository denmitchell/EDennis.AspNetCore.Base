use colordb;
begin transaction
declare @Color varchar(30) = 'brown'
declare @SysUser varchar(255) = 'moe@stooges.net'
declare @Input varchar(max) = (
select @Color Name
	for json path, without_array_wrapper
);
insert into Color(name, SysStart, SysEnd, SysUser) 
	values (@Color, getdate(), _.MaxDateTime2(), @SysUser);
declare @Expected varchar(max) = (
	select *
		from Color
		for json path
);
rollback transaction
exec _.ResetIdentities;
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Create','SqlRepo',@Color,'Input',@Input
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Create','SqlRepo',@Color,'Expected',@Expected
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Create','SqlRepo',@Color
