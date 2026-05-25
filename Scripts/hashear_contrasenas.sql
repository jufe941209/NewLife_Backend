-- =============================================
-- SCRIPT: Hashing de contraseñas y SPs faltantes
-- Base de datos: db_newLife (Azure SQL)
-- Ejecutar ANTES de redesplegar el backend
-- =============================================

-- =============================================
-- 1. Ampliar columnas contrasena a VARCHAR(255)
--    (SHA-256 = 64 chars, pero dejamos margen)
-- =============================================
ALTER TABLE ADMINISTRADOR ALTER COLUMN contrasena VARCHAR(255) NULL
GO
ALTER TABLE CLIENTE ALTER COLUMN contrasena VARCHAR(255) NULL
GO
ALTER TABLE RESPONSABLE ALTER COLUMN contrasena VARCHAR(255) NULL
GO
-- DOMICILIARIO: asegurarse que la columna existe y es VARCHAR(255)
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE Name = N'contrasena' AND Object_ID = OBJECT_ID(N'DOMICILIARIO')
)
BEGIN
    ALTER TABLE DOMICILIARIO ADD contrasena VARCHAR(255) NULL DEFAULT '111111'
END
ELSE
BEGIN
    ALTER TABLE DOMICILIARIO ALTER COLUMN contrasena VARCHAR(255) NULL
END
GO
UPDATE DOMICILIARIO SET contrasena = '111111' WHERE contrasena IS NULL
GO

-- =============================================
-- 2. SP para actualizar contraseña de Administrador
-- =============================================
CREATE OR ALTER PROCEDURE sp_ActualizarContrasena_Administrador
    @cedula_adm VARCHAR(20),
    @contrasena VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ADMINISTRADOR SET contrasena = @contrasena WHERE cedula_adm = @cedula_adm
END
GO

-- =============================================
-- 3. Actualizar SPs de Domiciliario para incluir contrasena
-- =============================================
CREATE OR ALTER PROCEDURE sp_Listar_Domiciliarios
AS
BEGIN
    SET NOCOUNT ON;
    SELECT cedula_domi, nombres, telefono, fecha_registro, disponibilidad, estado,
           ISNULL(contrasena, '111111') AS contrasena
    FROM DOMICILIARIO
    ORDER BY nombres
END
GO

CREATE OR ALTER PROCEDURE sp_Consultar_Domiciliario
    @cedula_domi VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT cedula_domi, nombres, telefono, fecha_registro, disponibilidad, estado,
           ISNULL(contrasena, '111111') AS contrasena
    FROM DOMICILIARIO
    WHERE cedula_domi = @cedula_domi
END
GO

CREATE OR ALTER PROCEDURE sp_Actualizar_Domiciliario
    @cedula_domi VARCHAR(20),
    @nombres     VARCHAR(100),
    @telefono    VARCHAR(20),
    @disponibilidad VARCHAR(20),
    @estado      VARCHAR(20),
    @contrasena  VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DOMICILIARIO
    SET nombres        = @nombres,
        telefono       = @telefono,
        disponibilidad = @disponibilidad,
        estado         = @estado,
        contrasena     = @contrasena
    WHERE cedula_domi = @cedula_domi
END
GO

-- =============================================
-- 4. Actualizar SP Responsable para incluir contrasena
-- =============================================
CREATE OR ALTER PROCEDURE sp_Listar_Responsables
AS
BEGIN
    SET NOCOUNT ON;
    SELECT cedula_resp, nombres, telefono, correo,
           ISNULL(contrasena, '111111') AS contrasena,
           fecha_registro, estado
    FROM RESPONSABLE
    ORDER BY nombres
END
GO

-- =============================================
-- 5. Actualizar SP Insertar_Responsable para contrasena
-- =============================================
CREATE OR ALTER PROCEDURE sp_Insertar_Responsable
    @cedula_resp VARCHAR(20),
    @nombres     VARCHAR(100),
    @telefono    VARCHAR(20),
    @correo      VARCHAR(100),
    @contrasena  VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO RESPONSABLE (cedula_resp, nombres, telefono, correo, contrasena, fecha_registro, estado)
    VALUES (@cedula_resp, @nombres, @telefono, @correo, @contrasena, GETDATE(), 'Activo')
END
GO

-- =============================================
-- 6. Actualizar SP Actualizar_Responsable para contrasena
-- =============================================
CREATE OR ALTER PROCEDURE sp_Actualizar_Responsable
    @cedula_resp VARCHAR(20),
    @nombres     VARCHAR(100),
    @telefono    VARCHAR(20),
    @correo      VARCHAR(100),
    @contrasena  VARCHAR(255),
    @estado      VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE RESPONSABLE
    SET nombres    = @nombres,
        telefono   = @telefono,
        correo     = @correo,
        contrasena = CASE WHEN @contrasena = '' THEN contrasena ELSE @contrasena END,
        estado     = @estado
    WHERE cedula_resp = @cedula_resp
END
GO

-- =============================================
-- 7. MIGRACIÓN: hashear contraseñas en texto plano
--    SHA2_256 de SQL Server produce VARBINARY(32),
--    lo convertimos a hex de 64 chars igual que C# Sha256()
--    NOTA: Solo correr UNA VEZ. El backend ya detecta hashes
--    con EsHash() y no doble-hashea.
-- =============================================

-- Clientes: solo hashear donde la contraseña NO tenga 64 chars (= no es hash SHA-256)
UPDATE CLIENTE
SET contrasena = LOWER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', ISNULL(contrasena,'111111')), 2))
WHERE LEN(ISNULL(contrasena,'')) <> 64
GO

-- Administradores
UPDATE ADMINISTRADOR
SET contrasena = LOWER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', contrasena), 2))
WHERE LEN(ISNULL(contrasena,'')) <> 64
GO

-- Responsables
UPDATE RESPONSABLE
SET contrasena = LOWER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', ISNULL(contrasena,'111111')), 2))
WHERE LEN(ISNULL(contrasena,'')) <> 64
GO

-- Domiciliarios
UPDATE DOMICILIARIO
SET contrasena = LOWER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', ISNULL(contrasena,'111111')), 2))
WHERE LEN(ISNULL(contrasena,'')) <> 64
GO

-- =============================================
-- 8. Verificación: ver primeras filas de cada tabla
-- =============================================
SELECT 'ADMINISTRADOR' AS tabla, cedula_adm AS id, LEFT(contrasena,10) AS hash_inicio, LEN(contrasena) AS len FROM ADMINISTRADOR
UNION ALL
SELECT 'CLIENTE', LEFT(numero_identificacion,10), LEFT(contrasena,10), LEN(contrasena) FROM CLIENTE
UNION ALL
SELECT 'RESPONSABLE', cedula_resp, LEFT(contrasena,10), LEN(contrasena) FROM RESPONSABLE
UNION ALL
SELECT 'DOMICILIARIO', cedula_domi, LEFT(contrasena,10), LEN(contrasena) FROM DOMICILIARIO
GO
