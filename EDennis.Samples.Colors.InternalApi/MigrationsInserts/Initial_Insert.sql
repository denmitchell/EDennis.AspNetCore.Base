--use ColorDb; --omit for testing purposes
--set identity_insert some_other_table off
set identity_insert Colors on
insert into Colors(Id, Name)
	values (1,'black'),(2,'white'),(3,'gray'),(4,'red'),(5,'green'),(6,'blue');
set identity_insert colors off

insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'beige','2017-01-01',dateadd(millisecond,-1,SysStart)
		from dbo.Colors
		where Id = 1
	
insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'brown','2016-01-01',dateadd(millisecond,-1,SysStart)
		from dbo_history.Colors
		where Id = 1 and SysStart = '2017-01-01'

insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'tan','2015-01-01',dateadd(millisecond,-1,SysStart)
		from dbo_history.Colors
		where Id = 1 and SysStart = '2016-01-01'

insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'pearl','2017-02-02',dateadd(millisecond,-1,SysStart)
		from dbo.Colors
		where Id = 2
	
insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'ivory','2016-02-02',dateadd(millisecond,-1,SysStart)
		from dbo_history.Colors
		where Id = 2 and SysStart = '2017-02-02'

insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'chiffon','2015-02-02',dateadd(millisecond,-1,SysStart)
		from dbo_history.Colors
		where Id = 2 and SysStart = '2016-02-02'

insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'slate','2017-03-03',dateadd(millisecond,-1,SysStart)
		from dbo.Colors
		where Id = 3
	
insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'charcoal','2016-03-03',dateadd(millisecond,-1,SysStart)
		from dbo_history.Colors
		where Id = 3 and SysStart = '2017-03-03'

insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'pewter','2015-03-03',dateadd(millisecond,-1,SysStart)
		from dbo_history.Colors
		where Id = 3 and SysStart = '2016-03-03'

insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'smoke','2014-03-03',dateadd(millisecond,-1,SysStart)
		from dbo_history.Colors
		where Id = 3 and SysStart = '2015-03-03'

insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'crimson','2017-04-04',dateadd(millisecond,-1,SysStart)
		from dbo.Colors
		where Id = 4

insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'chartreuse','2017-05-05',dateadd(millisecond,-1,SysStart)
		from dbo.Colors
		where Id = 5
	
insert into dbo_history.Colors(Id,Name,SysStart,SysEnd)
	select Id,'sage','2016-05-05',dateadd(millisecond,-1,SysStart)
		from dbo_history.Colors
		where Id = 5 and SysStart = '2017-05-05'
