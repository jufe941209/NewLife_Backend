-- ============================================================
-- FIX: Agregar 'Aprobado' al CHECK constraint de FACTURA_VENTA.estado_pago
-- Ejecutar en: Azure Portal > db_newLife > Query Editor
-- ============================================================

-- PASO 1: Ver la definicion actual del constraint
SELECT name, definition
FROM sys.check_constraints
WHERE name = 'CK_FACTURA_ESTADO'
GO

-- PASO 2: Eliminar el constraint existente
ALTER TABLE FACTURA_VENTA DROP CONSTRAINT CK_FACTURA_ESTADO
GO

-- PASO 3: Recrearlo con 'Aprobado' incluido
ALTER TABLE FACTURA_VENTA ADD CONSTRAINT CK_FACTURA_ESTADO
    CHECK (estado_pago IN ('Pendiente', 'Aprobado', 'Pagado', 'Cancelado'))
GO

-- PASO 4: Verificar
SELECT name, definition
FROM sys.check_constraints
WHERE name = 'CK_FACTURA_ESTADO'
GO
