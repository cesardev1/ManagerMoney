CREATE DATABASE BudgetManager;
GO

USE BudgetManager;
GO


/****** Object:  Table [dbo].[Categorias]    Script Date: 22-Dec-21 12:12:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(50) NOT NULL,
	[OperationTypeId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Cuentas]    Script Date: 22-Dec-21 12:12:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Accounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[AccountTypeId] [int] NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TiposCuentas]    Script Date: 22-Dec-21 12:12:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountsType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountsType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
	[OrderIndex] [int] NOT NULL,
 CONSTRAINT [PK_AccountsType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TiposOperaciones]    Script Date: 22-Dec-21 12:12:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OperationsType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OperationsType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_OperationsType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Transacciones]    Script Date: 22-Dec-21 12:12:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Transactions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Transactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Note] [nvarchar](1000) NULL,
	[AccountId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 22-Dec-21 12:12:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[NormalizedEmail] [nvarchar](256) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[OperationsType] ON 
GO
INSERT [dbo].[OperationsType] ( [Description]) VALUES ( N'Ingreso')
GO
INSERT [dbo].[OperationsType] ( [Description]) VALUES ( N'Gasto')
GO
SET IDENTITY_INSERT [dbo].[OperationsType] OFF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Categories_OperationsType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Categories]'))
ALTER TABLE [dbo].[Categories]  WITH CHECK ADD  CONSTRAINT [FK_Categories_OperationsType] FOREIGN KEY([OperationTypeId])
REFERENCES [dbo].[OperationsType] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Categories_OperationsType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Categories]'))
ALTER TABLE [dbo].[Categories] CHECK CONSTRAINT [FK_Categories_OperationsType]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Categories_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Categories]'))
ALTER TABLE [dbo].[Categories]  WITH CHECK ADD  CONSTRAINT [FK_Categories_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Categories_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Categories]'))
ALTER TABLE [dbo].[Categories] CHECK CONSTRAINT [FK_Categories_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Accounts_AccountsType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Accounts]'))
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_AccountsType] FOREIGN KEY([AccountTypeId])
REFERENCES [dbo].[AccountsType] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Accounts_AccountsType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Accounts]'))
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_AccountsType]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountsType_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountsType]'))
ALTER TABLE [dbo].[AccountsType]  WITH CHECK ADD  CONSTRAINT [FK_AccountsType_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountsType_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountsType]'))
ALTER TABLE [dbo].[AccountsType] CHECK CONSTRAINT [FK_AccountsType_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transactions_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Transactions]'))
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transactions_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Transactions]'))
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Categories]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transactions_Accounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Transactions]'))
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Accounts] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transactions_Accounts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Transactions]'))
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Accounts]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transactions_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Transactions]'))
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transactions_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Transactions]'))
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Users]
GO
/****** Object:  StoredProcedure [dbo].[TiposCuentas_Insertar]    Script Date: 22-Dec-21 12:12:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountsType_Insert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[TiposCuentas_Insertar] AS' 
END
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AccountsType_Insert] 
	@Name nvarchar(50),
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Order int;
	SELECT @Order = COALESCE(MAX(OrderIndex), 0)+1
	FROM AccountsType
	WHERE UserId = @UserId

	INSERT INTO AccountsType(AccountsType.Name, UserId, AccountsType.OrderIndex)
	VALUES (@Name, @UserId, @Order);

	SELECT SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[Transacciones_Actualizar]    Script Date: 22-Dec-21 12:12:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Transactions_Update]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Transactions_Update] AS' 
END
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Transactions_Update]
	-- Add the parameters for the stored procedure here
	@Id int,
	@TransactionDate datetime,
	@Amount decimal(18,2),
	@PreviousAmount decimal(18,2),
	@AccountId int,
	@PreviousAccountId int,
	@CategoryId int,
	@Note nvarchar(1000) = NULL
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Revertir transacción anterior
	UPDATE Accounts
	SET Balance -= @PreviousAmount
	WHERE Id = @PreviousAccountId;

	-- Realizar nueva transacción
	UPDATE Accounts
	SET Balance += @Amount
	WHERE Id = @AccountId;

	UPDATE Transactions
	SET Amount = ABS(@Amount), TransactionDate = @TransactionDate,
	CategoryId = @CategoryId, AccountId = @AccountId, Note = @Note
	WHERE Id = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[Transacciones_Borrar]    Script Date: 22-Dec-21 12:12:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Transactions_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Transactions_Delete] AS' 
END
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Transactions_Delete]
	@Id int	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Amount decimal(18,2);
	DECLARE @AccountId int;
	DECLARE @OperationTypeId int;

	SELECT @Amount = Amount, @AccountId = AccountId, @OperationTypeId = cat.OperationTypeId
	FROM Transactions
	inner join Categories cat
	ON cat.Id = Transactions.CategoryId
	WHERE Transactions.Id = @Id;

	DECLARE @MultiplicativeFactor int = 1;

	IF (@OperationTypeId = 2)
		SET @MultiplicativeFactor = -1;

	SET @Amount = @Amount * @MultiplicativeFactor;

	UPDATE Accounts
	SET Balance -= @Amount
	WHERE Id = @AccountId;

	DELETE Transactions
	WHERE Id = @Id;

END
GO
/****** Object:  StoredProcedure [dbo].[Transacciones_Insertar]    Script Date: 22-Dec-21 12:12:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Transactions_Insert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Transactions_Insert] AS' 
END
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Transactions_Insert]
	@UserId int,
	@TransactionDate date,
	@Amount decimal(18,2),
	@CategoryId int,
	@AccountId int,
	@Note nvarchar(1000) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Transactions(UserId, TransactionDate, Amount, CategoryId,
	AccountId, Note)
	Values(@UserId, @TransactionDate, ABS(@Amount), @CategoryId, @AccountId, @Note)

	UPDATE Accounts
	SET Balance += @Amount
	WHERE Id = @AccountId;

	SELECT SCOPE_IDENTITY();
END
GO