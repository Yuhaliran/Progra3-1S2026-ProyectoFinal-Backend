
CREATE TABLE Roles (
    IdRol          INT PRIMARY KEY IDENTITY(1,1),
    NombreRol      NVARCHAR(50) NOT NULL,
    NivelJerarquia INT NOT NULL -- 1, 2, 3
);

CREATE TABLE EstadosUsuario (
    IdEstado     INT PRIMARY KEY IDENTITY(1,1),
    NombreEstado NVARCHAR(50) NOT NULL
);

CREATE TABLE Usuarios (
    IdUsuario               INT PRIMARY KEY IDENTITY(1,1),
    IdRol                   INT FOREIGN KEY REFERENCES Roles(IdRol),
    IdEstado                INT FOREIGN KEY REFERENCES EstadosUsuario(IdEstado),
    Nombres                 NVARCHAR(100) NOT NULL,
    Apellidos               NVARCHAR(100) NOT NULL,
    DPI                     VARCHAR(15) NULL,
    IdentificadorBiblioteca AS (UPPER(LEFT(Nombres,1) + LEFT(Apellidos,1)) + CAST(IdUsuario AS VARCHAR(10))) PERSISTED,
    Email                   VARCHAR(100) UNIQUE NOT NULL,
    Telefono                VARCHAR(15)
);

CREATE TABLE Libros (
    ISBN            VARCHAR(20) PRIMARY KEY,
    Titulo          NVARCHAR(200) NOT NULL,
    Autor           NVARCHAR(200) NOT NULL,
    AnioPublicacion INT
);

CREATE TABLE EstadosFisicos (
    IdEstadoFisico INT PRIMARY KEY IDENTITY(1,1),
    NombreEstado   NVARCHAR(50) NOT NULL
);

CREATE TABLE Ejemplares (
    CodigoBarras   VARCHAR(50) PRIMARY KEY,
    ISBN           VARCHAR(20) FOREIGN KEY REFERENCES Libros(ISBN),
    IdEstadoFisico INT FOREIGN KEY REFERENCES EstadosFisicos(IdEstadoFisico),
    Disponible     BIT DEFAULT 1
);

CREATE TABLE Logs (
    IdLog       INT PRIMARY KEY IDENTITY(1,1),
    Fecha       DATETIME DEFAULT GETDATE(),
    IdUsuario   INT FOREIGN KEY REFERENCES Usuarios(IdUsuario),
    Accion      NVARCHAR(100),
    Descripcion NVARCHAR(MAX)
);

INSERT INTO Roles (NombreRol, NivelJerarquia) VALUES ('Bibliotecario R1', 1), ('Admin R2', 2), ('SuperAdmin R3', 3);
INSERT INTO EstadosUsuario (NombreEstado) VALUES ('Activo'), ('Bloqueado'), ('Moroso');
INSERT INTO EstadosFisicos (NombreEstado) VALUES ('Excelente'), ('Regular'), ('Dañado');