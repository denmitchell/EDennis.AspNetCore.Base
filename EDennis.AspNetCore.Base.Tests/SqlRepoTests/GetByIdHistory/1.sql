use colordb;
declare @Id int = 1;

declare @Expected varchar(max) = (
	select * from (
		select * from Colors 
			for system_time all
			where Id = @id
	) a
	for json path, include_null_values
);


exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdHistory','SqlRepo',@Id,'Id', @Id
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdHistory','SqlRepo',@Id,'Expected', @Expected

exec  _maintenance.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdHistory','SqlRepo',@Id
