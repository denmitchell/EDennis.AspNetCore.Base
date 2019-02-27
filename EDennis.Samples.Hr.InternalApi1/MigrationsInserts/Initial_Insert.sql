--use ColorDb; --omit for testing purposes
--set identity_insert some_other_table off
declare @end datetime2 = _.MaxDateTime2()
declare @jack varchar(255) = 'jack@hill.org'
declare @jill varchar(255) = 'jill@hill.org'

set identity_insert Employee on
insert into Employee(Id, SysStart, FirstName, SysEnd, SysUser)
	values 
	(1,'2018-01-01','Bob',@end,@jack),
	(2,'2018-02-02','Monty',@end,@jill),
	(3,'2018-03-03','Drew',@end,@jack),
	(4,'2018-04-04','Wayne',@end,@jill);
set identity_insert Employee off

set identity_insert Position on
insert into Position(Id, SysStart, Title, IsManager, SysEnd, SysUser)
	values 
	(1,'2018-01-01','Game Show Manager', 1,@end,@jack),
	(2,'2018-02-02','Game Show Host', 0,@end,@jill);
set identity_insert Position off

insert into EmployeePosition(EmployeeId, PositionId, SysStart, SysEnd, SysUser)
	values 
	(1,1,'2018-01-01',@end,@jack),
	(2,1,'2018-02-02',@end,@jill),
	(3,2,'2018-03-03',@end,@jack),
	(4,2,'2018-04-04',@end,@jill);


