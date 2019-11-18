use Hr;
declare @Id int = 1;
declare @Expected varchar(max) = (
	select * from Employee
	where Id = @Id
	for json path, include_null_values, without_array_wrapper);

exec _.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','GetEmployee','IntegrationTests',@Id,'Id',@Id
exec _.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','GetEmployee','IntegrationTests',@Id,'Expected',@Expected

exec  _.GetTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','GetEmployee','IntegrationTests',@Id
