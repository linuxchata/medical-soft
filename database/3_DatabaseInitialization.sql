-- =============================================
-- Author:			LinuxChata
-- Created date:	21.09.2012
-- Modified date:	24.06.2013
-- Description:		Perform database initialization
-- =============================================

USE [dentist]
GO

SET quoted_identifier ON
SET ANSI_NULLS ON

declare @CurrentTime DATETIME
declare @LoginName nvarchar(50)
declare @RoleName nvarchar(50)
declare @PositionName nvarchar(50)
declare @EducationShortName nvarchar(50)
declare @EducationName nvarchar(200)
declare @CultureName nvarchar(5)
declare @CultureDescription nvarchar(100)
declare @SettingKey nvarchar(100)
declare @Param nvarchar(100)
declare @BackupType nvarchar(50)
declare @NameOfTheSex nvarchar(50)

SET @CurrentTime =  GETDATE()

SET @RoleName = N'Administrator'
	
IF NOT EXISTS (SELECT NULL FROM [Role] WHERE Name = @RoleName)
	BEGIN
		INSERT INTO [Role]
				   ([Name])
			 VALUES
				   (@RoleName)		
	END
	
SET @RoleName = N'Backup'
	
IF NOT EXISTS (SELECT NULL FROM [Role] WHERE Name = @RoleName)
	BEGIN
		INSERT INTO [Role]
					([Name])
			 VALUES
					(@RoleName)
	END
	
SET @RoleName = N'User'
	
IF NOT EXISTS (SELECT NULL FROM [Role] WHERE Name = @RoleName)
	BEGIN
		INSERT INTO [Role]
					([Name])
			 VALUES
					(@RoleName)
	END

SET @LoginName = N'a'

IF NOT EXISTS (SELECT NULL FROM [Login] WHERE LoginName = @LoginName)
	BEGIN
		INSERT INTO [Login]
			   ([LoginName]
			   ,[Password]
			   ,[IsCanLogin]
			   ,[IsHaveToChangePass]
			   ,[RoleInSystem]
			   ,[StaffID]
			   ,[IsDeleted]
			   ,[IsSA]
			   ,[Created]
			   ,[Changed]
			   ,[CreatedBy]
			   ,[ChangedBy])
			VALUES
			   (@LoginName
			   ,N'0cc175b9c0f1b6a831c399e269772661'
			   ,1
			   ,0
			   ,1
			   ,0
			   ,0
			   ,1
			   ,@CurrentTime
			   ,@CurrentTime
			   ,0
			   ,0)
	END
	
SET @LoginName = N'backupuser'

IF NOT EXISTS (SELECT NULL FROM [Login] WHERE LoginName = @LoginName)
	BEGIN		
		INSERT INTO [Login]
			   ([LoginName]
			   ,[Password]
			   ,[IsCanLogin]
			   ,[IsHaveToChangePass]
			   ,[RoleInSystem]
			   ,[StaffID]
			   ,[IsDeleted]
			   ,[IsSA]
			   ,[Created]
			   ,[Changed]
			   ,[CreatedBy]
			   ,[ChangedBy])
			VALUES
			   (@LoginName
			   ,N'0cc175b9c0f1b6a831c399e269772661'
			   ,0
			   ,0
			   ,2
			   ,0
			   ,0
			   ,1
			   ,@CurrentTime
			   ,@CurrentTime
			   ,0
			   ,0)
	END

SET @PositionName = N'Врач'
	
IF NOT EXISTS (SELECT NULL FROM [Position] WHERE Name = @PositionName)
	BEGIN
		INSERT INTO [Position]
			   ([Name]
			   ,[IsDeleted])
		 VALUES
			   (@PositionName
			   ,0)
	END
	
SET @PositionName = N'Медсестра'
	
IF NOT EXISTS (SELECT NULL FROM [Position] WHERE Name = @PositionName)
	BEGIN
		INSERT INTO [Position]
			   ([Name]
			   ,[IsDeleted])
		 VALUES
			   (@PositionName
			   ,0)
	END
	
SET @EducationShortName = N'ВНМУ'
SET @EducationName = N'ВНМУ'
	
IF NOT EXISTS (SELECT NULL FROM [Education] WHERE [ShortName] = @EducationShortName)
	BEGIN
		INSERT INTO [Education]
				([ShortName]
				,[Name]
				,[IsDeleted])
		 VALUES
				(@EducationShortName
				,@EducationName
				,0)
	END
	
SET @CultureName = N'en-US'
SET @CultureDescription  = N'English'
	
IF NOT EXISTS (SELECT NULL FROM [Culture] WHERE Name = @CultureName)
	BEGIN
		INSERT INTO [Culture]
				([Name]
				,[Description])
		 VALUES
				(@CultureName
				,@CultureDescription)
  	END
	
SET @CultureName = N'ru-RU'
SET @CultureDescription  = N'Русский'
	
IF NOT EXISTS (SELECT NULL FROM [Culture] WHERE Name = @CultureName)
	BEGIN
		INSERT INTO [Culture]
				([Name]
				,[Description])
		 VALUES
				(@CultureName
				,@CultureDescription)
  	END
	
SET @CultureName = N'uk-UA'
SET @CultureDescription  = N'Українська'
	
IF NOT EXISTS (SELECT NULL FROM [Culture] WHERE Name = @CultureName)
	BEGIN
		INSERT INTO [Culture]
				([Name]
				,[Description])
		 VALUES
				(@CultureName
				,@CultureDescription)
  	END	

SET @CultureName = N'ru-RU'
SET @SettingKey = N'Language'

