use colordb;
declare @Id int = 3;

declare @Expected varchar(max) = (
	select * from (
		select * from Color 
			where Id = @id
		union select * from dbo_history.Color
			where Id = @id
	) a
	for json path, include_null_values
);


exec _.SaveTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','GetByIdHistory','SqlRepo',@Id,'Id', @Id
exec _.SaveTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','GetByIdHistory','SqlRepo',@Id,'Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','GetByIdHistory','SqlRepo',@Id
