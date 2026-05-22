-- =============================================
-- SCRIPT: Crear procedimientos almacenados faltantes
-- Base de datos: db_newLife (Azure SQL)
-- Ejecutar en: Azure Portal > SQL Database > Query Editor
-- =============================================

-- =============================================
-- 1. CATEGORIA
-- =============================================

CREATE OR ALTER PROCEDURE sp_Listar_Categorias
AS
BEGIN
    SELECT numero_categoria, nombre, descripcion
    FROM CATEGORIA
    ORDER BY nombre
END
GO

CREATE OR ALTER PROCEDURE sp_Consultar_Categoria
    @numero_categoria INT
AS
BEGIN
    SELECT numero_categoria, nombre, descripcion
    FROM CATEGORIA
    WHERE numero_categoria = @numero_categoria
END
GO

CREATE OR ALTER PROCEDURE sp_Insertar_Categoria
    @nombre VARCHAR(100),
    @descripcion VARCHAR(255)
AS
BEGIN
    INSERT INTO CATEGORIA (nombre, descripcion)
    VALUES (@nombre, @descripcion)
END
GO

-- =============================================
-- 2. ADMINISTRADOR
-- =============================================

CREATE OR ALTER PROCEDURE sp_Listar_Administradores
AS
BEGIN
    SELECT cedula_adm, correo, contrasena, nombres, fecha_registro, estado
    FROM ADMINISTRADOR
    ORDER BY nombres
END
GO

CREATE OR ALTER PROCEDURE sp_Consultar_Administrador
    @cedula_adm VARCHAR(20)
AS
BEGIN
    SELECT cedula_adm, correo, contrasena, nombres, fecha_registro, estado
    FROM ADMINISTRADOR
    WHERE cedula_adm = @cedula_adm
END
GO

CREATE OR ALTER PROCEDURE sp_Insertar_Administrador
    @cedula_adm VARCHAR(20),
    @correo VARCHAR(100),
    @contrasena VARCHAR(100),
    @nombres VARCHAR(150)
AS
BEGIN
    INSERT INTO ADMINISTRADOR (cedula_adm, correo, contrasena, nombres, fecha_registro, estado)
    VALUES (@cedula_adm, @correo, @contrasena, @nombres, GETDATE(), 'Activo')
END
GO

CREATE OR ALTER PROCEDURE sp_Actualizar_Administrador
    @cedula_adm VARCHAR(20),
    @correo VARCHAR(100),
    @nombres VARCHAR(150),
    @estado VARCHAR(10)
AS
BEGIN
    UPDATE ADMINISTRADOR
    SET correo = @correo,
        nombres = @nombres,
        estado = @estado
    WHERE cedula_adm = @cedula_adm
END
GO

-- =============================================
-- 3. DOMICILIARIO
-- =============================================

CREATE OR ALTER PROCEDURE sp_Listar_Domiciliarios
AS
BEGIN
    SELECT cedula_domi, nombres, telefono, fecha_registro, disponibilidad, estado
    FROM DOMICILIARIO
    ORDER BY nombres
END
GO

CREATE OR ALTER PROCEDURE sp_Consultar_Domiciliario
    @cedula_domi VARCHAR(20)
AS
BEGIN
    SELECT cedula_domi, nombres, telefono, fecha_registro, disponibilidad, estado
    FROM DOMICILIARIO
    WHERE cedula_domi = @cedula_domi
END
GO

CREATE OR ALTER PROCEDURE sp_Insertar_Domiciliario
    @cedula_domi VARCHAR(20),
    @nombres VARCHAR(150),
    @telefono VARCHAR(20)
AS
BEGIN
    INSERT INTO DOMICILIARIO (cedula_domi, nombres, telefono, fecha_registro, disponibilidad, estado)
    VALUES (@cedula_domi, @nombres, @telefono, GETDATE(), 'Disponible', 'Activo')
END
GO

