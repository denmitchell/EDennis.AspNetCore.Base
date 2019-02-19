use colordb;

declare @alpha varchar(10) = 'bl';
declare @id int = 2;

declare @expected varchar(max) = (
	select * from (
		select * 
			from Colors 
			where Name Like @alpha + '%' 
		except select * 
			from Colors 
			where Id = @id
	) a
	for json path, include_null_values
);

exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo','A','Alpha', @alpha
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo','A','Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo','A','Expected', @expected
