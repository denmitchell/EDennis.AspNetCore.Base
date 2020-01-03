--use ColorDb; --omit for testing purposes
--set identity_insert some_other_table off
declare @end datetime2 = _.MaxDateTime2()
declare @jack varchar(255) = 'jack@hill.org'
declare @jill varchar(255) = 'jill@hill.org'
insert into Color(Id, SysStart, Name, SysEnd, SysUser)
	values 
		(-999001,'2018-01-01','black',@end,@jack),
		(-999002,'2018-02-02','white',@end,@jack),
		(-999003,'2018-03-03','gray',@end,@jack)