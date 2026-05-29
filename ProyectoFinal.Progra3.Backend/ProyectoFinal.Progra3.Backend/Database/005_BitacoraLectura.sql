CREATE TABLE EstadosLectura (
    IdEstadoLectura INT PRIMARY KEY IDENTITY(1,1),
    NombreEstado NVARCHAR(50) NOT NULL
);

INSERT INTO EstadosLectura (NombreEstado) VALUES ('Leyendo'), ('Quiero leer'), ('Abandonado'), ('Próximo a leer');

CREATE TABLE ColaLectura (
    IdColaLectura INT PRIMARY KEY IDENTITY(1,1),
    IdUsuario INT FOREIGN KEY REFERENCES Usuarios(IdUsuario),
    ISBN VARCHAR(20) FOREIGN KEY REFERENCES Libros(ISBN),
    IdEstadoLectura INT FOREIGN KEY REFERENCES EstadosLectura(IdEstadoLectura),
    FechaInicio DATETIME NULL,
    FechaFin DATETIME NULL,
    MeGusto BIT NULL,
    CONSTRAINT UQ_Usuario_ISBN UNIQUE(IdUsuario, ISBN)
);
