use colordb;
declare @Id int = 2
declare @Expected varchar(max) = (
	select *
		from Color
		where Id = @Id
		for json path, without_array_wrapper
);
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Get','HttpClientExtensions',@Id,'Id',@Id
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Get','HttpClientExtensions',@Id,'Expected',@Expected
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Get','HttpClientExtensions',@Id
