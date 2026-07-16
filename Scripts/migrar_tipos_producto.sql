-- ============================================================
-- MIGRACIÓN: Limpiar TIPO_PRODUCTO
-- Dejar solo: Semillas y Biodegradables
-- Migrar todas las claves foráneas antes de eliminar tipos viejos
-- ============================================================

BEGIN TRANSACTION;

BEGIN TRY

    -- Paso 1: Insertar los dos tipos correctos
    DECLARE @id_semillas INT;
    DECLARE @id_biodegradables INT;

    INSERT INTO TIPO_PRODUCTO (nombre, descripcion)
    VALUES ('Semillas', 'Productos con semillas integradas que germinan al ser plantados');
    SET @id_semillas = SCOPE_IDENTITY();

    INSERT INTO TIPO_PRODUCTO (nombre, descripcion)
    VALUES ('Biodegradables', 'Productos fabricados con materiales 100% biodegradables');
    SET @id_biodegradables = SCOPE_IDENTITY();

    PRINT 'Tipos creados — Semillas ID: ' + CAST(@id_semillas AS VARCHAR) +
          ', Biodegradables ID: ' + CAST(@id_biodegradables AS VARCHAR);

    -- Paso 2: Migrar productos germinables → Semillas
    UPDATE PRODUCTO
    SET id_tipo_producto = @id_semillas
    WHERE nombres LIKE '%Germinable%';

    PRINT 'Productos migrados a Semillas: ' + CAST(@@ROWCOUNT AS VARCHAR);

    -- Paso 3: Migrar el resto → Biodegradables
    -- (los que aún tienen alguno de los 11 tipos viejos)
    UPDATE PRODUCTO
    SET id_tipo_producto = @id_biodegradables
    WHERE id_tipo_producto IN (1,2,3,4,5,6,7,8,9,10,11);

    PRINT 'Productos migrados a Biodegradables: ' + CAST(@@ROWCOUNT AS VARCHAR);

    -- Paso 4: Verificar que ya no queda ningún producto con tipos viejos
    DECLARE @huerfanos INT;
    SELECT @huerfanos = COUNT(*)
    FROM PRODUCTO
    WHERE id_tipo_producto IN (1,2,3,4,5,6,7,8,9,10,11);

    IF @huerfanos > 0
    BEGIN
        RAISERROR('Aún hay productos con tipos viejos. Abortando.', 16, 1);
    END

    -- Paso 5: Eliminar los 11 tipos incorrectos
    DELETE FROM TIPO_PRODUCTO
    WHERE id_tipo_producto IN (1,2,3,4,5,6,7,8,9,10,11);

    PRINT 'Tipos viejos eliminados: ' + CAST(@@ROWCOUNT AS VARCHAR);

    -- Paso 6: Resultado final
    SELECT id_tipo_producto, nombre, descripcion FROM TIPO_PRODUCTO ORDER BY id_tipo_producto;
    SELECT id_tipo_producto, COUNT(*) AS total_productos
    FROM PRODUCTO GROUP BY id_tipo_producto;

    COMMIT TRANSACTION;
    PRINT '✓ Migración completada con éxito.';

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'ERROR: ' + ERROR_MESSAGE();
    THROW;
END CATCH;
