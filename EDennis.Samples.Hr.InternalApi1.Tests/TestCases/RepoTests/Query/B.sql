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

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Query','Query',@TestCase,'Alpha', @alpha
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Query','Query',@TestCase,'Expected', @expected

exec  _.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Query','Query',@TestCase
