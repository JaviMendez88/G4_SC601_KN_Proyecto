-- Javier Méndez -- Tablas modificadas




IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ProyectoDB')
BEGIN
    CREATE DATABASE ProyectoDB;
END
GO

USE ProyectoDB;
GO


CREATE TABLE dbo.rol (
    id_rol INT IDENTITY(1,1) NOT NULL,
    rol VARCHAR(20) NOT NULL UNIQUE,
    fecha_creacion DATETIME NOT NULL DEFAULT GETDATE(),
    fecha_modificacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_rol PRIMARY KEY (id_rol)
);
GO

CREATE TABLE dbo.usuario (
    id_usuario INT IDENTITY(1,1) NOT NULL,
    nombre VARCHAR(40) NOT NULL,
    apellido_1 VARCHAR(30) NOT NULL,
    usuario VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    contrasena VARCHAR(225) NOT NULL,
    id_rol INT NOT NULL,
    intentos_fallidos INT NOT NULL DEFAULT 0,
    bloqueado BIT NOT NULL DEFAULT 0,
    CONSTRAINT PK_usuario PRIMARY KEY (id_usuario),
    CONSTRAINT FK_usuario_rol 
        FOREIGN KEY (id_rol)
        REFERENCES dbo.rol(id_rol)
);
GO

INSERT INTO dbo.rol (rol)
VALUES 
    ('ADMIN'),
    ('USUARIO');
GO

INSERT INTO dbo.usuario (
    nombre,
    apellido_1,
    usuario,
    email,
    contrasena,
    id_rol,
    intentos_fallidos,
    bloqueado
)
VALUES
(
    'Javier',
    'Mendez',
    'jmendez80868',
    'jmendez80867@ufide.ac.cr',
    'WvzLPrey',
    1,  -- ADMIN
    0,
    0
),
(
    'Otilio',
    'Jaen',
    'Ottis',
    'otilio0223@ufide.ac.cr',
    'ABC000',
    2,  -- USUARIO
    0,
    0
);
GO



CREATE TABLE errores (
    id_error INT IDENTITY(1,1) PRIMARY KEY,
    tipo VARCHAR(70) NOT NULL,
    descripcion VARCHAR(225)
);


CREATE TABLE categoria (
    id_categoria INT IDENTITY(1,1) PRIMARY KEY,
    titulo VARCHAR(40) UNIQUE
);

CREATE INDEX ndx_titulo ON categoria(titulo);


CREATE TABLE producto (
    id_producto INT IDENTITY(1,1) PRIMARY KEY,
    id_categoria INT NOT NULL,
    nombre VARCHAR(70) NOT NULL,
    descripcion VARCHAR(100) NOT NULL,
    precio DECIMAL(12,2) NOT NULL,
    CONSTRAINT chk_precio CHECK (precio >= 0),
    CONSTRAINT fk_producto_categoria 
        FOREIGN KEY (id_categoria) REFERENCES categoria(id_categoria)
);


CREATE TABLE ubicacion (
    id_ubicacion INT IDENTITY(1,1) PRIMARY KEY,
    provincia VARCHAR(20) NOT NULL,
    canton VARCHAR(70) NOT NULL,
    distrito VARCHAR(70) NOT NULL
);



CREATE TABLE bodega (
    id_bodega INT IDENTITY(1,1) PRIMARY KEY,
    id_ubicacion INT NOT NULL,
    nombre VARCHAR(30) NOT NULL,
    CONSTRAINT fk_bodega_ubicacion 
        FOREIGN KEY (id_ubicacion) REFERENCES ubicacion(id_ubicacion)
);


