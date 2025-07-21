CREATE DATABASE BudgetManager;
GO

USE BudgetManager;
GO

create table OperationsType
(
    Id          int identity
        constraint PK_OperationsType
            primary key,
    Description nvarchar(50) not null
)
go

create table Users
(
    Id              int identity
        constraint PK_Users
            primary key,
    Email           nvarchar(256) not null,
    NormalizedEmail nvarchar(256) not null,
    PasswordHash    nvarchar(max) not null
)
go

create table AccountsType
(
    Id         int identity
        constraint PK_AccountsType
            primary key,
    Name       nvarchar(50) not null,
    UserId     int          not null
        constraint FK_AccountsType_Users
            references Users,
    OrderIndex int          not null
)
go

create table Accounts
(
    Id            int identity
        constraint PK_Accounts
            primary key,
    Name          nvarchar(50)   not null,
    AccountTypeId int            not null
        constraint FK_Accounts_AccountsType
            references AccountsType,
    Balance       decimal(18, 2) not null,
    Description   nvarchar(1000)
)
go

create table Categories
(
    Id              int identity
        constraint PK_Categories
            primary key,
    Name            nvarchar(50) not null,
    OperationTypeId int          not null
        constraint FK_Categories_OperationsType
            references OperationsType,
    UserId          int          not null
        constraint FK_Categories_Users
            references Users
)
go

create table Transactions
(
    Id              int identity
        constraint PK_Transactions
            primary key,
    UserId          int            not null
        constraint FK_Transactions_Users
            references Users,
    TransactionDate datetime       not null,
    Amount          decimal(18, 2) not null,
    Note            nvarchar(1000),
    AccountId       int            not null
        constraint FK_Transactions_Accounts
            references Accounts,
    CategoryId      int            not null
        constraint FK_Transactions_Categories
            references Categories
)
go

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

    INSERT INTO AccountsType(Name, UserId, OrderIndex)
    VALUES (@Name, @UserId, @Order);

    SELECT SCOPE_IDENTITY();
END
go

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Transactions_Delete]
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
go

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
go

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
go

CREATE PROCEDURE [dbo].[CreateDataNewUser]
    @UserId int
AS
BEGIN 
    SET NOCOUNT ON; 
    
    DECLARE @Efectivo nvarchar(50) = 'Efectivo';
    DECLARE @CuentasDeBanco nvarchar(50) = 'Cuentas de Banco';
    DECLARE @Tarjetas nvarchar(50) = 'Tarjetas';
    
    INSERT INTO AccountsType(Name, UserId, OrderIndex)
    VALUES (@Efectivo, @UserId, 1),
           (@CuentasDeBanco, @UserId, 2),
           (@Tarjetas, @UserId, 3);
    
    INSERT INTO Accounts (Name, Balance, AccountTypeId)
    SELECT Name, 0, Id
    FROM AccountsType
    WHERE UserId = @UserId;
    
    INSERT INTO Categories(Name, OperationTypeId, UserId)
    VALUES 
        ('Libros',2, @UserId),
        ('Salario',1,@UserId),
        ('Mesada',1,@UserId),
        ('Comida',2,@UserId),
        ('Transporte',2,@UserId);

END
GO