use hr;

declare @alpha varchar(30) = 'o'

declare 
	@expected varchar(max) = 
(
	select * from Employee
	where FirstName like '%' + @alpha + '%'
	for json path, include_null_values
);

exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinq','A','Alpha', @alpha
exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinq','A','Expected', @expected

--exec _maintenance.ResetIdentities
