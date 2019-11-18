use hr;

declare @FirstName varchar(30) = 'Monty'

declare @Expected varchar(max) = 
(
	select * from Employee
	where FirstName = @FirstName
	for json path, include_null_values
);

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetFromSql','GetFromSql',@FirstName,'FirstName', @FirstName
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetFromSql','GetFromSql',@FirstName,'Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetFromSql','GetFromSql',@FirstName
