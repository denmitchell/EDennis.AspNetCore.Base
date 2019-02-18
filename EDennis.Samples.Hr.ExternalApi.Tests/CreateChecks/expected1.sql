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
		a.Status as [AgencyInvestigatorCheck.Status]
		from 
		(select EmployeeId from AgencyInvestigatorCheck
		union select EmployeeId from AgencyOnlineCheck
		union select EmployeeId from FederalBackgroundCheck
		union select EmployeeId from StateBackgroundCheck) emps
		left outer join
		(select top 1 DateCompleted, Status 
			from AgencyInvestigatorCheck..AgencyInvestigatorCheck 
			where EmployeeId = @Id
			order by DateCompleted desc
			for json path, without_array_wrapper) a
		left outer join 
		(select top 1 DateCompleted, Status 
			from AgencyOnlineCheck..AgencyOnlineCheck
			where EmployeeId = @Id
			order by DateCompleted desc
			for json path, without_array_wrapper) AgencyOnlineCheck,
		(select top 1 DateCompleted, Status 
			from FederalBackgroundCheck..FederalBackgroundCheck 
			where EmployeeId = @Id
			order by DateCompleted desc
			for json path, without_array_wrapper) FederalBackroundCheck,
		(select top 1 DateCompleted, Status 
			from StateBackgroundCheck..StateBackgroundCheck 
			where EmployeeId = @Id
			order by DateCompleted desc
			for json path, without_array_wrapper) StateBackroundCheck
		for json path);

select @Expected

rollback transaction
--exec _maintenance.ResetIdentities;
--exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','MultitierIntegrationTests_InMemory','1','Id',@Id
--exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','MultitierIntegrationTests_InMemory','1','Input',@Input
--exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','MultitierIntegrationTests_InMemory','1','Expected',@Expected

