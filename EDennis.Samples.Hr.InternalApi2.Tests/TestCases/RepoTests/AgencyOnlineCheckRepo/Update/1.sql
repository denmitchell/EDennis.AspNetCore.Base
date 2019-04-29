use AgencyOnlineCheck;

declare @Id int = 1
declare @EmployeeId int = 1
declare @DateCompleted date = '2018-01-02'
declare @Status varchar(15) = 'Fail'
declare @SysUser varchar(255) = 'jill@hill.org'

declare 
	@Input varchar(max) = 
( 
	select
	@Id Id,
	@EmployeeId EmployeeId,
	@DateCompleted DateCompleted,	
	@Status Status,
	@SysUser SysUser
		for json path, include_null_values, without_array_wrapper
);

begin transaction
update AgencyOnlineCheck
	set DateCompleted = @DateCompleted, Status = @Status, SysUser = @SysUser
	where Id = @Id

declare 
@Expected varchar(max) = 
(
	select * from AgencyOnlineCheck
	for json path, include_null_values
);

rollback transaction
exec _.ResetIdentities

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyOnlineRepo', 'Update', 'UpdateAndGetMultiple',@Id,'Input', @Input
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyOnlineRepo', 'Update', 'UpdateAndGetMultiple',@Id,'Id', @Id
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyOnlineRepo', 'Update', 'UpdateAndGetMultiple',@Id,'Expected', @Expected

exec _.GetTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyOnlineRepo', 'Update', 'UpdateAndGetMultiple', @Id
		