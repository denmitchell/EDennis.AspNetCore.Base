use todo;

declare @id int = 1

begin transaction
delete from Task where Id = @id;

declare 
	@expected varchar(max) = 
(
	select * from Task
	for json path, include_null_values
);

rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Delete','DeleteAndGetMultiple','A','Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Delete','DeleteAndGetMultiple','A','Expected', @expected

--exec _maintenance.ResetIdentities