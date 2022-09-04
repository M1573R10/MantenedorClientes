create database Clientes

use Clientes

create table MantenedorClientes(
	Nombre varchar(20) primary key not null,
	Empresa varchar(50) not null,
	Cargo varchar(20) not null,
)

