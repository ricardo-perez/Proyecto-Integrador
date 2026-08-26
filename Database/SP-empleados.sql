USE tienda_deportiva;

--

DROP PROCEDURE IF EXISTS sp_crear_empleado;

DELIMITER //

CREATE PROCEDURE sp_crear_empleado (
    IN p_id_rol INT,
    IN p_legajo INT,
    IN p_nombre VARCHAR(100),
    IN p_apellido VARCHAR(100),
    IN p_dni VARCHAR(20),
    IN p_telefono VARCHAR(30),
    IN p_direccion VARCHAR(150),
    IN p_email VARCHAR(254),
    IN p_nombre_usuario VARCHAR(100),
    IN p_contrasena VARCHAR(255),
    OUT p_id_empleado INT
)
BEGIN
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
        contrasena
    ) VALUES (
        p_id_rol,
        p_legajo,
        p_nombre,
        p_apellido,
        p_dni,
        p_telefono,
        p_direccion,
        p_email,
        p_nombre_usuario,
        p_contrasena
    );

    SET p_id_empleado = LAST_INSERT_ID();
END //

DELIMITER ;

--

DROP PROCEDURE IF EXISTS sp_modificar_empleado;

DELIMITER //

CREATE PROCEDURE sp_modificar_empleado (
    IN p_id_empleado INT,
    IN p_id_rol INT,
    IN p_legajo INT,
    IN p_nombre VARCHAR(100),
    IN p_apellido VARCHAR(100),
    IN p_dni VARCHAR(20),
    IN p_telefono VARCHAR(30),
    IN p_direccion VARCHAR(150),
    IN p_email VARCHAR(254),
    IN p_nombre_usuario VARCHAR(100),
    IN p_contrasena VARCHAR(255),
    IN p_activo BIT
)
BEGIN
    UPDATE empleados
    SET
        id_rol = p_id_rol,
        legajo = p_legajo,
        nombre = p_nombre,
        apellido = p_apellido,
        dni = p_dni,
        telefono = p_telefono,
        direccion = p_direccion,
        email = p_email,
        nombre_usuario = p_nombre_usuario,
        contrasena = p_contrasena,
        activo = p_activo
    WHERE id_empleado = p_id_empleado;
END //

DELIMITER ;

--

DROP PROCEDURE IF EXISTS sp_eliminar_empleado;

DELIMITER //

CREATE PROCEDURE sp_eliminar_empleado (
    IN p_id_empleado INT
)
BEGIN
    UPDATE empleados
    SET activo = 0
    WHERE id_empleado = p_id_empleado;
END //

DELIMITER ;