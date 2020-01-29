use Color2Db;
declare @ProjectName varchar(255) = 'Colors2Api'
declare @ClassName varchar(255) = 'RgbRepo'
declare @MethodName varchar(255) = 'ColorNameById'
declare @TestScenario varchar(255) = 'Verifying with Dynamic Linq, ArgumentException'
declare @TestCase varchar(255) = 'C'

declare @SpName varchar(255) = 'n32knf$#2'
declare @Id varchar(255) = -999001


declare @Params varchar(max) = 
(
    select @Id Id
  for json path, without_array_wrapper
)


declare @Exception varchar(255) = 'ArgumentException'
declare @Expected varchar(max) = null;

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Params', @Params
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Exception', @Exception
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

