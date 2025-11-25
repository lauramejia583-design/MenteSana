Create database DBMenteSana2
use DBMenteSana2

CREATE TABLE Rol (
id_rol INT PRIMARY KEY IDENTITY,
nombre VARCHAR(30) NOT NULL
)

INSERT INTO Rol (nombre) VALUES
('Estudiante'),
('Psicologo'),
('Admin_Bienestar')

CREATE TABLE Persona (
id_persona int PRIMARY KEY IDENTITY,
nombres VARCHAR(60) NOT NULL,
apellidos VARCHAR(60) NOT NULL,
correo_institucional VARCHAR(120) NOT NULL UNIQUE,
contrasena VARCHAR(255) NOT NULL,
id_rol INT NOT NULL,
FOREIGN KEY (id_rol) REFERENCES Rol(id_rol)
)
insert into Persona(nombres, apellidos, correo_institucional, contrasena, id_rol)
values( 'psicologo', 'prueba', 'p3@gmail.com', '8bb0cf6eb9b17d0f7d22b456f121257dc1254e1f01665370476383ea776df414', 2)

insert into Persona(nombres, apellidos, correo_institucional, contrasena, id_rol)
values('prueba', 'prueba', 'p@gmail.com', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 1)

insert into Persona(nombres, apellidos, correo_institucional, contrasena, id_rol)
values('prueba', 'prueba', 'p2@gmail.com', '8bb0cf6eb9b17d0f7d22b456f121257dc1254e1f01665370476383ea776df414', 1)


-- ESTADO_EMOCIONAL (texto viene del back)

CREATE TABLE Tipo_Estado_Emocional (
	id_tipo_estado int primary key identity,
	nombre_estado varchar(20) not null
)


CREATE TABLE Estado_Emocional (
id_estado INT IDENTITY PRIMARY KEY,
id_persona INT NOT NULL, -- FK a Persona
fecha_hora DATETIME NOT NULL DEFAULT SYSDATETIME(),
nota VARCHAR(max) NULL,
id_tipo_estado INT,
FOREIGN KEY (id_persona) REFERENCES Persona(id_persona)
)

-- CITA (estudiante con psicólogo)
CREATE TABLE Cita (
id_cita INT IDENTITY PRIMARY KEY,
id_estudiante int NOT NULL,
id_psicologo int NOT NULL,
fecha DATE NOT NULL,
hora TIME NOT NULL,
motivo VARCHAR(max),
FOREIGN KEY (id_estudiante) REFERENCES Persona(id_persona),
FOREIGN KEY (id_psicologo) REFERENCES Persona(id_persona)
)

-- RECOMENDACION (generada a partir de un estado)
CREATE TABLE Recomendacion (
	id_recomendacion int primary key identity,
	id_tipo_estado int,
	contenido varchar(max) not null,
	fecha_generada datetime,
	FOREIGN KEY (id_tipo_estado) references Tipo_Estado_Emocional(id_tipo_estado)
	)


-- ALERTA (por estado crítico)
CREATE TABLE Alerta (
id_alerta INT IDENTITY PRIMARY KEY,
id_estado INT NOT NULL,
tipo VARCHAR(20) NOT NULL, -- se envía desde el back (p.ej. 'Critica')
fecha DATETIME NOT NULL DEFAULT SYSDATETIME(),
FOREIGN KEY (id_estado) REFERENCES Estado_Emocional(id_estado)
)

-- REPORTE (hecho por psicólogo; opcionalmente ligado a estudiante)
CREATE TABLE Reporte (
id_reporte INT IDENTITY PRIMARY KEY,
id_psicologo int NOT NULL,
tipo VARCHAR(20) NOT NULL, -- 'General' | 'Individual' (desde back)
fecha_generacion DATETIME NOT NULL DEFAULT SYSDATETIME(),
id_estudiante int NOT NULL,
FOREIGN KEY (id_psicologo) REFERENCES Persona(id_persona),
FOREIGN KEY (id_estudiante) REFERENCES Persona(id_persona)
);
go

-------- Procedimientos de almacenado ---------
Create procedure insertar_Persona (
@id_persona VARCHAR(20),
@nombres varchar(60),
@apellidos varchar(60),
@correo_institucional VARCHAR(120),
@contraseña VARCHAR(15),
@id_rol INT)
as
begin
 Insert into Persona values(@id_persona, @nombres, @apellidos, @correo_institucional, @id_rol)
end
go

Create procedure actualizar_Persona (
@id_persona VARCHAR(20),
@nombres varchar(60),
@apellidos varchar(60),
@correo_institucional VARCHAR(120),
@contraseña VARCHAR(15),
@id_rol INT)
as
begin
	update Persona set nombres = @nombres, apellidos = @apellidos, correo_institucional = @correo_institucional, 
	id_rol = @id_rol where id_persona = @id_persona
end
go

------------
Create procedure eliminar_persona (
@id_persona varchar(20))
as
begin
 Delete Persona where id_persona = @id_persona
end
go

------------
Create procedure consulta_persona (
@id_persona varchar(20))
as
begin
 Select * from Persona where id_persona = @id_persona
end
go

---------
Create procedure listar_persona 
as
begin
 Select * from Persona 
end
select * from Persona
GO

Create procedure validar_usuario_web
@correo_institucional varchar(40),
@contrasena varchar(255)
as 
begin
 if(exists(select * from Persona where correo_institucional=@correo_institucional and contrasena=@contrasena))
   select id_persona from Persona where correo_institucional=@correo_institucional and contrasena=@contrasena
 else
   select '0'
end




------------ SP Emociones ------------

CREATE PROCEDURE insertar_estado_emocional
    @id_persona INT,
    @nombre_estado VARCHAR(30),
    @nota VARCHAR(2000)
AS
BEGIN
    INSERT INTO Estado_Emocional(id_persona, nombre_estado, nota)
    VALUES(@id_persona, @nombre_estado, @nota);
END
GO

------------ SP psicologos y citas ------------

CREATE PROCEDURE Listar_Psicologos
AS
BEGIN
    SELECT id_persona, nombres, apellidos
    FROM Persona
    WHERE id_rol = 2 -- Psicólogo
END
GO

------------- SP AGENDAR CITAS ------------------
CREATE PROCEDURE sp_Insertar_Cita
	@id_psicologo int,
	@id_estudiante int,
	@fecha date,
	@hora time,
	@motivo varchar
AS
BEGIN
	IF EXISTS(SELECT * FROM Cita WHERE id_psicologo = @id_psicologo AND fecha = @fecha AND hora = @hora)
	BEGIN
		RAISERROR('Esta fecha no está disponible', 16, 1)
		return;
	END
	INSERT INTO Cita (id_estudiante, id_psicologo, fecha, hora, motivo)
	VALUES (@id_estudiante, @id_psicologo, @fecha, @hora, @motivo)
END