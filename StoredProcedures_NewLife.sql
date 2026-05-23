-- ============================================================
-- STORED PROCEDURES - NewLife
-- Incluye: SPs faltantes + correcciones de SPs existentes
-- Ejecutar en Azure SQL db_newLife
-- ============================================================

-- ============================================================
-- CORRECCIONES DE SPs EXISTENTES (parametros desactualizados)
-- ============================================================
GO
CREATE OR ALTER PROCEDURE sp_Insertar_Producto
    @codigo_prod VARCHAR(20), @nombres VARCHAR(100), @descripcion VARCHAR(500),
    @stock_min INT, @stock_real INT, @img_url VARCHAR(500),
    @precio DECIMAL(10,2), @descuento DECIMAL(5,2),
    @capacidad VARCHAR(50), @temperatura_uso VARCHAR(50),
    @numero_categoria INT, @id_tipo_producto INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO PRODUCTO (codigo_prod,nombres,descripcion,stock_min,stock_real,img_url,precio,descuento,capacidad,temperatura_uso,numero_categoria,id_tipo_producto,fecha_registro,estado)
    VALUES (@codigo_prod,@nombres,@descripcion,@stock_min,@stock_real,@img_url,@precio,@descuento,@capacidad,@temperatura_uso,@numero_categoria,@id_tipo_producto,GETDATE(),'Activo')
END
GO
CREATE OR ALTER PROCEDURE sp_Insertar_Responsable
    @cedula_resp VARCHAR(20), @nombres VARCHAR(100),
    @telefono VARCHAR(20), @correo VARCHAR(100), @contrasena VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO RESPONSABLE (cedula_resp,nombres,telefono,correo,contrasena,fecha_registro,estado)
    VALUES (@cedula_resp,@nombres,@telefono,@correo,@contrasena,GETDATE(),'Activo')
END
GO
CREATE OR ALTER PROCEDURE sp_Insertar_DetalleFactura
    @cantidad INT, @descuento_porcentaje DECIMAL(5,2),
    @numero_factura VARCHAR(20), @codigo_prod VARCHAR(20), @precio_unitario DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO DETALLE_FACTURA (cantidad,descuento_porcentaje,numero_factura,codigo_prod,precio_unitario)
    VALUES (@cantidad,@descuento_porcentaje,@numero_factura,@codigo_prod,@precio_unitario)
END
GO
CREATE OR ALTER PROCEDURE sp_Insertar_Administrador
    @cedula_adm VARCHAR(20), @correo VARCHAR(100),
    @contrasena VARCHAR(255), @nombres VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ADMINISTRADOR (cedula_adm,correo,contrasena,nombres,fecha_registro,estado)
    VALUES (@cedula_adm,@correo,@contrasena,@nombres,GETDATE(),'Activo')
END
GO
CREATE OR ALTER PROCEDURE sp_Insertar_Transporte
    @cedula_domi VARCHAR(20), @placa VARCHAR(20),
    @tipo VARCHAR(50), @descripcion VARCHAR(200), @estado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO TRANSPORTE (cedula_domi,placa,tipo,descripcion,estado)
    VALUES (@cedula_domi,@placa,@tipo,@descripcion,@estado)
END
GO

-- sp_Listar_Productos: actualizar para incluir todas las columnas actuales
GO
CREATE OR ALTER PROCEDURE sp_Listar_Productos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT codigo_prod, nombres, descripcion, stock_min,
           ISNULL(stock_real, 0) AS stock_real, img_url, fecha_registro,
           precio, ISNULL(descuento, 0) AS descuento, estado,
           capacidad, temperatura_uso, numero_categoria, id_tipo_producto
    FROM PRODUCTO ORDER BY nombres
END
GO

-- ============================================================
-- CLIENTE
-- ============================================================
GO
CREATE OR ALTER PROCEDURE sp_Actualizar_Cliente
    @numero_identificacion VARCHAR(20), @nombres VARCHAR(100),
    @tipo_documento VARCHAR(10), @direccion VARCHAR(200),
    @telefono VARCHAR(20), @correo VARCHAR(100),
    @tipo_cliente VARCHAR(20), @estado VARCHAR(20), @cedula_adm VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE CLIENTE SET nombres=@nombres,tipo_documento=@tipo_documento,
        direccion=@direccion,telefono=@telefono,correo=@correo,
        tipo_cliente=@tipo_cliente,estado=@estado,cedula_adm=@cedula_adm
    WHERE numero_identificacion=@numero_identificacion
