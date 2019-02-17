use colordb;
declare @id int = 3;
begin transaction
declare @Input int = @id;
delete from Colors where Id = @id;
declare @Expected varchar(max) = (
	select Id, Name
		from Colors
		for json path
);
rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Delete','SqlRepo','A','Input',@Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Delete','SqlRepo','A','Expected',@Expected
