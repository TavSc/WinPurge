# WinPurge

Simple pero eficiente programa para desinstalar software de una computadora, así como los sobrantes que puedan quedar.

## Requerimientos:
- .NET 6.0 Desktop Runtime (v6.0.5) - Windows x64 - [Descargar](https://download.visualstudio.microsoft.com/download/pr/5681bdf9-0a48-45ac-b7bf-21b7b61657aa/bbdc43bc7bf0d15b97c1a98ae2e82ec0/windowsdesktop-runtime-6.0.5-win-x64.exe).

- **Mínimo** Windows 10 [Creators Update](https://blogs.windows.com/latam/2017/04/11/como-obtener-windows-10-creators-update/).
- **Recomendado** [Windows 11](https://www.microsoft.com/es-es/software-download/windows11)

## Bibliotecas utilizadas:
```csharp
using System.Collections;
```

Biblioteca que contiene clases e interfaces que ayudan a definir varios objetos, como lo son listas, queues, arrays, diccionarios y tablas hash.

[Documentación de Microsoft (.NET 6)](https://docs.microsoft.com/en-us/dotnet/api/system.collections?view=net-6.0)

```csharp
using Microsoft.Win32;
```

Biblioteca que ayuda a manejar eventos generados por el sistema operativo, así como la manipulación y obtención de datos del registro de Windows.

[Documentación de Microsoft (.NET 6)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32?view=net-6.0)

```csharp
using System.Diagnostics;
```
Biblioteca que ayuda a la interacción con procesos, contadores de rendimiento y logs.

[Documentación de Microsoft (.NET 6)](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics?view=net-6.0)
```csharp
using Microsoft.Toolkit.Uwp.Notifications;
```
Biblioteca que se encarga de la interacción y creación de notificaciones nativas para el sistema.

[Documentación de Microsoft (.NET 6)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.uwp.notifications?view=win-comm-toolkit-dotnet-7.0)
```csharp
using System.ComponentModel;
```
Biblioteca dedicada a la implementación del comportamiento en runtime y tiempo de diseño de componentes y controles de WinForms.

[Documentación de Microsoft (.NET 6)](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel?view=net-6.0)
```csharp
using System.Security.Principal;
```
Biblioteca utilizada para saber el contexto de seguridad de una aplicación.

[Documentación de Microsoft (.NET 6)](https://docs.microsoft.com/en-us/dotnet/api/system.security.principal?view=net-6.0)
```csharp
using System.Reflection;
```
Bblioteca que examina los metadatos del proyetco para obtener información sobre módulos, assemblies, parámetros, etc.

[Documentación de Microsoft (.NET 6)](https://docs.microsoft.com/en-us/dotnet/api/system.reflection?view=net-6.0)

## Programas
```
MainForm.cs
```
Este es el archivo fuente de la ventana principal del programa. En el archivo se manejan todos los botones de la pantalla principal, así como el DataGridView, el status de los desinstaladores, y el manejo de la selección de los desisntaladores que se van a llamar.
```
LeftoversForm.cs
```
```
StartForm.cs
```
```
UninstallForm.cs
```
