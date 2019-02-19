use hr;

declare @TestCase varchar(1) = 'B'
declare @alpha varchar(30) = 'n'

declare 
	@expected varchar(max) = 
(
	select * from Employee
	where FirstName like '%' + @alpha + '%'
	for json path, include_null_values
);

exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Query','Query',@TestCase,'Alpha', @alpha
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Query','Query',@TestCase,'Expected', @expected

exec  _maintenance.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Query','Query',@TestCase
