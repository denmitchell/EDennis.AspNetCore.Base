create or alter view vwHsl as
with prime as (
select Id, Name, red/255.0 r1, green/255.0 g1, blue/255.0 b1, SysUser, DateAdded
	from Rgb 
),
maxmin as (
select name, 
	case 
		when r1 >= g1 and r1 >= b1 then r1 
		when g1 >= r1 and g1 >= b1 then g1 
		when b1 >= r1 and b1 >= g1 then b1 
		end cmax,
	case 
		when r1 <= g1 and r1 <= b1 then r1 
		when g1 <= r1 and g1 <= b1 then g1 
		when b1 <= r1 and b1 <= g1 then b1 
		end cmin,
	case 
		when r1 >= g1 and r1 >= b1 then r1 
		when g1 >= r1 and g1 >= b1 then g1 
		when b1 >= r1 and b1 >= g1 then b1 
		end 
	- case 
		when r1 <= g1 and r1 <= b1 then r1 
		when g1 <= r1 and g1 <= b1 then g1 
		when b1 <= r1 and b1 <= g1 then b1 
		end delta
	
	from prime
)
select 
    p.Id,
    p.Name,
	convert(int,
		floor(case 
			when delta = 0 then 0
			when cmax = r1 then 60 * (((g1-b1)/delta) % 6)
			when cmax = g1 then 60 * (((b1-r1)/delta) + 2)
			when cmax = b1 then 60 * (((b1-r1)/delta) + 4)
			end)) Hue,
	convert(int,
		floor(255 * (case when delta = 0 then 0
			else delta/(1-abs(cmax+cmin-1))
			end))) Saturation,
	convert(int,
		floor(255*(cmax + cmin) /2)) Luminance,
	p.SysUser,
	p.DateAdded
	from prime p
	inner join maxmin m
		on p.Name = m.Name
