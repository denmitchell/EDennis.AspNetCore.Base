--use ColorDb; --omit for testing purposes
--set identity_insert some_other_table off

set identity_insert Employee on
insert into Employee(Id, FirstName)
	values (1,'Bob'),(2,'Monty'),(3,'Drew'),(4,'Wayne');
set identity_insert Employee off

set identity_insert Position on
insert into Position(Id, Title, IsManager)
	values (1,'Game Show Manager', 1),(2,'Game Show Host', 0);
set identity_insert Position off

insert into EmployeePosition(EmployeeId, PositionId)
	values (1,1),(2,1),(3,2),(4,2);