CREATE OR ALTER PROCEDURE sp_Actualizar_Domiciliario
    @cedula_domi VARCHAR(20),
    @nombres VARCHAR(150),
    @telefono VARCHAR(20),
    @disponibilidad VARCHAR(20),
    @estado VARCHAR(10)
AS
BEGIN
    UPDATE DOMICILIARIO
    SET nombres = @nombres,
        telefono = @telefono,
        disponibilidad = @disponibilidad,
        estado = @estado
    WHERE cedula_domi = @cedula_domi
END
GO

-- =============================================
-- 4. DESPACHO
-- =============================================

CREATE OR ALTER PROCEDURE sp_Listar_Despachos
AS
BEGIN
    SELECT numero_despacho, fecha_despacho, fecha_aprobacion, estado,
           fecha_entrega, numero_factura, cc_responsable, cc_domiciliario
    FROM DESPACHO
    ORDER BY numero_despacho DESC
END
GO

CREATE OR ALTER PROCEDURE sp_Consultar_Despacho
    @numero_despacho INT
AS
BEGIN
    SELECT numero_despacho, fecha_despacho, fecha_aprobacion, estado,
           fecha_entrega, numero_factura, cc_responsable, cc_domiciliario
    FROM DESPACHO
    WHERE numero_despacho = @numero_despacho
END
GO

CREATE OR ALTER PROCEDURE sp_Insertar_Despacho
    @fecha_despacho DATE,
    @numero_factura VARCHAR(20),
    @cc_responsable VARCHAR(20),
    @cc_domiciliario VARCHAR(20)
AS
BEGIN
    INSERT INTO DESPACHO (fecha_despacho, estado, numero_factura, cc_responsable, cc_domiciliario)
    VALUES (@fecha_despacho, 'Pendiente', @numero_factura, @cc_responsable, @cc_domiciliario)
END
GO

CREATE OR ALTER PROCEDURE sp_Eliminar_Despacho
    @numero_despacho INT
AS
BEGIN
    DELETE FROM DESPACHO WHERE numero_despacho = @numero_despacho
END
GO

-- =============================================
-- 5. CLIENTE
-- =============================================

CREATE OR ALTER PROCEDURE sp_Listar_Clientes
AS
BEGIN
    SELECT numero_identificacion, nombres, tipo_documento, direccion, telefono,
           correo, fecha_registro, tipo_cliente, estado, cedula_adm
    FROM CLIENTE
    ORDER BY nombres
END
GO

CREATE OR ALTER PROCEDURE sp_Consultar_Cliente
    @numero_identificacion VARCHAR(20)
AS
BEGIN
    SELECT numero_identificacion, nombres, tipo_documento, direccion, telefono,
           correo, fecha_registro, tipo_cliente, estado, cedula_adm
    FROM CLIENTE
    WHERE numero_identificacion = @numero_identificacion
END
GO

CREATE OR ALTER PROCEDURE sp_Insertar_Cliente
    @numero_identificacion VARCHAR(20),
    @nombres VARCHAR(150),
    @tipo_documento VARCHAR(10),
    @direccion VARCHAR(255),
    @telefono VARCHAR(20),
    @correo VARCHAR(100),
    @contrasena VARCHAR(100),
    @tipo_cliente VARCHAR(20),
    @estado VARCHAR(10),
    @cedula_adm VARCHAR(20)
AS
BEGIN
    INSERT INTO CLIENTE (numero_identificacion, nombres, tipo_documento, direccion, telefono,
                         correo, contrasena, tipo_cliente, estado, fecha_registro, cedula_adm)
    VALUES (@numero_identificacion, @nombres, @tipo_documento, @direccion, @telefono,
            @correo, @contrasena, @tipo_cliente, @estado, GETDATE(), @cedula_adm)
END
GO

CREATE OR ALTER PROCEDURE sp_Actualizar_Cliente
    @numero_identificacion VARCHAR(20),
    @nombres VARCHAR(150),
    @tipo_documento VARCHAR(10),
    @direccion VARCHAR(255),
    @telefono VARCHAR(20),
    @correo VARCHAR(100),
    @tipo_cliente VARCHAR(20),
    @estado VARCHAR(10),
    @cedula_adm VARCHAR(20)
