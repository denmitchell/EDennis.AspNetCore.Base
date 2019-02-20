use colordb;
declare @id int = 1;
declare @Color varchar(30) = 'brown'
begin transaction
declare @Input varchar(max) = (
select @id Id, @Color Name
	for json path, without_array_wrapper
);
update Colors set Name = @Color where Id = @id;
declare @Expected varchar(max) = (
	select Id, Name
		from Colors
		for json path
);
rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Update','SqlRepo',@Id,'Input',@Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Update','SqlRepo',@Id,'Expected',@Expected
exec  _maintenance.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Update','SqlRepo',@Id
