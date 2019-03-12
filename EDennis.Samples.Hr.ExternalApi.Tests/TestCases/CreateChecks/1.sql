use AgencyInvestigatorCheck;
begin transaction
declare @Id int = 1;
declare @Input varchar(max) = (
select 1 EmployeeId, '2018-12-01' DateCompleted, 'Fail' Status
	for json path, without_array_wrapper
);
insert into AgencyInvestigatorCheck(
	EmployeeId,SysStart,DateCompleted,Status,SysEnd,SysUser) 
	values (1,'2018-12-01','2018-12-01','Fail',_.MaxDateTime2(),'moe@tester.org');
declare @Expected varchar(max) = (
	select 
		a.DateCompleted as [AgencyInvestigatorCheck.DateCompleted],
		a.Status as [AgencyInvestigatorCheck.Status],
		b.DateCompleted as [AgencyOnlineCheck.DateCompleted],
		b.Status as [AgencyOnlineCheck.Status],
		c.DateCompleted as [FederalBackgroundCheck.DateCompleted],
		c.Status as [FederalBackgroundCheck.Status],
		d.DateCompleted as [StateBackgroundCheck.DateCompleted],
		d.Status as [StateBackgroundCheck.Status]
		from 
		(select @Id EmployeeId) emps
		cross join
		(select top 1 DateCompleted, Status 
			from AgencyInvestigatorCheck..AgencyInvestigatorCheck 
			where EmployeeId = @Id
			order by DateCompleted desc) a
		cross join 
		(select top 1 DateCompleted, Status 
			from AgencyOnlineCheck..AgencyOnlineCheck
			where EmployeeId = @Id
			order by DateCompleted desc) b
		cross join 
		(select top 1 DateCompleted, Status 
			from FederalBackgroundCheck..FederalBackgroundCheck 
			where EmployeeId = @Id
			order by DateCompleted desc) c
		cross join 
		(select top 1 DateCompleted, Status 
			from StateBackgroundCheck..StateBackgroundCheck 
			where EmployeeId = @Id
			order by DateCompleted desc) d
		for json path);

rollback transaction
exec _.ResetIdentities;
exec _.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','IntegrationTests',@Id,'Id',@Id
exec _.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','IntegrationTests',@Id,'Input',@Input
exec _.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','IntegrationTests',@Id,'Expected',@Expected

exec  _.GetTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','IntegrationTests',@Id
