use colordb;

declare @TestCase varchar(1) = 'A'
declare @alpha varchar(10) = 'e';

declare @expected varchar(max) = (
	select * 
		from Color 
		where Name Like '%' + @alpha + '%' 
	for json path, include_null_values
);

exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromJsonSql','SqlRepo',@TestCase,'Alpha', @alpha
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromJsonSql','SqlRepo',@TestCase,'Expected', @expected
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromJsonSql','SqlRepo',@TestCase