END
GO
CREATE OR ALTER PROCEDURE sp_Eliminar_Cliente
    @numero_identificacion VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM CLIENTE WHERE numero_identificacion=@numero_identificacion
END
GO
CREATE OR ALTER PROCEDURE sp_Consultar_Cliente
    @numero_identificacion VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT numero_identificacion,nombres,tipo_documento,direccion,telefono,correo,
           fecha_registro,tipo_cliente,estado,cedula_adm
    FROM CLIENTE WHERE numero_identificacion=@numero_identificacion
END
GO
CREATE OR ALTER PROCEDURE sp_Login_Cliente
    @correo VARCHAR(100), @contrasena VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT numero_identificacion,nombres,tipo_documento,direccion,telefono,correo,
           fecha_registro,tipo_cliente,estado,cedula_adm
    FROM CLIENTE WHERE correo=@correo AND contrasena=@contrasena AND estado='Activo'
END
GO
CREATE OR ALTER PROCEDURE sp_CambiarContrasena_Cliente
    @correo VARCHAR(100), @nuevaContrasena VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE CLIENTE SET contrasena=@nuevaContrasena WHERE correo=@correo
END
GO
CREATE OR ALTER PROCEDURE sp_ExisteCorreo_Cliente
    @correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) FROM CLIENTE WHERE correo=@correo AND estado='Activo'
END
GO

-- ============================================================
-- RESPONSABLE
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Actualizar_Responsable
    @cedula_resp VARCHAR(20), @nombres VARCHAR(100),
    @telefono VARCHAR(20), @correo VARCHAR(100),
    @contrasena VARCHAR(255), @estado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    IF @contrasena IS NOT NULL AND @contrasena != ''
        UPDATE RESPONSABLE SET nombres=@nombres,telefono=@telefono,correo=@correo,contrasena=@contrasena,estado=@estado WHERE cedula_resp=@cedula_resp
    ELSE
        UPDATE RESPONSABLE SET nombres=@nombres,telefono=@telefono,correo=@correo,estado=@estado WHERE cedula_resp=@cedula_resp
END
GO
CREATE OR ALTER PROCEDURE sp_Eliminar_Responsable
    @cedula_resp VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM RESPONSABLE WHERE cedula_resp=@cedula_resp
END
GO
CREATE OR ALTER PROCEDURE sp_Consultar_Responsable
    @cedula_resp VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT cedula_resp,nombres,telefono,correo,ISNULL(contrasena,'111111') AS contrasena,fecha_registro,estado
    FROM RESPONSABLE WHERE cedula_resp=@cedula_resp
END
GO
CREATE OR ALTER PROCEDURE sp_CambiarContrasena_Responsable
    @correo VARCHAR(100), @nuevaContrasena VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE RESPONSABLE SET contrasena=@nuevaContrasena WHERE correo=@correo
END
GO
CREATE OR ALTER PROCEDURE sp_ExisteCorreo_Responsable
    @correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) FROM RESPONSABLE WHERE correo=@correo
END
GO

-- ============================================================
-- PRODUCTO
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Actualizar_Producto
    @codigo_prod VARCHAR(20), @nombres VARCHAR(100),
    @descripcion VARCHAR(500), @stock_min INT, @stock_real INT,
    @img_url VARCHAR(500), @precio DECIMAL(10,2), @descuento DECIMAL(5,2),
    @estado VARCHAR(20), @capacidad VARCHAR(50), @temperatura_uso VARCHAR(50),
    @numero_categoria INT, @id_tipo_producto INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE PRODUCTO SET nombres=@nombres,descripcion=@descripcion,stock_min=@stock_min,
        stock_real=@stock_real,img_url=@img_url,precio=@precio,descuento=@descuento,
        estado=@estado,capacidad=@capacidad,temperatura_uso=@temperatura_uso,
        numero_categoria=@numero_categoria,id_tipo_producto=@id_tipo_producto
    WHERE codigo_prod=@codigo_prod
END
GO
CREATE OR ALTER PROCEDURE sp_Eliminar_Producto
    @codigo_prod VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM PRODUCTO WHERE codigo_prod=@codigo_prod
END
GO
CREATE OR ALTER PROCEDURE sp_Consultar_Producto
    @codigo_prod VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT codigo_prod,nombres,descripcion,stock_min,stock_real,img_url,fecha_registro,
           precio,ISNULL(descuento,0) AS descuento,estado,capacidad,temperatura_uso,
           numero_categoria,id_tipo_producto
    FROM PRODUCTO WHERE codigo_prod=@codigo_prod
