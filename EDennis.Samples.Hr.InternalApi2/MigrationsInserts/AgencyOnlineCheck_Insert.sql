--set identity_insert some_other_table off
declare @end datetime2 = _.MaxDateTime2()
declare @jack varchar(255) = 'jack@hill.org'
declare @jill varchar(255) = 'jill@hill.org'

set identity_insert AgencyOnlineCheck on
insert into AgencyOnlineCheck(Id, EmployeeId, DateCompleted, Status, SysUser)
	values 
	(1,1,'2018-01-01','Pass',@jack),
	(2,2,'2018-02-02','Pass',@jack),
	(3,3,'2018-03-03','Fail',@jill),
	(4,4,'2018-04-04','Pass',@jill);
set identity_insert AgencyOnlineCheck off