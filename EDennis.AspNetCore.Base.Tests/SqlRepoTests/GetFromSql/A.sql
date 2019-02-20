use colordb;

declare @TestCase varchar(1) = 'A'
declare @alpha varchar(10) = 'e';
declare @id int = 2;

declare @expected varchar(max) = (
	select * from (
		select * 
			from Colors 
			where Name Like '%' + @alpha + '%' 
		except select * 
			from Colors 
			where Id = @id
	) a
	for json path, include_null_values
);

exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo',@TestCase,'Alpha', @alpha
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo',@TestCase,'Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo',@TestCase,'Expected', @expected
exec  _maintenance.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo',@TestCase

