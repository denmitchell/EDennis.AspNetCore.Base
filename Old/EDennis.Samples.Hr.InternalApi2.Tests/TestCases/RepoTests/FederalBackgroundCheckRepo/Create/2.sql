use FederalBackgroundCheck;

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
insert into FederalBackgroundCheck(
	EmployeeId, 
	SysStart, SysEnd, SysUser,
	Status, DateCompleted)
	select
	@EmployeeId EmployeeId,
	'2018-01-01',_.MaxDateTime2(),'moe@tester.org',
	'Fail' Status,
	'2018-12-02' DateCompleted

declare 
@Expected varchar(max) = 
(
	select * from FederalBackgroundCheck
	for json path, include_null_values
);

rollback transaction
exec _.ResetIdentities

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'FederalBackgroundRepo', 'Create', 'CreateAndGetMultiple',@EmployeeId,'Input', @Input
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'FederalBackgroundRepo', 'Create', 'CreateAndGetMultiple',@EmployeeId,'Expected', @Expected

exec _.GetTestJson 'EDennis.Samples.Hr.InternalApi2', 'FederalBackgroundRepo', 'Create', 'CreateAndGetMultiple', @EmployeeId
		