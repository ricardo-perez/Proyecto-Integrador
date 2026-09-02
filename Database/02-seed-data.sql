USE tienda_deportiva;

-- =========================================================
-- 1. ROLES
-- =========================================================

INSERT INTO roles (nombre, descripcion) VALUES
('Administrador', 'Acceso completo al sistema y gestion de empleados'),
('Vendedor', 'Gestion de ventas y atencion a clientes'),
('Deposito', 'Control de stock y productos'),
('Cajero', 'Gestion de cobros y facturacion'),
('Supervisor', 'Supervision general de las operaciones');


-- =========================================================
-- 2. EMPLEADOS
-- =========================================================

INSERT INTO empleados (
    id_rol,
    legajo,
    nombre,
    apellido,
    dni,
    telefono,
    direccion,
    email,
    nombre_usuario,
    contrasena,
    activo
) VALUES
(1, 1001, 'Marcos', 'Gomez', '30111222', '1123456789',
 'Av. Corrientes 1250, CABA', 'marcos.gomez@tiendadeportiva.com',
 'mgomez', '123456', 1),

(2, 1002, 'Lucia', 'Fernandez', '31222333', '1134567890',
 'Av. Rivadavia 2450, CABA', 'lucia.fernandez@tiendadeportiva.com',
 'lfernandez', '123456', 1),

(3, 1003, 'Martin', 'Rodriguez', '32333444', '1145678901',
 'Av. Santa Fe 1850, CABA', 'martin.rodriguez@tiendadeportiva.com',
 'mrodriguez', '123456', 1),

(4, 1004, 'Sofia', 'Martinez', '33444555', '1156789012',
 'Av. Belgrano 850, CABA', 'sofia.martinez@tiendadeportiva.com',
 'smartinez', '123456', 1),

(5, 1005, 'Nicolas', 'Lopez', '34555666', '1167890123',
 'Av. Independencia 1450, CABA', 'nicolas.lopez@tiendadeportiva.com',
 'nlopez', '123456', 1);


-- =========================================================
-- 3. CLIENTES
-- =========================================================

INSERT INTO clientes (
    nombre,
    apellido,
    dni,
    telefono,
    direccion,
    email,
    cuit_cuil,
    contrasena,
    activo
) VALUES
('Juan', 'Perez', '40111222', '1122334455',
 'Av. Cabildo 1250, CABA', 'juan.perez@gmail.com',
 '20-40111222-3', '123456', 1),

('Carla', 'Gonzalez', '41222333', '1133445566',
 'Av. Las Heras 850, CABA', 'carla.gonzalez@gmail.com',
 '27-41222333-8', '123456', 1),

('Federico', 'Sanchez', '42333444', '1144556677',
 'Av. Juan B. Justo 2100, CABA', 'federico.sanchez@gmail.com',
 '20-42333444-1', '123456', 1),

('Valentina', 'Romero', '43444555', '1155667788',
 'Av. Directorio 1450, CABA', 'valentina.romero@gmail.com',
 '27-43444555-6', '123456', 1),

('Diego', 'Torres', '44555666', '1166778899',
 'Av. La Plata 950, CABA', 'diego.torres@gmail.com',
 '20-44555666-9', '123456', 1),

('Camila', 'Diaz', '45666777', '1177889900',
 'Av. Pueyrredon 1750, CABA', 'camila.diaz@gmail.com',
 '27-45666777-4', '123456', 1),

('Matias', 'Acosta', '46777888', '1188990011',
 'Av. Scalabrini Ortiz 1200, CABA', 'matias.acosta@gmail.com',
 '20-46777888-7', '123456', 1);


-- =========================================================
-- 4. PRODUCTOS
-- =========================================================

INSERT INTO productos (
    codigo,
    descripcion,
    descripcion_larga,
    stock_online,
    stock_fisico,
    activo_para_venta,
    precio_actual
) VALUES
('CAS-001',
 'Casco integral',
 'Casco integral deportivo para motocicleta con visor transparente',
 15, 10, 1, 185000.00),

('GUA-001',
 'Guantes deportivos',
 'Guantes para motocicleta con protecciones reforzadas',
 25, 18, 1, 65000.00),

('CAM-001',
 'Campera motociclista',
 'Campera deportiva para motociclista con protecciones',
 12, 8, 1, 210000.00),

