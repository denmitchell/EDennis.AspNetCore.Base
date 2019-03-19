use AgencyInvestigatorCheck;

declare @EmployeeId int = 2

declare @Expected varchar(max) = 
(
	select top 1 * 
	from AgencyInvestigatorCheck
	where EmployeeId = @EmployeeId
	order by DateCompleted desc
	for json path, include_null_values, without_array_wrapper
);

declare @Input int = @EmployeeId

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyInvestigatorCheckRepo', 'GetLastCheck', 'CreateAndGet', @EmployeeId, 'EmployeeId', @EmployeeId
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyInvestigatorCheckRepo', 'GetLastCheck', 'CreateAndGet', @EmployeeId, 'Expected', @Expected
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyInvestigatorCheckRepo', 'GetLastCheck', 'CreateAndGet', @EmployeeId, 'Input', @Input

exec  _.GetTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyInvestigatorCheckRepo', 'GetLastCheck', 'CreateAndGet', @EmployeeId
		