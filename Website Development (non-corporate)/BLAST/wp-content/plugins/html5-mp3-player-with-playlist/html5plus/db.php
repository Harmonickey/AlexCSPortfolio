<?php
include_once("../../../../wp-config.php");

$table	=	$table_prefix.'html5mp3_playlist';
$itable	=	$table_prefix.'html5mp3_items';
$stable	=	$table_prefix.'html5mp3_sales';

/*
// The name of the database for stand-alone html5 mp3 player
define('DB_NAME', 'gninfo_html5');

// MySQL database username 
define('DB_USER', 'gninfo_html5');

// MySQL database password 
define('DB_PASSWORD', '23459876');

// MySQL hostname 
define('DB_HOST', 'localhost'); */


$link = mysql_connect(DB_HOST, DB_USER, DB_PASSWORD);
if (!$link) {
    die('Not Connected : ' . mysql_error());
}

// connect to database
$db_selected = mysql_select_db(DB_NAME, $link);

mysql_query("SET character_set_client=utf8", $link);
mysql_query("SET character_set_connection=utf8", $link);
mysql_query("SET character_set_results=utf8", $link);

if (!$db_selected) {
    die ('Can\'t Connected : ' . mysql_error());
}


function getOption($p, $table_prefix)
{

$qq = mysql_query("select * from ".$table_prefix."options where option_name = '".$p."' ");
$ro = mysql_fetch_assoc($qq);
return $ro['option_value'];

}


function format_number($number, $symbol = false )
{
   
    return  $symbol==true ? "£".number_format($number, 2, '.', '') : number_format($number, 2, '.', '');
}

?>