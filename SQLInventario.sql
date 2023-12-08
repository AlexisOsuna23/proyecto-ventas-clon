--Crear base de datos
Create database CRUD_Inventario

--Usar la base de datos CRUD
Use CRUD_Inventario

--Crear Tabla Productos
create table Productos(
ProductoID varchar(10) Not Null Primary Key,
Descripcion varchar(50) Not Null,
PVenta decimal(10,2) Not Null,
Saldo float Not Null
)

--Crear Tabla Ventas
create table Ventas(
Folio int Not Null Primary Key,
Fecha datetime Not Null,
Total money Not Null
)

--Crear Tabla InvetarioDetalle
create table InventarioDetalle(
Renglon int Primary Key identity(1,1),
Cantidad float Not Null,
Precio decimal Not null,
ProductoID varchar(10) Not Null,
 FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID),
Folio int Not Null,
Foreign key (Folio) References Ventas(Folio)
)