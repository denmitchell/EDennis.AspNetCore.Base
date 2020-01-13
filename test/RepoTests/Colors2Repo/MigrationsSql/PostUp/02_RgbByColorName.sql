SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RgbByColorName] 
	@ColorName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	select Red, Green, Blue
		from Rgb
		where Name = @ColorName
END
