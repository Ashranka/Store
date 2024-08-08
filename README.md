# Store Application

Esta es una aplicación de tienda en línea que permite a los usuarios crear productos, añadir productos al carrito de compras y crear órdenes. El backend está desarrollado en .NET 8 y el frontend en React utilizando Vite.

## Características

- **Productos**: Crear, actualizar y listar productos.
- **Carrito de Compras**: Añadir productos al carrito, actualizar la cantidad de productos en el carrito y eliminar productos del carrito.
- **Órdenes**: Crear y gestionar órdenes de compra.

## Tecnologías Utilizadas

### Backend
- .NET 8
- Entity Framework Core
- AutoMapper
- ASP.NET Core Identity
- JWT para autenticación

### Frontend
- React
- Vite
- Axios para las peticiones HTTP

## Estructura del Proyecto

### Backend

El backend está ubicado en la carpeta `Store` y contiene los siguientes componentes principales:

- **Controllers**: Controladores de API para manejar las solicitudes HTTP.
- **Data**: Contexto de base de datos y configuraciones de Entity Framework.
- **Entities**: Entidades de base de datos.
- **DTOs**: Objetos de transferencia de datos.
- **Services**: Servicios de negocio y lógica de aplicación.
- **Mappings**: Configuraciones de AutoMapper.

### Frontend

El frontend está ubicado en la carpeta `Client` y contiene los siguientes componentes principales:

- **src/components**: Componentes de React.
- **src/pages**: Páginas de la aplicación.
- **src/services**: Servicios para interactuar con el backend.
- **src/store**: Estado global de la aplicación (opcional, si se usa Redux u otro manejador de estado).
- **src/styles**: Estilos CSS/SCSS de la aplicación.

## Configuración del Proyecto

### Backend

1. Clona el repositorio:
   ```bash
   git clone https://github.com/Ashranka/Store.git
   cd Store/Server
