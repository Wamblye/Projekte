<?php
  require_once("./config.php");


  $benutzer = isset($_POST['benutzer']) ? $_POST['benutzer'] : "";
  $kategorieHaupt = isset($_POST['kategorieHaupt']) ? $_POST['kategorieHaupt'] : "";

  if ($kategorieHaupt == "Alle") {
    // Query Schreiben für alle Daten in Liste
    $sql = "SELECT tagebuch.Id_Tagebuch, tagebuch.Zuordnung, tagebuch.Datum, tagebuch.Kategorie, tagebuch_elemente.Name, bewertung.Img, tagebuch.FK_Benutzer, tagebuch.Sticker
          FROM tagebuch
          LEFT JOIN tagebuch_elemente ON tagebuch.FK_Elemente = tagebuch_elemente.Id_Elemente
          LEFT JOIN bewertung ON tagebuch.FK_Bewertung = bewertung.Id_Bewertung
          WHERE tagebuch.FK_Benutzer='$benutzer'
          ORDER BY tagebuch.Datum desc, tagebuch.Kategorie";
  } else {
    // Query Schreiben für alle Daten in Liste
    $sql = "SELECT tagebuch.Id_Tagebuch, tagebuch.Zuordnung, tagebuch.Datum, tagebuch.Kategorie, tagebuch_elemente.Name, bewertung.Img, tagebuch.FK_Benutzer, tagebuch.Sticker
          FROM tagebuch
          LEFT JOIN tagebuch_elemente ON tagebuch.FK_Elemente = tagebuch_elemente.Id_Elemente
          LEFT JOIN bewertung ON tagebuch.FK_Bewertung = bewertung.Id_Bewertung
          WHERE tagebuch.FK_Benutzer='$benutzer' AND tagebuch.Kategorie='$kategorieHaupt'
          ORDER BY tagebuch.Datum desc, tagebuch.Kategorie";
  } 




  // Mach Query und hole die Infos
  $result = mysqli_query($conn, $sql);
              // results in Array
  $listen = mysqli_fetch_all($result, MYSQLI_ASSOC);
  // result aus dem Speicher löschen
  mysqli_free_result($result);
  // Verbindung schließen
  mysqli_close($conn);

  // Variable zurückgeben
  echo json_encode($listen);
?>