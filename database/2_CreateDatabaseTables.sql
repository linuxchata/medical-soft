-- =============================================
-- Author:		LinuxChata
-- Create date:	04.12.2012
-- Description:	Perform database installation
-- =============================================

USE [dentist]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Appointment](
	[ID] [uniqueidentifier] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Doctor] [nvarchar](200) NULL,
	[DoctorId] [int] NOT NULL,
	[Patient] [nvarchar](200) NULL,
	[PatientId] [int] NOT NULL,
	[Comment] [nvarchar](200) NULL,
	[Created] [datetime] NOT NULL,
	[Changed] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ChangedBy] [int] NOT NULL,
 CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--========================================================================================
--========================================================================================

CREATE TABLE [dbo].[BackupLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BackupTypesId] [int] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NOT NULL,
	[FileName] [nvarchar](200) NOT NULL,
	[Status] [nvarchar](4000) NOT NULL,
	[StartedBy] [int] NOT NULL,
 CONSTRAINT [PK_BackupLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--========================================================================================
--========================================================================================

CREATE TABLE [dbo].[BackupTypes](
	[RowId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CultureId] [int] NOT NULL,
 CONSTRAINT [PK_BackupTypes] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--========================================================================================
--========================================================================================

CREATE TABLE [dbo].[Culture](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](5) NOT NULL,
	[Description] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--========================================================================================
--========================================================================================

CREATE TABLE [dbo].[Education](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[ShortName] [nvarchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Educatio__3214EC2707020F21] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--========================================================================================
--========================================================================================

CREATE TABLE [dbo].[Login](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LoginName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[IsCanLogin] [bit] NOT NULL,
	[IsHaveToChangePass] [bit] NOT NULL,
	[RoleInSystem] [int] NOT NULL,
	[StaffID] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsSA] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Changed] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ChangedBy] [int] NOT NULL,
 CONSTRAINT [PK__Login__3214EC270AD2A005] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--========================================================================================
--========================================================================================

CREATE TABLE [dbo].[Patient](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CardNumber] [int] NOT NULL,
	[RegistrationDate] [datetime] NOT NULL,
	[Job] [nvarchar](200) NULL,
	[Profession] [nvarchar](100) NULL,
	[SurName] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NULL,
	[Sex] [int] NOT NULL,
	[Address] [nvarchar](200) NULL,
	[PhoneNumberHome] [nvarchar](50) NULL,
	[PhoneNumberWork] [nvarchar](50) NULL,
	[PhoneNumberCell] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Birthday] [datetime] NULL,
	[Comments] [nvarchar](300) NULL,
	[IsDeleted] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Changed] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ChangedBy] [int] NOT NULL,
 CONSTRAINT [PK__Patient__3214EC270EA330E9] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--========================================================================================
--========================================================================================

CREATE TABLE [dbo].[Position](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Position__3214EC271273C1CD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--========================================================================================
--========================================================================================

CREATE TABLE [dbo].[Role](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--========================================================================================
--========================================================================================

CREATE TABLE [dbo].[Setting](
	[nvKey] [nvarchar](100) NOT NULL,
	[nvValue] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[nvKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--========================================================================================
--========================================================================================

CREATE TABLE [dbo].[Sexs](
	[RowId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CultureId] [int] NOT NULL,
 CONSTRAINT [PK_Sexs] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--========================================================================================
--========================================================================================

CREATE TABLE [dbo].[Staff](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EducationID] [int] NOT NULL,
	[PositionID] [int] NOT NULL,
	[SurName] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NULL,
	[Sex] [int] NOT NULL,
	[Address] [nvarchar](200) NULL,
	[PhoneNumberHome] [nvarchar](50) NULL,
	[PhoneNumberWork] [nvarchar](50) NULL,
	[PhoneNumberCell] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Birthday] [datetime] NULL,
	[IsTaking] [bit] NOT NULL,
	[Comments] [nvarchar](300) NULL,
	[IsDeleted] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Changed] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ChangedBy] [int] NOT NULL,
 CONSTRAINT [PK__Staff__3214EC271DE57479] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Patient] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patient] ([ID])
GO

ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_Patient]
GO

ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Staff] FOREIGN KEY([DoctorId])
REFERENCES [dbo].[Staff] ([ID])
GO

ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_Staff]
GO

ALTER TABLE [dbo].[Appointment] ADD  CONSTRAINT [DF_Appointment_ID]  DEFAULT (newid()) FOR [ID]
GO

ALTER TABLE [dbo].[BackupTypes]  WITH CHECK ADD  CONSTRAINT [FK_BackupTypes_Culture] FOREIGN KEY([CultureId])
REFERENCES [dbo].[Culture] ([ID])
GO

ALTER TABLE [dbo].[BackupTypes] CHECK CONSTRAINT [FK_BackupTypes_Culture]
GO

ALTER TABLE [dbo].[Login]  WITH CHECK ADD  CONSTRAINT [FK_Login_Role] FOREIGN KEY([RoleInSystem])
REFERENCES [dbo].[Role] ([ID])
GO

ALTER TABLE [dbo].[Login] CHECK CONSTRAINT [FK_Login_Role]
GO

ALTER TABLE [dbo].[Sexs]  WITH CHECK ADD  CONSTRAINT [FK_Sexs_Culture] FOREIGN KEY([CultureId])
REFERENCES [dbo].[Culture] ([ID])
GO

ALTER TABLE [dbo].[Sexs] CHECK CONSTRAINT [FK_Sexs_Culture]
GO

ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_Education] FOREIGN KEY([EducationID])
REFERENCES [dbo].[Education] ([ID])
GO

ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_Education]
GO

ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_Position] FOREIGN KEY([PositionID])
REFERENCES [dbo].[Position] ([ID])
GO

ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_Position]
GO