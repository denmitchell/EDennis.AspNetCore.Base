use hr;

declare @FirstName varchar(30) = 'Bob'

declare 
	@expected varchar(max) = 
(
	select * from Employee
	where FirstName = @FirstName
	for json path, include_null_values
);

exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetFromSql','GetFromSql',@FirstName,'FirstName', @FirstName
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetFromSql','GetFromSql',@FirstName,'Expected', @expected

exec  _maintenance.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetFromSql','GetFromSql',@FirstName

