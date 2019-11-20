use StateBackgroundCheck;

declare @EmployeeId int = 1

declare 
@Expected varchar(max) = 
(
	select top 1 * 
	from StateBackgroundCheck
	where EmployeeId = @EmployeeId
	order by DateCompleted desc
	for json path, include_null_values, without_array_wrapper
);


exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'StateBackgroundCheckRepo', 'GetLastCheck', 'CreateAndGet', @EmployeeId, 'Input', @EmployeeId
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'StateBackgroundCheckRepo', 'GetLastCheck', 'CreateAndGet', @EmployeeId, 'EmployeeId', @EmployeeId
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'StateBackgroundCheckRepo', 'GetLastCheck', 'CreateAndGet', @EmployeeId, 'Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Hr.InternalApi2', 'StateBackgroundCheckRepo', 'GetLastCheck', 'CreateAndGet', @EmployeeId
		