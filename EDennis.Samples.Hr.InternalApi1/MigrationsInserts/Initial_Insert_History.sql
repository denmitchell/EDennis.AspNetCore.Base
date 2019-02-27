--use ColorDb; --omit for testing purposes
--set identity_insert some_other_table off
declare @jack varchar(255) = 'jack@hill.org'
declare @jill varchar(255) = 'jill@hill.org'
declare @id int = 1


-- EMPLOYEE HISTORY

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Bobby', _.RightBefore(SysStart),
		case SysUser when @jill then @jack else @jill end, SysUser
		from dbo.Employee
		where Id = @id

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Robby', _.RightBefore(SysStart),
			case SysUser when @jill then @jack else @jill end, SysUser
		from (select top 1 * from dbo_history.Employee where Id = @id order by SysStart asc) e
		where Id = @id and ord = 1

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Rob', _.RightBefore(SysStart),
			case SysUser when @jill then @jack else @jill end, SysUser
		from (select *, row_number() over (partition by id order by SysStart asc) ord
			from dbo_Employeedbo_history.Employee) e
		where Id = @id and ord = 1

set @id = 2
insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Montgomery', _.RightBefore(SysStart),
		case SysUser when @jill then @jack else @jill end, SysUser
		from dbo.Employee
		where Id = @id

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Gomer', _.RightBefore(SysStart),
			case SysUser when @jill then @jack else @jill end, SysUser
		from (select *, row_number() over (partition by id order by SysStart asc) ord
			from dbo_Employeedbo_history.Employee) e
		where Id = @id and ord = 1

set @id = 3

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Andrew', _.RightBefore(SysStart),
		case SysUser when @jill then @jack else @jill end, SysUser
		from dbo.Employee
		where Id = @id

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Andy', _.RightBefore(SysStart),
			case SysUser when @jill then @jack else @jill end, SysUser
		from (select *, row_number() over (partition by id order by SysStart asc) ord
			from dbo_Employeedbo_history.Employee) e
		where Id = @id and ord = 1

--no history for Id = 4


-- POSITION HISTORY

set @id = 1

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Andrew', _.RightBefore(SysStart),
		case SysUser when @jill then @jack else @jill end, SysUser
		from dbo.Employee
		where Id = @id

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Andy', _.RightBefore(SysStart),
			case SysUser when @jill then @jack else @jill end, SysUser
		from (select *, row_number() over (partition by id order by SysStart asc) ord
			from dbo_Employeedbo_history.Employee) e
		where Id = @id and ord = 1



insert into dbo_history.Position(Id, SysStart, Title, IsManager, SysStart, SysEnd, SysUser, SysUserNext)
	select 
	(1,'2018-01-01','Game Show Manager', 1,@end,@jack),
	(2,'2018-02-02','Game Show Host', 0,@end,@jill);

insert into dbo_history.EmployeePosition(EmployeeId, PositionId, SysStart, SysEnd, SysUser, SysUserNext)
	select 
	(1,1,'2018-01-01',@end,@jack),
	(2,1,'2018-02-02',@end,@jill),
	(3,2,'2018-03-03',@end,@jack),
	(4,2,'2018-04-04',@end,@jill);


