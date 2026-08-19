drop database if exists tienda_deportiva;
create database tienda_deportiva;
use tienda_deportiva;

CREATE TABLE roles (
	id_rol INT AUTO_INCREMENT PRIMARY KEY,
	nombre VARCHAR(20) NOT NULL,
	descripcion VARCHAR(100)
);

CREATE TABLE empleados (
	id_empleado INT AUTO_INCREMENT PRIMARY KEY,
	id_rol INT NOT NULL,
	legajo INT NOT NULL UNIQUE,
	nombre VARCHAR(100) NOT NULL,
	apellido VARCHAR(100) NOT NULL, 
	dni VARCHAR(20) NOT NULL UNIQUE,
	telefono VARCHAR(30),
	direccion VARCHAR(150),
	email VARCHAR(254) UNIQUE,
	nombre_usuario VARCHAR(100) NOT NULL UNIQUE,
	contrasena VARCHAR(255) NOT NULL,
	activo BIT NOT NULL DEFAULT 1,
	CONSTRAINT fk_empleado_rol
		FOREIGN KEY (id_rol)
		REFERENCES roles(id_rol)
		ON UPDATE CASCADE 
		ON DELETE RESTRICT
);

CREATE TABLE clientes (
	id_cliente INT AUTO_INCREMENT PRIMARY KEY,
	nombre VARCHAR(100) NOT NULL,
	apellido VARCHAR(100) NOT NULL, 
	dni VARCHAR(20) NOT NULL UNIQUE,
	telefono VARCHAR(30),
	direccion VARCHAR(150),
	email VARCHAR(254) NOT NULL UNIQUE,
	cuit_cuil VARCHAR(20) NOT NULL UNIQUE,
	password VARCHAR(255) NOT NULL,
	activo BIT NOT NULL DEFAULT 1
);

CREATE TABLE productos (
	id_producto INT AUTO_INCREMENT PRIMARY KEY,
	codigo VARCHAR(50) NOT NULL UNIQUE,
	descripcion VARCHAR(100) NOT NULL,
	descripcion_larga VARCHAR(255),
	stock_online INT NOT NULL DEFAULT 0,
	stock_fisico INT NOT NULL DEFAULT 0,
	activo_para_venta BIT NOT NULL DEFAULT 0,
	precio_actual DECIMAL(10, 2) NOT NULL DEFAULT 0.00
);

CREATE TABLE ventas (
	id_venta INT AUTO_INCREMENT PRIMARY KEY,
	id_cliente INT NOT NULL,
	fecha DATE NOT NULL DEFAULT (CURRENT_DATE),
	medio_de_pago VARCHAR(50) NOT NULL,
	monto_total DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
	CONSTRAINT fk_venta_cliente
		FOREIGN KEY (id_cliente)
		REFERENCES clientes(id_cliente)
		ON UPDATE CASCADE 
		ON DELETE RESTRICT
);

CREATE TABLE detalles_ventas (
	id_detalle_venta INT AUTO_INCREMENT PRIMARY KEY,
	id_venta INT NOT NULL,
	id_producto INT NOT NULL,
	cantidad INT NOT NULL DEFAULT 0,
	precio_unitario DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
	subtotal DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
	CONSTRAINT fk_detalle_venta
		FOREIGN KEY (id_venta)
		REFERENCES ventas(id_venta)
		ON UPDATE CASCADE 
		ON DELETE RESTRICT,
	CONSTRAINT fk_detalle_producto
		FOREIGN KEY (id_producto)
		REFERENCES productos(id_producto)
		ON UPDATE CASCADE
		ON DELETE RESTRICT
);

CREATE TABLE facturas (
	id_factura INT AUTO_INCREMENT PRIMARY KEY,
	id_venta INT NOT NULL,
	id_cliente INT NOT NULL,
	tipo_factura VARCHAR(5) NOT NULL,
	punto_venta VARCHAR(10) NOT NULL,
	numero_factura VARCHAR(20) NOT NULL,
	fecha_emision DATE NOT NULL DEFAULT (CURRENT_DATE),
	total DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
	CONSTRAINT fk_factura_venta
		FOREIGN KEY (id_venta)
		REFERENCES ventas(id_venta)
		ON UPDATE CASCADE 
		ON DELETE RESTRICT,
	CONSTRAINT fk_factura_cliente
		FOREIGN KEY (id_cliente)
		REFERENCES clientes(id_cliente)
		ON UPDATE CASCADE 
		ON DELETE RESTRICT
);

CREATE TABLE envios (
	id_envio INT AUTO_INCREMENT PRIMARY KEY,
	id_venta INT NOT NULL,
	estado VARCHAR(30) NOT NULL,
	CONSTRAINT fk_envio_venta
		FOREIGN KEY (id_venta)
		REFERENCES ventas(id_venta)
		ON UPDATE CASCADE 
		ON DELETE RESTRICT
);

CREATE TABLE gastos (
	id_gasto INT AUTO_INCREMENT PRIMARY KEY,
	fecha DATE NOT NULL DEFAULT (CURRENT_DATE),
	descripcion VARCHAR(150) NOT NULL,
	monto DECIMAL(10, 2) NOT NULL DEFAULT 0.00
);