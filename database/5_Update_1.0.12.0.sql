-- =========================================================
-- Author:			LinuxChata
-- Created date:	24.06.2013
-- Modified date:	14.09.2013
-- Description:		Perform update 1.0.12.0 of the database.
-- =========================================================
USE [dentist]

SET NOCOUNT ON
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

DECLARE @update_version nvarchar(255);
DECLARE @update_version_int int;
DECLARE @update_date datetime;

BEGIN TRANSACTION [Update10121Transaction]
	BEGIN TRY

	SET @update_version = '1.0.0.0';
	SET @update_version_int = 10000;
	SET @update_date = GETDATE();

		IF NOT EXISTS (SELECT * FROM sysobjects where name = 'SystemUpdates' AND xtype = 'U')
			BEGIN
				CREATE TABLE [dbo].[SystemUpdates]
				(
					[ID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
					[UpdateInformation] [nvarchar](1024) NULL,
					[UpdateVersion] [nvarchar](255) NOT NULL,
					[UpdateVersionInt] int NOT NULL,
					[UpdateDate] [datetime]  NOT NULL
				);
				PRINT 'SystemUpdates table has been created.'
			END
		ELSE 
			BEGIN
				PRINT 'SystemUpdates table already exists.'
			END

		-- Perform initial script.
		IF NOT EXISTS (SELECT NULL FROM [SystemUpdates] WHERE [UpdateVersion] = @update_version)
			BEGIN
				INSERT INTO [SystemUpdates]
						   ([UpdateInformation],
						   [UpdateVersion],
						   [UpdateVersionInt],
						   [UpdateDate])
					 VALUES
						   ('Initial script.',
						   @update_version,
						   @update_version_int,
						   @update_date);

				PRINT 'Inital script has been added to the SystemUpdates table.'
			END
		ELSE
			BEGIN
				PRINT 'Inital script already exist in the SystemUpdates table.'
			END

		-- Perform script version 1.0.12.1.
		SET @update_version = '1.0.12.1';
		SET @update_version_int = 10121;
		IF NOT EXISTS (SELECT NULL FROM [SystemUpdates] WHERE [UpdateVersion] = @update_version)
			BEGIN		
				IF EXISTS (SELECT * FROM sysobjects where name = 'BackupTypes' AND xtype = 'U')
					BEGIN
						DROP TABLE [BackupTypes];

						PRINT 'BackupTypes table has been dropped.';
					END

				IF EXISTS (SELECT * FROM sysobjects where name = 'Sexs' AND xtype = 'U')
					BEGIN
						DROP TABLE [Sexs];

						PRINT 'Sexs table has been dropped.';		
					END

				IF EXISTS (SELECT * FROM sysobjects where name = 'ReminderAlert' AND xtype = 'U')
					BEGIN
						DROP TABLE [ReminderAlert];

						PRINT 'ReminderAlert table has been dropped.';		
					END

				IF EXISTS (SELECT * FROM sysobjects where name = 'ReminderFilter' AND xtype = 'U')
					BEGIN
						DROP TABLE [ReminderFilter];

						PRINT 'ReminderFilter table has been dropped.';		
					END

				IF EXISTS (SELECT * FROM sysobjects where name = 'LanguageData' AND xtype = 'U')
					BEGIN
						DROP TABLE [LanguageData];

						PRINT 'LanguageData table has been dropped.';		
					END

				IF EXISTS (SELECT * FROM sysobjects where name = 'LanguageKeys' AND xtype = 'U')
					BEGIN
						DROP TABLE [LanguageKeys];

						PRINT 'LanguageKeys table has been dropped.';		
					END

				IF NOT EXISTS (SELECT * FROM sysobjects where name = 'LanguageKeys' AND xtype = 'U')
					BEGIN
						CREATE TABLE [dbo].[LanguageKeys]
						(
							[ID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
							[Key] [varchar](512) NOT NULL UNIQUE
						);

						PRINT 'LanguageKeys table has been created.';
					END

				IF NOT EXISTS (SELECT * FROM sysobjects where name = 'BackupTypes' AND xtype = 'U')
					BEGIN
						CREATE TABLE [dbo].[BackupTypes]
						(
							[ID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
							[KeyId] [int],
							FOREIGN KEY ([KeyId]) REFERENCES [LanguageKeys](ID)
						);

						PRINT 'BackupTypes table has been created.';
					END

				IF NOT EXISTS (SELECT * FROM sysobjects where name = 'Sexs' AND xtype = 'U')
					BEGIN
						CREATE TABLE [dbo].[Sexs]
						(
							[ID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
							[KeyId] [int],
							FOREIGN KEY ([KeyId]) REFERENCES [LanguageKeys](ID)
						);

						PRINT 'Sexs table has been created.';
					END

				IF NOT EXISTS (SELECT * FROM sysobjects where name = 'ReminderFilter' AND xtype = 'U')
					BEGIN
						CREATE TABLE [dbo].[ReminderFilter]
							(
								[ID] [int] IDENTITY(1,1) NOT NULL,
								[KeyId] [int] NOT NULL,
								PRIMARY KEY ([Id]),
								FOREIGN KEY ([KeyId]) REFERENCES [LanguageKeys]([ID])
							);

							PRINT 'Filter table has been created.';
					END

				IF NOT EXISTS (SELECT * FROM sysobjects where name = 'ReminderAlert' AND xtype = 'U')
					BEGIN
						CREATE TABLE [dbo].[ReminderAlert]
						(
							[ID] [int] IDENTITY(1,1) NOT NULL,
							[Days] [int] NOT NULL,
							[KeyId] [int] NOT NULL,
							PRIMARY KEY ([Id]),
							FOREIGN KEY ([KeyId]) REFERENCES [LanguageKeys]([ID])
						);

						PRINT 'Alert table has been created.';
					END

				IF NOT EXISTS (SELECT * FROM sysobjects where name = 'LanguageData' AND xtype = 'U')
					BEGIN
						CREATE TABLE [dbo].[LanguageData]
						(
							[ID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
							[KeyId] [int] NOT NULL,
							[Value] [nvarchar](512) NOT NULL,
							[CultureId] [int] NOT NULL,
							FOREIGN KEY ([KeyId]) REFERENCES [LanguageKeys](ID),
							FOREIGN KEY ([CultureId]) REFERENCES [Culture](Id)
						);

						PRINT 'LanguageData table has been created.';
					END

				IF NOT EXISTS (SELECT * FROM sys.columns  where name = N'intValue' AND Object_ID = Object_ID(N'Setting'))
					BEGIN
						ALTER TABLE [Setting] ALTER COLUMN [nvValue] nvarchar(100) NULL;
						ALTER TABLE [Setting] ADD [intValue] int;
						ALTER TABLE [Setting] ADD [bitValue] bit;
					END

				PRINT 'Setting table has been altered.';

				CREATE TABLE [dbo].[Reminder]
				(
					[ID] [int] IDENTITY(1,1) NOT NULL,
					[Date] [datetime] NOT NULL,
					[PatientId] [int] NOT NULL,
					[DoctorId] [int] NULL,
					[Message] [nvarchar](1024) NULL,
					[AlertId] [int] NOT NULL,
					[IsCompleted] [bit] NOT NULL,
					[Comment] [nvarchar](1024) NULL,
					[IsDeleted] [bit] NOT NULL,
					[Created] [datetime] NOT NULL,
					[Changed] [datetime] NOT NULL,
					[CreatedBy] [int] NOT NULL,
					[ChangedBy] [int] NOT NULL,
					PRIMARY KEY ([Id]),
					FOREIGN KEY ([AlertId]) REFERENCES [ReminderAlert]([ID])
				);

				PRINT 'Reminder table has been created.'

				SET @update_date = GETDATE();

				INSERT INTO [SystemUpdates]
						   ([UpdateInformation],
						   [UpdateVersion],
						   [UpdateVersionInt],
						   [UpdateDate])
					 VALUES
						   ('Create Reminder tables.',
						   @update_version,
						   @update_version_int,
						   @update_date);

				PRINT 'SystemUpdates table was updated.'
			END
		ELSE 
			BEGIN
				PRINT 'ERROR: Script version 1.0.12.1 already exist in the SystemUpdates table. Update was aborted.'
			END
	COMMIT TRANSACTION [Update10121Transaction]
	PRINT 'Perform script version 1.0.12.1. has been executed';
	END TRY
BEGIN CATCH
    PRINT 'ERROR: Perform script version 1.0.12.1. has not been executed';
	ROLLBACK TRANSACTION [Update10121Transaction]
END CATCH  

GO

DECLARE @update_version nvarchar(255);
DECLARE @update_version_int int;
DECLARE @update_date datetime;
DECLARE @filter_name nvarchar(255);
DECLARE @alert_name nvarchar(255);
DECLARE @alert_days int;
DECLARE @key_id int;
DECLARE @key_name nvarchar(512);
DECLARE @culture_id int;
DECLARE @name_of_backup_types nvarchar(50);
DECLARE @name_of_sex nvarchar(50);
DECLARE @name_of_filter nvarchar(100);
DECLARE @name_of_alert nvarchar(100);
DECLARE @setting_key nvarchar(100);
DECLARE @param nvarchar(100);
DECLARE @bit_param bit;

BEGIN TRANSACTION [Update10121Transaction]
	BEGIN TRY
		-- Perform script version 1.0.12.2.
		SET @update_version = '1.0.12.2';
		SET @update_version_int = 10122;
		IF NOT EXISTS (SELECT NULL FROM [SystemUpdates] WHERE [UpdateVersion] = @update_version)
			BEGIN
				PRINT 'Handle BackupTypes table...';

				SET @key_name = N'BackupTypesManual';

				IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
					BEGIN
						INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

						SET @key_id = @@IDENTITY;

						IF NOT EXISTS (SELECT NULL FROM [BackupTypes] WHERE [KeyId] = @key_id)
							BEGIN
								INSERT INTO [BackupTypes] ([KeyId]) VALUES (@key_id);
							END

						SET @name_of_backup_types = N'Manual';
						SET @culture_id = 1;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_backup_types,
											@culture_id);
							END

						SET @name_of_backup_types = N'Ручной';
						SET @culture_id = 2;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_backup_types,
											@culture_id);
							END

						SET @name_of_backup_types = N'Ручний';
						SET @culture_id = 3;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_backup_types,
											@culture_id);
							END
					END

				SET @key_name = N'BackupTypesScheduled';

				IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
					BEGIN
						INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

						SET @key_id = @@IDENTITY;

						IF NOT EXISTS (SELECT NULL FROM [BackupTypes] WHERE [KeyId] = @key_id)
							BEGIN
								INSERT INTO [BackupTypes] ([KeyId]) VALUES (@key_id);
							END

						SET @name_of_backup_types = N'Scheduled';
						SET @culture_id = 1;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_backup_types,
											@culture_id);
							END

						SET @name_of_backup_types = N'По расписанию';
						SET @culture_id = 2;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_backup_types,
											@culture_id);
							END

						SET @name_of_backup_types = N'По розкладу';
						SET @culture_id = 3;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_backup_types,
											@culture_id);
							END
					END

				PRINT 'Handle Sexs table...';

				SET @key_name = N'SexMale';

				IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
					BEGIN
						INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

						SET @key_id = @@IDENTITY;

						IF NOT EXISTS (SELECT NULL FROM [Sexs] WHERE [KeyId] = @key_id)
							BEGIN
								INSERT INTO [Sexs] ([KeyId]) VALUES (@key_id);
							END

						SET @name_of_sex = N'Male';
						SET @culture_id = 1;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_sex,
											@culture_id);
							END

						SET @name_of_sex = N'Мужской';
						SET @culture_id = 2;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_sex,
											@culture_id);
							END

						SET @name_of_sex = N'Чоловіча';
						SET @culture_id = 3;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_sex,
											@culture_id);
							END
					END

				SET @key_name = N'SexFemale';

				IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
					BEGIN
						INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

						SET @key_id = @@IDENTITY;

						IF NOT EXISTS (SELECT NULL FROM [Sexs] WHERE [KeyId] = @key_id)
							BEGIN
								INSERT INTO [Sexs] ([KeyId]) VALUES (@key_id);
							END

						SET @name_of_sex = N'Female';
						SET @culture_id = 1;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_sex,
											@culture_id);
							END

						SET @name_of_sex = N'Женский';
						SET @culture_id = 2;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_sex,
											@culture_id);
							END

						SET @name_of_sex = N'Жіноча';
						SET @culture_id = 3;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_sex,
											@culture_id);
							END
					END

				PRINT 'Handle Reminders tables...';

				SET @key_name = N'ReminderFilterSelect';

				IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
					BEGIN
						INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

						SET @key_id = @@IDENTITY;

						IF NOT EXISTS (SELECT NULL FROM [ReminderFilter] WHERE [KeyId] = @key_id)
							BEGIN
								INSERT INTO [ReminderFilter] ([KeyId]) VALUES (@key_id);
							END

						SET @name_of_filter = N'Select filter';
						SET @culture_id = 1;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END

						SET @name_of_filter = N'Выберите фильтр';
						SET @culture_id = 2;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END

						SET @name_of_filter = N'Оберіть фільтр';
						SET @culture_id = 3;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END
					END

				SET @key_name = N'ReminderFilterCurrentYear';

				IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
					BEGIN
						INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

						SET @key_id = @@IDENTITY;

						IF NOT EXISTS (SELECT NULL FROM [ReminderFilter] WHERE [KeyId] = @key_id)
							BEGIN
								INSERT INTO [ReminderFilter] ([KeyId]) VALUES (@key_id);
							END

						SET @name_of_filter = N'Current year';
						SET @culture_id = 1;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END

						SET @name_of_filter = N'За текущий год';
						SET @culture_id = 2;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END

						SET @name_of_filter = N'За поточний рік';
						SET @culture_id = 3;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END
					END

				SET @key_name = N'ReminderFilterCurrentMonth';

				IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
					BEGIN
						INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

						SET @key_id = @@IDENTITY;

						IF NOT EXISTS (SELECT NULL FROM [ReminderFilter] WHERE [KeyId] = @key_id)
							BEGIN
								INSERT INTO [ReminderFilter] ([KeyId]) VALUES (@key_id);
							END

						SET @name_of_filter = N'Current month';
						SET @culture_id = 1;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END

						SET @name_of_filter = N'За текущий месяц';
						SET @culture_id = 2;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END

						SET @name_of_filter = N'За поточний місяць';
						SET @culture_id = 3;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END
					END

				SET @key_name = N'ReminderFilterCurrentWeek';

				IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
					BEGIN
						INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

						SET @key_id = @@IDENTITY;

						IF NOT EXISTS (SELECT NULL FROM [ReminderFilter] WHERE [KeyId] = @key_id)
							BEGIN
								INSERT INTO [ReminderFilter] ([KeyId]) VALUES (@key_id);
							END

						SET @name_of_filter = N'Current week';
						SET @culture_id = 1;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END

						SET @name_of_filter = N'За текущую неделю';
						SET @culture_id = 2;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END

						SET @name_of_filter = N'За поточний тиждень';
						SET @culture_id = 3;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END
					END

				SET @key_name = N'ReminderFilterSelectedDate';

				IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
					BEGIN
						INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

						SET @key_id = @@IDENTITY;

						IF NOT EXISTS (SELECT NULL FROM [ReminderFilter] WHERE [KeyId] = @key_id)
							BEGIN
								INSERT INTO [ReminderFilter] ([KeyId]) VALUES (@key_id);
							END

						SET @name_of_filter = N'Selected date';
						SET @culture_id = 1;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END

						SET @name_of_filter = N'За выбранную дату';
						SET @culture_id = 2;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END

						SET @name_of_filter = N'За обрану дату';
						SET @culture_id = 3;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_filter,
											@culture_id);
							END
					END

				SET @key_name = N'ReminderAlertOnTheDayOfReminder';

				IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
					BEGIN
						INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

						SET @key_id = @@IDENTITY;
						SET @alert_days = 0;

						IF NOT EXISTS (SELECT NULL FROM [ReminderFilter] WHERE [KeyId] = @key_id)
							BEGIN
								INSERT INTO [ReminderAlert] ([KeyId], [Days]) VALUES (@key_id, @alert_days);
							END

						SET @name_of_alert = N'On the day of reminder';
						SET @culture_id = 1;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_alert,
											@culture_id);
							END

						SET @name_of_alert = N'В день напоминания';
						SET @culture_id = 2;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_alert,
											@culture_id);
							END

						SET @name_of_alert = N'У день нагадування';
						SET @culture_id = 3;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_alert,
											@culture_id);
							END
					END

				SET @key_name = N'ReminderAlert1DayPriorToTheReminder';

				IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
					BEGIN
						INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

						SET @key_id = @@IDENTITY;
						SET @alert_days = 1;

						IF NOT EXISTS (SELECT NULL FROM [ReminderFilter] WHERE [KeyId] = @key_id)
							BEGIN
								INSERT INTO [ReminderAlert] ([KeyId], [Days]) VALUES (@key_id, @alert_days);
							END

						SET @name_of_alert = N'1 day prior to the reminder';
						SET @culture_id = 1;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_alert,
											@culture_id);
							END

						SET @name_of_alert = N'За 1 день до напоминания';
						SET @culture_id = 2;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_alert,
											@culture_id);
							END

						SET @name_of_alert = N'За 1 день до нагадування';
						SET @culture_id = 3;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_alert,
											@culture_id);
							END
					END

				SET @key_name = N'ReminderAlert1WeekPriorToTheReminder';

				IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
					BEGIN
						INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

						SET @key_id = @@IDENTITY;
						SET @alert_days = 7;

						IF NOT EXISTS (SELECT NULL FROM [ReminderFilter] WHERE [KeyId] = @key_id)
							BEGIN
								INSERT INTO [ReminderAlert] ([KeyId], [Days]) VALUES (@key_id, @alert_days);
							END

						SET @name_of_alert = N'1 week prior to the reminder';
						SET @culture_id = 1;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_alert,
											@culture_id);
							END

						SET @name_of_alert = N'За 1 неделю до напоминания';
						SET @culture_id = 2;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_alert,
											@culture_id);
							END

						SET @name_of_alert = N'За 1 тиждень до нагадування';
						SET @culture_id = 3;

						IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_sex AND [CultureId] = @culture_id)
							BEGIN
								INSERT INTO [LanguageData] 
											([KeyId],
											[Value],
											[CultureId])
								VALUES		(@key_id,
											@name_of_alert,
											@culture_id);
							END
					END

				PRINT 'Lookup tables has been updated.';

				IF NOT EXISTS (SELECT Sex FROM [dbo].[Staff] WHERE Sex = 2)
					BEGIN
						-- Order is important!!!
						UPDATE [dbo].[Staff] SET Sex = 2 WHERE Sex = 1;
						UPDATE [dbo].[Staff] SET Sex = 1 WHERE Sex = 0;
					END

				IF NOT EXISTS (SELECT Sex FROM [dbo].[Patient] WHERE Sex = 2)
					BEGIN
						-- Order is important!!!
						UPDATE [dbo].[Patient] SET Sex = 2 WHERE Sex = 1;
						UPDATE [dbo].[Patient] SET Sex = 1 WHERE Sex = 0;
					END

				PRINT 'Sex columns in Staff and Patient tables have been updated.';	

				IF NOT EXISTS (SELECT name FROM sys.foreign_keys WHERE name = 'FK_Staff_Sex')
					BEGIN
						ALTER TABLE [dbo].[Staff] ADD CONSTRAINT FK_Staff_Sex
						FOREIGN KEY(Sex) REFERENCES [Sexs]([ID])
					END

				IF NOT EXISTS (SELECT name FROM sys.foreign_keys WHERE name = 'FK_Patient_Sex')
					BEGIN
						ALTER TABLE [dbo].[Patient] ADD CONSTRAINT FK_Patient_Sex
						FOREIGN KEY(Sex) REFERENCES [Sexs]([ID])
					END

				IF NOT EXISTS (SELECT name FROM sys.foreign_keys WHERE name = 'FK_BackupLogs_BackupTypes')
					BEGIN
						ALTER TABLE [dbo].BackupLogs ADD CONSTRAINT FK_BackupLogs_BackupTypes
						FOREIGN KEY(BackupTypesId) REFERENCES [BackupTypes]([ID])
					END

				PRINT 'Patient, Staff, BackupLogs tables has been updated.';

				SET @update_date = GETDATE();

				INSERT INTO [SystemUpdates]
							([UpdateInformation],
							[UpdateVersion],
							[UpdateVersionInt],
							[UpdateDate])
					 VALUES
							('Create Reminder tables.',
							@update_version,
							@update_version_int,
							@update_date);

				PRINT 'SystemUpdates table was updated.'

				SET @setting_key = N'NotificationOn'
				SET @bit_param = 1;

				IF NOT EXISTS (SELECT NULL FROM [Setting] WHERE nvKey = @setting_key)
					BEGIN
						INSERT INTO [Setting]
								([nvKey]
								,[bitValue])
						 VALUES
								(@setting_key
								,@bit_param)
					END

				SET @setting_key = N'NotificationDurationInMinutes'
				SET @param = '15'

				IF NOT EXISTS (SELECT NULL FROM [Setting] WHERE nvKey = @setting_key)
					BEGIN
						INSERT INTO [Setting]
								([nvKey]
								,[intValue])
						 VALUES
								(@setting_key
								,@param)
					END

				PRINT 'Setting table has been updated.';
			END
		ELSE 
			BEGIN
				PRINT 'ERROR: Script version 1.0.12.2 already exist in the SystemUpdates table. Update was aborted.'
			END

COMMIT TRANSACTION [Update10121Transaction]
PRINT 'Perform script version 1.0.12.2. has been executed';
	END TRY
BEGIN CATCH
    PRINT 'ERROR: Perform script version 1.0.12.2. has not been executed';
	ROLLBACK TRANSACTION [Update10121Transaction]
END CATCH