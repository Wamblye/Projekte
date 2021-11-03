/* ------------- LOGIN ------------------ */
function login() {
  var benutzername = document.getElementById("benutzername").value;
  var passwort = document.getElementById("passwort").value;

  $.ajax({
    url: 'php/login.php',
    data: 'benutzername=' + benutzername + '&passwort=' + passwort,
    type: "POST",
    success: function (data) {
      array = JSON.parse(data);
      if (array.length != 0) {
        console.log("Login erfolgreich");
        localStorage.setItem("user", array[0].Id_Benutzer);
        window.location.href = "./therapieplan.html";
      } else {
        console.log("Login fehlgeschlagen");
        showFehler("Benutzername oder Passwort falsch!");
      }
    }
  });
}
/* ---------- Kalender ----------- */
function loadDbKalender() {
  checkLoginStatus();
  kalenderMenu();
  var meine = localStorage.getItem("Meine");
  var station = localStorage.getItem("Station");
  kalenderMenuOben(meine, station);
  clearLocalStorage();
  var benutzer = localStorage.getItem("user");
  var array = [];
  var kalender = "kalender";

  if (onlineStatus == "online") {
    // Offline Daten in Online Database speichern
    // readAllDataKalender(onlineStatus, kalender);

    $.ajax({
      url: 'php/load-kalender.php',
      data: 'meine=' + meine + '&station=' + station + '&benutzer=' + benutzer,
      type: "POST",
      success: function (data) {
        array = JSON.parse(data);
        console.log("Laden Kalender erfolgreich");
        loadKalender(array);
        // Daten in IndexedDB hinzufügen
        addAllDataKalender(array, kalender);
      },
      error: function () {
        console.log("Ajax Error beim Laden der Kalender Daten");
      }
    });

    // Offline Daten in Online Database speichern
    readAllDataKalender(onlineStatus, kalender);
  } else if (onlineStatus == "offline") {
    console.log("error, kein Internet load-therapieplan");
    // Daten aus IndexedDB laden wenn User offline ist
    readAllDataKalender(onlineStatus, kalender);
  }
}

function updateDbKalender(title, start, end, benutzer, kategorie, kalender) {
  var offlineDBSQL = "update";
  // Termin in Datenbank speichern
  if (onlineStatus == "online") {
    $.ajax({
      url: 'php/update-termin.php',
      data: 'title=' + title + '&start=' + start + '&end=' + end + '&benutzer=' + benutzer + '&kategorie=' + kategorie,
      type: "POST",
      success: function (data) {
        console.log("Update Therapieplan erfolgreich");
      },
      error: function () {
        console.log("Ajax error, beim update-Termin");
      }
    });
  } else if (onlineStatus == "offline") {
    console.log("error, kein Internet update-termin");
    if (kalender == "kalender") {
      var kategorie = getKalenderKategorie();
    }
    addDataIndexedDBKalender(title, start, end, benutzer, kategorie, offlineDBSQL, kalender);
  }
}


function saveTagebuch(daten) {
  var offlineDBSQL = "insert";
  var array = [];
  // nur ausführen wenn Page: Tagebich Eintrag offen ist, da dort nur der Array gefüllt wird
  if (daten == "onlineDaten") {
    array = getTagebuchEinträge();
  }
  gesamtPunkte(array);
  console.log(array);
  const kategorie = localStorage.getItem("kategorie");
  var zuordnung = localStorage.getItem("Zuordnung");
  var sticker = localStorage.getItem("sticker");
  zuordnung++;
  var benutzer = localStorage.getItem("user");
  var heute = datumHeute();

  if (onlineStatus == "online") {
    if (array.length == 0) {
      console.log("Keine Einträge vorhanden")
    } else {
      //Tagebuch Speichern
      for (var item in array) {
        $.ajax({
          url: 'php/add-tagebuch.php',
          data: 'datum=' + heute + '&zuordnung=' + zuordnung + '&kategorie=' + kategorie + '&idElement=' + array[item].Id + '&bewertung=' + array[item].Bewertung + '&benutzer=' + benutzer + '&sticker=' + sticker,
          type: "POST",
          success: function (data) {
            console.log("Tagebuch erfolgreich gespeichert");
            // console.log(array);
          }
        });
      }
      //Punkte Benutzer Speichern
      savePunkteBenutzer();
      savePunkteVerlaufBenutzer(array);
      location.href = "./tagebuch.html";
    }
  } else if (onlineStatus =="offline") {
    console.log("error, kein Internet save-Tagebuch");
    // Daten in IndexedDB speichern wenn User offline ist
    savePunkteBenutzer();
    savePunkteVerlaufBenutzer(array);
    addDataIndexedDBTagebuchSave(array, heute, zuordnung, kategorie, benutzer, sticker, offlineDBSQL);
  }
}




