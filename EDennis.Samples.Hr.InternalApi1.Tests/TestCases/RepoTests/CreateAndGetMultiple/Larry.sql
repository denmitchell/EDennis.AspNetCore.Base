use hr;

declare @FirstName varchar(30) = 'Larry'

declare @Id int
select @Id = max(id) + 1 from Employee

declare @Input varchar(max) = 
( 
	select @FirstName FirstName
	for json path, include_null_values, without_array_wrapper
);

begin transaction
insert into Employee(FirstName)
	select @FirstName;

declare @Expected varchar(max) = 
(
	select * from Employee
	for json path, include_null_values
);

rollback transaction
exec _maintenance.ResetIdentities

exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGetMultiple',@FirstName,'Id', @Id
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGetMultiple',@FirstName,'Input', @Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGetMultiple',@FirstName,'Expected', @Expected

exec  _maintenance.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGetMultiple',@FirstName

