--use ColorDb; --omit for testing purposes
--set identity_insert some_other_table off
declare @end datetime2 = _.MaxDateTime2()
declare @jack varchar(255) = 'jack@hill.org'
declare @jill varchar(255) = 'jill@hill.org'
insert into Color(Id, SysStart, Name, SysEnd, SysUser)
	values 
		(-99901,'2018-01-01','black',@end,@jack),
		(-99902,'2018-02-02','white',@end,@jack),
		(-99903,'2018-03-03','gray',@end,@jack),
		(-99904,'2018-04-04','red',@end,@jill),
		(-99905,'2018-05-05','green',@end,@jill),
		(-99906,'2018-06-06','blue',@end,@jill);
