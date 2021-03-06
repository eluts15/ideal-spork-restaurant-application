CREATE DATABASE cuisine
GO
USE [cuisine]
GO
/****** Object:  Table [dbo].[cuisines]    Script Date: 6/7/2017 4:37:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cuisines](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[type] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[restaurants]    Script Date: 6/7/2017 4:37:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[restaurants](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[cuisine_id] [int] NULL,
	[city] [varchar](50) NULL,
	[rating] [int] NULL
) ON [PRIMARY]

GO
