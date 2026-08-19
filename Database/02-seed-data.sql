USE tienda_deportiva;

INSERT INTO productos
(descripcion, precio, stock)
VALUES
('Pelota de futbol', 25000.00, 30),
('Pelota de basquet', 28000.00, 25),
('Camiseta deportiva', 45000.00, 20),
('Short deportivo', 22000.00, 35),
('Zapatillas deportivas', 85000.00, 15),
('Guantes de arquero', 38000.00, 18),
('Medias deportivas', 8000.00, 50),
('Campera deportiva', 65000.00, 12);

INSERT INTO empleados
(legajo, nombre, apellido, dni, direccion)
VALUES
('EMP001', 'Juan', 'Perez', '30111222', 'Av. Rivadavia 1250'),
('EMP002', 'Maria', 'Gomez', '31222333', 'Av. San Martin 2450'),
('EMP003', 'Carlos', 'Rodriguez', '32333444', 'Belgrano 850'),
('EMP004', 'Lucia', 'Fernandez', '33444555', 'Sarmiento 1420'),
('EMP005', 'Martin', 'Lopez', '34555666', 'Mitre 730'),
('EMP006', 'Sofia', 'Martinez', '35666777', 'Moreno 1980');

INSERT INTO clientes
(dni, nombre, apellido, telefono, email, direccion)
VALUES
('40111222', 'Pedro', 'Gonzalez', '1123456789', 'pedro@gmail.com', 'Belgrano 125'),
('41222333', 'Ana', 'Martinez', '1134567890', 'ana@gmail.com', 'Mitre 450'),
('42333444', 'Lucas', 'Fernandez', '1145678901', 'lucas@gmail.com', 'Rivadavia 2300'),
('43444555', 'Carolina', 'Lopez', '1156789012', 'carolina@gmail.com', 'Sarmiento 780'),
('44555666', 'Diego', 'Sanchez', '1167890123', 'diego@gmail.com', 'Moreno 1450'),
('45666777', 'Valentina', 'Romero', '1178901234', 'valentina@gmail.com', 'Belgrano 920'),
('46777888', 'Federico', 'Torres', '1189012345', 'federico@gmail.com', 'San Martin 1560');

INSERT INTO usuarios
(id_empleado, usuario, contrasena, rol)
VALUES
(1, 'jperez', '123456', 'Administrador'),
(2, 'mgomez', '123456', 'Vendedor'),
(3, 'crodriguez', '123456', 'Gerente'),
(4, 'lfernandez', '123456', 'Vendedor'),
(5, 'mlopez', '123456', 'Vendedor'),
(6, 'smartinez', '123456', 'Vendedor');


INSERT INTO ventas
(id_venta, id_producto, cantidad, tipo_venta)
VALUES
-- Venta 1001 - Local
(1001, 1, 2, 'Local'),
(1001, 4, 1, 'Local'),
(1001, 7, 3, 'Local'),

-- Venta 1002 - Local
(1002, 3, 1, 'Local'),
(1002, 5, 1, 'Local'),

-- Venta 1003 - Online
(1003, 2, 1, 'Online'),
(1003, 6, 1, 'Online'),

-- Venta 1004 - Local
(1004, 8, 1, 'Local'),
(1004, 7, 2, 'Local'),

-- Venta 1005 - Online
(1005, 5, 2, 'Online'),
(1005, 4, 2, 'Online'),

-- Venta 1006 - Local
(1006, 1, 1, 'Local'),
(1006, 3, 1, 'Local'),
(1006, 7, 2, 'Local');


INSERT INTO factura
(id_venta, id_cliente, id_empleado, fecha, total_a_pagar)
VALUES
(1001, 1, 1, '2026-01-10 10:30:00', 99000.00),
(1002, 2, 2, '2026-01-15 16:45:00', 130000.00),
(1003, 3, NULL, '2026-02-05 14:20:00', 66000.00),
(1004, 4, 3, '2026-02-18 11:15:00', 81000.00),
(1005, 5, NULL, '2026-03-03 19:30:00', 214000.00),
(1006, 6, 4, '2026-03-20 13:10:00', 61000.00);


INSERT INTO gastos
(id_producto, cantidad, fecha, monto)
VALUES
(1, 20, '2026-01-05 09:00:00', 300000.00),
(2, 15, '2026-01-08 09:30:00', 315000.00),
(3, 10, '2026-01-12 10:00:00', 300000.00),
(4, 25, '2026-01-20 09:15:00', 275000.00),
(5, 10, '2026-02-02 11:00:00', 600000.00),
(6, 12, '2026-02-10 10:30:00', 300000.00),
(7, 50, '2026-02-15 09:45:00', 200000.00),
(8, 10, '2026-03-01 10:00:00', 450000.00);


INSERT INTO ganancias
(mes, ingresos_totales, gastos_totales, ganancia)
VALUES
('2026-01-01', 229000.00, 1175000.00, -946000.00),
('2026-02-01', 147000.00, 1100000.00, -953000.00),
('2026-03-01', 275000.00, 450000.00, -175000.00),
('2026-04-01', 385000.00, 180000.00, 205000.00),
('2026-05-01', 520000.00, 250000.00, 270000.00),
('2026-06-01', 610000.00, 300000.00, 310000.00);