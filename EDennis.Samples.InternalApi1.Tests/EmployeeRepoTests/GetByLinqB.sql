use hr;

declare @alpha varchar(30) = 'n'

declare 
	@expected varchar(max) = 
(
	select * from Employee
	where FirstName like '%' + @alpha + '%'
	for json path, include_null_values
);

exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinq','B','Alpha', @alpha
exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinq','B','Expected', @expected

--exec _maintenance.ResetIdentities