END
GO
CREATE OR ALTER PROCEDURE sp_ReducirStock_Producto
    @codigo_prod VARCHAR(20), @cantidad INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE PRODUCTO SET stock_real=CASE WHEN stock_real>=@cantidad THEN stock_real-@cantidad ELSE 0 END
    WHERE codigo_prod=@codigo_prod
END
GO

-- ============================================================
-- FACTURA_VENTA
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Actualizar_FacturaVenta
    @numero_factura VARCHAR(20), @metodo_pago VARCHAR(50),
    @estado_pago VARCHAR(30), @direccion_envio VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE FACTURA_VENTA SET metodo_pago=@metodo_pago,estado_pago=@estado_pago,direccion_envio=@direccion_envio
    WHERE numero_factura=@numero_factura
END
GO
CREATE OR ALTER PROCEDURE sp_Eliminar_FacturaVenta
    @numero_factura VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM DETALLE_FACTURA WHERE numero_factura=@numero_factura;
    DELETE FROM FACTURA_VENTA WHERE numero_factura=@numero_factura;
END
GO
CREATE OR ALTER PROCEDURE sp_Consultar_FacturaVenta
    @numero_factura VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT numero_factura,fecha,metodo_pago,estado_pago,direccion_envio,cedula_cli
    FROM FACTURA_VENTA WHERE numero_factura=@numero_factura
END
GO

-- ============================================================
-- DETALLE_FACTURA
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Actualizar_DetalleFactura
    @num_f_codigo INT, @cantidad INT,
    @descuento_porcentaje DECIMAL(5,2), @precio_unitario DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DETALLE_FACTURA SET cantidad=@cantidad,descuento_porcentaje=@descuento_porcentaje,precio_unitario=@precio_unitario
    WHERE num_f_codigo=@num_f_codigo
END
GO
CREATE OR ALTER PROCEDURE sp_Eliminar_DetalleFactura
    @num_f_codigo INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM DETALLE_FACTURA WHERE num_f_codigo=@num_f_codigo
END
GO
CREATE OR ALTER PROCEDURE sp_Consultar_DetalleFactura
    @num_f_codigo INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT num_f_codigo,cantidad,descuento_porcentaje,numero_factura,codigo_prod,precio_unitario
    FROM DETALLE_FACTURA WHERE num_f_codigo=@num_f_codigo
END
GO

-- ============================================================
-- ADMINISTRADOR
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Actualizar_Administrador
    @cedula_adm VARCHAR(20), @correo VARCHAR(100),
    @nombres VARCHAR(100), @estado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ADMINISTRADOR SET correo=@correo,nombres=@nombres,estado=@estado WHERE cedula_adm=@cedula_adm
END
GO
CREATE OR ALTER PROCEDURE sp_Eliminar_Administrador
    @cedula_adm VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ADMINISTRADOR WHERE cedula_adm=@cedula_adm
END
GO
CREATE OR ALTER PROCEDURE sp_Consultar_Administrador
    @cedula_adm VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT cedula_adm,correo,contrasena,nombres,fecha_registro,estado
    FROM ADMINISTRADOR WHERE cedula_adm=@cedula_adm
END
GO

-- ============================================================
-- DOMICILIARIO
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Actualizar_Domiciliario
    @cedula_domi VARCHAR(20), @nombres VARCHAR(100),
    @telefono VARCHAR(20), @disponibilidad VARCHAR(20), @estado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DOMICILIARIO SET nombres=@nombres,telefono=@telefono,disponibilidad=@disponibilidad,estado=@estado
    WHERE cedula_domi=@cedula_domi
END
GO
CREATE OR ALTER PROCEDURE sp_Eliminar_Domiciliario
    @cedula_domi VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DESPACHO SET cc_domiciliario=NULL WHERE cc_domiciliario=@cedula_domi;
    DELETE FROM TRANSPORTE WHERE cedula_domi=@cedula_domi;
    DELETE FROM DOMICILIARIO WHERE cedula_domi=@cedula_domi;
END
GO
CREATE OR ALTER PROCEDURE sp_Consultar_Domiciliario
    @cedula_domi VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT cedula_domi,nombres,telefono,fecha_registro,disponibilidad,estado
    FROM DOMICILIARIO WHERE cedula_domi=@cedula_domi
END
GO

-- ============================================================
-- DESPACHO
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Actualizar_Despacho
    @numero_despacho INT, @fecha_despacho DATE,
    @fecha_aprobacion DATETIME, @estado VARCHAR(30),
    @fecha_entrega DATETIME, @cc_responsable VARCHAR(20), @cc_domiciliario VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DESPACHO SET fecha_despacho=@fecha_despacho,fecha_aprobacion=@fecha_aprobacion,
        estado=@estado,fecha_entrega=@fecha_entrega,
        cc_responsable=NULLIF(@cc_responsable,''),cc_domiciliario=NULLIF(@cc_domiciliario,'')
    WHERE numero_despacho=@numero_despacho
