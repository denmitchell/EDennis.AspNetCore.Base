--set identity_insert some_other_table off

set identity_insert FederalBackgroundCheck on
insert into FederalBackgroundCheck(Id, EmployeeId, DateCompleted, Status)
	values 
	(1,1,'2018-01-01','Pass'),
	(2,2,'2018-02-02','Pass'),
	(3,3,'2018-03-03','Fail'),
	(4,4,'2018-04-04','Pass');
set identity_insert FederalBackgroundCheck off