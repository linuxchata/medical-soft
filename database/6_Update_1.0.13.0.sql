-- =========================================================
-- Author:			LinuxChata
-- Created date:	01.09.2013
-- Modified date:	25.10.2013
-- Description:		Perform update 1.0.13.0 of the database.
-- =========================================================
USE [dentist]

SET NOCOUNT ON
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

DECLARE @update_old_version nvarchar(255);
DECLARE @update_version nvarchar(255);
DECLARE @update_old_version_int int;
DECLARE @current_version_int int;
DECLARE @update_version_int int;
DECLARE @update_date datetime;
DECLARE @setting_key nvarchar(100);
DECLARE @param nvarchar(100);
DECLARE @int_param int;
DECLARE @bit_param bit;

DECLARE @key_id int;
DECLARE @key_name nvarchar(512);
DECLARE @name_of_status nvarchar(512);
DECLARE @culture_id int;

BEGIN TRY
	BEGIN TRANSACTION [Update10131Transaction]
		-- Perform script version 1.0.13.1.
		SET @update_old_version = '1.0.12.2';
		SET @update_old_version_int = 10122;
		SET @update_version = '1.0.13.1';
		SET @update_version_int = 10131;
		SET @update_date = GETDATE();

		IF EXISTS (SELECT NULL FROM [SystemUpdates] WHERE [UpdateVersion] = @update_version)
			BEGIN
				PRINT N'ERROR: Script version ' + @update_version + ' already exist in the database.'
				RaisError('ERROR: Script version %s already exist in the database. Update was aborted.', 15, 1, @update_version);
			END

		SET @current_version_int = (SELECT TOP 1 [UpdateVersionInt] FROM [SystemUpdates] ORDER BY [ID] DESC);
		IF (@current_version_int != @update_old_version_int)
			BEGIN
				PRINT N'ERROR: Script version ' + @update_old_version + ' has to be applied first.';
				RaisError(N'ERROR: Script version %s has to be applied first. Update was aborted.', 15, 1, @update_old_version);
			END

		IF NOT EXISTS (SELECT NULL FROM [SystemUpdates] WHERE [UpdateVersion] = @update_version)
			BEGIN
				INSERT INTO [SystemUpdates]
									([UpdateInformation],
									[UpdateVersion],
									[UpdateVersionInt],
									[UpdateDate])
							 VALUES
									('E-mail notifications.',
									@update_version,
									@update_version_int,
									@update_date);

				PRINT 'SystemUpdates table was updated.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: SystemUpdates table was not updated.'
			END

		IF NOT EXISTS (SELECT * FROM sysobjects where name = 'NotificationTemplate' AND xtype = 'U')
			BEGIN
				CREATE TABLE [dbo].[NotificationTemplate]
						(
							[ID] [int] IDENTITY(1,1) NOT NULL,
							[Description] [nvarchar](1024) NULL,
							[Title] [nvarchar](1024) NULL,
							[Body] [nvarchar](MAX) NULL,
							[IsDeleted] [bit] NOT NULL,
							[Created] [datetime] NOT NULL,
							[Changed] [datetime] NOT NULL,
							[CreatedBy] [int] NOT NULL,
							[ChangedBy] [int] NOT NULL,
							CONSTRAINT PK_NotificationTemplateId PRIMARY KEY ([Id])
						);

				PRINT 'NotificationTemplate table has been created.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: NotificationTemplate table has not been created, since it already exists.'
			END

		IF NOT EXISTS (SELECT * FROM sysobjects where name = 'NotificationGroupStatus' AND xtype = 'U')
			BEGIN
				CREATE TABLE [dbo].[NotificationGroupStatus]
					(
						[ID] [int] IDENTITY(1,1) NOT NULL,
						[KeyId] [int] NOT NULL,
						PRIMARY KEY ([Id]),
						FOREIGN KEY ([KeyId]) REFERENCES [LanguageKeys]([ID])
					);
					PRINT 'NotificationGroupStatus table has been created.';
			END
		ELSE
			BEGIN
				PRINT 'WARNING: NotificationGroupStatus table has not been created, since it already exists.'
			END

		IF NOT EXISTS (SELECT * FROM sysobjects where name = 'NotificationGroup' AND xtype = 'U')
			BEGIN
				CREATE TABLE [dbo].[NotificationGroup]
						(
							[ID] [int] IDENTITY(1,1) NOT NULL,
							[UniqueId] UNIQUEIDENTIFIER NOT NULL,
							[Description] [nvarchar](1024) NOT NULL,
							[TemplateId] [int] NOT NULL,
							[StartDate] [datetime] NOT NULL,
							[CompletedDate] [datetime] NULL,
							[Status] [int] NOT NULL,
							[IsDeleted] [bit] NOT NULL,
							[Created] [datetime] NOT NULL,
							[Changed] [datetime] NOT NULL,
							[CreatedBy] [int] NOT NULL,
							[ChangedBy] [int] NOT NULL,
							CONSTRAINT PK_NotificationGroupId PRIMARY KEY ([Id]),
							CONSTRAINT FK_Group_TemplateId FOREIGN KEY ([TemplateId]) REFERENCES [NotificationTemplate]([ID]),
							CONSTRAINT FK_Group_Status FOREIGN KEY ([Status]) REFERENCES [NotificationGroupStatus]([ID])
						);

				PRINT 'NotificationGroup table has been created.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: NotificationGroup table has not been created, since it already exists.'
			END

		IF NOT EXISTS (SELECT * FROM sysobjects where name = 'NotificationListStatus' AND xtype = 'U')
			BEGIN
				CREATE TABLE [dbo].[NotificationListStatus]
					(
						[ID] [int] IDENTITY(1,1) NOT NULL,
						[KeyId] [int] NOT NULL,
						PRIMARY KEY ([Id]),
						FOREIGN KEY ([KeyId]) REFERENCES [LanguageKeys]([ID])
					);
					PRINT 'NotificationListStatus table has been created.';
			END
		ELSE
			BEGIN
				PRINT 'WARNING: NotificationListStatus table has not been created, since it already exists.'
			END

		IF NOT EXISTS (SELECT * FROM sysobjects where name = 'NotificationList' AND xtype = 'U')
			BEGIN
				CREATE TABLE [dbo].[NotificationList]
						(
							[ID] [int] IDENTITY(1,1) NOT NULL,
							[PatientId] [int] NOT NULL,
							[GroupId] [int] NOT NULL,
							[StartDate] [datetime] NULL,
							[SendDate] [datetime] NULL,
							[Status] [int] NOT NULL,
							[ErrorDescription] [nvarchar](2048) NULL,
							CONSTRAINT PK_NotificationListId PRIMARY KEY ([Id]),
							CONSTRAINT FK_List_PatientId FOREIGN KEY ([PatientId]) REFERENCES [Patient]([ID]),
							CONSTRAINT FK_List_GroupId FOREIGN KEY ([GroupId]) REFERENCES [NotificationGroup]([ID]),
							CONSTRAINT FK_List_Status FOREIGN KEY ([Status]) REFERENCES [NotificationListStatus]([ID]),
							CONSTRAINT UC_PatientIdGroupId UNIQUE ([PatientId], [GroupId])
						);

				PRINT 'NotificationList table has been created.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: NotificationList table has not been created, since it already exists.'
			END

		IF NOT EXISTS(SELECT * FROM sysobjects WHERE xtype = 'UQ' AND name = 'uc_nvKey')
			BEGIN
				ALTER TABLE [Setting] ADD CONSTRAINT uc_nvKey UNIQUE ([nvKey])
				PRINT 'Unique constrant was added for nvKey column of the Setting table.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: Unique constrant was not added for nvKey column of the Setting table, since it already exists.'
			END

		IF NOT EXISTS(SELECT * FROM sysobjects WHERE xtype = 'UQ' AND name = 'uc_UpdateVersion')
			BEGIN
				ALTER TABLE [SystemUpdates] ADD CONSTRAINT uc_UpdateVersion UNIQUE ([UpdateVersion])
				PRINT 'Unique constrant was added for UpdateVersion column of the SystemUpdates table.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: Unique constrant was not added for UpdateVersion column of the SystemUpdates table, since it already exists.'
			END

		IF NOT EXISTS(SELECT * FROM sysobjects WHERE xtype = 'UQ' AND name = 'uc_UpdateVersionInt')
			BEGIN
				ALTER TABLE [SystemUpdates] ADD CONSTRAINT uc_UpdateVersionInt UNIQUE ([UpdateVersionInt])
				PRINT 'Unique constrant was added for UpdateVersionInt column of the SystemUpdates table.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: Unique constrant was not added for UpdateVersionInt column of the SystemUpdates table, since it already exists.'
			END

		IF NOT EXISTS(SELECT * FROM sysobjects WHERE xtype = 'UQ' AND name = 'uc_Key')
			BEGIN
				ALTER TABLE [LanguageKeys] ADD CONSTRAINT uc_Key UNIQUE ([Key])
				PRINT 'Unique constrant was added for Key column of the LanguageKeys table.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: Unique constrant was not added for Key column of the LanguageKeys table, since it already exists.'
			END

		IF NOT EXISTS (SELECT * FROM sys.columns  where name = N'IsEmailNotificationAllowed' AND Object_ID = Object_ID(N'Patient'))
			BEGIN
				ALTER TABLE [Patient] ADD [IsEmailNotificationAllowed] bit NOT NULL CONSTRAINT df_IsEmailNotificationAllowed DEFAULT (1);
			END

		PRINT 'Patient table has been altered.';

		SET @setting_key = N'EmailNotificationOn'
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

		SET @setting_key = N'SmtpEnableSsl'
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

		SET @setting_key = N'SmtpHost'
		SET @param = 'smtp.yandex.ru';

		IF NOT EXISTS (SELECT NULL FROM [Setting] WHERE nvKey = @setting_key)
			BEGIN
				INSERT INTO [Setting]
						([nvKey]
						,[nvValue])
					VALUES
						(@setting_key
						,@param)
			END

		SET @setting_key = N'SmtpPort'
		SET @int_param = 25;

		IF NOT EXISTS (SELECT NULL FROM [Setting] WHERE nvKey = @setting_key)
			BEGIN
				INSERT INTO [Setting]
						([nvKey]
						,[intValue])
					VALUES
						(@setting_key
						,@int_param)
			END

		SET @setting_key = N'SmtpFromAddress'
		SET @param = 'vindentlife@yandex.ru';

		IF NOT EXISTS (SELECT NULL FROM [Setting] WHERE nvKey = @setting_key)
			BEGIN
				INSERT INTO [Setting]
						([nvKey]
						,[nvValue])
					VALUES
						(@setting_key
						,@param)
			END

		SET @setting_key = N'SmtpPassword'
		SET @param = 'V!nDentl1fe#'; --Control question 'Dent Life'

		IF NOT EXISTS (SELECT NULL FROM [Setting] WHERE nvKey = @setting_key)
			BEGIN
				INSERT INTO [Setting]
						([nvKey]
						,[nvValue])
					VALUES
						(@setting_key
						,@param)
			END

		PRINT 'Setting table has been updated.';

	COMMIT TRANSACTION [Update10131Transaction]
	PRINT N'Script version ' + @update_version + ' has been executed.';
