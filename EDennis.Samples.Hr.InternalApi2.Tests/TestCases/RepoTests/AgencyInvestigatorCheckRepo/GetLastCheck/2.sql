use AgencyInvestigatorCheck;

declare @EmployeeId int = 2
declare 
	@Input varchar(max) = 
( 
	select
	@EmployeeId EmployeeId,
	'Fail' Status,
	'2018-12-02' DateCompleted
		for json path, include_null_values, without_array_wrapper
);

begin transaction
insert into AgencyInvestigatorCheck(EmployeeId, Status, DateCompleted)
	select
	@EmployeeId EmployeeId,
	'Fail' Status,
	'2018-12-02' DateCompleted

declare 
@Expected varchar(max) = 
(
	select top 1 * 
	from AgencyInvestigatorCheck
	where EmployeeId = @EmployeeId
	order by DateCompleted desc
	for json path, include_null_values
);

rollback transaction
exec _maintenance.ResetIdentities

exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyInvestigatorRepo', 'GetLastCheck', 'CreateAndGet', @EmployeeId, 'Input', @Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyInvestigatorRepo', 'GetLastCheck', 'CreateAndGet', @EmployeeId, 'EmployeeId', @EmployeeId
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyInvestigatorRepo', 'GetLastCheck', 'CreateAndGet', @EmployeeId, 'Expected', @Expected

exec  _maintenance.GetTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyInvestigatorRepo', 'GetLastCheck', 'CreateAndGet', @EmployeeId
		