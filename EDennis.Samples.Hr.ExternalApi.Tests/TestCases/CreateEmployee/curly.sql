use Hr;
begin transaction
declare @firstName varchar(30) = 'Curly'
declare @Input varchar(max) = (
select @firstName FirstName
	for json path, without_array_wrapper
);
insert into Employee(FirstName) 
	values (@firstName);
declare @Expected varchar(max) = (
	select * from Employee
		for json path);

select @Expected

rollback transaction
exec _maintenance.ResetIdentities;
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateEmployee','IntegrationTests',@firstName,'Input',@Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateEmployee','IntegrationTests',@firstName,'Expected',@Expected

