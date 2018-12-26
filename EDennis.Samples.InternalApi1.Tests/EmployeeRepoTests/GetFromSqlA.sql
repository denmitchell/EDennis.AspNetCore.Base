use hr;

declare @FirstName varchar(30) = 'Bob'

declare 
	@expected varchar(max) = 
(
	select * from Employee
	where FirstName = @FirstName
	for json path, include_null_values
);

exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'GetFromSql','GetFromSql','A','FirstName', @FirstName
exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'GetFromSql','GetFromSql','A','Expected', @expected

--exec _maintenance.ResetIdentities
