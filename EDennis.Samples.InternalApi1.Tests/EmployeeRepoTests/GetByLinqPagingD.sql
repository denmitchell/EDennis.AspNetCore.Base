use hr;
declare @pageNumber int = 2
declare @pageSize int = 1
declare @alpha varchar(30) = 'n'

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


exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinqPaging','D','Alpha', @alpha
exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinqPaging','D','PageNumber', @pageNumber
exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinqPaging','D','PageSize', @pageSize
exec _maintenance.SaveTestJson 'EDennis.Samples.InternalApi1', 'EmployeeRepo', 'GetByLinq','GetByLinqPaging','D','Expected', @expected

--exec _maintenance.ResetIdentities
