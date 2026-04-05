# GitHub и публичная ссылка на сайт

Кратко: код заливаешь на **GitHub** (учитель открывает репозиторий). Чтобы **любой человек** открыл сайт в браузере по ссылке, подключи бесплатный хостинг **Render** (или аналог) к этому репозиторию.

---

## 1. Копия проекта на рабочем столе

Папка **`PinkLoftBooking`** на рабочем столе — полная копия проекта для отправки на GitHub. Работай с ней для `git push`.

---

## 2. Залить код на GitHub

1. Зайди на [github.com](https://github.com) и войди в аккаунт.
2. **New repository** (зелёная кнопка или «+» → New repository).
3. Имя, например: `pink-loft-booking`. Можно **Public**. **Не** ставь галочки «Add README» (у нас уже есть файлы).
4. Создай репозиторий.

В **Терминале** на Mac:

```bash
cd ~/Desktop/PinkLoftBooking

git init
git add .
git commit -m "Розовый лофт: сервис бронирования зон"

git branch -M main
git remote add origin https://github.com/ТВОЙ_ЛОГИН/ИМЯ_РЕПОЗИТОРИЯ.git
git push -u origin main
```

GitHub попросит логин. Удобнее использовать **Personal Access Token** вместо пароля:  
GitHub → **Settings → Developer settings → Personal access tokens** → создать token с правом `repo`.

**Ссылка для учителя на код:**  
`https://github.com/ТВОЙ_ЛОГИН/ИМЯ_РЕПОЗИТОРИЯ`

---

## 3. Сайт по ссылке для всех (Render, бесплатно)

1. Зайди на [render.com](https://render.com) и зарегистрируйся через **GitHub**.
2. **New → Web Service**.
3. Подключи репозиторий `pink-loft-booking` (или как назвала).
4. Настройки:
   - **Runtime:** Docker (или «Docker» / образ из Dockerfile).
   - **Dockerfile Path:** `Dockerfile`
   - **Docker Build Context:** `.` (корень репозитория)
5. **Instance type:** Free.
6. В разделе **Environment** добавь переменную (обязательно для продакшена):

   | Key | Value (пример) |
   |-----|----------------|
   | `Jwt__SigningKey` | любая длинная случайная строка **от 32 символов**, например `moja-super-sekretnaya-fraza-dlya-jwt-2026!` |

   (В .NET вложенные ключи из `appsettings` задаются через **двойное подчёркивание:** `Jwt__SigningKey`.)

7. Нажми **Create Web Service**. Первая сборка займёт несколько минут.

После деплоя Render даст адрес вида **`https://pink-loft-booking.onrender.com`** — **эту ссылку** можно слать всем: откроется с телефона и с компьютера.

**Важно про бесплатный план:** сервис «засыпает» без посещений; первое открытие после паузы может грузиться **30–60 секунд**. Данные в SQLite на бесплатном диске могут сбрасываться при пересборке — для демо это обычно нормально.

---

## 4. Что отправить учителю

- Ссылка на **репозиторий GitHub** (код и история).
- По желанию — ссылка на **живой сайт** с Render (если настроила шаг 3).

---

## Локальный запуск (как раньше)

```bash
cd ~/Desktop/PinkLoftBooking
export PATH="$HOME/.dotnet:$PATH"   # если dotnet ставила скриптом в домашнюю папку
dotnet run --project PinkLoftBooking.Api --urls "http://127.0.0.1:5088"
```

Браузер: `http://127.0.0.1:5088`
