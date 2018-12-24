use todo;

declare @Id int = 1

declare 
	@expected varchar(max) = 
(
	select * from Task
	where id = @Id
	for json path, include_null_values, without_array_wrapper
);

exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Get','Get','A','Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Get','Get','A','Expected', @expected

exec _maintenance.ResetIdentities
