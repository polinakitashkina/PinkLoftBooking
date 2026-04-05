const TOKEN_KEY = "pinkloft_token";
const EMAIL_KEY = "pinkloft_email";
const ROLE_KEY = "pinkloft_role";

function token() {
  return sessionStorage.getItem(TOKEN_KEY) || "";
}

function authHeaders() {
  const h = { "Content-Type": "application/json" };
  const t = token();
  if (t) h.Authorization = "Bearer " + t;
  return h;
}

function flash(msg, ok) {
  const el = document.getElementById("api-flash");
  if (!el) return;
  el.textContent = msg;
  el.className = "api-flash show " + (ok ? "ok" : "err");
  clearTimeout(flash._t);
  flash._t = setTimeout(() => {
    el.classList.remove("show");
  }, 5200);
}

function updateAccountUi() {
  const status = document.getElementById("account-status");
  const logout = document.getElementById("btn-logout");
  const email = sessionStorage.getItem(EMAIL_KEY);
  const role = sessionStorage.getItem(ROLE_KEY);
  if (!status || !logout) return;
  if (email) {
    status.textContent = `Привет, ${email} · роль: ${role || "user"}`;
    logout.style.display = "inline-flex";
  } else {
    status.textContent = "Ты ещё не вошла 💌";
    logout.style.display = "none";
  }
}

async function parseJsonOrText(res) {
  const text = await res.text();
  if (!text) return null;
  try {
    return JSON.parse(text);
  } catch {
    return { message: text };
  }
}

document.querySelectorAll('a[href^="#"]').forEach((anchor) => {
  anchor.addEventListener("click", (e) => {
    const id = anchor.getAttribute("href");
    if (!id || id === "#") return;
    const el = document.querySelector(id);
    if (el) {
      e.preventDefault();
      el.scrollIntoView({ behavior: "smooth", block: "start" });
    }
  });
});

document.getElementById("form-register")?.addEventListener("submit", async (e) => {
  e.preventDefault();
  const fd = new FormData(e.target);
  const body = {
    email: String(fd.get("email") || "").trim(),
    password: String(fd.get("password") || ""),
  };
  const res = await fetch("/api/auth/register", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });
  const data = await parseJsonOrText(res);
  if (!res.ok) {
    flash(data?.message || "Ошибка регистрации", false);
    return;
  }
  sessionStorage.setItem(TOKEN_KEY, data.token);
  sessionStorage.setItem(EMAIL_KEY, data.email);
  sessionStorage.setItem(ROLE_KEY, data.role || "");
  flash("Аккаунт создан, ты внутри ✨", true);
  updateAccountUi();
  loadMyBookings();
});

document.getElementById("form-login")?.addEventListener("submit", async (e) => {
  e.preventDefault();
  const fd = new FormData(e.target);
  const body = {
    email: String(fd.get("email") || "").trim(),
    password: String(fd.get("password") || ""),
  };
  const res = await fetch("/api/auth/login", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });
  const data = await parseJsonOrText(res);
  if (!res.ok) {
    flash(data?.message || "Не получилось войти", false);
    return;
  }
  sessionStorage.setItem(TOKEN_KEY, data.token);
  sessionStorage.setItem(EMAIL_KEY, data.email);
  sessionStorage.setItem(ROLE_KEY, data.role || "");
  flash("С возвращением 💗", true);
  updateAccountUi();
  loadMyBookings();
});

document.getElementById("btn-logout")?.addEventListener("click", () => {
  sessionStorage.removeItem(TOKEN_KEY);
  sessionStorage.removeItem(EMAIL_KEY);
  sessionStorage.removeItem(ROLE_KEY);
  updateAccountUi();
  renderBookings([]);
  flash("Ты вышла из аккаунта", true);
});

function localInputToUtcIso(value) {
  if (!value) return null;
  const d = new Date(value);
  if (Number.isNaN(d.getTime())) return null;
  return d.toISOString();
}

async function loadResources() {
  const sel = document.getElementById("booking-resource");
  if (!sel) return;
  try {
    const res = await fetch("/api/resources");
    const list = await res.json();
    sel.innerHTML = '<option value="">Выбери зону</option>';
    for (const r of list) {
      const opt = document.createElement("option");
      opt.value = r.id;
      opt.textContent = r.name;
      sel.appendChild(opt);
    }
  } catch {
    sel.innerHTML = '<option value="">Не удалось загрузить зоны</option>';
  }
}

document.getElementById("form-booking")?.addEventListener("submit", async (e) => {
  e.preventDefault();
  if (!token()) {
    flash("Сначала войди в аккаунт", false);
    return;
  }
  const resourceId = document.getElementById("booking-resource")?.value;
  const startLocal = document.getElementById("booking-start")?.value;
  const endLocal = document.getElementById("booking-end")?.value;
  const startUtc = localInputToUtcIso(startLocal);
  const endUtc = localInputToUtcIso(endLocal);
  if (!resourceId || !startUtc || !endUtc) {
    flash("Заполни все поля", false);
    return;
  }
  const res = await fetch("/api/bookings", {
    method: "POST",
    headers: authHeaders(),
    body: JSON.stringify({ resourceId, startUtc, endUtc }),
  });
  const data = await parseJsonOrText(res);
  if (!res.ok) {
    flash(data?.message || "Бронь не создана", false);
    return;
  }
  flash("Бронь создана 🎀", true);
  loadMyBookings();
});

function renderBookings(items) {
  const ul = document.getElementById("bookings-list");
  if (!ul) return;
  ul.innerHTML = "";
  if (!items.length) {
    ul.innerHTML = '<li class="meta">Пока пусто — создай бронь выше.</li>';
    return;
  }
  for (const b of items) {
    const li = document.createElement("li");
    const left = document.createElement("div");
    left.innerHTML = `<strong>${escapeHtml(b.resourceName || "")}</strong><div class="meta">${fmtRange(b.startUtc, b.endUtc)} · ${escapeHtml(String(b.status))}</div>`;
    li.appendChild(left);
    if (String(b.status).toLowerCase() === "active") {
      const btn = document.createElement("button");
      btn.type = "button";
      btn.className = "btn btn-tiny";
      btn.textContent = "отменить";
      btn.addEventListener("click", () => cancelBooking(b.id));
      li.appendChild(btn);
    }
    ul.appendChild(li);
  }
}

function escapeHtml(s) {
  return s.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
}

function fmtRange(start, end) {
  try {
    const a = new Date(start);
    const b = new Date(end);
    return a.toLocaleString() + " — " + b.toLocaleString();
  } catch {
    return start + " — " + end;
  }
}

async function cancelBooking(id) {
  if (!token()) return;
  const res = await fetch("/api/bookings/" + encodeURIComponent(id), {
    method: "DELETE",
    headers: authHeaders(),
  });
  if (!res.ok) {
    const data = await parseJsonOrText(res);
    flash(data?.message || "Не удалось отменить", false);
    return;
  }
  flash("Бронь отменена", true);
  loadMyBookings();
}

async function loadMyBookings() {
  if (!token()) {
    renderBookings([]);
    return;
  }
  const res = await fetch("/api/bookings/my", { headers: authHeaders() });
  if (!res.ok) {
    renderBookings([]);
    return;
  }
  const list = await res.json();
  renderBookings(list);
}

updateAccountUi();
loadResources();
loadMyBookings();
