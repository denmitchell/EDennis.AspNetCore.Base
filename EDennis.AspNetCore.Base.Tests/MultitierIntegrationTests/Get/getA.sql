use colordb;
declare @Id int = 1
declare @Input varchar(max) = @id;
declare @Expected varchar(max) = (
	select Id, Name
		from Colors
		where Id = @Id
		for json path, without_array_wrapper
);
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Get','HttpClientExtensions','A','Input',@Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorController','Get','HttpClientExtensions','A','Expected',@Expected
