IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Warehouse')
CREATE database Warehouse;
GO

-- deleting tables
IF OBJECT_ID('tblArticle', 'U') IS NOT NULL DROP TABLE tblArticle;

use Warehouse
CREATE TABLE tblArticle (ArticleID int IDENTITY(1,1) PRIMARY KEY NOT NULL, Article nvarchar(50), Code nvarchar(50),
 Amount int, Price decimal, Stored bit);