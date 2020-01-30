use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Repo'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'ColorNameById'
declare @TestScenario varchar(255) = 'Verifying with Dynamic Linq, FormatException'
declare @TestCase varchar(255) = 'D'

declare @SpName varchar(255) = 'ColorNameById'
declare @Id varchar(255) = 'a$5@8fal'


declare @Params varchar(max) = 
(
    select @Id Id
  for json path, without_array_wrapper
)


declare @ThrowsException bit = 1
declare @Expected varchar(max) = 0;

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Params', @Params
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ThrowsException', @ThrowsException
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

