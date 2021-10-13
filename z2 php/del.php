<?
include('config.php');
$id = $_GET['id'];
$link->query("DELETE FROM `a_eng_qr_app` WHERE `a_eng_qr_app`.`id` = ".$id);
?>