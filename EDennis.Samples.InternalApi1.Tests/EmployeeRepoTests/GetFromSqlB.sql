use hr;

declare @FirstName varchar(30) = 'Monty'

declare 
	@expected varchar(max) = 
(
	select * from Employee
	where FirstName = @FirstName
	for json path, include_null_values
);

exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'GetFromSql','GetFromSql','B','FirstName', @FirstName
exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'GetFromSql','GetFromSql','B','Expected', @expected

--exec _maintenance.ResetIdentities
