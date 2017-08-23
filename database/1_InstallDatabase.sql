-- =============================================
-- Author:		LinuxChata
-- Create date:	04.12.2012
-- Description:	Perform database installation
-- =============================================

USE [master]
GO

IF EXISTS (SELECT * FROM sys.databases where name = 'dentist')
	BEGIN
		ALTER DATABASE [dentist] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
		PRINT 'dentist database has been switched to single user mode.';
	END
GO

-- Delete dentist database
IF EXISTS (SELECT * FROM sys.databases where name = 'dentist')
	BEGIN
		DROP DATABASE [dentist];
		PRINT 'dentist database has been dropped.';
	END
ELSE
	BEGIN
		PRINT 'dentist database does not exist.';
	END
GO

USE [master]
GO

-- Create dentist database
IF NOT EXISTS (SELECT * FROM sys.databases where name = 'dentist')
	BEGIN
		CREATE DATABASE [dentist];		
		ALTER DATABASE [dentist] SET COMPATIBILITY_LEVEL = 100;

		IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
		BEGIN
			EXEC [dentist].[dbo].[sp_fulltext_database] @action = 'enable';
		END

		ALTER DATABASE [dentist] SET ANSI_NULL_DEFAULT OFF;
		ALTER DATABASE [dentist] SET ANSI_NULLS OFF;
		ALTER DATABASE [dentist] SET ANSI_PADDING OFF;
		ALTER DATABASE [dentist] SET ANSI_WARNINGS OFF;
		ALTER DATABASE [dentist] SET ARITHABORT OFF;
		ALTER DATABASE [dentist] SET AUTO_CLOSE ON;
		ALTER DATABASE [dentist] SET AUTO_CREATE_STATISTICS ON;
		ALTER DATABASE [dentist] SET AUTO_SHRINK OFF;
		ALTER DATABASE [dentist] SET AUTO_UPDATE_STATISTICS ON;
		ALTER DATABASE [dentist] SET CURSOR_CLOSE_ON_COMMIT OFF;
		ALTER DATABASE [dentist] SET CURSOR_DEFAULT GLOBAL;
		ALTER DATABASE [dentist] SET CONCAT_NULL_YIELDS_NULL OFF;
		ALTER DATABASE [dentist] SET NUMERIC_ROUNDABORT OFF;
		ALTER DATABASE [dentist] SET QUOTED_IDENTIFIER OFF;
		ALTER DATABASE [dentist] SET RECURSIVE_TRIGGERS OFF;
		ALTER DATABASE [dentist] SET ENABLE_BROKER;
		ALTER DATABASE [dentist] SET AUTO_UPDATE_STATISTICS_ASYNC OFF;
		ALTER DATABASE [dentist] SET DATE_CORRELATION_OPTIMIZATION OFF;
		ALTER DATABASE [dentist] SET TRUSTWORTHY OFF;
		ALTER DATABASE [dentist] SET ALLOW_SNAPSHOT_ISOLATION OFF;
		ALTER DATABASE [dentist] SET PARAMETERIZATION SIMPLE;
		ALTER DATABASE [dentist] SET READ_COMMITTED_SNAPSHOT OFF;
		ALTER DATABASE [dentist] SET HONOR_BROKER_PRIORITY OFF;
		ALTER DATABASE [dentist] SET READ_WRITE;
		ALTER DATABASE [dentist] SET RECOVERY SIMPLE;
		ALTER DATABASE [dentist] SET MULTI_USER;
		ALTER DATABASE [dentist] SET PAGE_VERIFY CHECKSUM;	
		ALTER DATABASE [dentist] SET DB_CHAINING OFF;

		PRINT 'dentist database has been created.';
	END
GO