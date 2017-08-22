USE [master]
GO

IF EXISTS (SELECT * FROM sys.databases where name = 'dentist')
BEGIN
	ALTER DATABASE [dentist] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
END

GO

IF EXISTS (SELECT * FROM sys.databases where name = 'dentist')
	BEGIN
		DROP DATABASE [dentist];
		PRINT 'Database has been dropped.'
	END
ELSE
	BEGIN
		PRINT 'Database does not exist.'
	END