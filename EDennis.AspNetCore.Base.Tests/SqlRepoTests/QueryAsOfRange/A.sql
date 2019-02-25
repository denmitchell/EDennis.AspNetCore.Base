use colordb;
declare @TestCase varchar(3) = 'A';
declare @Alpha varchar(3) = 'e';
declare @From datetime2 = '2015-07-01';
declare @To datetime2 = '2017-07-01';

declare @Expected varchar(max) = (
	select * from (
		select * from Color 
			where Name like '%' + @Alpha + '%'
		union select * from dbo_history.Color
			where Name like '%' + @Alpha + '%'
	) a
	where a.SysStart <= @To 
		and a.SysEnd >= @From
	for json path, include_null_values
);


exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','QueryAsOfRange','SqlRepo',@TestCase,'Alpha', @Alpha
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','QueryAsOfRange','SqlRepo',@TestCase,'From', @From
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','QueryAsOfRange','SqlRepo',@TestCase,'To', @To
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','QueryAsOfRange','SqlRepo',@TestCase,'Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','QueryAsOfRange','SqlRepo',@TestCase
