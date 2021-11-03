/* --- Punkte banner --- */
var punkteBannerContainer = document.querySelector(".punkteBanner-container");


function showPunkteBanner() {
  var punkte = localStorage.getItem("gesamtPunkte");
  var bannerText = document.querySelector(".bannerText");

  if (punkte != null && punkte != 0 && isNaN(punkte) == false) {
    bannerText.innerHTML = "Du hast " + punkte + " Punkte verdient.";
    punkteBannerContainer.classList.add("active");

    setTimeout(() => {
    punkteBannerContainer.classList.remove("active");
    localStorage.removeItem("gesamtPunkte");
    }, 4000);
  }
}

function showLöschenBanner(value) {
  var bannerTitel = document.querySelector(".bannerTitel");
  var bannerText = document.querySelector(".bannerText");

  punkteBannerContainer.style.background = "#F78181";

  bannerTitel.innerHTML = "Tagebuch gelöscht";
  bannerText.innerHTML = "Du hast den Tagebuch Eintrag<br> <b>" + value + "</b> <br>erfolgreich gelöscht";
  punkteBannerContainer.classList.add("active");

  setTimeout(() => {
  punkteBannerContainer.classList.remove("active");
  }, 4000);
}

function loadPunkte(array) {
  var ergebnis = berechneLevel(array);
  var punkteText = document.querySelector(".punkte_titel");
  var profilBild = document.querySelector(".hintergrund_Sticker");
  var level = document.querySelector(".level");
  var progress = document.querySelector('.punkteBalken-fertig');
  var progressUntererBalken = document.querySelector('.punkteNextLevel');
  var nextLevelPunkte = parseInt(ergebnis[0].punkteProgressbar);
  var bild = getAllSticker();
  nextLevelPunkte -= 100;
  nextLevelPunkte *= -1;

  const htmlBild = `
    <img class="sticker gross" src="./bilder/sticker/${bild[ergebnis[0].level-1]}">
  `;
  profilBild.innerHTML = htmlBild;

  localStorage.setItem("level", ergebnis[0].level);

  level.innerHTML = "Level " + ergebnis[0].level;
  punkteText.innerHTML = "Meine Punkte: " + array[0].GesamtPunkte;
  progressUntererBalken.innerHTML = "noch " + nextLevelPunkte;

  if (ergebnis[0].punkteProgressbar > 10) {
    progress.innerHTML = array[0].GesamtPunkte;
  }

  progress.style.width = ergebnis[0].punkteProgressbar + '%';
  progress.style.opacity = 1;
}

/* --- Meine Punkte --- */
function berechneLevel(punkte) {
  var ergebnis = [];
  var punkteGesamt = parseInt(punkte[0].GesamtPunkte);

  if (isNaN(punkteGesamt) == true) {
    punkteGesamt = punkte;
  }
  
  var p = (punkteGesamt / 100).toString();
  if (punkteGesamt < 1000) {
    var level = parseInt(p.charAt(0));
    var punkteProgressbar = p.charAt(2) +0;
  } else {
    //var level = parseInt("1" + p.charAt(1));
    var level = parseInt(p.charAt(0) + p.charAt(1));
    var punkteProgressbar = p.charAt(3) +0;
  }

  // Maximal Level bis 18
  if (punkteGesamt > 1800) {
    level = 17;
    punkteProgressbar = 100;
  }

  ergebnis.push({"level":level+1, "punkteProgressbar":punkteProgressbar,},);
  return ergebnis;
}

function showSticker() {
  checkLoginStatus();
  var level = parseInt(localStorage.getItem("level"));
  var elements = document.getElementsByClassName('hintergrund_Sticker'); // get all elements

  for(var i = 0; i < level; i++){
    elements[i].style.opacity = "1";
  }
}

function getAllSticker() {
  var bild = ["katze.png", "maus.png", "esel.png", "schaf.png", "hund.png", "lowe.png", "elefant.png", "kuh.png", 
              "giraffe.png", "tiger.png", "nilpferd.png", "fuchs.png", "hase.png", "panda.png", "otter.png", "pinguin.png", 
              "koala.png", "rentier.png"];
  
  // return bild[level-1];
  return bild;
}

