DROP DATABASE IF EXISTS tienda_online;
GO
CREATE DATABASE tienda_online;
GO
USE tienda_online;
GO

CREATE TABLE roles (
    id_rol INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(50) NOT NULL UNIQUE,
    descripcion NVARCHAR(200) NULL
);
CREATE TABLE empleados (
    id_empleado INT IDENTITY(1,1) PRIMARY KEY,
    id_rol INT NOT NULL,
    legajo INT NOT NULL UNIQUE,
    nombre NVARCHAR(100) NOT NULL,
    apellido NVARCHAR(100) NOT NULL,
    dni VARCHAR(15) NOT NULL UNIQUE,
    telefono NVARCHAR(30) NULL,
    direccion NVARCHAR(200) NULL,
    email NVARCHAR(254) NULL UNIQUE,
    nombre_usuario NVARCHAR(100) NOT NULL UNIQUE,
    contrasenia VARCHAR(255) NOT NULL,
    imagen_ruta NVARCHAR(500) NULL,
    activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT fk_empleados_roles 
        FOREIGN KEY (id_rol) 
        REFERENCES roles(id_rol) 
        ON UPDATE CASCADE 
        ON DELETE NO ACTION
);
CREATE TABLE clientes (
    id_cliente INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(100) NOT NULL,
    apellido NVARCHAR(100) NOT NULL,
    dni VARCHAR(15) NOT NULL UNIQUE,
    telefono NVARCHAR(30) NULL,
    direccion NVARCHAR(200) NULL,
    email NVARCHAR(254) NOT NULL UNIQUE,
    cuit_cuil VARCHAR(13) NOT NULL UNIQUE,
    contrasenia VARCHAR(255) NOT NULL,
    activo BIT NOT NULL DEFAULT 1
);
CREATE TABLE productos (
    id_producto INT IDENTITY(1,1) PRIMARY KEY,
    sku NVARCHAR(50) NOT NULL UNIQUE,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(255) NOT NULL,
    descripcion_larga NVARCHAR(MAX) NULL,
    imagen_ruta NVARCHAR(500) NULL,
    precio DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    activo_para_venta BIT NOT NULL DEFAULT 0
);
CREATE TABLE medios_de_pago (
    id_medio_de_pago INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(50) NOT NULL UNIQUE,
    descripcion NVARCHAR(200) NULL
);
CREATE TABLE ventas (
    id_venta INT IDENTITY(1,1) PRIMARY KEY,
    id_cliente INT NOT NULL,
    id_medio_de_pago INT NOT NULL,
    fecha DATETIME2 NOT NULL DEFAULT GETDATE(),
    subtotal DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    descuento_total DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    monto_total DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    CONSTRAINT fk_ventas_clientes
        FOREIGN KEY (id_cliente)
        REFERENCES clientes(id_cliente)
        ON UPDATE CASCADE
        ON DELETE NO ACTION,
    CONSTRAINT fk_ventas_medios_de_pago
        FOREIGN KEY (id_medio_de_pago)
        REFERENCES medios_de_pago(id_medio_de_pago)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
CREATE TABLE tipos_movimiento (
    id_tipo_movimiento INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(50) NOT NULL UNIQUE,
    descripcion NVARCHAR(200) NULL
);
CREATE TABLE movimientos_stock (
    id_movimiento INT IDENTITY(1,1) PRIMARY KEY,
    id_producto INT NOT NULL,
    id_tipo_movimiento INT NOT NULL,
    id_empleado INT NULL,
    id_venta INT NULL,
    tipo_stock NVARCHAR(10) NOT NULL, -- 'FISICO' u 'ONLINE'
    cantidad INT NOT NULL, -- Positivos (ingresos) o Negativos (egresos | ventas)
    fecha DATETIME2 NOT NULL DEFAULT GETDATE(),
    motivo NVARCHAR(255) NULL,
    CONSTRAINT fk_movimientos_productos
        FOREIGN KEY (id_producto)
        REFERENCES productos(id_producto)
        ON UPDATE CASCADE
        ON DELETE NO ACTION,
    CONSTRAINT fk_movimientos_tipos_movimiento
        FOREIGN KEY (id_tipo_movimiento)
        REFERENCES tipos_movimiento(id_tipo_movimiento)
        ON UPDATE CASCADE
        ON DELETE NO ACTION,
    CONSTRAINT fk_movimientos_empleados
        FOREIGN KEY (id_empleado)
        REFERENCES empleados(id_empleado)
        ON UPDATE CASCADE
        ON DELETE NO ACTION,
    CONSTRAINT fk_movimientos_ventas
        FOREIGN KEY (id_venta)
        REFERENCES ventas(id_venta)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);
CREATE TABLE detalles_ventas (
    id_detalle_venta INT IDENTITY(1,1) PRIMARY KEY,
    id_venta INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL DEFAULT 1,
    precio_unitario DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    subtotal DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    CONSTRAINT fk_detalles_ventas_ventas
        FOREIGN KEY (id_venta)
        REFERENCES ventas(id_venta)
        ON UPDATE CASCADE
        ON DELETE NO ACTION,
    CONSTRAINT fk_detalles_ventas_productos
        FOREIGN KEY (id_producto)
        REFERENCES productos(id_producto)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
CREATE TABLE descuentos_ventas (
    id_descuento_venta INT IDENTITY(1,1) PRIMARY KEY,
    id_venta INT NOT NULL,
    concepto NVARCHAR(100) NOT NULL,
    porcentaje DECIMAL(5, 2) NULL DEFAULT 0.00,
    monto_descuento DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    CONSTRAINT fk_descuentos_ventas_ventas
        FOREIGN KEY (id_venta)
        REFERENCES ventas(id_venta)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
CREATE TABLE tipos_comprobante (
    id_tipo_comprobante INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(50) NOT NULL UNIQUE,
    descripcion NVARCHAR(200) NULL
);
CREATE TABLE facturas (
    id_factura INT IDENTITY(1,1) PRIMARY KEY,
    id_venta INT NOT NULL,
    id_tipo_comprobante INT NOT NULL,
    punto_venta VARCHAR(5) NOT NULL,
    numero_factura VARCHAR(15) NOT NULL,
    fecha_emision DATETIME2 NOT NULL DEFAULT GETDATE(),
    subtotal DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    descuento_total DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    total DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    CONSTRAINT fk_facturas_ventas
        FOREIGN KEY (id_venta)
        REFERENCES ventas(id_venta)
        ON UPDATE CASCADE
        ON DELETE NO ACTION,
    CONSTRAINT fk_facturas_tipos_comprobante
        FOREIGN KEY (id_tipo_comprobante) 
        REFERENCES tipos_comprobante(id_tipo_comprobante) 
        ON UPDATE CASCADE 
        ON DELETE NO ACTION
);
CREATE TABLE estados_envio (
    id_estado_envio INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(50) NOT NULL UNIQUE,
    descripcion NVARCHAR(200) NULL
);
CREATE TABLE envios (
    id_envio INT IDENTITY(1,1) PRIMARY KEY,
    id_venta INT NOT NULL,
    id_estado_envio INT NOT NULL,
    codigo_seguimiento NVARCHAR(100) NULL,
    fecha_despacho DATETIME2 NULL,
    fecha_entrega DATETIME2 NULL,
    CONSTRAINT fk_envios_ventas 
        FOREIGN KEY (id_venta)
        REFERENCES ventas(id_venta) 
        ON UPDATE CASCADE 
        ON DELETE NO ACTION,
    CONSTRAINT fk_envios_estados_envio 
        FOREIGN KEY (id_estado_envio) 
        REFERENCES estados_envio(id_estado_envio) 
        ON UPDATE CASCADE 
        ON DELETE NO ACTION
);
CREATE TABLE gastos (
    id_gasto INT IDENTITY(1,1) PRIMARY KEY,
    fecha DATETIME2 NOT NULL DEFAULT GETDATE(),
    descripcion NVARCHAR(255) NOT NULL,
    monto DECIMAL(12, 2) NOT NULL DEFAULT 0.00
);
GO