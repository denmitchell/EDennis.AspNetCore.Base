use ColorDb;
go
create schema _
go
create function _.RightBefore (@dt datetime2)
returns datetime2
as begin
	return dateadd(nanosecond,-100,@dt) 
end 
go
create function _.MaxDateTime2()
returns datetime2
as begin
	return convert(datetime2,'9999-12-31T23:59:59.9999999')
end
go