IF NOT EXISTS (SELECT NULL FROM [Setting] WHERE nvKey = @SettingKey)
	BEGIN
		INSERT INTO [Setting]
				([nvKey]
				,[nvValue])
		 VALUES
				(@SettingKey
				,@CultureName)
	END
	
SET @SettingKey = N'NameOfTheBackUpDB'
SET @Param = (SELECT DB_NAME() AS DataBaseName)

IF NOT EXISTS (SELECT NULL FROM [Setting] WHERE nvKey = @SettingKey)
	BEGIN
		INSERT INTO [Setting]
				([nvKey]
				,[nvValue])
		 VALUES
				(@SettingKey
				,@Param)
	END
	
SET @SettingKey = N'LocationOfTheBackUp'
SET @Param = N'D:\'

IF NOT EXISTS (SELECT NULL FROM [Setting] WHERE nvKey = @SettingKey)
	BEGIN
		INSERT INTO [Setting]
				([nvKey]
				,[nvValue])
		 VALUES
				(@SettingKey
				,@Param)
	END

SET @SettingKey = N'NameOfTheBackUpFile'
SET @Param = N'backup'

IF NOT EXISTS (SELECT NULL FROM [Setting] WHERE nvKey = @SettingKey)
	BEGIN
		INSERT INTO [Setting]
				([nvKey]
				,[nvValue])
		 VALUES
				(@SettingKey
				,@Param)
	END

SET @BackupType = N'Manual'

IF NOT EXISTS (SELECT NULL FROM [BackupTypes] WHERE [Name] = @BackupType)
	BEGIN
		INSERT INTO [BackupTypes]
				([Id]
				,[Name]
				,[CultureId])
		VALUES
				(1,
				@BackupType,
				1)
	END

SET @BackupType = N'Scheduled'

IF NOT EXISTS (SELECT NULL FROM [BackupTypes] WHERE [Name] = @BackupType)
	BEGIN
		INSERT INTO [BackupTypes]
				([Id]
				,[Name]
				,[CultureId])
		VALUES
				(2,
				@BackupType,
				1)
	END
	
SET @BackupType = N'Ручной'

IF NOT EXISTS (SELECT NULL FROM [BackupTypes] WHERE [Name] = @BackupType)
	BEGIN
		INSERT INTO [BackupTypes]
				([Id]
				,[Name]
				,[CultureId])
		VALUES
				(1,
				@BackupType,
				2)
	END

SET @BackupType = N'По расписанию'

IF NOT EXISTS (SELECT NULL FROM [BackupTypes] WHERE [Name] = @BackupType)
	BEGIN
		INSERT INTO [BackupTypes]
				([Id]
				,[Name]
				,[CultureId])
		VALUES
				(2,
				@BackupType,
				2)
	END
	
SET @BackupType = N'Ручний'

IF NOT EXISTS (SELECT NULL FROM [BackupTypes] WHERE [Name] = @BackupType)
	BEGIN
		INSERT INTO [BackupTypes]
				([Id]
				,[Name]
				,[CultureId])
		VALUES
				(1,
				@BackupType,
				3)
	END

SET @BackupType = N'По розкладу'

IF NOT EXISTS (SELECT NULL FROM [BackupTypes] WHERE [Name] = @BackupType)
	BEGIN
		INSERT INTO [BackupTypes]
				([Id]
				,[Name]
				,[CultureId])
		VALUES
				(2,
				@BackupType,
				3)
	END
	
SET @NameOfTheSex = N'Male'

IF NOT EXISTS (SELECT NULL FROM [Sexs] WHERE [Name] = @NameOfTheSex)
	BEGIN
		INSERT INTO [Sexs]
				([Id]
				,[Name]
				,[CultureId])
		VALUES
				(0,
				@NameOfTheSex,
				1)
	END

SET @NameOfTheSex = N'Female'

IF NOT EXISTS (SELECT NULL FROM [Sexs] WHERE [Name] = @NameOfTheSex)
	BEGIN
		INSERT INTO [Sexs]
				([Id]
				,[Name]
				,[CultureId])
		VALUES
				(1,
				@NameOfTheSex,
				1)
	END

SET @NameOfTheSex = N'Мужской'

IF NOT EXISTS (SELECT NULL FROM [Sexs] WHERE [Name] = @NameOfTheSex)
	BEGIN
		INSERT INTO [Sexs]
				([Id]
				,[Name]
				,[CultureId])
		VALUES
				(0,
				@NameOfTheSex,
				2)
	END

SET @NameOfTheSex = N'Женский'

IF NOT EXISTS (SELECT NULL FROM [Sexs] WHERE [Name] = @NameOfTheSex)
	BEGIN
		INSERT INTO [Sexs]
				([Id]
				,[Name]
				,[CultureId])
		VALUES
				(1,
				@NameOfTheSex,
				2)
	END

SET @NameOfTheSex = N'Чоловіча'

IF NOT EXISTS (SELECT NULL FROM [Sexs] WHERE [Name] = @NameOfTheSex)
	BEGIN
		INSERT INTO [Sexs]
				([Id]
				,[Name]
				,[CultureId])
		VALUES
				(0,
				@NameOfTheSex,
				3)
	END

SET @NameOfTheSex = N'Жіноча'

IF NOT EXISTS (SELECT NULL FROM [Sexs] WHERE [Name] = @NameOfTheSex)
	BEGIN
		INSERT INTO [Sexs]
				([Id]
				,[Name]
				,[CultureId])
		VALUES
				(1,
				@NameOfTheSex,
				3)
	END