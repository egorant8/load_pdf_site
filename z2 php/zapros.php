<?
include('config.php');
$unik = $_POST['unik'];
$fam = $_POST['fam'];
$key = $_POST['skey'];
if($key == 'qwe123')
{
	$link->query("INSERT INTO `a_eng_qr_app` (`id`, `fam`, `unik`) VALUES (NULL, '".$fam."', '".$unik."');");
	echo 'TRUE|ADD';
}
else
{
	echo 'FALSE|ERROR KEY';
}
?>