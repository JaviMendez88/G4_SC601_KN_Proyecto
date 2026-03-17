CREATE DATABASE SC604Proyecto_DB;

USE SC604Proyecto_DB;

-- TABLA ROLES, ERRORES, USUARIOS, PRODUCTO, CATEGORÍA, BODEGAS, RACKS, PASILLOS, Movimientos_inventarios, Lotes, Ubicaciones, stock, auditoria

CREATE TABLE errores (
id_error int not null auto_increment,
tipo varchar(70) not null,
descripcion varchar (225),
primary key (id_error)
);

create table categoria(
id_categoria int not null auto_increment,
titulo varchar (40) unique,
PRIMARY KEY (id_categoria),
index ndx_titulo (titulo)
);

create table producto(
id_producto int not null auto_increment,
id_categoria int not null,
nombre varchar (70) not null,
descripcion varchar(100) not null,
precio decimal(12,2) CHECK (precio >= 0),
PRIMARY KEY (id_producto),
foreign key  (id_categoria) references categoria(id_categoria)
);

create table ubicacion(
id_ubicacion int not null auto_increment, 
provincia varchar (20) not null, 
canton varchar (70) not null, 
distrito varchar (70) not null, 
PRIMARY KEY (id_ubicacion)
);

create table bodega(
id_bodega int not null auto_increment,
id_ubicacion int not null, 
nombre varchar (30) not null, 
primary key (id_bodega),
foreign key  (id_ubicacion) references ubicacion (id_ubicacion)

);

CREATE TABLE ruta (
    id_ruta INT AUTO_INCREMENT NOT NULL,
    ruta VARCHAR(255) NOT NULL,
    id_rol INT NULL,
    requiere_rol boolean NOT NULL DEFAULT TRUE,
  fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  fecha_modificacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    check (id_rol IS NOT NULL OR requiere_rol = FALSE),
    PRIMARY KEY (id_ruta),
    FOREIGN KEY (id_rol) REFERENCES rol(id_rol));
    
    
-- RACKS, PASILLOS, Movimientos_inventarios, Lotes, auditoria

create table rack(
id_rack int not null auto_increment, 
id_bodega int not null, 
primary key(id_rack),
foreign key  (id_bodega) references bodega(id_bodega)
);

create table pasillo(
id_pasillo int not null, 
id_ubicacion int not null, 
primary key (id_pasillo),
foreign key (id_ubicacion) references ubicacion (id_ubicacion)
);


CREATE TABLE lote (
  id_lote INT not null AUTO_INCREMENT,
  id_producto INT NOT NULL,
  codigo_lote VARCHAR(50),
  fecha_vencimiento DATE NOT NULL,
  fecha_ingreso DATE NOT NULL,
  primary key (id_lote),
  FOREIGN KEY (id_producto) REFERENCES producto(id_producto),
  CHECK (fecha_vencimiento > fecha_ingreso)
);

CREATE TABLE stock (
  id_producto INT NOT NULL,
  id_lote INT NOT NULL,
  id_ubicacion INT NOT NULL,
  cantidad INT UNSIGNED NOT NULL,
  PRIMARY KEY (id_producto, id_lote, id_ubicacion),
  FOREIGN KEY (id_producto) REFERENCES producto(id_producto),
  FOREIGN KEY (id_lote) REFERENCES lote(id_lote),
  FOREIGN KEY (id_ubicacion) REFERENCES ubicacion(id_ubicacion)
);

create table registro_error(
id_reporte int not null auto_increment, 
id_error int not null, 
primary key(id_reporte),
foreign key (id_error) references errores(id_error)
);

CREATE TABLE movimiento_inventario (
  id_movimiento INT NOT NULL AUTO_INCREMENT,
  id_producto INT NOT NULL,
  id_lote INT,
  id_ubicacion INT NOT NULL,
  tipo ENUM('ENTRADA','SALIDA','AJUSTE') NOT NULL,
  cantidad INT NOT NULL,
  fecha TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  id_usuario INT NOT NULL,
  primary key (id_movimiento),
  FOREIGN KEY (id_producto) REFERENCES producto(id_producto),
  FOREIGN KEY (id_lote) REFERENCES lote(id_lote),
  FOREIGN KEY (id_ubicacion) REFERENCES ubicacion(id_ubicacion),
  FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario)
);

  
  
  CREATE TABLE auditoria (
  id_auditoria INT AUTO_INCREMENT,
  id_usuario INT NOT NULL,
  accion VARCHAR(50),
  tabla_afectada VARCHAR(50),
  fecha TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  primary key (id_auditoria),
  FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario)
);


-- Javier Méndez -- Tablas modificadas

USE SC604Proyecto_DB;

--TABLAS ROL, USUARIO

CREATE TABLE dbo.rol (
    id_rol INT IDENTITY(1,1) NOT NULL,
    rol VARCHAR(20) NOT NULL UNIQUE,
    fecha_creacion DATETIME NOT NULL DEFAULT GETDATE(),
    fecha_modificacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_rol PRIMARY KEY (id_rol)
);

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
    CONSTRAINT FK_usuario_rol FOREIGN KEY (id_rol)
        REFERENCES dbo.rol(id_rol)
);


-- Roles creados.

INSERT INTO dbo.rol (rol)
VALUES ('ADMIN');

INSERT INTO dbo.rol (rol)
VALUES ('USUARIO');





