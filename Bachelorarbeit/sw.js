const staticCacheName = 'site-static-v2';
const dynamicCacheName = 'site-dynamic';
const assetsCache = [
    '/',
    './index.html',
    './registrieren.html',
    './registrieren_erfolgreich.html',
    './passwort_vergessen.html',
    './passwort_vergessen_geändert.html',
    './impressum.html',
    './kontakt.html',
    './kalender.html',
    './meine_punkte_sticker.html',
    './meine_punkte.html',
    './tagebuch_eintrag_bearbeiten_element.html',
    './tagebuch_eintrag_bewertung.html',
    './tagebuch_eintrag_neues_element.html',
    './tagebuch_eintrag.html',
    './tagebuch_menu.html',
    './tagebuch.html',
    './therapieplan.html',
    './js/app.js',
    './js/sw.js',
    './js/navigation.js',
    './js/array.js',
    './js/db_html_aktualisieren.js',
    './js/db.js',
    './js/jquery-3.5.1.min.js',
    './js/moment.js',
    './js/punkte_banner.js',
    './js/steuerung.js',
    './css/styles.css',
    './font/Bromine.otf',
    './font/Bromine.ttf',
    './bilder/app_icons/app_icon_72x72.png',
    './bilder/app_icons/app_icon_96x96.png',
    './bilder/app_icons/app_icon_128x128.png',
    './bilder/app_icons/app_icon_144x144.png',
    './bilder/app_icons/app_icon_152x152.png',
    './bilder/app_icons/app_icon_192x192.png',
    './bilder/app_icons/app_icon_384x384.png',
    './bilder/app_icons/app_icon_512x512.png',
    './bilder/app_icons/maskable_icon.png',
    './bilder/eintrag/angst.png',
    './bilder/eintrag/bewegung.png',
    './bilder/eintrag/gemuse.png',
    './bilder/eintrag/laune.png',
    './bilder/eintrag/mahlzeit.png',
    './bilder/eintrag/mundspulung.png',
    './bilder/eintrag/obst.png',
    './bilder/eintrag/schlaf.png',
    './bilder/eintrag/schmerz.png',
    './bilder/eintrag/sorgen.png',
    './bilder/eintrag/sport.png',
    './bilder/eintrag/stress.png',
    './bilder/favicon/animated_favicon1.gif',
    './bilder/favicon/favicon.ico',
    './bilder/sticker/elefant.png',
    './bilder/sticker/esel.png',
    './bilder/sticker/frosch.png',
    './bilder/sticker/fuchs.png',
    './bilder/sticker/giraffe.png',
    './bilder/sticker/hase.png',
    './bilder/sticker/hund.png',
    './bilder/sticker/katze.png',
    './bilder/sticker/koala.png',
    './bilder/sticker/kuh.png',
    './bilder/sticker/lowe.png',
    './bilder/sticker/maus.png',
    './bilder/sticker/nilpferd.png',
    './bilder/sticker/otter.png',
    './bilder/sticker/panda.png',
    './bilder/sticker/punguin.png',
    './bilder/sticker/rentier.png',
    './bilder/sticker/schaf.png',
    './bilder/sticker/schwein.png',
    './bilder/sticker/tiger.png',
    './bilder/hintergrund_berge_klein.png',
    './bilder/hintergrund_berge.png',
    './bilder/hintergrund_natur_klein.png',
    './bilder/hintergrund_natur.png',
    './bilder/icon-benutzer.png',
    './bilder/logo_halb.png',
    './bilder/logo.png',
    './bilder/plus.png',
    './bilder/punkt.png',
    './bilder/smiley_gut.png',
    './bilder/smiley_neutral.png',
    './bilder/smiley_schlecht.png',
    './bilder/smiley_sehrgut.png',
    './bilder/smiley_sehrschlecht.png',
    './bilder/stern-weiss.png',
    './bilder/uk-logo.png',
    './fullcalendar/locales/de.js',
    './fullcalendar/drag_and_drop.js',
    './fullcalendar/locales-all.js',
    './fullcalendar/locales-all.min.js',
    './fullcalendar/main.css',
    './fullcalendar/main.js',
    './fullcalendar/main.min.css',
    './fullcalendar/main.min.js',
    './fullcalendar/selectable.js',
    './draganddrop/scripts/drag-and-drop.js',
    './draganddrop/scripts/jquery.dad.js',
    './draganddrop/scripts/jquery.min.js',
    './draganddrop/styles/bootstrap.css',
    './draganddrop/styles/reset.css',
    './draganddrop/styles/styles.css',
    'https://fonts.googleapis.com/icon?family=Material+Icons',
    'https://fonts.gstatic.com/s/materialicons/v47/flUhRq6tzZclQEJ-Vdg-IuiaDsNcIhQ8tQ.woff2',
    './fallback.html'
  ];

// Cache Größe Limit
const limitCacheSize = (name, size) => {
    caches.open(name).then(cache => {
        cache.keys().then(keys => {
            if(keys.length > size) {
                cache.delete(keys[0]).then(limitCacheSize(name, size));
            }
        })
    })
};


// Service Worker event installieren
self.addEventListener('install', evt => {
    // ganzen Dateien in Cache speichern
    evt.waitUntil(
        caches.open(staticCacheName).then((cache) => {
        //console.log(assetsCache);
        cache.addAll(assetsCache);
        })
    );
  });

// service worker aktivieren
self.addEventListener('activate', evt => {
    evt.waitUntil(
        caches.keys().then(keys => {
            // alten Cache löschen
            return Promise.all(keys
                .filter(key => key !== staticCacheName && key !== dynamicCacheName)
                .map(key => caches.delete(key))
                )
        })
    );
});

// Fetch Event
self.addEventListener('fetch', evt => {
    evt.respondWith(
        caches.match(evt.request).then(cacheRes => {
            
            // if (navigator.onLine === true) {
            //     console.log('Sie sind Online');
            //     // offlineDataInOnlineDatabase();
            //     // Wenn der User wieder Internet hat, alle Daten in MySQL Datenbank aktualisieren
            //   } else if (navigator.onLine === false) {
            //     console.log('Sie sind Offline');
            //   }
              
            // gibt den Cache zurück, falls vorhanden
            return cacheRes || fetch(evt.request).then(fetchRes => {
                return caches.open(dynamicCacheName).then(cache => {
                    cache.put(evt.request.url, fetchRes.clone());

                    // Dynamik Cache Größe auf 100 items begrenzen
                    limitCacheSize(dynamicCacheName, 150);
                    return fetchRes;
                
                })
            });
            // Offline seite anzeigen, falls eine Seite nicht im Cache ist
        }).catch(() => {
            console.log('Sie sind Offline, Fallback');
            if(evt.request.url.indexOf('.html') > -1) {
                return caches.match('./fallback.html');
            }
        })
    );
});