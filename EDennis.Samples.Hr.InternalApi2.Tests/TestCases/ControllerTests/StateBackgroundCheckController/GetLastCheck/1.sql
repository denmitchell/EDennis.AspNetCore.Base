use StateBackgroundCheck;

declare @EmployeeId int = 1
declare 
	@Input varchar(max) = 
( 
	select
	@EmployeeId EmployeeId,
	'Fail' Status,
	'2018-12-01' DateCompleted
		for json path, include_null_values, without_array_wrapper
);

begin transaction
insert into StateBackgroundCheck(EmployeeId, Status, DateCompleted)
	select
	@EmployeeId EmployeeId,
	'Fail' Status,
	'2018-12-01' DateCompleted

declare 
@Expected varchar(max) = 
(
	select top 1 * 
	from StateBackgroundCheck
	where EmployeeId = @EmployeeId
	order by DateCompleted desc
	for json path, include_null_values
);

rollback transaction
exec _maintenance.ResetIdentities

exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'StateBackgroundController', 'GetLastCheck', 'PostAndGet', @EmployeeId, 'Input', @Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'StateBackgroundController', 'GetLastCheck', 'PostAndGet', @EmployeeId, 'EmployeeId', @EmployeeId
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'StateBackgroundController', 'GetLastCheck', 'PostAndGet', @EmployeeId, 'Expected', @Expected

exec  _maintenance.GetTestJson 'EDennis.Samples.Hr.InternalApi2', 'StateBackgroundController', 'GetLastCheck', 'PostAndGet', @EmployeeId
		