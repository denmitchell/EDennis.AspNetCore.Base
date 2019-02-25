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
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Delete','SqlRepo',@Id,'Input',@Input
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Delete','SqlRepo',@Id,'Expected',@Expected
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Delete','SqlRepo',@Id
