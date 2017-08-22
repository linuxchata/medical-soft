-- =========================================================
-- Author:			LinuxChata
-- Created date:	20.12.2013
-- Modified date:	20.12.2013
-- Description:		Perform update 1.0.17.2 of the database.
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

BEGIN TRY
	BEGIN TRANSACTION [Update10171Transaction]
		-- Perform script version 1.0.17.1.
		SET @update_old_version = '1.0.16.2';
		SET @update_old_version_int = 10162;
		SET @update_version = '1.0.17.1';
		SET @update_version_int = 10171;
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
									('Update smtp e-mail configuration.',
									@update_version,
									@update_version_int,
									@update_date);

				PRINT 'SystemUpdates table was updated.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: SystemUpdates table was not updated.'
			END

		IF(NOT EXISTS(SELECT * FROM sys.columns WHERE name = N'Id' AND OBJECT_ID = OBJECT_ID(N'Setting')))
		BEGIN
			PRINT 'Add column Id to the Setting table.';
			ALTER TABLE [Setting] ADD [Id] INT NULL;
		END

		UPDATE [Setting] SET [nvKey] = 'EmailNotificationIsOn' WHERE [nvKey] = 'EmailNotificationOn';
		UPDATE [Setting] SET [nvKey] = 'EmailSendingDelayInSeconds' WHERE [nvKey] = 'EmailNotificationSendingDelaySec';

		UPDATE [Setting] SET [nvKey] = 'ReminderIsOn' WHERE [nvKey] = 'NotificationOn';
		UPDATE [Setting] SET [nvKey] = 'ReminderCheckDelay' WHERE [nvKey] = 'NotificationDurationInMinutes';

		UPDATE [Setting] SET [nvKey] = 'BackupLocation' WHERE [nvKey] = 'LocationOfTheBackUp';
		UPDATE [Setting] SET [nvKey] = 'BackupDatabaseName' WHERE [nvKey] = 'NameOfTheBackUpDB';
		UPDATE [Setting] SET [nvKey] = 'BackupFileName' WHERE [nvKey] = 'NameOfTheBackUpFile';

		INSERT INTO [Setting] ([nvKey], [nvValue])	VALUES('CompanyName', 'Company Name');
		INSERT INTO [Setting] ([nvKey], [bitValue])	VALUES('BackupIsOn', 1);
		INSERT INTO [Setting] ([nvKey], [nvValue])	VALUES('BackupHour', '18');
		INSERT INTO [Setting] ([nvKey], [nvValue])	VALUES('BackupMinute', '00');
		INSERT INTO [Setting] ([nvKey], [intValue])	VALUES('BackupDelayInDays', 3);
		INSERT INTO [Setting] ([nvKey], [nvValue])	VALUES('Code', '');

		COMMIT TRANSACTION [Update10171Transaction]
	PRINT N'Script version ' + @update_version + ' has been executed.';
END TRY
BEGIN CATCH
	PRINT N'ERROR: Script version ' + @update_version + ' has not been executed.';
	ROLLBACK TRANSACTION [Update10171Transaction]
END CATCH

GO

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

BEGIN TRY
	BEGIN TRANSACTION [Update10172Transaction]
		-- Perform script version 1.0.17.2.
		SET @update_old_version = '1.0.17.1';
		SET @update_old_version_int = 10171;
		SET @update_version = '1.0.17.2';
		SET @update_version_int = 10172;
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
									('Update smtp e-mail configuration.',
									@update_version,
									@update_version_int,
									@update_date);

				PRINT 'SystemUpdates table was updated.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: SystemUpdates table was not updated.'
			END

		UPDATE [Setting] SET [Id] = 1 WHERE [nvKey] = 'CompanyName';
		UPDATE [Setting] SET [Id] = 2 WHERE [nvKey] = 'Language';
		UPDATE [Setting] SET [Id] = 3 WHERE [nvKey] = 'BackupIsOn';
		UPDATE [Setting] SET [Id] = 4 WHERE [nvKey] = 'BackupDelayInDays';
		UPDATE [Setting] SET [Id] = 5 WHERE [nvKey] = 'BackupDatabaseName';
		UPDATE [Setting] SET [Id] = 6 WHERE [nvKey] = 'BackupLocation';
		UPDATE [Setting] SET [Id] = 7 WHERE [nvKey] = 'BackupFileName';
		UPDATE [Setting] SET [Id] = 8 WHERE [nvKey] = 'BackupHour';
		UPDATE [Setting] SET [Id] = 9 WHERE [nvKey] = 'BackupMinute';
		UPDATE [Setting] SET [Id] = 10 WHERE [nvKey] = 'ReminderIsOn';
		UPDATE [Setting] SET [Id] = 11 WHERE [nvKey] = 'ReminderCheckDelay';
		UPDATE [Setting] SET [Id] = 12 WHERE [nvKey] = 'EmailNotificationIsOn';
		UPDATE [Setting] SET [Id] = 13 WHERE [nvKey] = 'EmailSendingDelayInSeconds';
		UPDATE [Setting] SET [Id] = 14 WHERE [nvKey] = 'SmtpEnableSsl';
		UPDATE [Setting] SET [Id] = 15 WHERE [nvKey] = 'SmtpHost';
		UPDATE [Setting] SET [Id] = 16 WHERE [nvKey] = 'SmtpPort';
		UPDATE [Setting] SET [Id] = 17 WHERE [nvKey] = 'SmtpFromAddress';
		UPDATE [Setting] SET [Id] = 18 WHERE [nvKey] = 'SmtpUserName';
		UPDATE [Setting] SET [Id] = 19 WHERE [nvKey] = 'SmtpPassword';
		UPDATE [Setting] SET [Id] = 20 WHERE [nvKey] = 'Code';

		IF(EXISTS(SELECT * FROM sys.columns WHERE name = N'Id' AND OBJECT_ID = OBJECT_ID(N'Setting')))
		BEGIN
			PRINT 'Add column Id to the Setting table.';

			IF(EXISTS((SELECT name FROM sys.key_constraints WHERE [type] = 'PK' AND [parent_object_id] = Object_id('Setting'))))
			BEGIN
				DECLARE @ConstraintName NVARCHAR(255) = (SELECT name FROM sys.key_constraints WHERE [type] = 'PK' AND [parent_object_id] = Object_id('Setting'));
				DECLARE @SQL VARCHAR(4000) = 'ALTER TABLE [Setting] DROP CONSTRAINT ' + @ConstraintName;
				EXEC (@SQL);
			END

			ALTER TABLE [Setting] ALTER COLUMN [Id] INT NOT NULL;

			IF(NOT EXISTS((SELECT name FROM sys.key_constraints WHERE [type] = 'PK' AND [parent_object_id] = Object_id('Setting'))))
			BEGIN
				ALTER TABLE [Setting] ADD CONSTRAINT pk_Id PRIMARY KEY ([Id]);
			END
		END

	COMMIT TRANSACTION [Update10172Transaction]
	PRINT N'Script version ' + @update_version + ' has been executed.';
END TRY
BEGIN CATCH
	PRINT N'ERROR: Script version ' + @update_version + ' has not been executed.';
	ROLLBACK TRANSACTION [Update10172Transaction]
END CATCH