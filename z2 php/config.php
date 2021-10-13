<?
$link = new mysqli("localhost", "gorpol1_epid", "Qwe123!2","gorpol1_epid");
if ($link->connect_error) {
  die("Connection failed: " . $link->connect_error);
}
?>