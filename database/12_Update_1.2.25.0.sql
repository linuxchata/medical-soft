-- =========================================================
-- Author:			LinuxChata
-- Created date:	30.12.2015
-- Modified date:	30.12.2015
-- Description:		Perform update 1.2.25.0 of the database.
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

DECLARE @startHtml nvarchar(512);
DECLARE @endHtml nvarchar(512);

BEGIN TRY
	BEGIN TRANSACTION [Update1225Transaction]
		-- Perform script version 1.2.25.0.
		SET @update_old_version = '1.1.20.0';
		SET @update_old_version_int = 11200;
		SET @update_version = '1.2.25.0';
		SET @update_version_int = 1225;
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
							('Add Photo column to Patient table.',
							@update_version,
							@update_version_int,
							@update_date);

				PRINT 'SystemUpdates table was updated.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: SystemUpdates table was not updated.'
			END

		IF EXISTS (SELECT * FROM sysobjects where name = 'Patient' AND xtype = 'U')
			BEGIN
				IF NOT EXISTS (SELECT * FROM sys.columns  where name = N'Photo' AND Object_ID = Object_ID(N'Patient'))
					BEGIN
						ALTER TABLE [Patient] ADD [Photo] VARBINARY(MAX) NULL;
					END

				PRINT 'Patient table has been updated.'
			END
		ELSE
			BEGIN
				PRINT 'WARNING: Photo in Patient table has not been added, since it already exists.'
			END

	COMMIT TRANSACTION [Update1225Transaction]
	PRINT N'Script version ' + @update_version + ' has been executed.';
END TRY
BEGIN CATCH
	PRINT N'ERROR: Script version ' + @update_version + ' has not been executed.';
	ROLLBACK TRANSACTION [Update1225Transaction]
END CATCH

GO