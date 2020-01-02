declare @jack varchar(255) = 'jack@hill.org'
declare @jill varchar(255) = 'jill@hill.org'

insert into dbo_history.Color(Id,SysStart,Name,SysEnd,SysUser,SysUserNext)
	select Id,'2017-02-02','pearl',_.RightBefore(SysStart),@jill,@jack
		from dbo.Color
		where Id = -999002
	
