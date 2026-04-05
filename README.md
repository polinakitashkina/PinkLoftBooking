# Розовый лофт — задание 1 (C#)

Простой **сервис бронирования ресурсов** под требования ЯЗИМЕП: HTTP API, SQLite, JWT, слои **Controller → Service → Repository**, DTO, доменные сущности, валидация, логирование, Swagger, минимальные тесты. Визуально — розовый лендинг в `wwwroot`.

## Что внутри

| Путь | Назначение |
|------|------------|
| `PinkLoftBooking.Api/` | ASP.NET Core 8 Web API + статика |
| `PinkLoftBooking.Api/wwwroot/` | Девчачий UI (шрифты, формы входа/брони) |
| `PinkLoftBooking.Tests/` | xUnit-тесты правил пересечения интервалов |
| `PinkLoftBooking.sln` | Решение Visual Studio / Rider |

## Запуск

Нужен [.NET 8 SDK](https://dotnet.microsoft.com/download).

```bash
cd "/Users/polinakitaskina/Desktop/язимп"
dotnet restore
dotnet build
dotnet run --project PinkLoftBooking.Api
```

Открой в браузере адрес из консоли (по умолчанию **http://localhost:5088**). Документация API: **http://localhost:5088/swagger**.

При первом запуске создаётся файл **`pinkloft.db`** в рабочей директории процесса (обычно рядом с запуском из папки `PinkLoftBooking.Api`).

## Демо-данные

- **6 ресурсов** (бьюти, платья, фотозона, йога, кухня, котокафе) — сидятся при пустой таблице.
- **Администратор** (если ещё нет пользователя с этим email):
  - email: `admin@pinkloft.local`
  - пароль: `Admin123!` (меняется в `appsettings.json`, секция `Seed`)

Админ может вызывать `POST /api/resources` (в Swagger нажми **Authorize** и вставь JWT после входа).

## Соответствие заданию 1

- Регистрация и вход (`/api/auth/register`, `/api/auth/login`), пароли через **BCrypt**, токен **JWT**.
- Ресурсы: просмотр у всех (`GET /api/resources`), создание — **только Admin** (`POST /api/resources`).
- Брони: создание, изменение (`PUT /api/bookings/{id}`), отмена (`DELETE` → статус `cancelled`).
- **Пересечения по времени** для одного ресурса отсекаются для активных броней.
- **Лимит**: не больше `BookingPolicy:MaxBookingsPerDay` активных броней с началом в один календарный день **UTC** на пользователя (по умолчанию 5).
- **Уведомления**: логически — запись в лог (`INotificationService`).
- **Роли**: `User` и `Admin` (claim `role` в JWT).

## Тесты

```bash
dotnet test
```

## Сброс базы

Удали `pinkloft.db` (и при необходимости `-shm`/`-wal`) и перезапусти приложение — таблицы и сид создадутся снова.

## GitHub и ссылка для всех

Пошагово: файл **[DEPLOY-GITHUB.md](DEPLOY-GITHUB.md)** — заливка на GitHub и бесплатный хостинг **Render** (Docker), чтобы сайт открывался по `https://...` с любого устройства.

Копия проекта для сдачи лежит в папке **`PinkLoftBooking` на рабочем столе** (см. DEPLOY-GITHUB.md).
