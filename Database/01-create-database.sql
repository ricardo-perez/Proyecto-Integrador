drop database if exists tienda_deportiva;
create database tienda_deportiva;
use tienda_deportiva;

create table productos (
    id_producto int auto_increment primary key,
    descripcion varchar(100) not null,
    precio decimal(10,2) not null,
    stock int not null default 0,

    constraint chk_producto_precio
        check (precio >= 0),

    constraint chk_producto_stock
        check (stock >= 0)
);


create table empleados (
    id_empleado int auto_increment primary key,
    legajo varchar(20) not null unique,
    nombre varchar(50) not null,
    apellido varchar(50) not null,
    dni varchar(20) not null unique,
    direccion varchar(150),

    constraint chk_empleado_nombre
        check (length(nombre) >= 2),

    constraint chk_empleado_apellido
        check (length(apellido) >= 2)
);


create table clientes (
    id_cliente int auto_increment primary key,
    dni varchar(20) not null unique,
    nombre varchar(50) not null,
    apellido varchar(50) not null,
    telefono varchar(30),
    email varchar(100) unique,
    direccion varchar(150),

    constraint chk_cliente_nombre
        check (length(nombre) >= 2),

    constraint chk_cliente_apellido
        check (length(apellido) >= 2)
);


create table usuarios (
    id_usuario int auto_increment primary key,
    id_empleado int not null unique,
    usuario varchar(50) not null unique,
    contrasena varchar(255) not null,
    rol varchar(30) not null,

    constraint fk_usuario_empleado
        foreign key (id_empleado)
        references empleados(id_empleado)
        on update cascade
        on delete restrict,

    constraint chk_usuario_rol
        check (rol in ('Administrador', 'Vendedor', 'Gerente'))
);


create table ventas (
    id_detalle int auto_increment primary key,
    id_venta int not null,
    id_producto int not null,
    cantidad int not null,
    tipo_venta enum('Local', 'Online') not null,

    constraint fk_venta_producto
        foreign key (id_producto)
        references productos(id_producto)
        on update cascade
        on delete restrict,

    constraint chk_venta_cantidad
        check (cantidad > 0)
);


create table factura (
    id_factura int auto_increment primary key,
    id_venta int not null,
    id_cliente int,
    id_empleado int,
    fecha datetime not null default current_timestamp,
    total_a_pagar decimal(10,2) not null,

    constraint fk_factura_cliente
        foreign key (id_cliente)
        references clientes(id_cliente)
        on update cascade
        on delete set null,

    constraint fk_factura_empleado
        foreign key (id_empleado)
        references empleados(id_empleado)
        on update cascade
        on delete set null,

    constraint chk_factura_total
        check (total_a_pagar >= 0)
);


create table gastos (
    id_gasto int auto_increment primary key,
    id_producto int not null,
    cantidad int not null,
    fecha datetime not null default current_timestamp,
    monto decimal(10,2) not null,

    constraint fk_gasto_producto
        foreign key (id_producto)
        references productos(id_producto)
        on update cascade
        on delete restrict,

    constraint chk_gasto_cantidad
        check (cantidad > 0),

    constraint chk_gasto_monto
        check (monto >= 0)
);


create table ganancias (
    mes date primary key,
    ingresos_totales decimal(12,2) not null default 0,
    gastos_totales decimal(12,2) not null default 0,
    ganancia decimal(12,2) not null default 0,

    constraint chk_ganancia_ingresos
        check (ingresos_totales >= 0),

    constraint chk_ganancia_gastos
        check (gastos_totales >= 0)
);