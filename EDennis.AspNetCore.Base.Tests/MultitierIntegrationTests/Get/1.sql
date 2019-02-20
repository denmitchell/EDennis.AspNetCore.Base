use colordb;
declare @Id int = 1
declare @Expected varchar(max) = (
	select Id, Name
		from Colors
		where Id = @Id
		for json path, without_array_wrapper
);
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Get','HttpClientExtensions',@Id,'Id',@Id
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Get','HttpClientExtensions',@Id,'Expected',@Expected
exec  _maintenance.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Get','HttpClientExtensions',@Id
