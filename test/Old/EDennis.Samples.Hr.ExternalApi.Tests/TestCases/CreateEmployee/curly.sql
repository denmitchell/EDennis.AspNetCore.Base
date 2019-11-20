use Hr;
declare @Id int;
declare @FirstName varchar(30) = 'Curly'
declare @SysStart datetime2 = '2019-01-01'
declare @SysEnd datetime2 = _.MaxDateTime2()
declare @SysUser varchar(255) = 'joe@hill.org'
declare @Input varchar(max) = (
select @SysStart SysStart, @FirstName FirstName, @SysEnd SysEnd, @SysUser SysUser
	for json path, without_array_wrapper
);

begin transaction
insert into Employee(SysStart,FirstName,SysEnd,SysUser) 
	values (@SysStart,@FirstName,@SysEnd,@SysUser);

select top 1 @Id = Id from Employee order by Id desc;

declare @Expected varchar(max) = (
	select * from Employee
	where Id = @Id
	for json path, include_null_values, without_array_wrapper);

rollback transaction
exec _.ResetIdentities;

exec _.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateEmployee','IntegrationTests',@FirstName,'Input',@Input
exec _.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateEmployee','IntegrationTests',@FirstName,'Id',@Id
exec _.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateEmployee','IntegrationTests',@FirstName,'Expected',@Expected

exec  _.GetTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateEmployee','IntegrationTests',@FirstName
