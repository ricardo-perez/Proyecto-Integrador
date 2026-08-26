USE tienda_deportiva;

--

DROP PROCEDURE IF EXISTS sp_crear_venta;

DELIMITER //

CREATE PROCEDURE sp_crear_venta (
    IN p_id_cliente INT,
    IN p_medio_de_pago VARCHAR(50),
    IN p_monto_total DECIMAL(10, 2),
    OUT p_id_venta INT
)
BEGIN
    INSERT INTO ventas (
        id_cliente,
        medio_de_pago,
        monto_total
    ) VALUES (
        p_id_cliente,
        p_medio_de_pago,
        p_monto_total
    );

    SET p_id_venta = LAST_INSERT_ID();
END //

DELIMITER ;

--

DROP PROCEDURE IF EXISTS sp_actualizar_stock;

DELIMITER //

CREATE PROCEDURE sp_actualizar_stock (
    IN p_id_producto INT,
    IN p_cantidad INT
)
BEGIN
    DECLARE v_activo_para_venta BIT DEFAULT 0;
    DECLARE v_cantidad_filas INT DEFAULT 0;

    SELECT activo_para_venta 
    INTO v_activo_para_venta
    FROM productos 
    WHERE id_producto = p_id_producto;

    IF v_activo_para_venta IS NULL OR v_activo_para_venta = 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El producto no existe o no se encuentra activo para la venta.';
    END IF;

    UPDATE productos
    SET stock_online = stock_online - p_cantidad
    WHERE id_producto = p_id_producto
        AND stock_online >= p_cantidad;

    SET v_cantidad_filas = ROW_COUNT();

    IF v_cantidad_filas = 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Stock online insuficiente para realizar la operación.';
    END IF;
END //

DELIMITER ;

--

DROP PROCEDURE IF EXISTS sp_crear_detalle_venta;

DELIMITER //

CREATE PROCEDURE sp_crear_detalle_venta (
    IN p_id_venta INT,
    IN p_id_producto INT,
    IN p_cantidad INT,
    IN p_precio_unitario DECIMAL(10, 2)
)
BEGIN
    DECLARE v_subtotal DECIMAL(10, 2);

    CALL sp_actualizar_stock(p_id_producto, p_cantidad);

    SET v_subtotal = p_cantidad * p_precio_unitario;

    INSERT INTO detalles_ventas (
        id_venta,
        id_producto,
        cantidad,
        precio_unitario,
        subtotal
    ) VALUES (
        p_id_venta,
        p_id_producto,
        p_cantidad,
        p_precio_unitario,
        v_subtotal
    );
END //

DELIMITER ;
