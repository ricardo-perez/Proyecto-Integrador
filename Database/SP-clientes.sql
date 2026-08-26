USE tienda_deportiva;

--

DROP PROCEDURE IF EXISTS sp_crear_cliente;

DELIMITER //

CREATE PROCEDURE sp_crear_cliente (
    IN p_nombre VARCHAR(100),
    IN p_apellido VARCHAR(100),
    IN p_dni VARCHAR(20),
    IN p_telefono VARCHAR(30),
    IN p_direccion VARCHAR(150),
    IN p_email VARCHAR(254),
    IN p_cuit_cuil VARCHAR(20),
    IN p_contrasena VARCHAR(255),
    OUT p_id_cliente INT
)
BEGIN
    INSERT INTO clientes (
        nombre,
        apellido,
        dni,
        telefono,
        direccion,
        email,
        cuit_cuil,
        contrasena
    ) VALUES (
        p_nombre,
        p_apellido,
        p_dni,
        p_telefono,
        p_direccion,
        p_email,
        p_cuit_cuil,
        p_contrasena,
    );

    SET p_id_cliente = LAST_INSERT_ID();
END //

DELIMITER ;

--

DROP PROCEDURE IF EXISTS sp_modificar_cliente;

DELIMITER //

CREATE PROCEDURE sp_modificar_cliente (
    IN p_id_cliente INT,
    IN p_nombre VARCHAR(100),
    IN p_apellido VARCHAR(100),
    IN p_dni VARCHAR(20),
    IN p_telefono VARCHAR(30),
    IN p_direccion VARCHAR(150),
    IN p_email VARCHAR(254),
    IN p_cuit_cuil VARCHAR(20),
    IN p_contrasena VARCHAR(255),
    IN p_activo BIT
)
BEGIN
    UPDATE clientes
    SET
        nombre = p_nombre,
        apellido = p_apellido,
        dni = p_dni,
        telefono = p_telefono,
        direccion = p_direccion,
        email = p_email,
        cuit_cuil = p_cuit_cuil,
        contrasena = p_contrasena,
        activo = p_activo
    WHERE id_cliente = p_id_cliente;
END //

DELIMITER ;

--

DROP PROCEDURE IF EXISTS sp_eliminar_cliente;

DELIMITER //

CREATE PROCEDURE sp_eliminar_cliente (
    IN p_id_cliente INT
)
BEGIN
    UPDATE clientes
    SET activo = 0
    WHERE id_cliente = p_id_cliente;
END //

DELIMITER ;