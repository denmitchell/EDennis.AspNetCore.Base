use colordb;
begin transaction
declare @Input varchar(max) = (
select 'brown' Name
	for json path, without_array_wrapper
);
insert into Colors(name) values ('brown');
declare @Expected varchar(max) = (
	select Id, Name
		from Colors
		for json path
);
rollback transaction
exec _maintenance.ResetIdentities;
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Post','HttpClientExtensions','A','Input',@Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Post','HttpClientExtensions','A','Expected',@Expected
