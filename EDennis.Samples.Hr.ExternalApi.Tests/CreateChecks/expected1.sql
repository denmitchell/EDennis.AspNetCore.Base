use AgencyInvestigatorCheck;
begin transaction
declare @Id int = 1;
declare @Input varchar(max) = (
select 1 EmployeeId, '2018-12-01' DateCompleted, 'Pass' Status
	for json path, without_array_wrapper
);
insert into AgencyInvestigatorCheck(EmployeeId,DateCompleted,Status) 
	values (1,'2018-12-01','Pass');
declare @Expected varchar(max) = (
	select 
		(select top 1 DateCompleted, Status 
			from AgencyInvestigatorCheck..AgencyInvestigatorCheck 
			where EmployeeId = @Id
			order by SysStart desc
			for json path) AgencyInvestigatorCheck,
		(select top 1 DateCompleted, Status 
			from AgencyOnlineCheck..AgencyOnlineCheck
			where EmployeeId = @Id
			order by SysStart desc
			for json path) AgencyOnlineCheck,
		(select top 1 DateCompleted, Status 
			from FederalBackroundCheck..FederalBackroundCheck 
			where EmployeeId = @Id
			order by SysStart desc
			for json path) FederalBackroundCheck,
		(select top 1 DateCompleted, Status 
			from StateBackroundCheck..StateBackroundCheck 
			where EmployeeId = @Id
			order by SysStart desc
			for json path) StateBackroundCheck);
rollback transaction
exec _maintenance.ResetIdentities;
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','MultitierIntegrationTests_InMemory','1','Id',@Id
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','MultitierIntegrationTests_InMemory','1','Input',@Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','MultitierIntegrationTests_InMemory','1','Expected',@Expected