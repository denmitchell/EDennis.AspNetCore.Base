use AgencyInvestigatorCheck;
begin transaction
declare @Id int = 2;
declare @Input varchar(max) = (
select 2 EmployeeId, '2018-12-02' DateCompleted, 'Fail' Status
	for json path, without_array_wrapper
);
insert into AgencyInvestigatorCheck(EmployeeId,DateCompleted,Status) 
	values (2,'2018-12-02','Fail');
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

select @Expected

rollback transaction
exec _maintenance.ResetIdentities;
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','IntegrationTests','2','Id',@Id
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','IntegrationTests','2','Input',@Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','IntegrationTests','2','Expected',@Expected

