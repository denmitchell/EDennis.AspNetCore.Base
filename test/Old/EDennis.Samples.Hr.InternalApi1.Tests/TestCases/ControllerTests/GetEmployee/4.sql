use hr;

declare @Id int = 4
declare @Expected varchar(max) = 
(
	select * from Employee
	where Id = @Id
	for json path, include_null_values, without_array_wrapper
);

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'GetEmployee', 'Get', @Id, 'Id', @Id
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'GetEmployee', 'Get', @Id, 'Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'GetEmployee', 'Get', @Id
