# Proyecto Final API Bancario

Una API REST en .NET 8.0 para gestionar clientes y tarjetas, con capas de servicios y un proyecto auxiliar de utilidades

## Estructura del proyecto
```
ProyectoFinal/
├─ ProyectiFinal/
│  ├─ .gitignore
│  ├─ ProyectiFinal.sln
│  ├─ ProyectiFinal.csproj
│  ├─ ProyectiFinal.csproj.user
│  ├─ ProyectiFinal.http
│  ├─ Program.cs
│  ├─ appsettings.json
│  ├─ appsettings.Development.json
│  ├─ Controllers/
│  │   ├─ CardsController.cs
│  │   └─ ClientController.cs
│  ├─ JSONData/
│  │   └─ Clientes.json
│  ├─ Properties/
│  │   └─ launchSettings.json
│  └─ Services/
│       ├─ CardService.cs
│       └─ ClientService.cs
└─ Utils/
   ├─ Utils.csproj
   ├─ Class1.cs
   ├─ dtos/
   │   ├─ CardsNumber.cs
   │   ├─ ClientsDPI.cs
   │   └─ ValidationResponse.cs
   ├─ enums/
   │   ├─ Genero.cs
   │   ├─ TipoSolicitud.cs
   │   └─ TipoTarjeta.cs
   ├─ EstructuraDeDatos/
   │   ├─ Arboles/
   │   ├─ DispercionesHash/
   │   ├─ Interfaces/
   │   └─ ListasEnlazadas/
   ├─ models/
   │   ├─ Cliente.cs
   │   ├─ Clientes.cs
   │   ├─ Response.cs
   │   ├─ Solicitud.cs
   │   ├─ Tarjeta.cs
   │   └─ Transaccion.cs
   └─ validations/
       └─ Validations.cs
```


### Requisitos:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/es-es/download/dotnet/8.0)
- [Git](https://git-scm.com/downloads)
- [Visual Studio](https://visualstudio.microsoft.com/es/downloads/)


## Instalación y ejecución

- Clonar el repositorio:

```sh
$ git clone https://github.com/kgarcia2020050/Estucturas-de-Datos-Core-Bancario.git
$ cd ProyectoFinal/ProyectiFinal
```

### Usando CLI

- Restaurar paquetes y construir:

```sh
$ dotnet restore
$ dotnet build
```

- Ejecutar el API:

```sh
$ dotnet run
```
Por defecto, se levantará en http://localhost:5089

- Accede a la documentación Swagger:

```
https://localhost:5089/swagger 
https://localhost:7227/swagger
```

### Usando Visual studio

- Abre ProyectiFinal.sln con Visual Studio 2022 (o superior).
- En el Explorador de soluciones, haz clic derecho sobre el proyecto ProyectiFinal y selecciona Establecer como proyecto de inicio.
- Asegúrate de que la configuración de ejecución esté en Debug y selecciona IIS Express o Project en la barra de herramientas de depuración.
- Presiona F5 o haz clic en el botón Iniciar para compilar y ejecutar la API.
- Visual Studio abrirá automáticamente el navegador en la URL configurada (https://localhost:5089/swagger o https://localhost:7227/swagger), donde podrás ver y probar los endpoints.

## Endpoints Disponibles

### ClientController (/api/Client)
| Metodo | Ruta | Descripción |
|----------------|------------ |---------------------|
| GET | /getAllClients | Obtener la información de todos los clientes del sistema| 
| GET | /getActiveClients | Obtener la información de todos los clientes activos del sistema |
| GET | /findByDpi?dpi= | Encontrar cliente por medio de DPI |
| POST | /create | Crear cliente |
| POST | /initialData | Carga masiva de clientes y tarjetas |
| PUT | /update | Actualizar cliente |
| PUT | /updateStatus | Activar o inactivar cliente |
| DELETE | /delete?dpi= | Eliminar cliente |

Ejemplo (curl)

```
curl -X POST https://localhost:7227/api/Client/create \
-H "Content-Type: application/json" \
-d '{ "dpi": "3589978600101", "nombre": "Kenneth", "apellido": "Garcia", "direccion": "Avenida Reforma 429 Edif. 277 , Depto. 335, Guastatoya, GT 63578", "telefono": "12345678", "email": "kgarcia@gmail.com", "genero": "M", "nacimiento": "30/09/2002", "nacionalidad": "Guatemalteco", "nit": 123456789 }'
```

### CardsController (/api/Cards)
| Metodo | Ruta | Descripción |
|----------------|------------ |---------------------|
| POST | /addCard | Agregar tarjeta a un cliente| 
| POST | /makePayments | Realizar transacciones con una tarjeta |
| POST | /retrieveBalances | Obtener saldo de una tarjeta |
| POST | /retrieveTransactions | Obtener transacciones y solicitudes de una tarjeta |
| PUT | /updatePin | Actualizar pin de una tarjeta |
| PUT | /updateStatus | Activar o inactivar una tarjeta |
| PUT | /updateExpirationDate | Renovar tarjeta |
| PUT | /incrementLimit | Aumento de límite de tarjeta de crédito |

Ejemplo (curl)

```
curl -X POST https://localhost:7227/api/Cards/addCard \
-H "Content-Type: application/json" \
-d '{ "dpi": "3589978600101", "numeroTarjeta": "1234567890987665", "fechaExpiracion": "10/20", "cvv": "123", "pin": "1234", "limiteCredito": 0, "saldoActual": 10000, "tipo": "DEBITO" }'
```

## Proyecto Utils

El proyecto Utils ofrece:

- DTOs para peticiones y respuestas.
- Enums: Genero, TipoTarjeta, TipoSolicitud, etc.
- Estructuras de datos: árboles, listas enlazadas, tablas hash.
- Validations: validaciones genéricas (fechas, formatos, Números de tarjeta, DPI, etc.).

Se referencia desde la API principal para abstraer lógica común.
