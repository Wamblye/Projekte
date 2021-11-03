<?php
  require_once("./config.php");

  $kategorie = isset($_POST['kategorie']) ? $_POST['kategorie'] : "";
  $benutzer = isset($_POST['benutzer']) ? $_POST['benutzer'] : "";
  $id = isset($_POST['id']) ? $_POST['id'] : "";

  $sqlDelete = "DELETE from tagebuch_elemente WHERE Id_Elemente='$id'";

  $result = mysqli_query($conn, $sqlDelete);

  if (! $result) {
      $result = mysqli_error($conn);
  }

  // Verbindung schließen
  mysqli_close($conn);
?>
