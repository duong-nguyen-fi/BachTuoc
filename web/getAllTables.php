<?php
	$servername = "localhost";
	$username = "nzuong_nzuong";
	$password = "tyntyn";
	$dbname = "nzuong_btuoc";

	
	$conn = new mysqli($servername,$username,$password,$dbname);

	
	$sql ="SELECT  `all_tables`.`num` ,  `current_table`.`all_tables_id` 
FROM  `current_table` ,  `all_tables` 
WHERE  `current_table`.`all_tables_id` =  `all_tables`.`id` ";
	
	$result = $conn->query($sql);
	$data = array();
	//$data[] = array('id' -> $result['id'], 'num' -> $result['num'], 'total' -> $result['total'] );
	$data = $result->fetch_all(MYSQL_ASSOC);
	
	
	echo json_encode($data)
;	//setTimeout(delay(),500);

?>