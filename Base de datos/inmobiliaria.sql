-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 24-08-2024 a las 01:27:15
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `Id_Contrato` int(11) NOT NULL,
  `Id_Inmueble` int(11) DEFAULT NULL,
  `Id_Propietario` int(11) DEFAULT NULL,
  `Id_Inquilino` int(11) DEFAULT NULL,
  `Fecha_Inicio` date DEFAULT NULL,
  `Fecha_Finalizacion` date DEFAULT NULL,
  `Monto` double DEFAULT NULL,
  `Finalizacion_Anticipada` date DEFAULT NULL,
  `Id_Creado_Por` int(50) NOT NULL,
  `Id_Terminado_Por` int(50) NOT NULL,
  `Estado_Contrato` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `Id_Inmueble` int(11) NOT NULL,
  `Id_Propietario` int(11) DEFAULT NULL,
  `Direccion` varchar(255) DEFAULT NULL,
  `Uso` varchar(50) DEFAULT NULL,
  `Ambientes` int(11) DEFAULT NULL,
  `Coordenadas` int(11) DEFAULT NULL,
  `Tamano` double DEFAULT NULL,
  `Id_Tipo_Inmueble` int(11) DEFAULT NULL,
  `Servicios` varchar(255) DEFAULT NULL,
  `Bano` int(11) DEFAULT NULL,
  `Cochera` int(11) DEFAULT NULL,
  `Patio` int(11) DEFAULT NULL,
  `Precio` double DEFAULT NULL,
  `Condicion` varchar(50) DEFAULT NULL,
  `Estado_Inmueble` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `Id_Inquilino` int(11) NOT NULL,
  `Dni` int(11) DEFAULT NULL,
  `Apellido` varchar(255) DEFAULT NULL,
  `Nombre` varchar(255) DEFAULT NULL,
  `Telefono` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Estado_Inquilino` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`Id_Inquilino`, `Dni`, `Apellido`, `Nombre`, `Telefono`, `Email`, `Estado_Inquilino`) VALUES
(1, 34567891, 'Ramirez', 'Claudia', '9876-5432', 'claudia.ramirez@example.com', 1),
(2, 45678912, 'Gómez', 'Roberto', '8765-4321', 'roberto.gomez@example.com', 0),
(3, 56789123, 'Sosa', 'Patricia', '7654-3210', 'patricia.sosa@example.com', 1),
(4, 67891234, 'Molina', 'Javier', '6543-2109', 'javier.molina@example.com', 0),
(5, 78912345, 'Navarro', 'Gabriela', '5432-1098', 'gabriela.navarro@example.com', 1),
(6, 89123456, 'Ríos', 'Federico', '4321-0987', 'federico.rios@example.com', 1),
(7, 91234567, 'Vega', 'Natalia', '3210-9876', 'natalia.vega@example.com', 0),
(8, 12345678, 'Castro', 'Diego', '2109-8765', 'diego.castro@example.com', 1),
(9, 23456789, 'Ortiz', 'Florencia', '1098-7654', 'florencia.ortiz@example.com', 0),
(10, 34567890, 'Paz', 'Ricardo', '0987-6543', 'ricardo.paz@example.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `Id_Pago` int(11) NOT NULL,
  `Id_Contrato` int(11) DEFAULT NULL,
  `Importe` double DEFAULT NULL,
  `Mes` int(11) DEFAULT NULL,
  `Fecha` date DEFAULT NULL,
  `Multa` double DEFAULT NULL,
  `Id_Creado_Por` int(50) NOT NULL,
  `Id_Terminado_Por` int(50) NOT NULL,
  `Estado_Pago` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `Id_Propietario` int(11) NOT NULL,
  `Dni` int(11) DEFAULT NULL,
  `Apellido` varchar(255) DEFAULT NULL,
  `Nombre` varchar(255) DEFAULT NULL,
  `Direccion` varchar(255) DEFAULT NULL,
  `Telefono` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Estado_Propietario` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`Id_Propietario`, `Dni`, `Apellido`, `Nombre`, `Direccion`, `Telefono`, `Email`, `Estado_Propietario`) VALUES
