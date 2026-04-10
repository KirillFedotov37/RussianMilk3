CREATE DATABASE [РосМолоко];
GO

USE [РосМолоко];
GO

CREATE TABLE [Продукция] (
    [IdПродукта] INT IDENTITY(1,1) PRIMARY KEY,
    [Название] NVARCHAR(100) NOT NULL,
    [Жирность] DECIMAL(4,2) NOT NULL,
    [ТипУпаковки] NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE [Склады] (
    [IdСклада] INT IDENTITY(1,1) PRIMARY KEY,
    [НазваниеСклада] NVARCHAR(100) NOT NULL,
    [Адрес] NVARCHAR(200) NOT NULL
);
GO

CREATE TABLE [Партии] (
    [IdПартии] INT IDENTITY(1,1) PRIMARY KEY,
    [IdПродукта] INT NOT NULL,
    [IdСклада] INT NOT NULL,
    [ДатаПроизводства] DATE NOT NULL,
    [СрокГодности] DATE NOT NULL,
    [Количество] INT NOT NULL CHECK ([Количество] > 0),
    CONSTRAINT [FK_Партии_Продукция] FOREIGN KEY ([IdПродукта]) REFERENCES [Продукция]([IdПродукта]),
    CONSTRAINT [FK_Партии_Склады] FOREIGN KEY ([IdСклада]) REFERENCES [Склады]([IdСклада])
);
GO

INSERT INTO [Продукция] ([Название], [Жирность], [ТипУпаковки])
VALUES
(N'Молоко пастеризованное', 2.50, N'Пакет 1 л'),
(N'Кефир', 3.20, N'Бутылка 0.9 л'),
(N'Сметана', 15.00, N'Стакан 300 г'),
(N'Ряженка', 4.00, N'Бутылка 0.5 л');
GO

INSERT INTO [Склады] ([НазваниеСклада], [Адрес])
VALUES
(N'Склад №1', N'Москва, ул. Центральная, 10'),
(N'Склад №2', N'Подольск, ул. Заводская, 5'),
(N'Холодильный склад', N'Люберцы, Промышленный проезд, 7');
GO

INSERT INTO [Партии] ([IdПродукта], [IdСклада], [ДатаПроизводства], [СрокГодности], [Количество])
VALUES
(1, 1, '2026-04-01', '2026-04-10', 500),
(2, 2, '2026-04-03', '2026-04-12', 300),
(3, 1, '2026-04-05', '2026-04-20', 200),
(4, 3, '2026-04-07', '2026-04-16', 150);
GO

SELECT * FROM [Продукция];
SELECT * FROM [Склады];
SELECT * FROM [Партии];
GO
