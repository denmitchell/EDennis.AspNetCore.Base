﻿SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ColorNameById] 
	@Id int
AS
BEGIN
	SET NOCOUNT ON;
	select Name
		from Rgb
		where Id = @Id
END