('ESC-001',
 'Escape deportivo',
 'Sistema de escape deportivo para motocicletas',
 5, 4, 1, 350000.00),

('MOC-001',
 'Mochila deportiva',
 'Mochila impermeable para motociclistas',
 20, 15, 1, 85000.00),

('ACC-001',
 'Soporte para celular',
 'Soporte universal para celular para motocicleta',
 30, 20, 1, 45000.00),

('CHA-001',
 'Chaleco reflectivo',
 'Chaleco reflectivo de alta visibilidad para motociclistas',
 18, 12, 1, 55000.00),

('BOT-001',
 'Botas deportivas',
 'Botas reforzadas para motocicleta de uso deportivo',
 10, 7, 1, 175000.00);


-- =========================================================
-- 5. VENTAS
-- =========================================================

INSERT INTO ventas (
    id_cliente,
    fecha,
    medio_de_pago,
    monto_total
) VALUES
(1, '2026-08-20', 'Tarjeta de credito', 250000.00),

(2, '2026-08-21', 'Transferencia bancaria', 130000.00),

(3, '2026-08-22', 'Efectivo', 350000.00),

(4, '2026-08-25', 'Tarjeta de debito', 295000.00),

(5, '2026-08-27', 'Mercado Pago', 255000.00),

(6, '2026-08-29', 'Transferencia bancaria', 220000.00),

(7, '2026-09-01', 'Tarjeta de credito', 175000.00);


-- =========================================================
-- 6. DETALLES DE VENTAS
-- =========================================================

INSERT INTO detalles_ventas (
    id_venta,
    id_producto,
    cantidad,
    precio_unitario,
    subtotal
) VALUES
-- Venta 1
(1, 1, 1, 185000.00, 185000.00),
(1, 6, 1, 45000.00, 45000.00),
(1, 2, 1, 65000.00, 65000.00),

-- Venta 2
(2, 2, 2, 65000.00, 130000.00),

-- Venta 3
(3, 4, 1, 350000.00, 350000.00),

-- Venta 4
(4, 3, 1, 210000.00, 210000.00),
(4, 6, 1, 45000.00, 45000.00),
(4, 7, 1, 55000.00, 55000.00),

-- Venta 5
(5, 5, 3, 85000.00, 255000.00),

-- Venta 6
(6, 3, 1, 210000.00, 210000.00),
(6, 6, 1, 45000.00, 45000.00),

-- Venta 7
(7, 8, 1, 175000.00, 175000.00);


-- =========================================================
-- 7. FACTURAS
-- =========================================================

INSERT INTO facturas (
    id_venta,
    tipo_factura,
    punto_venta,
    numero_factura,
    fecha_emision,
    total
) VALUES
(1, 'B', '0001', '00000001', '2026-08-20', 295000.00),

(2, 'B', '0001', '00000002', '2026-08-21', 130000.00),

(3, 'A', '0001', '00000003', '2026-08-22', 350000.00),

(4, 'B', '0001', '00000004', '2026-08-25', 310000.00),

(5, 'B', '0001', '00000005', '2026-08-27', 255000.00),

(6, 'A', '0001', '00000006', '2026-08-29', 255000.00),

(7, 'B', '0001', '00000007', '2026-09-01', 175000.00);


-- =========================================================
-- 8. ENVIOS
-- =========================================================

INSERT INTO envios (
    id_venta,
    estado
) VALUES
(1, 'entregado'),
(2, 'enviado'),
(3, 'pendiente'),
(4, 'entregado'),
(5, 'enviado'),
(6, 'pendiente'),
(7, 'enviado');


-- =========================================================
-- 9. GASTOS
-- =========================================================

INSERT INTO gastos (
    fecha,
    descripcion,
    monto
) VALUES
('2026-08-01', 'Compra de mercaderia para reposicion', 850000.00),

('2026-08-05', 'Pago de servicio de internet', 45000.00),

('2026-08-10', 'Servicio de electricidad del local', 78000.00),

('2026-08-15', 'Mantenimiento de equipamiento', 95000.00),

('2026-08-20', 'Publicidad y marketing', 120000.00),

('2026-08-25', 'Compra de insumos para el local', 65000.00),

('2026-08-30', 'Servicio de limpieza', 55000.00);


-- =========================================================
-- FIN DE LAS INSERCIONES
-- =========================================================