END TRY
BEGIN CATCH
	PRINT N'ERROR: Script version ' + @update_version + ' has not been executed.';
	ROLLBACK TRANSACTION [Update10131Transaction]
END CATCH

BEGIN TRY
	BEGIN TRANSACTION [Update10132Transaction]	
		-- Perform script version 1.0.13.2.
		SET @update_old_version = '1.0.13.1';
		SET @update_old_version_int = 10131;
		SET @update_version = '1.0.13.2';
		SET @update_version_int = 10132;
		SET @update_date = GETDATE();

		IF EXISTS (SELECT NULL FROM [SystemUpdates] WHERE [UpdateVersion] = @update_version)
			BEGIN
				PRINT N'ERROR: Script version ' + @update_version + ' already exist in the database.'
				RaisError('ERROR: Script version %s already exist in the database. Update was aborted.', 15, 1, @update_version);
			END

		SET @current_version_int = (SELECT TOP 1 [UpdateVersionInt] FROM [SystemUpdates] ORDER BY [ID] DESC);
		IF (@current_version_int != @update_old_version_int)
			BEGIN
				PRINT N'ERROR: Script version ' + @update_old_version + ' has to be applied first.';
				RaisError(N'ERROR: Script version %s has to be applied first. Update was aborted.', 15, 1, @update_old_version);
			END

		IF NOT EXISTS (SELECT NULL FROM [SystemUpdates] WHERE [UpdateVersion] = @update_version)
			BEGIN
				INSERT INTO [SystemUpdates]
									([UpdateInformation],
									[UpdateVersion],
									[UpdateVersionInt],
									[UpdateDate])
							 VALUES
									('E-mail notifications.',
									@update_version,
									@update_version_int,
									@update_date);

				PRINT 'SystemUpdates table was updated.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: SystemUpdates table was not updated.'
			END

		SET @key_name = N'NotificationListStatusNotSent';

		IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
			BEGIN
				INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);
				
				SET @key_id = @@IDENTITY;
				
				IF NOT EXISTS (SELECT NULL FROM [NotificationListStatus] WHERE [KeyId] = @key_id)
					BEGIN
						INSERT INTO [NotificationListStatus] ([KeyId]) VALUES (@key_id);
					END

				SET @name_of_status = N'Not sent';
				SET @culture_id = 1;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Не отправлено';
				SET @culture_id = 2;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Не надіслано';
				SET @culture_id = 3;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END
			END

		SET @key_name = N'NotificationListStatusSuccess';

		IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
			BEGIN
				INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

				SET @key_id = @@IDENTITY;

				IF NOT EXISTS (SELECT NULL FROM [NotificationListStatus] WHERE [KeyId] = @key_id)
					BEGIN
						INSERT INTO [NotificationListStatus] ([KeyId]) VALUES (@key_id);
					END

				SET @name_of_status = N'Success';
				SET @culture_id = 1;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Успешно';
				SET @culture_id = 2;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Успішно';
				SET @culture_id = 3;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END
			END

		SET @key_name = N'NotificationListStatusFailed';

		IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
			BEGIN
				INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

				SET @key_id = @@IDENTITY;

				IF NOT EXISTS (SELECT NULL FROM [NotificationListStatus] WHERE [KeyId] = @key_id)
					BEGIN
						INSERT INTO [NotificationListStatus] ([KeyId]) VALUES (@key_id);
					END

				SET @name_of_status = N'Fail';
				SET @culture_id = 1;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Неуспешно';
				SET @culture_id = 2;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Неуспішно';
				SET @culture_id = 3;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END
			END

		PRINT 'Notification status lookup table has been localized.'

		SET @key_name = N'NotificationGroupStatusNotProcessed';

		IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
			BEGIN
				INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

				SET @key_id = @@IDENTITY;

				IF NOT EXISTS (SELECT NULL FROM [NotificationGroupStatus] WHERE [KeyId] = @key_id)
					BEGIN
						INSERT INTO [NotificationGroupStatus] ([KeyId]) VALUES (@key_id);
					END

				SET @name_of_status = N'Not processed';
				SET @culture_id = 1;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Не обработана';
				SET @culture_id = 2;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Не оброблена';
				SET @culture_id = 3;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END
			END

		SET @key_name = N'NotificationGroupStatusProcessing';

		IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
			BEGIN
				INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

				SET @key_id = @@IDENTITY;

				IF NOT EXISTS (SELECT NULL FROM [NotificationGroupStatus] WHERE [KeyId] = @key_id)
					BEGIN
						INSERT INTO [NotificationGroupStatus] ([KeyId]) VALUES (@key_id);
					END

				SET @name_of_status = N'Processing';
				SET @culture_id = 1;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Обрабатывается';
				SET @culture_id = 2;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Обробляється';
				SET @culture_id = 3;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END
			END

		SET @key_name = N'NotificationGroupStatusCancelled';

		IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
			BEGIN
				INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

				SET @key_id = @@IDENTITY;

				IF NOT EXISTS (SELECT NULL FROM [NotificationGroupStatus] WHERE [KeyId] = @key_id)
					BEGIN
						INSERT INTO [NotificationGroupStatus] ([KeyId]) VALUES (@key_id);
					END

				SET @name_of_status = N'Cancelled';
				SET @culture_id = 1;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Отменено';
				SET @culture_id = 2;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END
				
				SET @name_of_status = N'Відмінено';
				SET @culture_id = 3;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END
			END

		SET @key_name = N'NotificationGroupStatusProcessed';

		IF NOT EXISTS (SELECT NULL FROM [LanguageKeys] WHERE [Key] = @key_name)
			BEGIN
				INSERT INTO [LanguageKeys] ([Key]) VALUES (@key_name);

				SET @key_id = @@IDENTITY;

				IF NOT EXISTS (SELECT NULL FROM [NotificationGroupStatus] WHERE [KeyId] = @key_id)
					BEGIN
						INSERT INTO [NotificationGroupStatus] ([KeyId]) VALUES (@key_id);
					END

				SET @name_of_status = N'Processed';
				SET @culture_id = 1;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Обработана';
				SET @culture_id = 2;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END

				SET @name_of_status = N'Оброблена';
				SET @culture_id = 3;

				IF NOT EXISTS (SELECT NULL FROM [LanguageData] WHERE [KeyId] = @key_id AND [Value] = @name_of_status AND [CultureId] = @culture_id)
					BEGIN
						INSERT INTO [LanguageData] 
									([KeyId],
									[Value],
									[CultureId])
						VALUES		(@key_id,
									@name_of_status,
									@culture_id);
					END
			END

		PRINT 'Notification group status lookup table has been localized.'

	COMMIT TRANSACTION [Update10132Transaction]
	PRINT N'Script version ' + @update_version + ' has been executed.';
END TRY
BEGIN CATCH
	PRINT N'ERROR: Script version ' + @update_version + ' has not been executed.';
	ROLLBACK TRANSACTION [Update10132Transaction]
END CATCH