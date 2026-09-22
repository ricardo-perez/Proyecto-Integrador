USE tienda_online;
GO

INSERT INTO estados_envio (nombre, descripcion) VALUES
(N'Pendiente', N'El pedido fue registrado y está a la espera de confirmación de pago o procesamiento.'),
(N'En preparación', N'El pago fue confirmado y el producto está siendo empaquetado en depósito.'),
(N'Despachado', N'El paquete fue entregado al correo o servicio de mensajería y se encuentra en tránsito.'),
(N'Entregado', N'El pedido fue entregado con éxito al cliente en la dirección de destino.'),
(N'Cancelado', N'El envío fue anulado debido a una cancelación de la venta o problema logístico.');

INSERT INTO tipos_comprobante (nombre, descripcion) VALUES
(N'Factura A', N'Comprobante emitido a Responsables Inscriptos con discriminación de IVA.'),
(N'Factura B', N'Comprobante emitido a Consumidores Finales, Exentos o Monotributistas.'),
(N'Factura C', N'Comprobante de venta emitido por sujetos exentos o Monotributistas.'),
(N'Nota de Crédito A', N'Documento de anulación o descuento sobre una Factura A.'),
(N'Nota de Crédito B', N'Documento de anulación o descuento sobre una Factura B.'),
(N'Nota de Crédito C', N'Documento de anulación o descuento sobre una Factura C.'),
(N'Nota de Débito', N'Comprobante por cargos o intereses adicionales vinculados a una factura previa.');

INSERT INTO medios_de_pago (nombre, descripcion) VALUES
(N'Efectivo', N'Pago en moneda local al momento de la entrega o cobro presencial.'),
(N'Tarjeta de Débito', N'Cobro directo desde cuenta bancaria vía pasarela de pago.'),
(N'Tarjeta de Crédito', N'Pago financiado en cuotas mediante pasarela de pago.'),
(N'Transferencia Bancaria', N'Transferencia directa por CBU/CVU con verificación manual o automática.'),
(N'Mercado Pago', N'Pago procesado mediante billetera virtual o código QR.');

INSERT INTO tipos_movimiento (nombre, descripcion) VALUES
(N'Ingreso por Reposición', N'Ingreso de mercadería comprada a proveedores para reabastecimiento.'),
(N'Venta Online', N'Egreso de stock reservado y vendido a través de la tienda web.'),
(N'Venta Presencial', N'Egreso de stock comerciado de forma directa en punto de venta físico.'),
(N'Devolución de Cliente', N'Ingreso de mercadería restituida por un cliente en buen estado.'),
(N'Producto Fallado / Dañado', N'Egreso por pérdida, rotura, defecto de fábrica o fallo de lectura del soporte físico.'),
(N'Transferencia Interna', N'Movimiento de ajuste de stock entre DEPÓSITO y ONLINE.');

INSERT INTO clientes (nombre, apellido, dni, telefono, direccion, email, cuit_cuil, contrasena, activo) VALUES
(N'Ricardo', N'Pérez', '28507441', '1135874197', N'Libertad 1391, Belén de Escobar', 'ricardo.perez.brivio@gmail.com', '20-28507441-0', '1234', 1);

INSERT INTO productos (sku, nombre, descripcion, descripcion_larga, imagen_ruta, precio, activo_para_venta, activo) VALUES
('MUS-CD-001', N'Pink Floyd - The Dark Side of the Moon (CD)', N'Edición remasterizada en CD audio estándar.', N'Álbum conceptual icónico de 1973 en formato CD digipack remasterizado en alta definición.', '/images/products/pink_floyd_dark_side.jpg', 25000.00, 1, 1),
('MUS-CD-002', N'Daft Punk - Random Access Memories (CD)', N'Álbum de estudio en CD Jewel Case.', N'Edición original en CD que incluye los sencillos Get Lucky y Lose Yourself to Dance.', '/images/products/daft_punk_ram.jpg', 22000.00, 1, 1),
('PEL-DVD-001', N'The Matrix - Edición Especial (DVD)', N'Pelicula en DVD con material extra.', N'Disco DVD región 4 con audio Dolby Digital 5.1 y comentarios del director.', '/images/products/matrix_dvd.jpg', 12000.00, 1, 1),
('PEL-BR-001', N'Interstellar - 4K Ultra HD + BluRay', N'Combo 4K UHD + BluRay Disc.', N'Edición de coleccionista de 2 discos con resolución 4K HDR y pista de audio DTS-HD Master Audio.', '/images/products/interstellar_bluray.jpg', 45000.00, 1, 1),
('PEL-BR-002', N'El Señor de los Anillos: La Comunidad del Anillo (BluRay)', N'Edición Extendida en BluRay.', N'Versión remasterizada en alta definición con escenas inéditas y libro conmemorativo.', '/images/products/lotr_fellowship_bluray.jpg', 38000.00, 1, 1);

INSERT INTO movimientos_stock (id_producto, id_tipo_movimiento, id_empleado, id_venta, tipo_stock, cantidad, motivo) VALUES
-- SKU: MUS-CD-001 - Pink Floyd
(1, 1, NULL, NULL, 'DEPÓSITO', 45, N'Ingreso de lote completo de mercadería a depósito central'),
(1, 6, NULL, NULL, 'DEPÓSITO', -15, N'Transferencia interna: Egreso de depósito hacia canal web'),
(1, 6, NULL, NULL, 'ONLINE', 15, N'Transferencia interna: Habilitación de stock para venta online'),
-- SKU: MUS-CD-002 - Daft Punk
(2, 1, NULL, NULL, 'DEPÓSITO', 30, N'Ingreso de lote completo de mercadería a depósito central'),
(2, 6, NULL, NULL, 'DEPÓSITO', -10, N'Transferencia interna: Egreso de depósito hacia canal web'),
(2, 6, NULL, NULL, 'ONLINE', 10, N'Transferencia interna: Habilitación de stock para venta online'),
-- SKU: PEL-DVD-001 - The Matrix
(3, 1, NULL, NULL, 'DEPÓSITO', 17, N'Ingreso de lote completo de mercadería a depósito central'),
(3, 6, NULL, NULL, 'DEPÓSITO', -5, N'Transferencia interna: Egreso de depósito hacia canal web'),
(3, 6, NULL, NULL, 'ONLINE', 5, N'Transferencia interna: Habilitación de stock para venta online'),
-- SKU: PEL-BR-001 - Interstellar
(4, 1, NULL, NULL, 'DEPÓSITO', 18, N'Ingreso de lote completo de mercadería a depósito central'),
(4, 6, NULL, NULL, 'DEPÓSITO', -8, N'Transferencia interna: Egreso de depósito hacia canal web'),
(4, 6, NULL, NULL, 'ONLINE', 8, N'Transferencia interna: Habilitación de stock para venta online'),
-- SKU: PEL-BR-002 - LOTR
(5, 1, NULL, NULL, 'DEPÓSITO', 37, N'Ingreso de lote completo de mercadería a depósito central'),
(5, 6, NULL, NULL, 'DEPÓSITO', -12, N'Transferencia interna: Egreso de depósito hacia canal web'),
(5, 6, NULL, NULL, 'ONLINE', 12, N'Transferencia interna: Habilitación de stock para venta online');
GO