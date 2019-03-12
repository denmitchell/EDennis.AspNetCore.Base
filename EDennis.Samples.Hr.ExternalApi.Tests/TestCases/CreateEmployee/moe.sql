use Hr;
begin transaction
declare @FirstName varchar(30) = 'Moe'
declare @Input varchar(max) = (
select @FirstName FirstName
	for json path, without_array_wrapper
);
insert into Employee(FirstName,SysStart,SysEnd) 
	values (@FirstName,'2018-01-01',_.MaxDateTime2());
declare @Expected varchar(max) = (
	select * from Employee
		for json path);

select @Expected

rollback transaction
exec _.ResetIdentities;
exec _.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateEmployee','IntegrationTests',@FirstName,'Input',@Input
exec _.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateEmployee','IntegrationTests',@FirstName,'Expected',@Expected

exec  _.GetTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateEmployee','IntegrationTests',@FirstName
