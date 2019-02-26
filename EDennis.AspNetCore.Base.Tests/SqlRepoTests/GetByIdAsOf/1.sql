use colordb;
declare @Id int = 1;
declare @AsOf datetime2 = '2017-07-01';

declare @Expected varchar(max) = (
	select * from (
		select * from Color 
			where Id = @id
		union select * from dbo_history.Color
			where Id = @id
	) a
	where @AsOf between a.SysStart and a.SysEnd
	for json path, without_array_wrapper, include_null_values
);


exec _.SaveTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@Id,'Id', @Id
exec _.SaveTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@Id,'AsOf', @AsOf
exec _.SaveTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@Id,'Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','GetByIdAsOf','SqlRepo',@Id
