﻿use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'Get'
declare @TestScenario varchar(255) = 'Verifying with Dynamic Linq, MissingEntityException'
declare @TestCase varchar(255) = 'C'

declare @TargetId int = -999299
declare @Exception varchar(255) = 'MissingEntityException'

begin transaction

declare @Expected varchar(max) = 
(
	select * from Rgb 
		where Id = @TargetId
	for json path, without_array_wrapper
);

rollback transaction
--exec _.ResetSequences --only needed if no explicit Ids are provided

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Id', @TargetId
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase, 'Exception', @Exception
exec _.GetTestJson @ProjectName, @ClassName, @MethodName, @TestScenario, @TestCase