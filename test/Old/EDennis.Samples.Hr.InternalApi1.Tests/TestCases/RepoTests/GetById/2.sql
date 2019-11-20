use hr;

declare @Id int = 2

declare 
	@expected varchar(max) = 
(
	select * from Employee
	where id = @Id
	for json path, include_null_values, without_array_wrapper
);

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetById','GetById',@Id,'Id', @Id
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetById','GetById',@Id,'Expected', @expected

exec  _.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetById','GetById',@Id
