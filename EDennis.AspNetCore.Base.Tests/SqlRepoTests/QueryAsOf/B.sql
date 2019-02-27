use colordb;
declare @TestCase varchar(3) = 'B';
declare @Alpha varchar(3) = 'a';
declare @AsOf datetime2 = '2016-07-01';

declare @Expected varchar(max) = (
	select * from (
		select * from Color 
			where Name like '%' + @Alpha + '%'
		union select * from dbo_history.Color
			where Name like '%' + @Alpha + '%'
	) a
	where @AsOf between a.SysStart and a.SysEnd
	for json path, include_null_values
);


exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','QueryAsOf','SqlRepo',@TestCase,'Alpha', @Alpha
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','QueryAsOf','SqlRepo',@TestCase,'AsOf', @AsOf
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','QueryAsOf','SqlRepo',@TestCase,'Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','QueryAsOf','SqlRepo',@TestCase
