USE [Demo]
GO
/****** Object:  Table [dbo].[a_account]    Script Date: 05/08/2014 13:39:55 ******/
ALTER TABLE [dbo].[a_account] DROP CONSTRAINT [DF_a_account_PlayerID]
GO
ALTER TABLE [dbo].[a_account] DROP CONSTRAINT [DF_a_account_SessionKey]
GO
ALTER TABLE [dbo].[a_account] DROP CONSTRAINT [DF_a_account_UpdateDate]
GO
ALTER TABLE [dbo].[a_account] DROP CONSTRAINT [DF_a_account_IsStop]
GO
DROP TABLE [dbo].[a_account]
GO
/****** Object:  Table [dbo].[a_account_stop_id]    Script Date: 05/08/2014 13:39:55 ******/
DROP TABLE [dbo].[a_account_stop_id]
GO
/****** Object:  Table [dbo].[a_member]    Script Date: 05/08/2014 13:39:55 ******/
ALTER TABLE [dbo].[a_member] DROP CONSTRAINT [DF_a_member_Money]
GO
ALTER TABLE [dbo].[a_member] DROP CONSTRAINT [DF_a_member_Coin]
GO
DROP TABLE [dbo].[a_member]
GO
/****** Object:  Table [dbo].[l_webmethod]    Script Date: 05/08/2014 13:39:56 ******/
ALTER TABLE [dbo].[l_webmethod] DROP CONSTRAINT [DF_l_webmethod_ActionDate]
GO
ALTER TABLE [dbo].[l_webmethod] DROP CONSTRAINT [DF_l_webmethod_PID]
GO
ALTER TABLE [dbo].[l_webmethod] DROP CONSTRAINT [DF_l_webmethod_MethodName]
GO
ALTER TABLE [dbo].[l_webmethod] DROP CONSTRAINT [DF_l_webmethod_Args]
GO
DROP TABLE [dbo].[l_webmethod]
GO
/****** Object:  Table [dbo].[l_webmethod]    Script Date: 05/08/2014 13:39:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[l_webmethod](
	[MethodLogID] [int] IDENTITY(1,1) NOT NULL,
	[ActionDate] [datetime] NOT NULL CONSTRAINT [DF_l_webmethod_ActionDate]  DEFAULT (getdate()),
	[PID] [int] NOT NULL CONSTRAINT [DF_l_webmethod_PID]  DEFAULT ((0)),
	[MethodName] [nvarchar](max) NOT NULL CONSTRAINT [DF_l_webmethod_MethodName]  DEFAULT (''),
	[Args] [nvarchar](max) NOT NULL CONSTRAINT [DF_l_webmethod_Args]  DEFAULT (''),
 CONSTRAINT [PK_l_webmethod] PRIMARY KEY CLUSTERED 
(
	[MethodLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[a_member]    Script Date: 05/08/2014 13:39:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[a_member](
	[PlayerID] [int] IDENTITY(1,1) NOT NULL,
	[PlayerName] [nvarchar](20) NOT NULL,
	[Money] [int] NOT NULL CONSTRAINT [DF_a_member_Money]  DEFAULT ((0)),
	[Coin] [int] NOT NULL CONSTRAINT [DF_a_member_Coin]  DEFAULT ((0)),
 CONSTRAINT [PK_a_member] PRIMARY KEY CLUSTERED 
(
	[PlayerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[a_account_stop_id]    Script Date: 05/08/2014 13:39:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[a_account_stop_id](
	[StopID] [int] IDENTITY(1,1) NOT NULL,
	[StopName] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_a_account_stop_id] PRIMARY KEY CLUSTERED 
(
	[StopID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[a_account]    Script Date: 05/08/2014 13:39:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[a_account](
	[AccountID] [int] IDENTITY(3,1) NOT NULL,
	[Account] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
	[PlayerID] [int] NOT NULL CONSTRAINT [DF_a_account_PlayerID]  DEFAULT ((0)),
	[SessionKey] [nvarchar](100) NOT NULL CONSTRAINT [DF_a_account_SessionKey]  DEFAULT (''),
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_a_account_UpdateDate]  DEFAULT (getdate()),
	[IsStop] [int] NOT NULL CONSTRAINT [DF_a_account_IsStop]  DEFAULT ((0)),
	[StopDate] [datetime] NULL,
 CONSTRAINT [PK_a_account] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
