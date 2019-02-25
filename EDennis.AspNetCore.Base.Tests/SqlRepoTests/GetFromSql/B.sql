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

exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo',@TestCase,'Alpha', @alpha
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo',@TestCase,'Id', @id
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo',@TestCase,'Expected', @expected
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetFromSql','SqlRepo',@TestCase

