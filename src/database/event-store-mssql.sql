create table [Events]
(
	Id int identity(1,1) not null,
	UtcTimestamp datetime not null,
	EventTypeName nvarchar(500) not null,
	SerializedEventData nvarchar(max) not null,

	constraint PK_Events primary key (Id)
)