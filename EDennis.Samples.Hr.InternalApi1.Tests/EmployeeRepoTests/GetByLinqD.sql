use hr;

declare @alpha varchar(30) = 'e'

declare 
	@expected varchar(max) = 
(
	select * from Employee
	where FirstName like '%' + @alpha + '%'
	for json path, include_null_values
);

exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinq','D','Alpha', @alpha
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinq','D','Expected', @expected

--exec _maintenance.ResetIdentities
