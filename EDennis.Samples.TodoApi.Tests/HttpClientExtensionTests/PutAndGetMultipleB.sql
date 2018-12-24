use todo;

declare @id int = 2

if object_id('tempdb..#input') is not null
    drop table #input

select
    @id Id,
	'Clean the garage' Title,
	60 PercentComplete,
	CONVERT(datetime,'2018-12-02') DueDate
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
	for json path, include_null_values
);

rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Put','PutAndGetMultiple','B','Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Put','PutAndGetMultiple','B','Input', @input
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Put','PutAndGetMultiple','B','Expected', @expected

--exec _maintenance.ResetIdentities
drop table #input;