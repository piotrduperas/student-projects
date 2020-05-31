create database Library
use Library
go

create table Books
(
	ID int identity
		constraint Books_pk
			primary key nonclustered,
	Author varchar(60) not null,
	Title varchar(100) not null,
	Year int not null
)
go

create unique index Books_ID_uindex
	on Books (ID)
go

create table Readers
(
	ID int identity
		constraint Readers_pk
			primary key nonclustered,
	Name varchar(30) not null,
	Surname varchar(30) not null,
	Debt money default 0.00 not null
)
go

create unique index Readers_ID_uindex
	on Readers (ID)
go

create table Rentals
(
	ID int identity
		constraint RentalsPK
			primary key nonclustered,
	ReaderID int not null
		constraint RentalsReadersFK
			references Readers
				on delete cascade,
	BookID int not null
		constraint RentalsBooksFK
			references Books
				on delete cascade,
	RentalDate date not null,
	ReturnDate date
)
go

create unique index Rentals_ID_uindex
	on Rentals (ID)
go

create index Rentals_Book_index
	on Rentals (BookID)
go

create index Rentals_Reader_index
	on Rentals (ReaderID)
go