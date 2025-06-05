#SulzerAirlines

Challenge de API de gestión de vuelos y reservas para Sulzer Airlines

.Características

- Consulta de rutas aéreas entre ciudades, incluyendo rutas más económicas y mejores horarios.
- Reserva de vuelos con control de asientos disponibles
- Cálculo de precios dinámicos según reglas de horario.
- API segura con JWT (ver punto 4)
- Arquitectura en capas: Dominio, Aplicación, Infraestructura y API
- Tests unitarios con xUnit y Moq.

> Estructura del Proyecto..

- **SulzerAirlines.Domain**: Entidades, Value Objects y lógica de dominio.
- **SulzerAirlines.Application**: Servicios de aplicación, interfaces y lógica de negocio orquestada.
- **SulzerAirlines.Infrastructure**: Repositorios y servicios de infraestructura (por ejemplo, almacenamiento en memoria).
- **SulzerAirlines.Api**: Controladores y configuración de la API.
- **SulzerAirlines.Tests**: Pruebas unitarias.

. Requisitos

- .NET 8 SDK
- Visual Studio 2022 o superior

> Ejecución

1. Clona el repositorio:

git clone https://github.com/Acidman1331/SulzerAirlines.git cd SulzerAirlines

2. Restaura los paquetes y compila: dotnet restore dotnet rbuild

3. ejecucion de  API: dotnet run --project SulzerAirlines.Api

4. Acceso a  Swagger : https://localhost:7016/swagger 

https://localhost:7016/login
{username: "test", password: "123"}

Para poder acceder a los endpoints, hay que copiar el valor del token que devuelve /login 
y hacer Autorizar (clic botón Authorize arriba a la derecha), agregando Barear seguido de un espacio y el valor del token que obtuvo en login (sin comillas)

## Pruebas

 Ejemplo de Uso de la API

GET /api/flight/cheapest?from=A&to=B&time=10:00

GET /api/flight/besttime?from=A&to=B

POST /api/booking/book?from=A&to=B&time=10:00&BPrice=120&seats=2
