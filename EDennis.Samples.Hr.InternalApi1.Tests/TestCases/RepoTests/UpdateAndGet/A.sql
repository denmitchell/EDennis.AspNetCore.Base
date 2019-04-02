use hr;

declare @TestCase varchar(1) = 'A'
declare @Id int = 1
declare @FirstName varchar(30) = 'Larry'
declare @User varchar(255) = 'tester@example.org'

declare 
	@Input varchar(max) = 
	( 
		select @Id Id, @FirstName FirstName 
		from Employee
		where Id = @Id
		for json path, include_null_values, without_array_wrapper
	);

begin transaction
update Employee
	set FirstName = @FirstName,
	SysUser = @User
	where Id = @Id

declare @Expected varchar(max) = 
(
	select * from Employee
	where Id = @Id
	for json path, include_null_values, without_array_wrapper
);

rollback transaction
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Update','UpdateAndGet',@TestCase,'Id', @Id
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Update','UpdateAndGet',@TestCase,'Input', @Input
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Update','UpdateAndGet',@TestCase,'Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Update','UpdateAndGet',@TestCase
