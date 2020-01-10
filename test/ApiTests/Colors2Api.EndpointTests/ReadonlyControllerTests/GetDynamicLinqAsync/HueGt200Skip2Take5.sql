use Colors2;
declare @ProjectName varchar(255) = 'EDennis.Samples.Colors2Api'
declare @ClassName varchar(255) = 'HslController'
declare @MethodName varchar(255) = 'GetDynamicLinqAsync'
declare @TestScenario varchar(255) = 'WhereSkipTake'
declare @TestCase varchar(255) = 'HueGt200Skip2Take5'

declare @Where varchar(255) = 'Hue gt 200'
declare @Skip int = 2
declare @Take int = 5

declare 
	@Expected varchar(max) = 
(
	select * from vwHsl
	where Hue > 200 
	order by Id
	offset @Skip ROWS FETCH NEXT @Take ROWS ONLY
	for json path, include_null_values
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Where', @Where
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Skip', @Skip
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Take', @Take
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase