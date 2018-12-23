use todo;

if object_id('tempdb..#input') is not null
    drop table #input

select
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
insert into Task(Title,PercentComplete,DueDate)
	select Title,PercentComplete,DueDate from #input;

declare 
	@expected varchar(max) = 
(
	select * from Task
	for json path, include_null_values
);

rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Post','PostAndGetMultiple','B','Input', @input
exec _maintenance.SaveTestJson 'EDennis.Samples.TodoApi', 'TaskController', 'Post','PostAndGetMultiple','B','Expected', @expected

exec _maintenance.ResetIdentities