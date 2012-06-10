create database [Taro.BookStore]
go

use [Taro.BookStore]
go

create table [Account]
(
	Id varchar(36) not null,
	Balance money not null,
	NotificationSettings_Enabled bit not null,
	NotificationSettings_MinAmount money not null,

	constraint PK_Account primary key(Id),
)

create table [AccountLog]
(
	Id varchar(36) not null,
	Amount money not null,
	[Message] nvarchar(1000) null,
	LogTime datetime not null,
	AccountId varchar(36) not null,

	constraint PK_AccountLog primary key(Id),
	constraint FK_AccountLog_AccountId foreign key (AccountId) references Account(Id)
)

create table [User]
(
	Id varchar(36) not null,
	NickName nvarchar(20) not null,
	Email nvarchar(100) not null,
	[Password] varchar(20) not null,
	Gender int not null,
	CreatedTime datetime not null,
	AccountId varchar(36) not null,

	constraint PK_User primary key(Id),
	constraint FK_User_AccountId foreign key (AccountId) references Account(Id)
)

create unique index UQ_User_Email on [User](Email)

create table [Message]
(
	Id varchar(36) not null,
	Content nvarchar(1000) not null,
	SentTime datetime not null,
	IsRead bit not null,
	UserId varchar(36) not null,

	constraint PK_Message primary key (Id),
	constraint FK_Message_UserId foreign key (UserId) references [User](Id)
)

create table [MessageBoxInfo]
(
	UserId varchar(36) not null,
	TotalMessages int not null,
	UnReadMessages int not null,

	constraint PK_MessageBoxInfo primary key (UserId),
	constraint FK_MessageBoxInfo_UserId foreign key (UserId) references [User](Id)
)

create table [Book]
(
	ISBN varchar(50) not null,
	Title nvarchar(100) not null,
	Author nvarchar(50) null,
	Price money not null,
	Stock int not null,
	PublishedDate datetime not null,
	CreatorId varchar(36) not null,

	constraint PK_Book primary key (ISBN)
)

create table [BuyedBook]
(
	Id varchar(36) not null,
	UserId varchar(36) not null,
	BookISBN varchar(50) not null,
	Price money not null,
	BuyTime datetime not null,

	constraint PK_BuyedBook primary key (Id),
	constraint FK_BuyedBook_UserId foreign key (UserId) references [User](Id),
	constraint FK_BuyedBook_BookISBN foreign key (BookISBN) references Book(ISBN)
)

create table [BookSalesCounter]
(
	ISBN varchar(50) not null,
	Title nvarchar(100) not null,
	TotalSoldCount int not null,

	constraint PK_BookSalesCounter primary key (ISBN),
	constraint FK_BookSalesCounter_ISBN foreign key (ISBN) references Book(ISBN)
)

create table [WebsiteInfo]
(
	[Key] varchar(50) not null,
	[Value] nvarchar(1000) null,

	constraint PK_WebsiteInfo primary key ([Key])
)

go

insert into [Account] values ('3cad5740-6409-4e99-b95b-80a0c38b4e77', 1000, 1, 100);
insert into [User] values ('93b7294c-3e25-48cb-9d6e-cbc18f1fb90d', 'Admin', 'admin@admin.com', 'abc123', 1, '2012-03-20', '3cad5740-6409-4e99-b95b-80a0c38b4e77')