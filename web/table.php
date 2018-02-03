<!DOCTYPE html>
<html>
<head>
<style>
table {
    width: 100%;
    height: 40%;

    border-collapse: collapse;
}

table, td, th {
    border: 1px solid black;
    padding: 5px;
    font-size: 50px;

}

th {text-align: left;}
</style>
</head>
<body>

<?php
	$servername = "localhost";
	$username = "nzuong_nzuong";
	$password = "tyntyn";
	$dbname = "nzuong_btuoc";

	$id = intval($_GET['id']);
	$num = intval($_GET['num']);
	
	$conn = new mysqli($servername,$username,$password,$dbname);

	
	$sql ="SELECT `name`, `quantity`, `price`, `datetime`, `served` FROM  `Order` WHERE `all_tables_id`= " .$id ." ORDER BY `datetime`  LIMIT 0,30 ";
	
	$result = $conn->query($sql);
	
	
	
	echo "<table>
	<tr>
	<th>Ten</th>
	<th>So Luong</th>
	<th>Thanh Tien</th>
	<th>Ngay Gio</th>
	<th>Ra Mon</th>
	</tr>";
	while($row = mysqli_fetch_array($result)) {
	    echo "<tr>";
	    echo "<td>" . $row['name'] . "</td>";
	    echo "<td>" . $row['quantity'] . "</td>";
	    echo "<td>" . $row['price'] . "</td>";
	    $date = new DateTime($row['datetime']);
	    echo "<td>" . $date->format('H:m:s') . "</td>";
	    //echo "<td>" . $row['datetime']. "</td>";
	    if ($row['served'] =="1")
	    	$served ="x";
	    else
	    	$served = " ";

	    echo "<td>" .  $served . "</td>";
	    echo "</tr>";
	}
	
	
	$sql ="SELECT  `total` FROM  `all_tables` WHERE id = ".$id ;
	$result = $conn->query($sql);
	while($row = mysqli_fetch_array($result)) {
		echo "<tr>";
		$total = $row['total'];
		echo "<td> Tong Cong </td>";
		echo "<td>  </td>";
		echo "<td>" .  $total. "</td>";
		echo "<td>  </td>";
		echo "<td> </td>";
	    echo "</tr>";
	}
	echo "</table>";
	mysqli_close($conn);
?>
</body>
</html>