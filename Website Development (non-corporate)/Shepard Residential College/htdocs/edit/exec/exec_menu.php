<?php 
include 'HTTP_auth.php';
?>

<html>
	<head>
	</head>
	<body>
	<a href="../">Edit</a> &nbsp; &gt; &nbsp; Current Page<br>
	<h1>Please select position you wish to edit</h1>
		<form name="Position" action="exec_edit.php" method="get">
<?php
include '../../mysql_credentials.php';
$database = mysqli_connect($host, $user, $password, $db)
	or die ('Failed to connect to database');
$query = 'SELECT * FROM exec';
$result = mysqli_query($database, $query)
	or die ('Failed to execute query');

while ($row = mysqli_fetch_array($result)){
	?><input type="radio" name="position" value="<?php echo $row['Position']?>"><?php echo ucfirst($row['Position'])?></br><?php
}

mysqli_close($database);
?>			
			<input type="submit" value="Edit">
		</form>
	</body>
</html>