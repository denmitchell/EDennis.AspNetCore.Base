declare @jack varchar(255) = 'jack@hill.org'
declare @jill varchar(255) = 'jill@hill.org'

insert into AgencyInvestigatorCheck(Id, SysStart, EmployeeId, DateCompleted, Status, SysEnd, SysUser, SysUserNext)
	select Id,'2017-01-01',1,'2018-01-01','Fail',_.RightBefore(SysStart),@jill,@jack
		from AgencyInvestigatorCheck
		where Id = 1

insert into AgencyInvestigatorCheck(Id, SysStart, EmployeeId, DateCompleted, Status, SysEnd, SysUser, SysUserNext)
	select top 1 Id,'2016-01-01',1,'2018-01-01','Pass',_.RightBefore(SysStart),@jack,@jill
		from dbo_history.AgencyInvestigatorCheck
		where Id = 1
		order by SysStart desc 

insert into AgencyInvestigatorCheck(Id, SysStart, EmployeeId, DateCompleted, Status, SysEnd, SysUser, SysUserNext)
	select top 1 Id,'2015-01-01',1,'2018-01-01','Fail',_.RightBefore(SysStart),@jill,@jack
		from dbo_history.AgencyInvestigatorCheck
		where Id = 1
		order by SysStart desc 

insert into AgencyInvestigatorCheck(Id, SysStart, EmployeeId, DateCompleted, Status, SysEnd, SysUser, SysUserNext)
	select Id,'2017-02-02',2,'2018-02-02','Fail',_.RightBefore(SysStart),@jill,@jack
		from AgencyInvestigatorCheck
		where Id = 2

insert into AgencyInvestigatorCheck(Id, SysStart, EmployeeId, DateCompleted, Status, SysEnd, SysUser, SysUserNext)
	select top 1 Id,'2016-02-02',2,'2018-02-02','Pass',_.RightBefore(SysStart),@jack,@jill
		from dbo_history.AgencyInvestigatorCheck
		where Id = 2
		order by SysStart desc 

insert into AgencyInvestigatorCheck(Id, SysStart, EmployeeId, DateCompleted, Status, SysEnd, SysUser, SysUserNext)
	select top 1 Id,'2015-02-02',2,'2018-02-02','Fail',_.RightBefore(SysStart),@jill,@jack
		from dbo_history.AgencyInvestigatorCheck
		where Id = 2
		order by SysStart desc 

insert into AgencyInvestigatorCheck(Id, SysStart, EmployeeId, DateCompleted, Status, SysEnd, SysUser, SysUserNext)
	select Id,'2017-03-03',3,'2018-03-03','Pass',_.RightBefore(SysStart),@jack,@jill
		from AgencyInvestigatorCheck
		where Id = 3
