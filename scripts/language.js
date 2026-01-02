function languageSet() {
    const language = localStorage.getItem("language") === "1" ? "nl" : "en";
    const body = document.body;

    if (!body) return;

    body.classList.toggle("lang-en", language === "en");
    body.classList.toggle("lang-nl", language === "nl");
    document.documentElement.setAttribute("lang", language);
}

function changeLang() {
    const next = localStorage.getItem("language") === "1" ? "0" : "1";
    localStorage.setItem("language", next);
    languageSet();
}

document.addEventListener("DOMContentLoaded", languageSet);
