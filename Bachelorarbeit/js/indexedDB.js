//prefixes of implementation that we want to test
window.indexedDB = window.indexedDB || window.mozIndexedDB || 
window.webkitIndexedDB || window.msIndexedDB;

//prefixes of window.IDB objects
window.IDBTransaction = window.IDBTransaction || 
window.webkitIDBTransaction || window.msIDBTransaction;
window.IDBKeyRange = window.IDBKeyRange || window.webkitIDBKeyRange || 
window.msIDBKeyRange

if (!window.indexedDB) {
   window.alert("Your browser doesn't support a stable version of IndexedDB.")
}

var db;
var request = window.indexedDB.open("kinder-onkologie", 1);

request.onerror = function(event) {
   console.log("error: ");
};

request.onsuccess = function(event) {
   db = request.result;
};

request.onupgradeneeded = function(event) {
   var db = event.target.result;
   var objectStoreTherapieplan = db.createObjectStore("therapieplan", {keyPath: "id"});
   var objectStoreTagebuch = db.createObjectStore("tagebuch", {keyPath: "id_tagebuch"});
   objectStoreTagebuch.createIndex("zuordnung", "zuordnung", { unique: false });
}

/* ------------------------------ Therapieplan und Kalender -------------------------------------*/
function readAllDataKalender(onlineStatus, kalender) {
   var objectStore = db.transaction(kalender).objectStore(kalender);
   var array = [];
   
   objectStore.openCursor().onsuccess = function(event) {
      var cursor = event.target.result;
      
      if (cursor) {
        // console.log("id: " + cursor.key + " title: " + cursor.value.title + ", start: " + cursor.value.start + ", end: " + cursor.value.end);
        array.push({ "id":cursor.key, "title":cursor.value.title, "start":cursor.value.start, "end":cursor.value.end,
                    "benutzer":cursor.value.benutzer, "kategorie":cursor.value.kategorie, "database":cursor.value.database });
         cursor.continue();
      } else {
        console.log("Laden aus IndexedDB erfolgreich");
         if(onlineStatus == "online") {
            for (var i in array) {
                if (array[i].database == "insert") {
                   console.log("update");
                  addDbKalender(array[i].title, array[i].start, array[i].end, array[i].benutzer, array[i].kategorie);
                } else if (array[i].database == "update") {
                  updateDbKalender(array[i].title, array[i].start, array[i].end, array[i].benutzer, array[i].kategorie);
                } else if (array[i].database == "delete") {
                  if (kalender == "therapieplan") {
                     deleteDbKalender(array[i].title, array[i].start, array[i].end, array[i].benutzer, array[i].kategorie);
                  } else if (kalender == "kalender") {
                     deleteOhneKategorieDbKalender(array[i].title, array[i].start, array[i].end, array[i].benutzer);
                  }
                }
             }
             removeAllDataDatabase(kalender);
         } else if (onlineStatus == "offline") {
            if (kalender == "therapieplan") {
               loadTherapieplan(array);
            } else if (kalender == "kalender") {
               loadKalender(array);
            }
         }
      }
   };
}

/* ---------------------------------------------- Tagebuch Elemente -----------------------------------------------------*/
// Alle Daten aus der MySQL Datenbank 
function addAllDataTagebuchElemente(array) {
   var database = "tagebuch_elemente";
   var request = "";

     for (var i in array) {
        request = db.transaction([database], "readwrite")
        .objectStore(database)
        .add({ Id_Elemente: array[i].Id_Elemente, id: array[i].Id_Elemente, Name: array[i].Name, Img: array[i].Img, Punkte: array[i].Punkte, 
            Kategorie: array[i].Kategorie, database: "no" }, array[i].Id_Elemente);
     }


  request.onsuccess = function(event) {
   console.log("Tagebuch Elemente Daten wurden der IndexedDB Datenbank hinzugefügt.");
  };
  
  request.onerror = function(event) {
   console.log("Tagebuch Elemente Daten konnten nicht der IndexedDB Datenbank hinzugefügt werden");
  }
}


function removeDataIndexedDBTagebuchElement(idElemente) {
   var database = "tagebuch_elemente";
   var singleKeyRange = IDBKeyRange.only(idElemente);
   var objectStore = db.transaction([database], "readwrite").objectStore(database);
   var index = objectStore.index("Id_Elemente");

   index.openCursor(singleKeyRange).onsuccess = function(event) {
      var cursor = event.target.result;
      if (cursor) {
      cursor.delete();
        cursor.continue();
      }
    };
   
   request.onsuccess = function(event) {
    console.log("Tagebuch Element wurde erfolgreich aus IndexedDB entfernt");
   };
}

/* -------------------------------------------------- Allgemein-------------------------------------------------------*/

function removeAllDataDatabase(database) {
   var request = db.transaction([database], "readwrite")
      .objectStore(database)
      .clear();

   request.onsuccess = function(event) {
   console.log(database + " Database IndexedDB erfolgreich gelöscht");
   };
}

function randomID() {
   var min = 1000;
   var max = 10000;
   var id = Math.round((Math.random() * (max - min))) + min;

   return id;
}
