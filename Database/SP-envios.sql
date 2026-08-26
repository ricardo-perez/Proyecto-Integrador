USE tienda_deportiva;

--

DROP PROCEDURE IF EXISTS sp_crear_envio;

DELIMITER //

CREATE PROCEDURE sp_crear_envio (
	IN p_id_venta int,
    INT p_estado VARCHAR(30)
	OUT p_id_envio INT
)
BEGIN
    INSERT INTO envios (
        id_venta,
        estado
    ) VALUES (
        p_id_venta,
        p_estado
    );

    SET p_id_envio = LAST_INSERT_ID();
END //

DELIMITER ;

--

DROP PROCEDURE IF EXISTS sp_modificar_envio;

DELIMITER //

CREATE PROCEDURE sp_modificar_envio (
    IN p_id_envio INT,
    IN p_id_venta INT,
    IN p_estado VARCHAR(30)
)

BEGIN
    UPDATE envios
    SET 
        id_venta = p_id_venta,
        estado = p_estado
    WHERE id_envio = p_id_envio;
END //

DELIMITER ;
