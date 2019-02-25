use hr;

declare @pageNumber int = 2
declare @pageSize int = 1

declare @Expected varchar(max) = 
(
	select * from Employee
	order by id
	offset @pageSize * (@pageNumber - 1) rows
	fetch next @pageSize rows only 
	for json path, include_null_values
);

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'GetEmployees','Paging','Page2PageSize1','PageNumber', @pageNumber
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'GetEmployees','Paging','Page2PageSize1','PageSize', @pageSize
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'GetEmployees','Paging','Page2PageSize1','Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'GetEmployees','Paging','Page2PageSize1'
