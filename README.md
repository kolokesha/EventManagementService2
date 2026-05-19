# Event Management Service

Простой REST API сервис для управления событиями (Events), реализованный на ASP.NET Core.

## 🚀 Возможности

* Получение списка событий
* Получение события по Id
* Создание события
* Обновление события
* Удаление события
* Валидация входящих данных
* Кастомная обработка ошибок
* In-memory хранение (без БД)

---

## 🏗 Архитектура

Проект разделён на слои:

```
EventManagementService
│
├── Application
│   ├── Events
│   │   ├── Dto
│   │   ├── Services (интерфейсы)
│
├── Domain
│   ├── Entities
│
├── Infrastructure
│   ├── Repository (in-memory)
│   ├── Services (реализация бизнес-логики)
│   ├── DI
│
├── Controllers
├── Middlewares
```

---

## 🧠 Основные принципы

* Разделение DTO и Domain моделей
* Dependency Injection
* Repository pattern (in-memory реализация)
* Бизнес-логика в сервисах
* Обработка ошибок через middleware

---

## 📦 Технологии

* ASP.NET Core
* C#
* Dependency Injection
* DataAnnotations
* Swagger (OpenAPI)

---

## ▶️ Запуск проекта

### 1. Клонировать репозиторий

```bash
git clone <your-repo-url>
cd EventManagementService
```

### 2. Запуск

```bash
dotnet run
```

### 3. Swagger

После запуска:

```
https://localhost:<port>/swagger
```

---

## 📡 API Эндпоинты

### Получить все события

```
GET /event
```

---

### Получить событие по Id

```
GET /event/{id}
```

---

Query параметры:
page (default = 1)
pageSize (default = 10)
title (optional)
from (optional)
to (optional)

Пример:
GET /event?page=1&pageSize=10&title=Music

### Создать событие

```
POST /event
```

Пример запроса:

```json
{
  "totalCount": 20,
  "page": 1,
  "pageSize": 10,
  "items": [
    {
      "id": 1,
      "title": "Music Event",
      "description": "Some description",
      "startAt": "2025-01-01T10:00:00",
      "endAt": "2025-01-01T12:00:00"
    }
  ]
}
```

---

### Обновить событие

```
PUT /event/{id}
```

---

### Удалить событие

```
DELETE /event/{id}
```

---

## ⚠️ Валидация

* `[Required]` для обязательных полей
* Проверка бизнес-логики:

    * `EndAt` не может быть раньше `StartAt`

При ошибке возвращается:

```json
{
  "message": "Дата окончания не может быть раньше даты начала"
}
```

---

## 🧪 Хранение данных

Данные хранятся в памяти:

* Используется `Dictionary<int, EventModel>`
* При перезапуске приложения данные удаляются

---



## 👨‍💻 Автор

Alexey Kolomeitsev

---

## 📄 Лицензия

MIT
