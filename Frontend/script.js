const API_URL = "http://localhost:5298/calculator";

const toggleBtn = document.getElementById("theme-toggle");
const icon = toggleBtn.querySelector("img");

toggleBtn.addEventListener("click", () => {
    const currentTheme = document.documentElement.getAttribute("data-theme");

    if (currentTheme === "dark") {
        document.documentElement.removeAttribute("data-theme");
        icon.src = "img/moon.png";
    } else {
        document.documentElement.setAttribute("data-theme", "dark");
        icon.src = "img/sun-icon-30.png";
    }
});
let expr = "";

function refresh() {
document.getElementById("expression").innerText = expr || "";
}

function append(ch) {
expr += ch;
refresh();
}

function clearAll() {
expr = "";
document.getElementById("res").innerText = "";
document.getElementById("status").innerText = "";
refresh();
}

function backspace() {
expr = expr.slice(0, -1);
refresh();
}

function applySquare() {
if (!expr) return;
expr = `(${expr})^2`;
refresh();
}

function applySqrt() {
if (!expr) return;
expr = `sqrt(${expr})`;
refresh();
}

function applyExpN() {
const n = document.getElementById("exp-n-input").value;
if (!expr || n === "") return;
expr = `(${expr})^${n}`;
refresh();
}

async function faireCalcul() {
if (!expr) return;
const statusEl = document.getElementById("status");
const resEl = document.getElementById("res");
statusEl.innerText = "…";
resEl.innerText = "";

try {
    const response = await fetch(`${API_URL}/calculer`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(expr)
    });

    const text = await response.text();
    const data = JSON.parse(text);
    resEl.innerText = data.res ?? data.result ?? data;
    statusEl.innerText = "";
    chargerHistorique();
} catch (err) {
    statusEl.innerText = "Erreur API";
}
}

async function chargerHistorique() {
try {
    const response = await fetch(`${API_URL}/historique`);
    const logs = await response.json();
    const liste = document.getElementById("liste-historique");
    liste.innerHTML = "";

    logs.forEach(log => {
    const li = document.createElement("li");
    const left = document.createElement("span");
    left.innerHTML = `${log.expression} <span class="eq">= ${log.result}</span>`;
    const right = document.createElement("span");
    right.className = "date";
    right.innerText = new Date(log.createdAt).toLocaleString();
    li.appendChild(left);
    li.appendChild(right);
    liste.appendChild(li);
    });
} catch {
    const liste = document.getElementById("liste-historique");
    liste.innerHTML = "<li><span>Historique non disponible</span></li>";
}
}



window.onload = chargerHistorique;

