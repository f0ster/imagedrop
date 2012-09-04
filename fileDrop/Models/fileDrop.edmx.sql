
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 09/04/2012 16:42:34
-- Generated from EDMX file: D:\code\visual_ink\fileDrop\fileDrop\Models\fileDrop.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [fileDrop];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DroppedFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DroppedFiles];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DroppedFiles'
CREATE TABLE [dbo].[DroppedFiles] (
    [id] int IDENTITY(1,1) NOT NULL,
    [DateCreated] datetime  NULL,
    [FName] varchar(255)  NULL,
    [UploaderIP] varchar(39)  NULL,
    [Caption] varchar(255)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'DroppedFiles'
ALTER TABLE [dbo].[DroppedFiles]
ADD CONSTRAINT [PK_DroppedFiles]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------