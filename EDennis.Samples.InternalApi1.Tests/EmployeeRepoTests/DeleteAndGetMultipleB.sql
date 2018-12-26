use hr;

declare @id int = 2

begin transaction
delete from EmployeePosition where EmployeeId = @id;
delete from Employee where Id = @id;

declare 
	@expected varchar(max) = 
(
	select * from Employee
	for json path, include_null_values
);

rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'Delete','DeleteAndGetMultiple','B','Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'Delete','DeleteAndGetMultiple','B','Expected', @expected

--exec _maintenance.ResetIdentities