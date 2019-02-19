use hr;

declare @Id int = 1
declare @Expected varchar(max) = 
(
	select * from Employee
	where Id = @Id
	for json path, include_null_values, without_array_wrapper
);

exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'GetEmployee', 'Get', @Id,'Id', @Id
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'GetEmployee', 'Get', @Id,'Expected', @Expected

exec  _maintenance.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'GetEmployee', 'Get', @Id
