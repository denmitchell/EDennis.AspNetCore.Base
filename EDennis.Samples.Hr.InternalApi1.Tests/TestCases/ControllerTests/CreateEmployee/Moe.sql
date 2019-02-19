use hr;

declare @firstName varchar(30) = 'Moe'
declare @Input varchar(max) = 
( 
	select @firstName FirstName
	for json path, include_null_values, without_array_wrapper
);


begin transaction
insert into Employee(FirstName)
	values (@firstName);

declare @Expected varchar(max) = 
(
	select * from Employee
	for json path, include_null_values
);
rollback transaction
exec _maintenance.ResetIdentities

exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'CreateEmployee','CreateAndGetAll',@firstName,'Input', @input
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'CreateEmployee','CreateAndGetAll',@firstName,'Expected', @expected

exec _maintenance.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'CreateEmployee','CreateAndGetAll',@firstName
