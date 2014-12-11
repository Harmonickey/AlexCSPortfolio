<div id="left2">
	<h1>Exec Board</h1>

<?php
include '/home/users/web/b277/pow.shepardadmin/htdocs/mysql_credentials.php';
$link = mysql_connect($host, $user, $password)
	or die('Failed to connect to database');
mysql_select_db($db)
	or die ('Failed to select database');
$query = 'SELECT * FROM exec';
$result = mysql_query($query)
	or die('Failed to execute query');

while ($row = mysql_fetch_array($result)){
	?>
	<div class="profile">
	<img src="<?php echo $html_htdocs . 'images/profiles/' . $row['image'] . '.' . $row['ImgType'] . '"';?> height="200" />
	<name><?php echo $row['FirstName'] . ' ' . $row['LastName']; ?></name>
	<p><?php echo $row['Position'];?><br />
	<?php echo $row['Status'] . ', ' . $row['Major']; ?><br />
	<?php echo $row['Email']; ?></p>
	<p><i><?php echo $row['Description']; ?></i></p>
	<div class="aspacer"></div>
</div>
<p><?php echo $row['About']; ?></p>
 <?php }
?>

</div>
<div id="right2">
<h5>What is the Exec Board?</h5>
<p>The Exec Board is a panel of students that plans events for current residents, alumni, and faculty fellows. They organize most of the programming that goes on during the year, including major events like the Shepard Formal.</p>

<h5>How do I meet these people?</h5>
<p>Go talk to them in their rooms! They are also happy to talk if you email them questions about Northwestern or Shepard.</p>

<h5>How do I run for exec?</h5>
<p>See our <a href="policies">Policies</a>.</p>

</div>