AS
BEGIN
    UPDATE CLIENTE
    SET nombres = @nombres,
        tipo_documento = @tipo_documento,
        direccion = @direccion,
        telefono = @telefono,
        correo = @correo,
        tipo_cliente = @tipo_cliente,
        estado = @estado,
        cedula_adm = @cedula_adm
    WHERE numero_identificacion = @numero_identificacion
END
GO

-- =============================================
-- 6. Columna fecha_ultima_modificacion en PRODUCTO
--    (necesaria para que el endpoint /api/producto no falle)
-- =============================================
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE Name = N'fecha_ultima_modificacion'
    AND Object_ID = OBJECT_ID(N'PRODUCTO')
)
BEGIN
    ALTER TABLE PRODUCTO ADD fecha_ultima_modificacion DATETIME NULL
END
GO

-- =============================================
-- 7. Columna stock_real en PRODUCTO (por si acaso no existe)
-- =============================================
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE Name = N'stock_real'
    AND Object_ID = OBJECT_ID(N'PRODUCTO')
)
BEGIN
    ALTER TABLE PRODUCTO ADD stock_real INT NOT NULL DEFAULT 0
END
GO

-- =============================================
-- 8. Columna precio_unitario en DETALLE_FACTURA (para el checkout)
-- =============================================
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE Name = N'precio_unitario'
    AND Object_ID = OBJECT_ID(N'DETALLE_FACTURA')
)
BEGIN
    ALTER TABLE DETALLE_FACTURA ADD precio_unitario DECIMAL(10,2) NOT NULL DEFAULT 0
END
GO

-- =============================================
-- 9. TIPO_PRODUCTO
-- =============================================

CREATE OR ALTER PROCEDURE sp_Listar_TiposProducto
AS
BEGIN
    SELECT id_tipo_producto, nombre, descripcion
    FROM TIPO_PRODUCTO
    ORDER BY nombre
END
GO

CREATE OR ALTER PROCEDURE sp_Consultar_TipoProducto
    @id_tipo_producto INT
AS
BEGIN
    SELECT id_tipo_producto, nombre, descripcion
    FROM TIPO_PRODUCTO
    WHERE id_tipo_producto = @id_tipo_producto
END
GO

CREATE OR ALTER PROCEDURE sp_Insertar_TipoProducto
    @nombre VARCHAR(100),
    @descripcion VARCHAR(255)
AS
BEGIN
    INSERT INTO TIPO_PRODUCTO (nombre, descripcion)
    VALUES (@nombre, @descripcion)
END
GO

CREATE OR ALTER PROCEDURE sp_Actualizar_TipoProducto
    @id_tipo_producto INT,
    @nombre VARCHAR(100),
    @descripcion VARCHAR(255)
AS
BEGIN
    UPDATE TIPO_PRODUCTO
    SET nombre = @nombre,
        descripcion = @descripcion
    WHERE id_tipo_producto = @id_tipo_producto
END
GO

CREATE OR ALTER PROCEDURE sp_Eliminar_TipoProducto
    @id_tipo_producto INT
AS
BEGIN
    DELETE FROM TIPO_PRODUCTO WHERE id_tipo_producto = @id_tipo_producto
END
GO

-- =============================================
-- 10. Columna contrasena en RESPONSABLE
-- =============================================
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE Name = N'contrasena'
    AND Object_ID = OBJECT_ID(N'RESPONSABLE')
)
BEGIN
    ALTER TABLE RESPONSABLE ADD contrasena VARCHAR(100) NULL DEFAULT '111111'
END
GO

UPDATE RESPONSABLE SET contrasena = '111111' WHERE contrasena IS NULL
GO

-- =============================================
-- VERIFICACION FINAL
-- =============================================
SELECT name AS procedimiento_creado
FROM sys.objects
WHERE type = 'P'
ORDER BY name
