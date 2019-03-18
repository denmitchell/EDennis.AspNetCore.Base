use hr;

declare @FirstName varchar(30) = 'Larry'
declare @User varchar(30) = 'moe@stooges.org'

declare @Id int
select @Id = max(id) + 1 from Employee

declare @Input varchar(max) = 
( 
	select @FirstName FirstName
	for json path, include_null_values, without_array_wrapper
);

begin transaction
insert into Employee(FirstName,SysStart,SysEnd,SysUser) 
	values (@FirstName,'2018-01-01',_.MaxDateTime2(),@User);

declare @Expected varchar(max) = 
(
	select * from Employee
	for json path, include_null_values
);

rollback transaction
exec _.ResetIdentities

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGetMultiple',@FirstName,'Id', @Id
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGetMultiple',@FirstName,'Input', @Input
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGetMultiple',@FirstName,'Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGetMultiple',@FirstName

