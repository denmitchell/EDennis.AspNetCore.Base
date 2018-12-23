use todo;

declare @id int = 1

if object_id('tempdb..#input') is not null
    drop table #input

select
    @id Id,
	'Clean the kitchen' Title,
	30 PercentComplete,
	CONVERT(datetime,'2018-12-01') DueDate
into #input 

declare 
	@input varchar(max) = 
	( 
		select * from #input
		for json path, include_null_values, without_array_wrapper
	);

begin transaction
update t
	set Title= i.Title, 
		PercentComplete = i.PercentComplete,
		DueDate = i.DueDate
	from Task t
	inner join #input i
		on i.Id = t.Id

declare 
	@expected varchar(max) = 
(
	select * from Task
	where id = @id
	for json path, include_null_values, without_array_wrapper
);

rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Put','PutAndGet','A','Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Put','PutAndGet','A','Input', @input
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Put','PutAndGet','A','Expected', @expected

--exec _maintenance.ResetIdentities