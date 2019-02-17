--use ColorDb; --omit for testing purposes
--set identity_insert some_other_table off
set identity_insert colors on
insert into colors(id, name)
	values (1,'black'),(2,'white'),(3,'gray'),(4,'red'),(5,'green'),(6,'blue');
set identity_insert colors off