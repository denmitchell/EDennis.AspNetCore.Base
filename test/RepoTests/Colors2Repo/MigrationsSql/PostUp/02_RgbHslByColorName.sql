SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[RgbHslByColorName] 
	@ColorName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	select Rgb.Id Id, Rgb.Name, Red, Green, Blue, Hue, Saturation, Luminance 
		from Rgb
		inner join vwHsl
		on Rgb.Id = vwHsl.Id
		where Rgb.Name = @ColorName
END
