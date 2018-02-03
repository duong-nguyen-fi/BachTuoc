<?php
	$servername = "localhost";
	$username = "id2789640_btuoc_user";
	$password = "111111";
	$dbname = "id2789640_btuoc_db";

	
	$conn = new mysqli($servername,$username,$password,$dbname);

	
	$sql ="SELECT  `all_tables`.`num` ,  `current_table`.`all_tables_id` 
FROM  `current_table` ,  `all_tables` 
WHERE  `current_table`.`all_tables_id` =  `all_tables`.`id` ";
	
	

	 $result = $conn->query($sql);
	 //echo "COnnect success";
	// $data = array();
	// //$data[] = array('id' -> $result['id'], 'num' -> $result['num'], 'total' -> $result['total'] );
	 $data = $result->fetch_all(MYSQLI_NUM);
	
	
	echo json_encode($data)
;	//setTimeout(delay(),500);

?>