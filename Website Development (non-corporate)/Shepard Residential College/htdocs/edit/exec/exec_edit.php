<?php 
include 'HTTP_auth.php';
?>

<html>
<body>
<a href="../">Edit</a> &nbsp; &gt; &nbsp; <a href="exec_menu.php">Select Position</a>&nbsp; &gt; &nbsp; Current Page<br><br><br>
<?php
//mysql creds
include '../../mysql_credentials.php';
//make sure something was submitted
if($_POST){
	//removing any html braces or other bad characters
	function replace_char($string, $pairings){
		//first add slashes
		$string2 = addslashes($string);
		//now replace bad characters
		//loop length = array length
		$loop_length = sizeof($pairings);
		for ($i=0; $i < $loop_length; $i=$i+2) {
			$string2 = str_replace($pairings[$i],$pairings[$i+1],$string2);
		}
		//Capitalize and return
		return ucfirst($string2);
	}
	//pairings of the form : array(bad character 1, good character 1, bad 2, good 2, etc...)
	$pairings=array(
		//'<','&lt;',
		//'>','&gt;'
	);
	//naming variables
	$Position = replace_char($_POST['Position'],$pairings);
	$FirstName = replace_char($_POST['FirstName'],$pairings);
	$LastName = replace_char($_POST['LastName'],$pairings);
	$Major = replace_char($_POST['Major'],$pairings);
	$Description = replace_char($_POST['Description'],$pairings);
	$About = replace_char($_POST['About'],$pairings);
	//whether we include img ext in querry
	if ($_FILES['profilepic']['size']){ //if the image is size 0, assume none was uploaded (prevents bugs in submission)
		//find out extension
		$temp = explode('.',$_FILES['profilepic']['name']);
		$ImgType = end($temp);
		//see if allowed
		$allowed_ext = array('gif', 'jpeg', 'jpg', 'png');
		if(in_array($ImgType, $allowed_ext)){
			$ImgQuery = ", ImgType='$ImgType'";
		}
		//or else don't upload
		else{
			echo '<br>Image type not supported; Image upload cancelled';
		}
	}
	//if imgQuery doesnt' exist, make it 0
	if(!isset($ImgQuery)){
		$ImgQuery = '';
	}
	$database = mysqli_connect($host, $user, $password, $db)
		or die('Failed to connect to database');
	$query = "UPDATE exec SET FirstName='$FirstName', LastName='$LastName', Major='$Major', Description='$Description', About='$About'" . $ImgQuery . " WHERE Position='$Position'";
	$result = mysqli_query($database, $query)
		or die('Failed to execute query');
	echo 'Data uploaded successfully to database';
	unset($query);
	unset($result);
	
	//upload the image
	if ($_FILES['profilepic']){
		if (!$ImgQuery){
			exit;
		}
		if (!$_FILES['profilepic']['size']){ //if the image is size 0, assume none was uploaded (prevents bugs in submission)
			echo '<br> No image upload detected';
			exit;
		}
		//checking for errors
		if($_FILES['profilepic']['error']){
			echo '<br> Sorry, but an error occurred in uploading your photo';
			exit;
		}
		//Now to save profile photo
		$query = "SELECT * FROM exec WHERE Position='$Position'";
		$result = mysqli_query($database, $query)
			or die ('<br> Failed to connect to database for file upload');
		include '../../htdoc_dir.php';
		//come up with new name/dir
		while ($row = mysqli_fetch_array($result)){
			$upload_name = $php_htdocs . 'images/profiles/' . $row['image'] . '.' . $row['ImgType'];
		}
		$success = move_uploaded_file($_FILES['profilepic']['tmp_name'],$upload_name);
		if (!$success){
			echo '<br> Upload Failed';
			exit;
		}
		else {
			echo '<br> Upload Successful';
		}
		//set permissions
		chmod($upload_name, 0644);
	}
	mysqli_close($database);
}
else {
	//if nothing was selected to edit
	if(!$_GET){
		echo 'You must first select a position to edit <a href="exec_edit_1.php">here</a>';
		exit;
	}
	elseif(!$_GET['position']){
		echo 'You must first select a position to edit <a href="exec_edit_1.php">here</a>';
		exit;
	}
	//now we edit a user
	else{
		include '../../mysql_credentials.php';
		$database = mysqli_connect($host, $user, $password, $db)
			or die('Failed to connect to database');
		$query = 'SELECT * FROM exec WHERE position=\'' . $_GET['position'] . '\'';
		$result = mysqli_query($database, $query)
			or die('Failed to execute query');
		while ($row = mysqli_fetch_array($result)){
	?>

<form enctype="multipart/form-data" action="" method="post">
<b>Position:</b> <input type="text" name="Position" value="<?php echo $row['Position']; ?>" readonly style="color:gray"><br><br>

<b>First Name:</b> <input type="text" name="FirstName" value="<?php echo $row['FirstName']; ?>"><br><br>

<b>Last Name:</b> <input type="text" name="LastName" value="<?php echo $row['LastName']; ?>"><br><br>

<b>Major:</b> <input type="text" name="Major" value="<?php echo $row['Major']; ?>"><br><br>

<b>Role Description:</b> <br> 
<textarea name="Description" style="width:45em; height:8em"><?php echo $row['Description'] ?></textarea><br><br>

<b>About Me:</b> <br>
<textarea name="About" style="width:45em; height:16em"><?php echo $row['About'] ?></textarea><br><br>

<!-- MAX_FILE_SIZE must precede the file input field -->
<input type="hidden" name="MAX_FILE_SIZE" value="2000000" />
<b>Upload New Profile Pic</b> <br> Be advised, uploading a new pic will delete the old pic. <br>Upload size <b>&lt;</b> 2mb; only jpeg, jpb, gif and png allowed <br>
<input name="profilepic" type="file" value=""/><br><br>
<input type="submit" value="Submit">
</form>
<?php }
	mysqli_close($database);
	}
}
?>
</body>
</html>