
document.addEventListener('DOMContentLoaded', () => {
    // Für alle Input Felder das Öffnen/Speichern per Enter Taste ermöglichen
    // Registrieren Page
    var inputNameReg = document.getElementById("Rbenutzername");
    var inputPasswortReg = document.getElementById("Rpasswort");
    var inputPasswortWReg = document.getElementById("RpasswortW");
  
    if (inputNameReg != null) {
      inputNameReg.addEventListener("keyup", function(event) {
        if (event.key === "Enter") {
          event.preventDefault();
          checkBenutzer('registrieren');
        }
      }); 
    }
    if (inputPasswortReg != null) {
      inputPasswortReg.addEventListener("keyup", function(event) {
        if (event.key === "Enter") {
          event.preventDefault();
          checkBenutzer('registrieren');
        }
      }); 
    }
    if (inputPasswortWReg != null) {
      inputPasswortWReg.addEventListener("keyup", function(event) {
        if (event.key === "Enter") {
          event.preventDefault();
          checkBenutzer('registrieren');
        }
      }); 
    }
  });

/* -------------------------- Benutzeranmeldung --------------------------------*/
function checkLoginStatus() {
    /*var benutzer = localStorage.getItem("user");
  
    if (benutzer == null) {
      window.localStorage.clear();
      window.location.href = "./index.html";
    }*/
}

function logOut() {
    /*localStorage.removeItem("user");
    clearLocalStorage();
    window.location.href = "./index.html";*/
}

// bei falscher Text Eingabe Fehlermeldung anzeigen
function showFehler(text) {
    var fehler = document.getElementById("fehler");
    fehler.innerText = text;
    fehler.style.opacity = 1;
}

/* ----------------------------- Therapieplan --------------------------*/
/* --- Pop-Up Text Eingabe im Therapieplan --- */
var blockFreierText;
function popUpTextSpeichern() {
  var input = document.querySelector(".popUp_Input");
  var container = document.querySelector(".popUp_ContainerAlles");

  blockFreierText.innerHTML = input.value;
  container.classList.remove("active");
}


/* ------- Bewertung ------- */
function showBewertung() {
  const bewertungContainer = document.querySelector(".bewertung-container");
  bewertungContainer.classList.add("active");
}

function hideBewertung() {
  const bewertungContainer = document.querySelector(".bewertung-container");
  bewertungContainer.classList.remove("active");
}

/* ------- Pop-Up Sticker Auswahl im Tagebuch Eintrag ----------- */
function showStickerAuswahl() {
  // console.log(array);
  var benutzerGesamtPunkte = localStorage.getItem("benutzerGesamtPunkte");
  var container = document.querySelector(".popUp_ContainerAlles");
  var ergebnis = berechneLevel(benutzerGesamtPunkte);
  var sticker = getAllSticker();
  
  anzeigeSticker(sticker, ergebnis[0].level);
  // Pop Up anzeigen
  container.classList.add("active");
}

