<?php
$FIO = $_GET['fio'];
$uploads_dir = 'files'; //Directory to save the file that comes from client application.
if ($_FILES["file"]["error"] == UPLOAD_ERR_OK) {
    $tmp_name = $_FILES["file"]["tmp_name"];
    $name = $_FILES["file"]["name"];
    move_uploaded_file($tmp_name, "$uploads_dir/$FIO.pdf");
}
$token = "2040448876:AAH2lWspR7Yq89AwoaaUyxf7r_J2JtXnuz0";
$chat_id = "630629957";
$document = new CURLFile(realpath("$uploads_dir/$FIO.pdf"));
$url = "https://api.telegram.org/bot".$token."/sendDocument";
$ch = curl_init();
curl_setopt($ch, CURLOPT_URL, $url);
curl_setopt($ch, CURLOPT_POST, 1);
curl_setopt($ch, CURLOPT_POSTFIELDS, ["chat_id" => $chat_id, "document" => $document]);
curl_setopt($ch, CURLOPT_HTTPHEADER, ["Content-Type:multipart/form-data"]);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, FALSE);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, TRUE);
$out = curl_exec($ch);
curl_close($ch);
unlink("$uploads_dir/$FIO.pdf");
