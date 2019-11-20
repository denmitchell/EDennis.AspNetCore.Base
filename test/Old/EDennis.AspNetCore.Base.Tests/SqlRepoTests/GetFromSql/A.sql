use colordb;

declare @TestCase varchar(1) = 'A'
declare @alpha varchar(10) = 'e';
declare @id int = 2;

declare @expected varchar(max) = (
	select * from (
		select * 
			from Color 
			where Name Like '%' + @alpha + '%' 
		except select * 
			from Color 
			where Id = @id
	) a
	for json path, include_null_values
);

exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo',@TestCase,'Alpha', @alpha
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo',@TestCase,'Id', @id
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo',@TestCase,'Expected', @expected
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo',@TestCase

