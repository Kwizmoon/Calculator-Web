const API_URL = "https://calculatortp2-eca3egh7gffhdpfy.canadacentral-01.azurewebsites.net/calculator";

const toggleBtn = document.getElementById("theme-toggle");
const icon = toggleBtn.querySelector("img");

toggleBtn.addEventListener("click", () => {
    const currentTheme = document.documentElement.getAttribute("data-theme");

    if (currentTheme === "dark") {
        document.documentElement.removeAttribute("data-theme");
        icon.src = "moon.png";
    } else {
        document.documentElement.setAttribute("data-theme", "dark");
        icon.src = "sun-icon-30.png";
    }
});
let expr = "";

function refresh() {
    document.getElementById("expression").innerText = expr || "";
}

function append(ch) {

    document.getElementById("res").classList.remove("error-text");

    const operators = ["+", "-", "*", "/", "^"];
    const lastChar = expr.slice(-1);

    // Prevent double operators 
    if (operators.includes(ch) && operators.includes(lastChar)) {
        return; 
    }
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
if (!expr) return;
    expr += "^"; 
    refresh();
}

async function faireCalcul() {
    if (!expr) return;
    const statusEl = document.getElementById("status");
    const resEl = document.getElementById("res");

    resEl.classList.remove("error-text");
    resEl.innerText = "";

    const operators = ["+", "-", "*", "/", "^"];
    if (operators.includes(expr.slice(-1))) {
        document.getElementById("res").innerText = "Error: Incomplete expression";
        resEl.classList.add("error-text");
        statusEl.innerText = "";
        return;
    }

    try {
        const response = await fetch(`${API_URL}/calculer`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ expression: expr })
        });

        const text = await response.text();
        const data = JSON.parse(text);
        finalResult = data.res ?? data.result ?? data;
        resEl.innerHTML = finalResult;

        if (typeof finalResult === "string" && (finalResult.includes("Error") || finalResult.includes("Invalid"))) {
            resEl.classList.add("error-text");
        }
        
        statusEl.innerText = "";
        chargerHistorique();
    } catch (err) {
        statusEl.innerText = "Erreur API";
        resEl.classList.add("error-text");
        statusEl.innerText = "";
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

