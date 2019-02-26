use colordb;
declare @Id int = 4;

declare @expected bit = CAST(
   CASE WHEN EXISTS (
	select *
		from Color
		where Id = @Id
) THEN 1 ELSE 0 END AS BIT);

exec _.SaveTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','Exists','SqlRepo',@Id,'Id', @id
exec _.SaveTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','Exists','SqlRepo',@Id,'Expected', @expected
exec  _.GetTestJson 'EDennis.Samples.Color.InternalApi','ColorRepo','Exists','SqlRepo',@Id
