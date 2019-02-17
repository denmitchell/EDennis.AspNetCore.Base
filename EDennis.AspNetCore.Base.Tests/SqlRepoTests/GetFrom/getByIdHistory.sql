use colordb;

declare @id int = 2;
declare @asOf datetime;
begin transaction

declare @updateInput1 varchar(max) = (
select @id Id, 'brown' Name
	for json path, without_array_wrapper
);
update Colors set Name = 'brown' where Id = @id;

WAITFOR DELAY '00:00:02'

declare @updateInput2 varchar(max) = (
select @id Id, 'beige' Name
	for json path, without_array_wrapper
);
update Colors set Name = 'beige' where Id = @id;

--select row_number() over (partition by Id order by SysStart) rowid, SysStart 
--		from dbo_history.Colors
--		where Id = @id


declare @expected varchar(max) = (
	select * from (
		select * from Colors 
			for system_time all
			where Id = @id
	) a
	for json path, include_null_values
);

rollback transaction

exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdHistory','SqlRepo','A','Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdHistory','SqlRepo','A','UpdateInput1', @updateInput1
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdHistory','SqlRepo','A','UpdateInput2', @updateInput2
exec _maintenance.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','GetByIdHistory','SqlRepo','A','Expected', @expected

select * from _maintenance.TestJson 
	where ProjectName = 'EDennis.Samples.Colors.InternalApi'
		and ClassName = 'ColorRepo'
		and MethodName = 'GetByIdHistory'
		and TestScenario = 'SqlRepo'
		and TestCase = 'A'
