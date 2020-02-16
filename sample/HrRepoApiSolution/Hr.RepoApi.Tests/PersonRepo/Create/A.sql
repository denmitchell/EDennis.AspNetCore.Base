﻿use Hr123;
declare @ProjectName varchar(255) = 'Hr.RepoApi'
declare @ClassName varchar(255) = 'PersonRepo'
declare @MethodName varchar(255) = 'Create'
declare @TestScenario varchar(255) = 'Success'
declare @TestCase varchar(255) = 'A'


declare @Id int = -999101
declare @FirstName varchar(255) = 'Chandler'
declare @LastName varchar(255) = 'Bing'
declare @DateOfBirth datetime = '1970-05-01'
declare @SysUser varchar(255) = 'tester@example.org'

declare @LinqWhere varchar(255) = 'Id ge -999101 and Id le -999099'

declare @Exception varchar(255) = null --Success

begin transaction
insert into Person (Id, FirstName, LastName, DateOfBirth, SysUser) 
    values 
        (@Id, @FirstName, @LastName, @DateOfBirth, @SysUser)

declare @Input varchar(max) = 
(
	select * from Person where Id = @Id
	for json path, without_array_wrapper
)

declare @Expected varchar(max) = 
(
	select * from Person where Id between -999101 and -999099
	for json path
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Input', @Input
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Exception', @Exception
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'LinqWhere', @LinqWhere
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase
