use AgencyInvestigatorCheck;
begin transaction
declare @Id int = 1;
--declare @Input varchar(max) = (
--select 1 EmployeeId, '2018-12-01' DateCompleted, 'Pass' Status
--	for json path, without_array_wrapper
--);
--insert into AgencyInvestigatorCheck(EmployeeId,DateCompleted,Status) 
--	values (1,'2018-12-01','Pass');
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
		left outer join
		(select top 1 DateCompleted, Status 
			from AgencyInvestigatorCheck..AgencyInvestigatorCheck 
			where EmployeeId = @Id
			order by DateCompleted desc) a
			on 1=1
		left outer join 
		(select top 1 DateCompleted, Status 
			from AgencyOnlineCheck..AgencyOnlineCheck
			where EmployeeId = @Id
			order by DateCompleted desc) b
			on 1=1
		left outer join 
		(select top 1 DateCompleted, Status 
			from FederalBackgroundCheck..FederalBackgroundCheck 
			where EmployeeId = @Id
			order by DateCompleted desc) c
			on 1=1
		left outer join
		(select top 1 DateCompleted, Status 
			from StateBackgroundCheck..StateBackgroundCheck 
			where EmployeeId = @Id
			order by DateCompleted desc) d
			on 1=1
		for json path);

select @Expected

rollback transaction
--exec _maintenance.ResetIdentities;
--exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','MultitierIntegrationTests_InMemory','1','Id',@Id
--exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','MultitierIntegrationTests_InMemory','1','Input',@Input
--exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','MultitierIntegrationTests_InMemory','1','Expected',@Expected

