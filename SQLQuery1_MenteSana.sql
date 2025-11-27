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
values( 'Carlos', 'Alvarez', 'psicologo@gmail.com', '8bb0cf6eb9b17d0f7d22b456f121257dc1254e1f01665370476383ea776df414', 2)

insert into Persona(nombres, apellidos, correo_institucional, contrasena, id_rol)
values('Pepito', 'Perez', 'pepito@gmail.com', '8bb0cf6eb9b17d0f7d22b456f121257dc1254e1f01665370476383ea776df414', 1)

insert into Persona(nombres, apellidos, correo_institucional, contrasena, id_rol)
values('bienestar', 'prueba', 'p4@gmail.com', '8bb0cf6eb9b17d0f7d22b456f121257dc1254e1f01665370476383ea776df414', 3)


-- ESTADO_EMOCIONAL (texto viene del back)

CREATE TABLE Tipo_Estado_Emocional  (
    id_tipo_estado int primary key identity,
    nombre_estado varchar(20) not null
)

 insert into Tipo_Estado_Emocional (nombre_estado)
 values  
 ('Muy-Feliz'),
 ('Feliz'),
('Tranquilo'),
('Ansioso'),
('Triste');


CREATE TABLE Estado_Emocional (
id_estado INT IDENTITY PRIMARY KEY,
id_persona INT NOT NULL, -- FK a Persona
fecha_hora DATETIME NOT NULL DEFAULT SYSDATETIME(),
nota VARCHAR(max) NULL,
id_tipo_estado INT,
FOREIGN KEY (id_persona) REFERENCES Persona(id_persona),
FOREIGN KEY (id_tipo_estado) REFERENCES Tipo_Estado_Emocional(id_tipo_estado)
)

-- CITA (estudiante con psicólogo)alter

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

