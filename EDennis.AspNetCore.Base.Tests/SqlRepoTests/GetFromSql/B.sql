use colordb;

declare @TestCase varchar(1) = 'B'
declare @alpha varchar(10) = 'bl';
declare @id int = 1;

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

