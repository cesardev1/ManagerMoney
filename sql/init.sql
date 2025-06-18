CREATE DATABASE ManejoPresupuesto;
GO

USE ManejoPresupuesto
GO
/****** Object:  Table [dbo].[Categorias]    Script Date: 10-Nov-21 5:23:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [OperationTypeId] [int] NOT NULL,
    [UserId] [int] NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 10-Nov-21 5:23:42 PM ******/
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[Account](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [AccountTypeId] [int] NOT NULL,
    [Balance] [decimal](18, 2) NOT NULL,
    [Description] [nvarchar](1000) NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[TiposAccounts]    Script Date: 10-Nov-21 5:23:42 PM ******/
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[AccountType](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [UserId] [int] NOT NULL,
    [OrderBy] [int] NOT NULL,
    CONSTRAINT [PK_AccountType] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[TiposOperaciones]    Script Date: 10-Nov-21 5:23:42 PM ******/
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[OperationType](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Description] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_OperationsType] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
    GO

    SET IDENTITY_INSERT [dbo].[OperationType] ON
    GO
    INSERT [dbo].[OperationType] ([Id], [Description]) VALUES (1, N'Income')
    GO
    INSERT [dbo].[OperationType] ([Id], [Description]) VALUES (2, N'Expense')
    GO
    SET IDENTITY_INSERT [dbo].[OperationType] OFF
    GO


/****** Object:  Table [dbo].[Transactions]    Script Date: 10-Nov-21 5:23:42 PM ******/
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[Transaction](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NOT NULL,
    [DateTransaction] [datetime] NOT NULL,
    [Amount] [decimal](18, 2) NOT NULL,
    [OperationTypeId] [int] NOT NULL,
    [Note] [nvarchar](1000) NULL,
    [AccountId] [int] NOT NULL,
    [CategoryId] [int] NOT NULL,
    CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 10-Nov-21 5:23:42 PM ******/
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[Users](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Email] [nvarchar](256) NOT NULL,
    [EmailNormalizado] [nvarchar](256) NOT NULL,
    [PasswordHash] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
ALTER TABLE [dbo].[Categories]  WITH CHECK ADD  CONSTRAINT [FK_Categories_OperationType] FOREIGN KEY([OperationTypeId])
    REFERENCES [dbo].[OperationType] ([Id])
    GO
ALTER TABLE [dbo].[Categories] CHECK CONSTRAINT [FK_Categories_OperationType]
    GO
ALTER TABLE [dbo].[Categories]  WITH CHECK ADD  CONSTRAINT [FK_Categories_Users] FOREIGN KEY([UserId])
    REFERENCES [dbo].[Users] ([Id])
    GO
ALTER TABLE [dbo].[Categories] CHECK CONSTRAINT [FK_Categories_Users]
    GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_AccountType] FOREIGN KEY([AccountTypeId])
    REFERENCES [dbo].[AccountType] ([Id])
    GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_AccountType]
    GO
ALTER TABLE [dbo].[AccountType]  WITH CHECK ADD  CONSTRAINT [FK_AccountType_Users] FOREIGN KEY([UserId])
    REFERENCES [dbo].[Users] ([Id])
    GO
ALTER TABLE [dbo].[AccountType] CHECK CONSTRAINT [FK_AccountType_Users]
    GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Categories] FOREIGN KEY([CategoryId])
    REFERENCES [dbo].[Categories] ([Id])
    GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Categories]
    GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Accounts] FOREIGN KEY([AccountId])
    REFERENCES [dbo].[Account] ([Id])
    GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Accounts]
    GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_OperationType] FOREIGN KEY([OperationTypeId])
    REFERENCES [dbo].[OperationType] ([Id])
    GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_OperationType]
    GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Users] FOREIGN KEY([UserId])
    REFERENCES [dbo].[Users] ([Id])
    GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Users]
    GO
/****** Object:  StoredProcedure [dbo].[Transactions_Insertar]    Script Date: 10-Nov-21 5:23:42 PM ******/
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Transactions_Insertar]
    @UserId nvarchar(450),
    @DateTransaction date,
    @Amount decimal(18,2),
    @OperationTypeId int,
    @Note nvarchar(1000) = NULL
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
INSERT INTO [Transaction](UserId, DateTransaction, Amount, OperationTypeId, Note)
Values(@UserId, @DateTransaction, @Amount, @OperationTypeId, @Note)
END
GO
/****** Object:  StoredProcedure [dbo].[Transactions_SelectConTipoOperacion]    Script Date: 10-Nov-21 5:23:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Transactions_SelectConTipoOperacion]
@dateTransaction DATE
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

Select [Transaction].Id, UserId, Amount, Note, Description
From [Transaction]
    INNER JOIN OperationType
ON [Transaction].OperationTypeId = OperationType.Id
WHERE DateTransaction = @dateTransaction
ORDER BY UserId DESC
END
GO