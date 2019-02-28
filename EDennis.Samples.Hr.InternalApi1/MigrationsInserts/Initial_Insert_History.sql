--use Hr; --omit for testing purposes
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

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Rob', _.RightBefore(SysStart),
			case SysUser when @jill then @jack else @jill end, SysUser
		from (select top 1 * from dbo_history.Employee where Id = @id order by SysStart asc) e

set @id = 2
insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Montgomery', _.RightBefore(SysStart),
		case SysUser when @jill then @jack else @jill end, SysUser
		from dbo.Employee
		where Id = @id

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Gomer', _.RightBefore(SysStart),
			case SysUser when @jill then @jack else @jill end, SysUser
		from (select top 1 * from dbo_history.Employee where Id = @id order by SysStart asc) e


set @id = 3

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Andrew', _.RightBefore(SysStart),
		case SysUser when @jill then @jack else @jill end, SysUser
		from dbo.Employee
		where Id = @id

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Andy', _.RightBefore(SysStart),
			case SysUser when @jill then @jack else @jill end, SysUser
		from (select top 1 * from dbo_history.Employee where Id = @id order by SysStart asc) e


--no history for Id = 4


set @id = 5 --delete

insert into dbo_history.Employee(Id, SysStart, FirstName, SysEnd, SysUser, SysUserNext)
	values (@id, '2017-05-05', 'Wink', '2018-12-31T11:59:59', @jill, @jack)



-- POSITION HISTORY

set @id = 1

insert into dbo_history.Position(Id, SysStart, Title, IsManager, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Game Show Mgr.', 1, _.RightBefore(SysStart),
		case SysUser when @jill then @jack else @jill end, SysUser
		from dbo.Position
		where Id = @id

insert into dbo_history.Position(Id, SysStart, Title, IsManager, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Manager', 1, _.RightBefore(SysStart),
			case SysUser when @jill then @jack else @jill end, SysUser
		from (select top 1 * from dbo_history.Position where Id = @id order by SysStart asc) e


set @id = 2

insert into dbo_history.Position(Id, SysStart, Title, IsManager, SysEnd, SysUser, SysUserNext)
	select Id, DateAdd(day,-1,SysStart), 'Host', 0, _.RightBefore(SysStart),
		case SysUser when @jill then @jack else @jill end, SysUser
		from dbo.Position
		where Id = @id


set @id = 3 -- delete

insert into dbo_history.Position(Id, SysStart, Title, IsManager, SysEnd, SysUser, SysUserNext)
	values (@id, '2017-03-03', 'Contestant', 0, '2018-12-31T11:59:59',@jill,@jack)


-- UPDATED ASSOCIATION

insert into dbo_history.EmployeePosition(EmployeeId, PositionId, SysStart, SysEnd, SysUser, SysUserNext)
	values(1,2,'2014-01-01',_.RightBefore('2018-01-01'),@jill,@jack) 


-- DELETED ASSOCIATION

insert into dbo_history.EmployeePosition(EmployeeId, PositionId, SysStart, SysEnd, SysUser, SysUserNext)
	values(2,2,'2015-02-02',_.RightBefore('2018-02-02'),@jill,@jack) 



