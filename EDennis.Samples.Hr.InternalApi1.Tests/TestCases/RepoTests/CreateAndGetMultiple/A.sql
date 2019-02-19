﻿use hr;

declare @Id int
select @Id = max(id) + 1 from Employee

if object_id('tempdb..#input') is not null
    drop table #input

select
	'Larry' FirstName
into #input 

declare 
	@input varchar(max) = 
	( 
		select * from #input
		for json path, include_null_values, without_array_wrapper
	);


begin transaction
insert into Employee(FirstName)
	select FirstName from #input;

declare 
	@expected varchar(max) = 
(
	select * from Employee
	for json path, include_null_values
);

rollback transaction
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGetMultiple','A','Id', @id
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGetMultiple','A','Input', @input
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'Create','CreateAndGetMultiple','A','Expected', @expected

exec _maintenance.ResetIdentities
drop table #input;