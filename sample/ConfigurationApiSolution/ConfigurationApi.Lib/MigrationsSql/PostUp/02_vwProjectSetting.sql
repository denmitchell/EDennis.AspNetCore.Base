CREATE OR ALTER VIEW vwProjectSetting AS
	SELECT ProjectName, SettingKey, SettingValue
		FROM ProjectSetting ps
		INNER JOIN Project p
			on p.Id = ps.ProjectId
		INNER JOIN Setting s
			on s.Id = ps.SettingKey