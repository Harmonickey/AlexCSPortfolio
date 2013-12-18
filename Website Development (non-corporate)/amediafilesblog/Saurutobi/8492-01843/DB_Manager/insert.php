<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Insert Database</title>
</head>

<body>
</body>
</html>

<!-- When creating a post:
			ID: computer generated (num)
            Username: user generated (alphanum)
            Title: user generated (alphanum)
            Post: user generated (alphanum)
            Spoiler: user generated (alphanum)
            IMDB_URL: user generated (alphanum)
            Type: selection generated (alphanum)
            Report: user generated (alphanum)
            Date: computer generated (datetime)
-->
<?php
$mysql_host = "mysql10.000webhost.com";
$mysql_database = "a9341052_lbkdb";
$mysql_user = "a9341052_lbk";
$mysql_password = "men1992";

$con=mysqli_connect("$mysql_host","$mysql_user","$mysql_password","$mysql_database");
// Check connection
if (mysqli_connect_errno())
{
	echo "Failed to connect to MySQL: " . mysqli_connect_error();
}
else
{
	echo "Connected to MySQL\n";	
}

$title = $_POST[title];
if (!isset($_POST[title]))
	$title = $_REQUEST[title];
	
$username = 'marcel_englmaier';

$post = $_POST[post];
if (!isset($_POST[post]))
	$post = $_REQUEST[post];
	
$spoiler = $_POST[spoiler];
if (!isset($_POST[spoiler]))
	$spoiler = $_REQUEST[spoiler];
	
$mediatype = $_POST[mediatype];
if (!isset($_POST[mediatype]))
	$mediatype = $_REQUEST[mediatype];
	
$report = $_POST[Report];
if (!isset($_POST[Report]))
	$report = $_REQUEST[Report];
	
$IMDB_URL = $_POST[IMDB_URL];
if (!isset($_POST[IMDB_URL]))
	$IMDB_URL = $_REQUEST[IMDB_URL];

//insert into database with full column data
$sql="INSERT INTO BlogPosts (Username, Title, Post, Spoiler, IMDB, MediaType, Report, Date) VALUES ('$username', '$title', '$post', '$spoiler', '$IMDB_URL', '$mediatype', '$report', sysdate())";

echo "<br>";

echo $sql;

echo "<br>";

if (!mysqli_query($con,$sql))
{
	die('Error: ' . mysqli_error($con));
}
echo "1 record added";

mysqli_close($con);
?>