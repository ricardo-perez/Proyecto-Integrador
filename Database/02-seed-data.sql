USE tienda_online;
GO

INSERT INTO tipos_comprobante (nombre, descripcion) 
VALUES
    ('Factura A', 'Responsables Inscriptos'),
    ('Factura B', 'Consumidores Finales o Exentos'),
    ('Factura C', 'Monotributistas'),
    ('Nota de Crédito A', 'Anulación o descuento sobre Factura A'),
    ('Nota de Crédito B', 'Anulación o descuento sobre Factura B'),
    ('Nota de Crédito C', 'Anulación o descuento sobre Factura C');
INSERT INTO estados_envio (nombre, descripcion) 
VALUES
    ('Pendiente', 'Pago verificado, pendiente de embalaje'),
    ('En Preparación', 'Producto empaquetado en depósito'),
    ('Despachado', 'Entregado al correo'),
    ('Entregado', 'Recibido por el cliente'),
    ('Cancelado', 'Envío anulado');
INSERT INTO roles (nombre, descripcion) 
VALUES
    ('Administrador', 'Acceso total al sistema'),
    ('Gerente', 'Gestión de stock y precios'),
    ('Vendedor', 'Atención y procesamiento de ventas');
INSERT INTO medios_de_pago (nombre, descripcion) 
VALUES
    ('Tarjeta de Débito', 'Pago mediante tarjeta de débito bancaria'),
    ('Tarjeta de Crédito', 'Pago mediante tarjeta de crédito en 1 o más cuotas'),
    ('Transferencia Bancaria', 'Transferencia bancaria directa o mediante CBU/CVU'),
    ('Efectivo', 'Pago en efectivo al momento de retiro/entrega');
INSERT INTO tipos_movimiento (nombre, descripcion) 
VALUES
    ('Ingreso por Compra', 'Entrada de mercadería proveniente de proveedores'),
    ('Venta Online', 'Egreso automático por venta realizada en la plataforma'),
    ('Ajuste Manual', 'Corrección de inventario'),
    ('Devolución', 'Ingreso por anulación de compra o cambio de producto'),
    ('Rotura', 'Egreso por pérdida, rotura o vencimiento');
GO