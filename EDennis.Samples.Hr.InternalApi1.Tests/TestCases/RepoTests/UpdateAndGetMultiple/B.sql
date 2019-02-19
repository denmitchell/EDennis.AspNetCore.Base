use hr;

declare @TestCase varchar(1) = 'B'
declare @Id int = 1
declare @FirstName varchar(30) = 'Curly'

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
	set FirstName = @FirstName
	where Id = @Id

declare @Expected varchar(max) = 
(
	select * from Employee
	for json path, include_null_values
);

rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Update','UpdateAndGetMultiple',@TestCase,'Id', @Id
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Update','UpdateAndGetMultiple',@TestCase,'Input', @Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Update','UpdateAndGetMultiple',@TestCase,'Expected', @Expected

exec  _maintenance.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Update','UpdateAndGetMultiple',@TestCase
