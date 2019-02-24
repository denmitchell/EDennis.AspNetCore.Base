--use ColorDb; --omit for testing purposes
--set identity_insert some_other_table off
set identity_insert Colors on
declare @end datetime2 = _.MaxDateTime2()
declare @jack varchar(255) = 'jack@hill.org'
declare @jill varchar(255) = 'jill@hill.org'
insert into Colors(Id, SysStart, Name, SysEnd, SysUser)
	values 
		(1,'2018-01-01','black',@end,@jack),
		(2,'2018-02-02','white',@end,@jack),
		(3,'2018-03-03','gray',@end,@jack),
		(4,'2018-04-04','red',@end,@jill),
		(5,'2018-05-05','green',@end,@jill),
		(6,'2018-06-06','blue',@end,@jill);
set identity_insert colors off
