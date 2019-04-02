use hr;

declare @firstName varchar(30) = 'Curly'
declare @Input varchar(max) = 
( 
	select @firstName FirstName
	for json path, include_null_values, without_array_wrapper
);


begin transaction
insert into Employee(FirstName,SysStart,SysEnd) 
	values (@FirstName,'2018-01-01',_.MaxDateTime2());

declare @Expected varchar(max) = 
(
	select * from Employee
	for json path, include_null_values
);
rollback transaction
exec _.ResetIdentities

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'CreateEmployee','CreateAndGetAll',@firstName,'Input', @input
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'CreateEmployee','CreateAndGetAll',@firstName,'Expected', @expected

exec _.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'CreateEmployee','CreateAndGetAll',@firstName
