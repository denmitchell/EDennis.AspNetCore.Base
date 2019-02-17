use hr;
declare @pageNumber int = 1
declare @pageSize int = 1
declare @alpha varchar(30) = 'o'

declare 
	@expected varchar(max) = 
(
	select * from Employee
	where FirstName like '%' + @alpha + '%'
	order by id
	offset @pageSize * (@pageNumber - 1) rows
	fetch next @pageSize rows only 
	for json path, include_null_values
);


exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinqPaging','A','Alpha', @alpha
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinqPaging','A','PageNumber', @pageNumber
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinqPaging','A','PageSize', @pageSize
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinqPaging','A','Expected', @expected

--exec _maintenance.ResetIdentities
