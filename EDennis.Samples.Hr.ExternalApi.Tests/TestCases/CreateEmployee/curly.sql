use Hr;
begin transaction
declare @FirstName varchar(30) = 'Curly'
declare @Input varchar(max) = (
select @FirstName FirstName
	for json path, without_array_wrapper
);
insert into Employee(FirstName) 
	values (@FirstName);
declare @Expected varchar(max) = (
	select * from Employee
		for json path);

rollback transaction
exec _maintenance.ResetIdentities;
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateEmployee','IntegrationTests',@FirstName,'Input',@Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateEmployee','IntegrationTests',@FirstName,'Expected',@Expected

exec  _maintenance.GetTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateEmployee','IntegrationTests',@FirstName
