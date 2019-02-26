use colordb;
declare @Id int = 2
declare @Input varchar(max) = @Id;
declare @Expected varchar(max) = (
	select *
		from Color
		where Id = @Id
		for json path, without_array_wrapper
);
exec _.SaveTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','GetById','SqlRepo',@Id,'Input',@Input
exec _.SaveTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','GetById','SqlRepo',@Id,'Expected',@Expected
exec  _.GetTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','GetById','SqlRepo',@Id
