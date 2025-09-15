# 📚 Sistema de Gestión de Préstamos de Libros - Universidad UPB

## 📌 Caso de Negocio
El proceso de préstamo de libros en la biblioteca de la Universidad UPB presenta actualmente limitaciones importantes debido a la ausencia de un sistema automatizado que gestione de manera integral y confiable todas las operaciones.

Actualmente:
- El registro de préstamos, devoluciones y control de ejemplares se realiza de forma manual, lo que no garantiza precisión ni eficiencia.
- No existe validación automática que confirme si el usuario pertenece a la institución.
- El control de disponibilidad de libros es poco eficiente, pudiendo generar solicitudes sobre ejemplares ya prestados.
- No se asignan **identificadores únicos de préstamo**, dificultando la trazabilidad.
- No se controlan automáticamente las fechas de vencimiento de los préstamos.
- El stock no se actualiza en tiempo real, generando decisiones basadas en información incorrecta.

✅ Se propone implementar un sistema de gestión de préstamos automatizado que:
- Valide la identidad de los estudiantes.
- Consulte y actualice en tiempo real la disponibilidad de ejemplares.
- Genere identificadores únicos de préstamo.
- Asigne automáticamente fechas de vencimiento.
- Controle devoluciones de manera eficiente.

---

## 👥 Usuarios
- **Estudiantes de la UPB**: acceden al sistema con su documento de identidad para solicitar préstamos de libros.  
- **Sistema de Biblioteca**: valida la identidad del estudiante, gestiona el stock de libros, asigna préstamos con ID único y controla fechas de devolución.

---

## 🎯 Valor Esperado
El sistema permitirá automatizar el proceso de préstamo logrando:
- Validación automática de estudiantes mediante documento de identidad.
- Consulta inmediata de disponibilidad de libros.
- Asignación automática de un **ID único** por transacción.
- Descuento automático del stock al momento del préstamo.
- Control de plazos de devolución con un período estándar de **15 días**.

---

## 📖 Historias de Usuario

### HU1: Registro de estudiante en préstamo
- **Como** estudiante de la UPB  
- **Quiero** identificarme con mi documento de identidad  
- **Para** acceder al sistema y realizar un préstamo.  

**Criterios de aceptación:**
- **Given** el estudiante ingresa su documento.  
- **When** el sistema valida que pertenece a la universidad.  
- **Then** permite continuar con el préstamo.  

---

### HU2: Validación de disponibilidad del libro
- **Como** estudiante  
- **Quiero** consultar la disponibilidad del libro  
- **Para** confirmar si puedo prestarlo  

**Criterios de aceptación:**
- **Given** el estudiante ingresa el ID del libro  
- **When** el stock es mayor a 0  
- **Then** el sistema permite continuar.  

---

### HU3: Generación de préstamo
- **Como** estudiante  
- **Quiero** recibir un ID único de préstamo  
- **Para** identificar mi transacción  

**Criterios de aceptación:**
- **Given** que el libro está disponible  
- **When** se confirma el préstamo  
- **Then** el sistema asigna un ID único y lo registra.  

---

### HU4: Control de stock
- **Como** sistema de biblioteca  
- **Quiero** descontar en 1 el stock del libro prestado  
- **Para** mantener actualizado el inventario.  

**Criterios de aceptación:**
- **Given** se genera un préstamo  
- **When** este se registra  
- **Then** el stock disminuye automáticamente.  

---

### HU5: Fecha de vencimiento automática
- **Como** estudiante  
- **Quiero** conocer la fecha límite de devolución  
- **Para** devolver el libro a tiempo  

**Criterios de aceptación:**
- **Given** se registra un préstamo  
- **When** este se crea  
- **Then** el sistema asigna automáticamente una fecha de vencimiento de 15 días.  

---

### HU6: Devolución de libro
- **Como** estudiante  
- **Quiero** devolver el libro dentro del plazo  
- **Para** liberar el préstamo y actualizar el stock  

**Criterios de aceptación:**
- **Given** el estudiante devuelve el libro  
- **When** se registra la devolución  
- **Then** el stock aumenta y el préstamo se marca como cerrado.  

---

## 📝 Requerimientos del Negocio
- Solo estudiantes de la Universidad UPB podrán acceder al sistema.  
- Cada préstamo debe estar asociado a un estudiante válido y un libro existente.  
- La devolución debe realizarse máximo 15 días después del préstamo.  
- El sistema debe garantizar un control de stock confiable.  
- Todo préstamo debe contar con un identificador único.  

---

## ⚙️ Requerimientos Funcionales
1. Validar el documento de identidad del estudiante.  
2. Consultar disponibilidad de libros.  
3. Generar un préstamo con ID único.  
4. Registrar fecha de préstamo y vencimiento.  
5. Descontar stock al realizar préstamo.  
6. Registrar devolución y actualizar stock.  
7. Marcar préstamo como vencido si excede el plazo.  
8. Mostrar información detallada de un préstamo.  

---

## 🛡️ Requerimientos No Funcionales
1. **Rendimiento**: validaciones y consultas en menos de 2 segundos.  
2. **Seguridad**: solo estudiantes registrados acceden.  
3. **Logging**: registrar préstamos y devoluciones en consola.  
4. **Mantenibilidad**: aplicar principios de POO y diseño modular.  
5. **Testabilidad**: soporte para pruebas unitarias.  
6. **UX consola**: interacción clara y sencilla mediante menús.  

---

## 🗂️ Modelo Conceptual (Relaciones)

- **Estudiante 1.. → 0..* Préstamo**  
  (Un estudiante puede tener varios préstamos activos o históricos).  
- **Libro 1.. → 0..* Préstamo**  
  (Un libro puede estar en varios préstamos, en distintos momentos).  
- **Autor 1 → * Libro**  
  (Un autor puede escribir varios libros, un libro pertenece a un autor).  
- **Préstamo 1 → 1 Estudiante y 1 Libro**  
  (Cada préstamo está asociado a un estudiante y un libro).  

---

## 🏗️ Diseño POO
Este sistema aplica los 4 pilares de la Programación Orientada a Objetos (POO):  

- **Abstracción**: clases como `Persona`, `Estudiante`, `Autor`, `Libro` y `Prestamo` modelan entidades reales ocultando detalles innecesarios.  
- **Encapsulación**: se restringe acceso directo a propiedades sensibles usando validaciones y `private set`, asegurando integridad de datos.  
- **Herencia**: la clase abstracta `Persona` sirve como base para `Estudiante` y `Autor`, reutilizando código y agregando especializaciones.  
- **Polimorfismo**: sobrescritura del método `ToString` y uso de la interfaz `IPrestamo` permiten trabajar con distintos tipos de objetos de forma uniforme y extensible.  

---

## ✅ Plan de Pruebas

| Caso                  | Entrada                        | Resultado esperado |
|------------------------|--------------------------------|--------------------|
| Validación estudiante | Documento válido               | El sistema permite continuar |
| Libro disponible      | ID libro con stock = 3         | Se permite préstamo, stock pasa a 2 |
| Libro no disponible   | ID libro con stock = 0         | Mensaje de no disponible |
| Generación de préstamo| Documento válido + ID libro    | Se crea préstamo con ID único y fecha +15 días |
| Devolución            | ID préstamo activo             | Stock aumenta en 1 y préstamo cerrado |
| Préstamo vencido      | ID préstamo con fecha vencida  | El sistema marca el préstamo como vencido |

---
