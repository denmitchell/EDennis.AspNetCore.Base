use todo;

declare @id int = 2

begin transaction
delete from Task where Id = @id;

declare 
	@expected varchar(max) = 
(
	select * from Task
	for json path, include_null_values
);

rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Delete','DeleteAndGetMultiple','B','Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Delete','DeleteAndGetMultiple','B','Expected', @expected

--exec _maintenance.ResetIdentities