CREATE TABLE ruta (
    id_ruta INT IDENTITY(1,1) PRIMARY KEY,
    ruta VARCHAR(255) NOT NULL,
    id_rol INT NULL,
    requiere_rol BIT NOT NULL DEFAULT 1,
    fecha_creacion DATETIME2 DEFAULT SYSDATETIME(),
    fecha_modificacion DATETIME2 DEFAULT SYSDATETIME(),
    CONSTRAINT chk_ruta_rol CHECK (
        id_rol IS NOT NULL OR requiere_rol = 0
    ),
    CONSTRAINT fk_ruta_rol 
        FOREIGN KEY (id_rol) REFERENCES rol(id_rol)
);



CREATE TABLE rack (
    id_rack INT IDENTITY(1,1) PRIMARY KEY,
    id_bodega INT NOT NULL,
    CONSTRAINT fk_rack_bodega 
        FOREIGN KEY (id_bodega) REFERENCES bodega(id_bodega)
);


CREATE TABLE pasillo (
    id_pasillo INT PRIMARY KEY,
    id_ubicacion INT NOT NULL,
    CONSTRAINT fk_pasillo_ubicacion 
        FOREIGN KEY (id_ubicacion) REFERENCES ubicacion(id_ubicacion)
);



CREATE TABLE lote (
    id_lote INT IDENTITY(1,1) PRIMARY KEY,
    id_producto INT NOT NULL,
    codigo_lote VARCHAR(50),
    fecha_vencimiento DATE NOT NULL,
    fecha_ingreso DATE NOT NULL,
    CONSTRAINT chk_fechas_lote 
        CHECK (fecha_vencimiento > fecha_ingreso),
    CONSTRAINT fk_lote_producto 
        FOREIGN KEY (id_producto) REFERENCES producto(id_producto)
);



CREATE TABLE stock (
    id_producto INT NOT NULL,
    id_lote INT NOT NULL,
    id_ubicacion INT NOT NULL,
    cantidad INT NOT NULL,
    CONSTRAINT chk_cantidad CHECK (cantidad >= 0),
    PRIMARY KEY (id_producto, id_lote, id_ubicacion),
    CONSTRAINT fk_stock_producto FOREIGN KEY (id_producto) REFERENCES producto(id_producto),
    CONSTRAINT fk_stock_lote FOREIGN KEY (id_lote) REFERENCES lote(id_lote),
    CONSTRAINT fk_stock_ubicacion FOREIGN KEY (id_ubicacion) REFERENCES ubicacion(id_ubicacion)
);



CREATE TABLE registro_error (
    id_reporte INT IDENTITY(1,1) PRIMARY KEY,
    id_error INT NOT NULL,
    CONSTRAINT fk_registro_error 
        FOREIGN KEY (id_error) REFERENCES errores(id_error)
);



CREATE TABLE movimiento_inventario (
    id_movimiento INT IDENTITY(1,1) PRIMARY KEY,
    id_producto INT NOT NULL,
    id_lote INT NULL,
    id_ubicacion INT NOT NULL,
    tipo VARCHAR(10) NOT NULL,
    cantidad INT NOT NULL,
    fecha DATETIME2 DEFAULT SYSDATETIME(),
    id_usuario INT NOT NULL,
    CONSTRAINT chk_tipo_movimiento 
        CHECK (tipo IN ('ENTRADA','SALIDA','AJUSTE')),
    CONSTRAINT fk_mov_producto FOREIGN KEY (id_producto) REFERENCES producto(id_producto),
    CONSTRAINT fk_mov_lote FOREIGN KEY (id_lote) REFERENCES lote(id_lote),
    CONSTRAINT fk_mov_ubicacion FOREIGN KEY (id_ubicacion) REFERENCES ubicacion(id_ubicacion),
    CONSTRAINT fk_mov_usuario FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario)
);



CREATE TABLE auditoria (
    id_auditoria INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    accion VARCHAR(50),
    tabla_afectada VARCHAR(50),
    fecha DATETIME2 DEFAULT SYSDATETIME(),
    CONSTRAINT fk_auditoria_usuario 
        FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario)
);



SELECT * FROM dbo.rol;
SELECT * FROM dbo.usuario;

