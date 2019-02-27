use colordb;

declare @TestCase varchar(1) = 'B'

declare @pageNumber int = 2
declare @pageSize int = 1
declare @alpha varchar(max) = 'bl';

declare @expected varchar(max) = (
	select *
		from Color
		where Name Like '%' + @alpha + '%'
		order by id
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only 
		for json path, include_null_values
);

exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Query','SqlRepo',@TestCase,'Alpha', @alpha
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Query','SqlRepo',@TestCase,'PageNumber', @pageNumber
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Query','SqlRepo',@TestCase,'PageSize', @pageSize
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Query','SqlRepo',@TestCase,'Expected', @expected
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Query','SqlRepo',@TestCase