insert into Recomendacion(id_tipo_estado, contenido, fecha_generada)
values 
(1,'Aprovecha este momento de energía. Haz algo que te guste mucho o comparte tu alegría con alguien cercano. 
También puedes escribir lo que te hizo sentir así para recordarlo en días difíciles.', GETDATE()),

(2,'Disfruta el momento. Respira profundo y agradécete por lo que has logrado hoy. 
Mantener pequeños hábitos que te hacen bien ayuda a que esta emoción dure más tiempo', GETDATE()),

(3,'Mantén actividades que refuercen esta calma: música suave, una caminata corta o estiramientos. 
Estar en paz también es progreso, permítete sentirlo sin prisa.', GETDATE()),

(4,'Haz una pausa. Respira profundo 4 segundos, exhala 6. Identifica una sola cosa que sí puedes controlar ahora. 
Hablar con alguien de confianza o escribir lo que sientes puede ayudar a reducir la tensión.

(Recuerda: sin instrucciones de autolesión, todo seguro según las reglas para menores.)', GETDATE()),

(5,'No ignores lo que sientes. Reconócelo con compasión. Busca una actividad suave: música, dibujar, 
salir un momento o conversar con alguien. 
Pedir apoyo está bien; no tienes que cargar con todo sola.', GETDATE())


-- ALERTA (por estado crítico)
CREATE TABLE Alerta (
id_alerta INT IDENTITY PRIMARY KEY,
id_estado INT NOT NULL,
fecha DATETIME NOT NULL DEFAULT SYSDATETIME(),
FOREIGN KEY (id_estado) REFERENCES Estado_Emocional(id_estado)
)
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

create procedure validar_usuario_web
@correo_institucional varchar(40),
@contrasena varchar(255)
as 
begin
 if(exists(select * from Persona where correo_institucional=@correo_institucional and contrasena=@contrasena))
   select id_persona, id_rol from Persona where correo_institucional=@correo_institucional and contrasena=@contrasena 
 else
   select '0'
end
go

------------ SP Emociones ------------
CREATE PROCEDURE sp_insertar_estado_emocional
    @id_persona INT,
    @nota VARCHAR(max) = null,
    @id_tipo_estado INT
AS
BEGIN
    INSERT INTO Estado_Emocional(id_persona, fecha_hora, id_tipo_estado, nota)
    VALUES(@id_persona, getdate(), @id_tipo_estado, @nota);
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
create PROCEDURE sp_Insertar_Cita
    @id_psicologo int,
    @id_estudiante int,
    @fecha date,
    @hora time,
    @motivo varchar(max)
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
go


------------ sp recomendacion ---------------------
ALTER PROCEDURE sp_obtener_recomendaciones_por_estado
AS
BEGIN
    SELECT R.contenido, ti.nombre_estado
    FROM Recomendacion R
	INNER JOIN Estado_Emocional Es
	ON R.id_tipo_estado = Es.id_tipo_estado
	INNER JOIN Tipo_Estado_Emocional Ti
	ON Es.id_tipo_estado = Ti.id_tipo_estado
    ORDER BY Es.fecha_hora asc;
END
GO

SELECT * FROM Estado_Emocional

ALTER PROCEDURE sp_obtener_recomendaciones_por_estado
AS
BEGIN
    -- Obtenemos el último estado emocional registrado
    DECLARE @ultimoEstado INT;

    SELECT TOP 1 @ultimoEstado = id_estado
    FROM Estado_Emocional
    ORDER BY fecha_hora DESC;

    -- Traemos solo las recomendaciones del último estado
    SELECT R.contenido, Ti.nombre_estado
    FROM Recomendacion R
    INNER JOIN Estado_Emocional Es
	ON R.id_tipo_estado = 
    WHERE R.id_tipo_estado = @ultimoEstado;
END

select top 1 id_estado from Estado_Emocional
ORDER BY fecha_hora DESC;

ALTER PROCEDURE sp_obtener_recomendaciones_por_estado
    @id_persona INT
AS
BEGIN
    DECLARE @ultimoEstado INT;

    SELECT TOP 1 @ultimoEstado = id_tipo_estado
    FROM Estado_Emocional
    WHERE id_persona = @id_persona
    ORDER BY fecha_hora DESC;

    SELECT R.contenido, TE.nombre_estado
    FROM Recomendacion R
    INNER JOIN Tipo_Estado_Emocional TE
        ON R.id_tipo_estado = TE.id_tipo_estado
    WHERE R.id_tipo_estado = @ultimoEstado;
END
exec sp_obtener_recomendaciones_por_estado
@id_persona = 2
select * from Estado_Emocional


select * from Cita

go
------------SP Listar Citas ----------------------
CREATE PROCEDURE sp_listar_cita_psicologo
@id_psicologo int
as
begin
SELECT
    est.nombres   AS nombre_estudiante,
    est.apellidos AS apellido_estudiante,
    psi.nombres   AS nombre_psicologo,
    psi.apellidos AS apellido_psicologo,
    c.fecha,
    c.hora
FROM Cita c
INNER JOIN Persona est
    ON c.id_estudiante = est.id_persona
INNER JOIN Persona psi
    ON c.id_psicologo = psi.id_persona
where c.id_psicologo = @id_psicologo;
END
go
ALTER PROCEDURE sp_listar_cita_psicologo
@id_psicologo int
AS
BEGIN
    SELECT
        c.id_cita,
        est.nombres   AS nombre_estudiante,
        est.apellidos AS apellido_estudiante,
        psi.nombres   AS nombre_psicologo,
        psi.apellidos AS apellido_psicologo,
        c.fecha,
        c.hora
    FROM Cita c
    INNER JOIN Persona est
        ON c.id_estudiante = est.id_persona
    INNER JOIN Persona psi
        ON c.id_psicologo = psi.id_persona
    WHERE c.id_psicologo = @id_psicologo;
END
GO

CREATE PROCEDURE TraerCitasBienestar
as
begin
SELECT
    est.nombres   AS nombre_estudiante,
    est.apellidos AS apellido_estudiante,
    psi.nombres   AS nombre_psicologo,
    psi.apellidos AS apellido_psicologo,
    c.fecha,
    c.hora
FROM Cita c
INNER JOIN Persona est
    ON c.id_estudiante = est.id_persona
INNER JOIN Persona psi
    ON c.id_psicologo = psi.id_persona;
END

select * from Estado_Emocional

CREATE PROCEDURE sp_detalle_cita
    @id_cita INT
AS
BEGIN
    SELECT 
        c.id_cita,
        c.fecha,
        c.hora,
        c.motivo,
        est.nombres AS nombre_estudiante,
        est.apellidos AS apellido_estudiante,
        psi.nombres AS nombre_psicologo,
        psi.apellidos AS apellido_psicologo
    FROM Cita c
    INNER JOIN Persona est ON c.id_estudiante = est.id_persona
    INNER JOIN Persona psi ON c.id_psicologo = psi.id_persona
    WHERE c.id_cita = @id_cita;
END
GO
