use colordb;
declare @id int = 4;
begin transaction
declare @Input int = @id;
delete from Colors where Id = @id;
declare @Expected varchar(max) = (
	select Id, Name
		from Colors
		for json path
);
rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Delete','SqlRepo',@Id,'Input',@Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Delete','SqlRepo',@Id,'Expected',@Expected
exec  _maintenance.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Delete','SqlRepo',@Id
