<?php
include('config.php');
$sql = "SELECT count(id) as 'id_co' FROM `a_eng_qr_app`";
if ($result = $link->query($sql))
{
	while($obj = $result->fetch_object())
	{
		$fam=$obj->id_co;
		echo $fam;
	}
}

?>