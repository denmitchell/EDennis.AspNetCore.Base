use colordb;
declare @id int = 2;
begin transaction
declare @Input varchar(max) = (
select @id Id, 'brown' Name
	for json path, without_array_wrapper
);
update Colors set Name = 'brown' where Id = @id;
declare @Expected varchar(max) = (
	select Id, Name
		from Colors
		for json path
);
rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Update','SqlRepo','A','Input',@Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Update','SqlRepo','A','Expected',@Expected
