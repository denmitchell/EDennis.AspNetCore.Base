use hr;

declare @id int = 2

if object_id('tempdb..#input') is not null
    drop table #input

select
    @id Id,
	'Curly' FirstName
into #input 

declare 
	@input varchar(max) = 
	( 
		select * from #input
		for json path, include_null_values, without_array_wrapper
	);

begin transaction
update e
	set FirstName= i.FirstName
	from Employee e
	inner join #input i
		on i.Id = e.Id

declare 
	@expected varchar(max) = 
(
	select * from Employee
	where id = @id
	for json path, include_null_values, without_array_wrapper
);

rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'Update','UpdateAndGet','B','Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'Update','UpdateAndGet','B','Input', @input
exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'Update','UpdateAndGet','B','Expected', @expected

--exec _maintenance.ResetIdentities
drop table #input;