/* ------------------------------- Tagebuch ------------------------------*/
function loadTagebuch(array) {
  clearLocalStorage();
  showPunkteBanner();
  menuWahlHauptFärben();
  var kategorie = localStorage.getItem("kategorie");
  var farbe = localStorage.getItem("farbe");
  var zuordnungArray = [];

  for ( var i=0, len=array.length; i < len; i++ ) {
    zuordnungArray[array[i]['Zuordnung']] = array[i];
  }

  zuordnungArray.sort(sortObjectArray('Datum', 'desc'));

  anzeigeTagebuch(array, zuordnungArray);
}

  function gesamtPunkte(array) {
    var gesamtPunkte = 0;
    for (var item in array) {
      gesamtPunkte += parseInt(array[item].Punkte);
    }
    localStorage.setItem("gesamtPunkte", gesamtPunkte);
    localStorage.setItem("gesamtPunkteOffline", gesamtPunkte);
  }

  function setBackground (kategorie) {
    var body = document.querySelector(".hintergrund_eintrag");
    const img = document.querySelector(".setImg");

    const htmlImgBerge = `
        <img class="bild_hintergrund" src="./bilder/hintergrund_berge.png">
          `;
    const htmlImgNatur = `
        <img class="bild_hintergrund" src="./bilder/hintergrund_natur.png">
          `;

    if (kategorie == "Befinden") {
      body.style.background = "linear-gradient(to bottom, #01B0EB, #E8F9FF)";
      img.innerHTML += htmlImgBerge;
    } else if (kategorie == "Tagesablauf") {
      body.style.background = "linear-gradient(to bottom, #AAE551, #E1FFB4)";
      img.innerHTML += htmlImgNatur;
    }
  }

  function checkLength() {
    var textKorrekt = "";
    const icon = localStorage.getItem("Icon");
    var fehlermeldung = document.querySelector('#fehler');
    var textInput = document.querySelector('.input_aussehen');
    
    if (textInput.value.length == 0) {
      fehlermeldung.setAttribute('style', 'opacity:1');
      fehlermeldung.innerHTML = "Name des Eintrags eingeben !";
      textKorrekt = false;
    } else if (textInput.value.length > 15) {
      fehlermeldung.setAttribute('style', 'opacity:1');
      fehlermeldung.innerHTML = "Maximal 8 Zeichen pro Wort und zwei Wörter!";
      textKorrekt = false;
    } else if (icon == null) {
      fehlermeldung.setAttribute('style', 'opacity:1');
      fehlermeldung.innerHTML = "Icon auswählen !";
      textKorrekt = true;
    } else {
      fehlermeldung.setAttribute('style', 'opacity:0');
      textKorrekt = true;
    }
    return textKorrekt;
  }
  
  // Icons entsprechend der ausgewählten Kategorie färben
  function iconFärben() {
    const kategorie = localStorage.getItem("kategorie");
    var elements = document.getElementsByClassName('box_icon'); // get all elements
  
    if (kategorie == "Befinden") {
      for(var i = 0; i < elements.length; i++){
        elements[i].style.backgroundColor = "#72DCFF";
      }
    } else if (kategorie == "Tagesablauf") {
      for(var i = 0; i < elements.length; i++){
        elements[i].style.backgroundColor = "#CFFF87";
      }
    }
  }
  /* ------------------------------- Kalender -----------------------------*/
  function kalenderMenuOben(meine, station) {
    var meineTermine = document.querySelector(".meine");
    var stationTermine = document.querySelector(".station");
    const farbeHellblau = "#72DCFF";
    const farbeDunkelBlau = "#5D8BDB";
  
    if (meine == "on") {
      meineTermine.style.backgroundColor = farbeDunkelBlau;
    } else {
      meineTermine.style.backgroundColor = farbeHellblau;
    }
    if (station == "on") {
      stationTermine.style.backgroundColor = farbeDunkelBlau;
    } else {
      stationTermine.style.backgroundColor = farbeHellblau;
    }
  }

  /* ------------------------------- Allgemein ----------------------------------*/
  function clearLocalStorage() {
    localStorage.removeItem("Zuordnung");
    localStorage.removeItem("sticker");
    localStorage.removeItem("elementID");
    localStorage.removeItem("kategorie");
    localStorage.removeItem("farbe");
    localStorage.removeItem("elementText");
    localStorage.removeItem("level");
  }
  
  function savePage(page) {
    localStorage.setItem("Page", page);
  }

  function datumHeute() {
    var heute = new Date();
    var day = heute.getDate(); // Tag
    var month = heute.getMonth()+1; // Monat
    var year = heute.getFullYear(); // Jahr
    // Formatierung 01-09 statt 1-9
    if(day < 10) {
      day = '0'+ day;
    }
    if(month < 10) {
      month = '0'+ month;
    }
    heute = year + '.' + month + '.' + day;
  
    return heute;
  }

