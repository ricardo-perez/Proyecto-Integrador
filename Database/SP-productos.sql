USE tienda_deportiva;

--

DROP PROCEDURE IF EXISTS sp_crear_producto;

DELIMITER //

CREATE PROCEDURE sp_crear_producto (
    IN p_codigo VARCHAR(50),
    IN p_descripcion VARCHAR(100),
    IN p_descripcion_larga VARCHAR(255),
    IN p_stock_online INT,
    IN p_stock_fisico INT,
    IN p_activo_para_venta BIT,
    IN p_precio_actual DECIMAL(10, 2),
    OUT p_id_producto INT
)
BEGIN
    INSERT INTO productos (
        codigo,
        descripcion,
        descripcion_larga,
        stock_online,
        stock_fisico,
        activo_para_venta,
        precio_actual
    ) VALUES (
        p_codigo,
        p_descripcion,
        p_descripcion_larga,
        p_stock_online,
        p_stock_fisico,
        p_activo_para_venta,
        p_precio_actual
    );

    SET p_id_producto = LAST_INSERT_ID();
END //

DELIMITER ;

--

DROP PROCEDURE IF EXISTS sp_modificar_producto;

DELIMITER //

CREATE PROCEDURE sp_modificar_producto (
    IN p_id_producto INT,
    IN p_codigo VARCHAR(50),
    IN p_descripcion VARCHAR(100),
    IN p_descripcion_larga VARCHAR(255),
    IN p_stock_online INT,
    IN p_stock_fisico INT,
    IN p_activo_para_venta BIT,
    IN p_precio_actual DECIMAL(10, 2)
)
BEGIN
    UPDATE productos
    SET
        codigo = p_codigo,
        descripcion = p_descripcion,
        descripcion_larga = p_descripcion_larga,
        stock_online = p_stock_online,
        stock_fisico = p_stock_fisico,
        activo_para_venta = p_activo_para_venta,
        precio_actual = p_precio_actual
    WHERE id_producto = p_id_producto;
END //

DELIMITER ;

--

DROP PROCEDURE IF EXISTS sp_eliminar_producto;

DELIMITER //

CREATE PROCEDURE sp_eliminar_producto (
    IN p_id_producto INT
)
BEGIN
    UPDATE productos
    SET activo_para_venta = 0
    WHERE id_producto = p_id_producto;
END //

DELIMITER ;