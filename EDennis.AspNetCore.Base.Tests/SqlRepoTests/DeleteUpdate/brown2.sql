use colordb;
declare @id int = 2;
declare @Color varchar(30) = 'brown'
declare @TestCase varchar(35) = @Color + convert(varchar(1),@id)
begin transaction
declare @Input varchar(max) = (
select @id Id, @Color Name
	for json path, without_array_wrapper
);
update Colors set Name = @Color where Id = @id;
delete from Colors where Id = @id;
declare @Expected varchar(max) = (
	select Id, Name
		from Colors
		for json path
);
declare @ExpectedHistory varchar(max) = (
	select Id, Name
		from dbo_history.Colors
		for json path
);
rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','DeleteUpdate','SqlRepo',@TestCase,'Input',@Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','DeleteUpdate','SqlRepo',@TestCase,'Expected',@Expected
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','DeleteUpdate','SqlRepo',@TestCase,'ExpectedHistory',@ExpectedHistory
exec  _maintenance.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','DeleteUpdate','SqlRepo',@TestCase
