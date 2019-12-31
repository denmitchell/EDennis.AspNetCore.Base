SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[HslByColorName] 
	@ColorName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	select Hue, Saturation, Luminance 
		from vwHsl
		where Name = @ColorName
END
