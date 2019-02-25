use hr;

declare @FirstName varchar(30) = 'Curly'

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
	where Id = @Id
	for json path, include_null_values, without_array_wrapper
);

rollback transaction
exec _.ResetIdentities

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGet',@FirstName,'Id', @Id
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGet',@FirstName,'Input', @Input
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGet',@FirstName,'Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGet',@FirstName
