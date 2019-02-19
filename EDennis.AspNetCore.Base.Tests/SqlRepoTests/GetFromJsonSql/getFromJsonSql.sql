use colordb;

declare @alpha varchar(10) = 'e';

declare @expected varchar(max) = (
	select * 
		from Colors 
		where Name Like '%' + @alpha + '%' 
	for json path, include_null_values
);

exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromJsonSql','SqlRepo','A','Alpha', @alpha
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromJsonSql','SqlRepo','A','Expected', @expected
