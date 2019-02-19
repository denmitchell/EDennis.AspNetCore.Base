use colordb;
declare @pageNumber int = 2
declare @pageSize int = 2
declare @alpha varchar(max) = 'e';

declare @expected varchar(max) = (
	select Id, Name
		from Colors
		where Name Like '%' + @alpha + '%'
		order by id
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only 
		for json path, include_null_values
);

exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByLinq','SqlRepo','A','Alpha', @alpha
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByLinq','SqlRepo','A','PageNumber', @pageNumber
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByLinq','SqlRepo','A','PageSize', @pageSize
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByLinq','SqlRepo','A','Expected', @expected
