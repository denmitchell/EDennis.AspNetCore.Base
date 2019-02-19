use hr;

declare @Id int = 1

declare 
	@expected varchar(max) = 
(
	select * from Employee
	where id = @Id
	for json path, include_null_values, without_array_wrapper
);

exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetById','GetById','A','Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetById','GetById','A','Expected', @expected

--exec _maintenance.ResetIdentities
