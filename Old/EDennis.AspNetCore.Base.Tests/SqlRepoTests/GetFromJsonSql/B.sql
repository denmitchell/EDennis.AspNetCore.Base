use colordb;

declare @TestCase varchar(1) = 'B'
declare @alpha varchar(10) = 'bl';

declare @expected varchar(max) = (
	select * 
		from Color 
		where Name Like '%' + @alpha + '%' 
	for json path, include_null_values
);

exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromJsonSql','SqlRepo',@TestCase,'Alpha', @alpha
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromJsonSql','SqlRepo',@TestCase,'Expected', @expected
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromJsonSql','SqlRepo',@TestCase