(1, 12345678, 'González', 'María', 'Calle Falsa 123', '1234-5678', 'maria.gonzalez@example.com', 1),
(2, 23456789, 'Pérez', 'Juan', 'Av. Siempre Viva 742', '2345-6789', 'juan.perez@example.com', 0),
(3, 34567890, 'Rodríguez', 'Ana', 'Calle 9 de Julio 456', '3456-7890', 'ana.rodriguez@example.com', 1),
(4, 45678901, 'López', 'Carlos', 'Boulevard San Martín 789', '4567-8901', 'carlos.lopez@example.com', 0),
(5, 56789012, 'Fernández', 'Laura', 'Pasaje Rivadavia 321', '5678-9012', 'laura.fernandez@example.com', 1),
(6, 67890123, 'García', 'Pedro', 'Avenida Belgrano 654', '6789-0123', 'pedro.garcia@example.com', 1),
(7, 78901234, 'Martínez', 'Lucía', 'Calle Mitre 987', '7890-1234', 'lucia.martinez@example.com', 0),
(8, 89012345, 'Sánchez', 'Jorge', 'Ruta 8 km 45', '8901-2345', 'jorge.sanchez@example.com', 1),
(9, 90123456, 'Ramírez', 'Sofía', 'Calle Alberdi 112', '9012-3456', 'sofia.ramirez@example.com', 0),
(10, 1234567, 'Torres', 'Fernando', 'Avenida Libertador 200', '0123-4567', 'fernando.torres@example.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo_inmueble`
--

CREATE TABLE `tipo_inmueble` (
  `Id_Tipo_Inmueble` int(11) NOT NULL,
  `Tipo` varchar(50) DEFAULT NULL,
  `Estado_Tipo_Inmueble` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `Id_Usuario` int(11) NOT NULL,
  `Apellido` varchar(255) DEFAULT NULL,
  `Nombre` varchar(255) DEFAULT NULL,
  `Dni` int(11) DEFAULT NULL,
  `Telefono` varchar(255) DEFAULT NULL,
  `Rol` varchar(50) DEFAULT NULL,
  `Avatar` int(11) DEFAULT NULL,
  `Estado_Usuario` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`Id_Contrato`),
  ADD KEY `Id_Inmueble` (`Id_Inmueble`),
  ADD KEY `Id_Propietario` (`Id_Propietario`),
  ADD KEY `Id_Inquilino` (`Id_Inquilino`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`Id_Inmueble`),
  ADD KEY `Id_Propietario` (`Id_Propietario`),
  ADD KEY `Id_Tipo_Inmueble` (`Id_Tipo_Inmueble`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`Id_Inquilino`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`Id_Pago`),
  ADD KEY `Id_Contrato` (`Id_Contrato`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`Id_Propietario`);

--
-- Indices de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  ADD PRIMARY KEY (`Id_Tipo_Inmueble`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`Id_Usuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `Id_Contrato` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `Id_Inmueble` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `Id_Inquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `Id_Pago` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `Id_Propietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  MODIFY `Id_Tipo_Inmueble` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `Id_Usuario` int(11) NOT NULL AUTO_INCREMENT;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`Id_Inmueble`) REFERENCES `inmueble` (`Id_Inmueble`),
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`Id_Propietario`) REFERENCES `propietario` (`Id_Propietario`),
  ADD CONSTRAINT `contrato_ibfk_3` FOREIGN KEY (`Id_Inquilino`) REFERENCES `inquilino` (`Id_Inquilino`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`Id_Propietario`) REFERENCES `propietario` (`Id_Propietario`),
  ADD CONSTRAINT `inmueble_ibfk_2` FOREIGN KEY (`Id_Tipo_Inmueble`) REFERENCES `tipo_inmueble` (`Id_Tipo_Inmueble`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`Id_Contrato`) REFERENCES `contrato` (`Id_Contrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
