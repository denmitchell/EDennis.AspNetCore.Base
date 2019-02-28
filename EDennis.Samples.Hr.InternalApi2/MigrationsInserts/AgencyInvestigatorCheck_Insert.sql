--set identity_insert some_other_table off
declare @end datetime2 = _.MaxDateTime2()
declare @jack varchar(255) = 'jack@hill.org'
declare @jill varchar(255) = 'jill@hill.org'

set identity_insert AgencyInvestigatorCheck on
insert into AgencyInvestigatorCheck(Id, SysStart, EmployeeId, DateCompleted, Status, SysEnd, SysUser)
	values 
	(1,'2018-01-01',1,'2018-01-01','Pass',@end,@jack),
	(2,'2018-02-02',2,'2018-02-02','Pass',@end,@jack),
	(3,'2018-03-03',3,'2018-03-03','Fail',@end,@jill),
	(4,'2018-04-04',4,'2018-04-04','Pass',@end,@jill);
set identity_insert AgencyInvestigatorCheck off