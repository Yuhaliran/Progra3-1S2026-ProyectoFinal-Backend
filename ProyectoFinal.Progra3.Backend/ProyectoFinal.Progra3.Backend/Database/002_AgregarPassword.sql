-- Script para agregar la columna PasswordHash a la tabla Usuarios
ALTER TABLE Usuarios ADD PasswordHash VARCHAR(MAX) NOT NULL DEFAULT '';
