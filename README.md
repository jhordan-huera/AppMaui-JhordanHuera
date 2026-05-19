# 🏨 Hotel Reservas — App .NET MAUI

Aplicación móvil desarrollada con **.NET MAUI** y **SQLite** para la gestión de reservas hoteleras con arquitectura **Master-Detail** y patrón **MVVM**.

> 📱 Universidad Técnica Nacional | 7° Semestre | Aplicaciones Móviles 2026

---

## ✨ ¿Qué hace?

- **Dashboard** con estadísticas en tiempo real del total de reservas por estado
- **CRUD de Reservas** — crear, editar, eliminar reservas con código auto-generado, datos del huésped, fechas de check-in/check-out y estado (Pendiente, Confirmada, En Curso, Completada, Cancelada)
- **CRUD de Habitaciones** (detalle por reserva) — asignar múltiples habitaciones con tipo (Individual, Doble, Suite, Familiar, Deluxe), precio por noche y cálculo automático de subtotales
- **Recálculo automático** del total de la reserva al modificar habitaciones
- **Eliminación en cascada** — al borrar una reserva se eliminan sus habitaciones asociadas

---

## 📸 Capturas de Pantalla

<!-- Reemplaza las rutas con tus imágenes -->

### Dashboard
![Dashboard](ruta/a/dashboard.png)

### Lista de Reservas
![Reservas](ruta/a/reservas.png)

### Detalle de Reserva
![Detalle](ruta/a/detalle.png)

### Formulario de Reserva
![Formulario Reserva](ruta/a/form-reserva.png)

### Formulario de Habitación
![Formulario Habitación](ruta/a/form-habitacion.png)

---

## 🛠️ Tecnologías

- .NET MAUI 10
- SQLite (sqlite-net-pcl)
- CommunityToolkit.Mvvm

## 👨‍💻 Autor

**Jhordán** — Universidad Técnica Nacional, 2026
