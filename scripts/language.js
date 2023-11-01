function languageSet() {
    let language = parseInt(localStorage.getItem('language')) || 0;
    let EngText = document.getElementsByClassName("English");
    let NlText = document.getElementsByClassName("Dutch");

    for (let i = 0; i < EngText.length; i++) {
        if (language === 0) {
            EngText[i].style.display = 'block';
            NlText[i].style.display = 'none';
        } else {
            EngText[i].style.display = 'none';
            NlText[i].style.display = 'block';
        }
    }

    console.log("called");
    console.log(localStorage.getItem('language'));
}

function changeLang(){
    localStorage.getItem('language') == "0" ? localStorage.setItem('language', "1") : localStorage.setItem('language', "0");
    languageSet();
    console.log("CHANGE!");
}