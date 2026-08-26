USE tienda_deportiva;

--

DROP PROCEDURE IF EXISTS sp_crear_factura;

DELIMITER //

CREATE PROCEDURE sp_crear_factura (
	IN p_id_venta INT,
	IN p_tipo_factura VARCHAR(5),
	IN p_punto_venta VARCHAR(10),
	IN p_numero_factura VARCHAR(20),
	-- IN p_fecha_emision DATE,
	IN p_total DECIMAL(10, 2),
	OUT p_id_factura INT
)

BEGIN
	INSERT INTO facturas (
		id_venta,
		tipo_factura,
		punto_venta,
		numero_factura,
		-- fecha_emision,
		total
	) VALUES (
		p_id_venta,
		p_tipo_factura,
		p_punto_venta,
		p_numero_factura,
		-- p_fecha_emision,
		p_total
	);
	SET p_id_factura = LAST_INSERT_ID();
END //

DELIMITER ;

--

DROP PROCEDURE IF EXISTS sp_modificar_factura;

DELIMITER //

CREATE PROCEDURE sp_modificar_factura (
	IN p_id_factura INT,
	IN p_id_venta INT,
	IN p_tipo_factura VARCHAR(5),
	IN p_punto_venta VARCHAR(10),
	IN p_numero_factura VARCHAR(20),
	-- IN p_fecha_emision DATE,
	IN p_total DECIMAL(10, 2)
)

BEGIN
	UPDATE facturas
	SET 
		id_venta = p_id_venta,
		tipo_factura = p_tipo_factura,
		punto_venta = p_punto_venta,
		numero_factura = p_numero_factura,
		-- fecha_emision = p_fecha_emision,
		total = p_total
	WHERE id_factura = p_id_factura;
END //

DELIMITER ;

--