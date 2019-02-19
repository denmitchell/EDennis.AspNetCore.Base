use colordb;
declare @Id int = 4;

declare @expected bit = CAST(
   CASE WHEN EXISTS (
	select Id, Name
		from Colors
		where Id = @Id
) THEN 1 ELSE 0 END AS BIT);

exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Exists','SqlRepo',@Id,'Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Exists','SqlRepo',@Id,'Expected', @expected
exec  _maintenance.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Exists','SqlRepo',@Id