END
GO
CREATE OR ALTER PROCEDURE sp_Eliminar_Despacho
    @numero_despacho INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM DESPACHO WHERE numero_despacho=@numero_despacho
END
GO
CREATE OR ALTER PROCEDURE sp_Consultar_Despacho
    @numero_despacho INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT numero_despacho,fecha_despacho,fecha_aprobacion,estado,fecha_entrega,
           numero_factura,cc_responsable,cc_domiciliario
    FROM DESPACHO WHERE numero_despacho=@numero_despacho
END
GO

-- ============================================================
-- TIPO_PRODUCTO
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Actualizar_TipoProducto
    @id_tipo_producto INT, @nombre VARCHAR(100), @descripcion VARCHAR(300)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TIPO_PRODUCTO SET nombre=@nombre,descripcion=@descripcion WHERE id_tipo_producto=@id_tipo_producto
END
GO
CREATE OR ALTER PROCEDURE sp_Eliminar_TipoProducto
    @id_tipo_producto INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM TIPO_PRODUCTO WHERE id_tipo_producto=@id_tipo_producto
END
GO
CREATE OR ALTER PROCEDURE sp_Consultar_TipoProducto
    @id_tipo_producto INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_tipo_producto,nombre,descripcion FROM TIPO_PRODUCTO WHERE id_tipo_producto=@id_tipo_producto
END
GO

-- ============================================================
-- TRANSPORTE
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Actualizar_Transporte
    @placa VARCHAR(20), @cedula_domi VARCHAR(20),
    @tipo VARCHAR(50), @descripcion VARCHAR(200), @estado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TRANSPORTE SET cedula_domi=@cedula_domi,tipo=@tipo,descripcion=@descripcion,estado=@estado
    WHERE placa=@placa
END
GO
CREATE OR ALTER PROCEDURE sp_Eliminar_Transporte
    @placa VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM TRANSPORTE WHERE placa=@placa
END
GO
CREATE OR ALTER PROCEDURE sp_Consultar_Transporte
    @placa VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT cedula_domi,placa,tipo,descripcion,estado FROM TRANSPORTE WHERE placa=@placa
END
GO

-- ============================================================
-- CATEGORIA
-- ============================================================
CREATE OR ALTER PROCEDURE sp_Actualizar_Categoria
    @numero_categoria INT, @nombre VARCHAR(100), @descripcion VARCHAR(300)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE CATEGORIA SET nombre=@nombre,descripcion=@descripcion WHERE numero_categoria=@numero_categoria
END
GO
CREATE OR ALTER PROCEDURE sp_Eliminar_Categoria
    @numero_categoria INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM CATEGORIA WHERE numero_categoria=@numero_categoria
END
GO
CREATE OR ALTER PROCEDURE sp_Consultar_Categoria
    @numero_categoria INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT numero_categoria,nombre,descripcion FROM CATEGORIA WHERE numero_categoria=@numero_categoria
END
GO

-- ============================================================
-- CODIGOS_VERIFICACION
-- ============================================================
CREATE OR ALTER PROCEDURE sp_GuardarCodigo_Verificacion
    @correo VARCHAR(255), @codigo VARCHAR(10), @tipo VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE CODIGOS_VERIFICACION SET usado=1 WHERE correo=@correo AND tipo=@tipo AND usado=0;
    INSERT INTO CODIGOS_VERIFICACION (correo,codigo,tipo,fecha_creacion,fecha_expiracion,usado)
    VALUES (@correo,@codigo,@tipo,GETDATE(),DATEADD(MINUTE,15,GETDATE()),0);
END
GO
CREATE OR ALTER PROCEDURE sp_VerificarCodigo
    @correo VARCHAR(255), @codigo VARCHAR(10), @tipo VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @valido INT=0;
    SELECT @valido=COUNT(*) FROM CODIGOS_VERIFICACION
    WHERE correo=@correo AND codigo=@codigo AND tipo=@tipo AND usado=0 AND fecha_expiracion>GETDATE();
    IF @valido>0
        UPDATE CODIGOS_VERIFICACION SET usado=1
        WHERE correo=@correo AND codigo=@codigo AND tipo=@tipo AND usado=0;
    SELECT @valido AS resultado;
END
GO
