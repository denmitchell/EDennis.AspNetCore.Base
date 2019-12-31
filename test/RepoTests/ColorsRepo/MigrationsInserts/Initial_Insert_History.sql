declare @jack varchar(255) = 'jack@hill.org'
declare @jill varchar(255) = 'jill@hill.org'

insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2017-01-01','beige',_.RightBefore(SysStart),@jill,@jack
		from dbo.Color
		where Id = -99901
	
insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2016-01-01','brown',_.RightBefore(SysStart),@jack,@jill
		from dbo_history.Color
		where Id = -99901 and SysStart = '2017-01-01'

insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2015-01-01','tan',_.RightBefore(SysStart),@jill,@jack
		from dbo_history.Color
		where Id = -99901 and SysStart = '2016-01-01'

insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2017-02-02','pearl',_.RightBefore(SysStart),@jill,@jack
		from dbo.Color
		where Id = -99902
	
insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2016-02-02','ivory',_.RightBefore(SysStart),@jack,@jill
		from dbo_history.Color
		where Id = -99902 and SysStart = '2017-02-02'

insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2015-02-02','chiffon',_.RightBefore(SysStart),@jill,@jack
		from dbo_history.Color
		where Id = -99902 and SysStart = '2016-02-02'

insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2017-03-03','slate',_.RightBefore(SysStart),@jill,@jack
		from dbo.Color
		where Id = -99903
	
insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2016-03-03','charcoal',_.RightBefore(SysStart),@jack,@jill
		from dbo_history.Color
		where Id = -99903 and SysStart = '2017-03-03'

insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2015-03-03','pewter',_.RightBefore(SysStart),@jill,@jack
		from dbo_history.Color
		where Id = -99903 and SysStart = '2016-03-03'

insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2014-03-03','smoke',_.RightBefore(SysStart),@jack,@jill
		from dbo_history.Color
		where Id = -99903 and SysStart = '2015-03-03'

insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2017-04-04','crimson',_.RightBefore(SysStart),@jack,@jill
		from dbo.Color
		where Id = -99904

insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2017-05-05','chartreuse',_.RightBefore(SysStart),@jack,@jill
		from dbo.Color
		where Id = -99905
	
insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2016-05-05','sage',_.RightBefore(SysStart),@jill,@jack
		from dbo_history.Color
		where Id = -99905 and SysStart = '2017-05-05'
