var onlineStatus = "";

if('serviceWorker' in navigator){
  
  if (navigator.onLine === true) {
    var user = localStorage.getItem("user");
    console.log('App, Sie sind Online');
    onlineStatus = "online";
    // Wenn der User wieder Internet hat, alle Daten in MySQL Datenbank aktualisieren
  } else if (navigator.onLine === false) {
    onlineStatus = "offline";
    console.log('App, Sie sind Offline');
  }
    navigator.serviceWorker.register('./sw.js')
      .then(reg => console.log('service worker registriert', reg))
      .catch(err => console.log('service worker nicht registriert', err));
  }
