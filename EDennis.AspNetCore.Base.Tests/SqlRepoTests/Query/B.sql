use colordb;

declare @TestCase varchar(1) = 'B'

declare @pageNumber int = 2
declare @pageSize int = 1
declare @alpha varchar(max) = 'bl';

declare @expected varchar(max) = (
	select Id, Name
		from Colors
		where Name Like '%' + @alpha + '%'
		order by id
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only 
		for json path, include_null_values
);

exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Query','SqlRepo',@TestCase,'Alpha', @alpha
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Query','SqlRepo',@TestCase,'PageNumber', @pageNumber
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Query','SqlRepo',@TestCase,'PageSize', @pageSize
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Query','SqlRepo',@TestCase,'Expected', @expected
exec  _maintenance.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Query','SqlRepo',@TestCase

