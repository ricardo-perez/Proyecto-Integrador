USE tienda_deportiva;

--

DROP PROCEDURE IF EXISTS sp_crear_gasto;

DELIMITER //

CREATE PROCEDURE sp_crear_gasto (
	IN p_fecha DATE,
	IN p_descripcion VARCHAR(150),
	IN p_monto DECIMAL(10, 2)
	OUT p_id_gasto INT
)

BEGIN
	INSERT INTO gastos (
		fecha,
		descripcion,
		monto
	) VALUES (
		p_fecha,
		p_descripcion,
		p_monto
	);
	SET p_id_gasto = LAST_INSERT_ID();
END //

DELIMITER ;

--

DROP PROCEDURE IF EXISTS sp_modificar_gasto;

DELIMITER //

CREATE PROCEDURE sp_modificar_gasto (
	IN p_id_gasto INT,
	IN p_fecha DATE,
	IN p_descripcion VARCHAR(150),
	IN p_monto DECIMAL(10, 2)
)

BEGIN
	UPDATE gastos
	SET 
		fecha = p_fecha,
		descripcion = p_descripcion,
		monto = p_monto
	WHERE id_gasto = p_id_gasto;
END //

DELIMITER ;
