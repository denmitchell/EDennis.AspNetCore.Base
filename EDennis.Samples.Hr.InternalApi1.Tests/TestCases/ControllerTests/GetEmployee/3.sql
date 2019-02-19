use hr;

declare @Id int = 3
declare @Expected varchar(max) = 
(
	select * from Employee
	where Id = @Id
	for json path, include_null_values, without_array_wrapper
);

exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'GetEmployee','Get',@Id,'Id', @Id
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'GetEmployee','Get',@Id,'Expected', @Expected

select * from _maintenance.TestJson
	where ProjectName = 'EDennis.Samples.Hr.InternalApi1'
		and ClassName = 'EmployeeController'
		and MethodName = 'GetEmployee'
		and TestScenario = 'Get'
		and TestCase = @Id