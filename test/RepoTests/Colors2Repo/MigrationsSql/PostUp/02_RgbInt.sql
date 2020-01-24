SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RgbInt] 
	@ColorName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	select convert(int,
			convert(varbinary(1), Red) 
			+ convert(varbinary(1), Green)
			+ convert(varbinary(1), Blue))
			RgbInt
	from rgb
		where Name = @ColorName
END
