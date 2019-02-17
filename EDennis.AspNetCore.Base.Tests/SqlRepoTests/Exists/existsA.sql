use colordb;
declare @id int = 4;

declare @expected bit = CAST(
   CASE WHEN EXISTS (
	select Id, Name
		from Colors
		where Id = @id
) THEN 1 ELSe 0 END AS BIT);

exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Exists','SqlRepo','A','Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Exists','SqlRepo','A','Expected', @expected
