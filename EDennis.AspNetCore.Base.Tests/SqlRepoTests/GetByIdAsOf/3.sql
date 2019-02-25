use colordb;
declare @Id int = 3;
declare @AsOf datetime2 = '2016-07-01';

declare @Expected varchar(max) = (
	select * from (
		select * from Colors 
			for system_time as of @asOf
			where Id = @id
	) a
	for json path, without_array_wrapper, include_null_values
);


exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@Id,'Id', @Id
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@Id,'AsOf', @AsOf
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@Id,'Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@Id
