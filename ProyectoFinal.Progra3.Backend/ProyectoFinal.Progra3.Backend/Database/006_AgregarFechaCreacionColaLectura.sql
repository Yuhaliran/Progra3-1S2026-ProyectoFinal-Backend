IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.ColaLectura') 
      AND name = 'FechaCreacion'
)
BEGIN
    ALTER TABLE dbo.ColaLectura 
    ADD FechaCreacion DATETIME NOT NULL DEFAULT GETDATE();
END
