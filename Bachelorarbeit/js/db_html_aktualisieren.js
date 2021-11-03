const einträge = document.querySelector('#einträge_anzeige');
var tagebuchPanel = document.querySelector('#listeTagebuch');

var datum;
var tagebuch = [];

// Drag and Drop - Tagebucheinträge aus Datenbank anzeigen
const anzeigeEinträge = (array, farbe, kategorie) => {
  // Wenn es mehr als 16 Elemente gibt, soll ein Scrollbalken aktiviert werden
  var grid = document.querySelector("#einträge_anzeige");
  if (array.length > 16) {
    grid.classList.add("active");
  } else {
    grid.classList.remove("active");
  }

  for (var item in array) {
    if (array[item].database != "delete") {
      if (kategorie == array[item].Kategorie) {
        if (array[item].Punkte != 0) {
          const htmlMitStern = `
            <div value="${array[item].Punkte}" data-id="${array[item].Id_Elemente}" name="${array[item].Name}">
              <div class="item" data-id="${array[item].Id_Elemente}" onclick="elementBearbeitenPageÖffnen(this)" name="${array[item].Name}" value="${array[item].Punkte}" style="background:${farbe}">
                <span class="items">${array[item].Name}</span>
                <img class="imgStern" src="./bilder/stern-weiss.png">
                <img class="imgIcon" src="./bilder/eintrag/${array[item].Img}">
              </div>
            </div>
          `;
          einträge.innerHTML += htmlMitStern;
        } else {
          const html = `
              <div value="${array[item].Punkte}" data-id="${array[item].Id_Elemente}" name="${array[item].Name}">
                <div class="item" data-id="${array[item].Id_Elemente}" onclick="elementBearbeitenPageÖffnen(this)" name="${array[item].Name}" value="${array[item].Punkte}" style="background:${farbe}">
                  <span class="items">${array[item].Name}</span>
                  <img class="imgIcon" src="./bilder/eintrag/${array[item].Img}">
                </div>
              </div>
          `;
          einträge.innerHTML += html;
        }
      }
    }
  }
};

function datumsFormat(date, typ) {
  var thisDatum = date.split('-');
  var neuesDatum;
  var monat;

  if (thisDatum[1] == "01") {
    monat = "Januar";
  } else if (thisDatum[1] == "02") {
    monat = "Februar";
  } else if (thisDatum[1] == "03") {
    monat = "März";
  } else if (thisDatum[1] == "04") {
    monat = "April";
  } else if (thisDatum[1] == "05") {
    monat = "Mai";
  } else if (thisDatum[1] == "06") {
    monat = "Juni";
  } else if (thisDatum[1] == "07") {
    monat = "Juli";
  } else if (thisDatum[1] == "08") {
    monat = "August";
  } else if (thisDatum[1] == "09") {
    monat = "September";
  } else if (thisDatum[1] == "10") {
    monat = "Oktober";
  } else if (thisDatum[1] == "11") {
    monat = "November";
  } else if (thisDatum[1] == "12") {
    monat = "Dezember";
  }

  if (typ == "start") {
    neuesDatum = [thisDatum[2]-0,monat].join(" ");
  } else {
    neuesDatum = [thisDatum[2]-1,monat].join(" ");
  }
  return neuesDatum;
}

