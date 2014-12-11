<?php
//include_once("db.php");
include_once("function.php");

global $wpdb;
$itable		=	$wpdb->prefix."html5mp3_items";

if(isset($_REQUEST['id']) && $_REQUEST['id']!="")
 $id = $_REQUEST['id'];
 

$qq = mysql_query("select * from `".$table."` where id = '".$id."' ");

$docdata = mysql_fetch_assoc($qq);
 

$url = isset($_REQUEST['url'])?$_REQUEST['url']:"";

$pimage = isset($_REQUEST['pimage'])?$_REQUEST['pimage']:"";

$ptitle = isset($_REQUEST['ptitle'])?$_REQUEST['ptitle']:"";
$partist = isset($_REQUEST['partist'])?$_REQUEST['partist']:"";

$description = isset($_REQUEST['description'])?$_REQUEST['description']:"";

$facebook = isset($_REQUEST['facebook'])?$_REQUEST['facebook']:"";
$twitter = isset($_REQUEST['twitter'])?$_REQUEST['twitter']:"";
$link = isset($_REQUEST['link'])?$_REQUEST['link']:"";

$sandbox = isset($_REQUEST['sandbox'])?$_REQUEST['sandbox']:"0";
$email = isset($_REQUEST['email'])?$_REQUEST['email']:"";

$brcolor = isset($_REQUEST['brcolor'])?$_REQUEST['brcolor']:"";
$bgcolor = isset($_REQUEST['bgcolor'])?$_REQUEST['bgcolor']:"";
$width = isset($_REQUEST['width'])?$_REQUEST['width']:"";
$height = isset($_REQUEST['height'])?$_REQUEST['height']:"";
$popout = isset($_REQUEST['popout'])?$_REQUEST['popout']:"0";
$shuffle = isset($_REQUEST['shuffle'])?$_REQUEST['shuffle']:"0";

$size = isset($_REQUEST['size'])?$_REQUEST['size']:"";
$autoplay = isset($_REQUEST['autoplay'])?$_REQUEST['autoplay']:"0";

$clientid = isset($_REQUEST['clientid'])?$_REQUEST['clientid']:"";
$clientsecret = isset($_REQUEST['clientsecret'])?$_REQUEST['clientsecret']:"";

$embed = isset($_REQUEST['embed'])?$_REQUEST['embed']:"0";

$playlist = isset($_REQUEST['playlist'])?$_REQUEST['playlist']:"0";
$scrollingtext = isset($_REQUEST['scrollingtext'])?$_REQUEST['scrollingtext']:"0";

$toggleplaylist = isset($_REQUEST['toggleplaylist'])?$_REQUEST['toggleplaylist']:"0";

$sourcetype = isset($_REQUEST['sourcetype'])?$_REQUEST['sourcetype']:"default";
$sourceurl = isset($_REQUEST['sourceurl'])?$_REQUEST['sourceurl']:"";
$sourcedownload = isset($_REQUEST['sourcedownload'])?$_REQUEST['sourcedownload']:"0";



$xml = isset($_REQUEST['xml'])?$_REQUEST['xml']:"";
$id = isset($_REQUEST['id'])?$_REQUEST['id']:"";
$host = isset($_REQUEST['host'])?$_REQUEST['host']:"";


if(isset($_REQUEST['submit']) && $_REQUEST['submit']=="Save Playlist")
{

$c = count($_REQUEST['song']);

/*$xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n";

$xml .= "<list>\r\n";*/

//print_r($_REQUEST['song']);
//die();

$maxfields = count($_REQUEST['title']);

$selectboxes = $_REQUEST['download'];
$selectbuys = $_REQUEST['buynow'];

for($i = 0; $i < $maxfields; $i++)
{
  
  if(!isset($selectboxes[$i])) $selectboxes[$i] = 0;
  if(!isset($selectbuys[$i])) $selectbuys[$i] = 0;

}


/*for($i=0;$i<$c;$i++)
{

  $xml .= "\r\n<item>\r\n";

    $xml .= "<title><![CDATA[".$_REQUEST['title'][$i]."]]></title>\r\n";

	$xml .= "<artist><![CDATA[".$_REQUEST['artist'][$i]."]]></artist>\r\n";

	$xml .= "<song><![CDATA[".$_REQUEST['song'][$i]."]]></song>\r\n";

    $xml .= "<image><![CDATA[".$_REQUEST['image'][$i]."]]></image>\r\n";
	
	$xml .= "<download>".$selectboxes[$i]."</download>\r\n";

  $xml .= "</item>\r\n\r\n";

}
	
$xml .= "</list>\r\n";


if(isset($docdata['xml']))
file_put_contents(plugin_dir_path(__FILE__)."../xml/".$docdata['xml'], PHP_slashes($xml, "strip")); */

$xml = 1;

$ierror = "Please save playlist using Save Playlist button ";


}


if(isset($xml) && $xml!="")
{


$qq = mysql_query("select * from `".$table."` where id = '".$id."' ");

$docdata = mysql_fetch_assoc($qq);


	if(isset($docdata['id']) && $docdata['id']!="")
	{
	
	  //die("update `xml` set `url` = '".$url."', `size` = '".$size."', `xml` = '".base64_encode($xml)."', `adddate` = now() where id = '".$docdata['id']."' ");
	  
	  $xxmmll = "";
	  
	  /*$xxmmll = $docdata['xml'];
	  
	  if($xxmmll=="") $xxmmll = time().".xml";
	  
      file_put_contents(plugin_dir_path(__FILE__)."/../xml/".$xxmmll, PHP_slashes($xml, "strip"));*/
	  
	  
	  mysql_query("update `".$table."` set `title` = '".$ptitle."', `artist` = '".$partist."', `description` = '".$description."', `url` = '".$url."', `image` = '".$pimage."', `facebook` = '".$facebook."', `twitter` = '".$twitter."', `link` = '".$link."', `size` = '".$size."', `email` = '".$email."', `brcolor` = '".$brcolor."', `bgcolor` = '".$bgcolor."', `width` = '".$width."', `height` = '".$height."', `popout` = '".$popout."', `autoplay` = '".$autoplay."', `shuffle` = '".$shuffle."', `embed` = '".$embed."', `playlist` = '".$playlist."', `scrollingtext`='".$scrollingtext."', `toggleplaylist` = '".$toggleplaylist."', `sourcetype` = '".$sourcetype."', `sourceurl` = '".$sourceurl."', `sourcedownload` = '".$sourcedownload."', `clientid` = '".$clientid."', `clientsecret` = '".$clientsecret."', `xml` = '".$xxmmll."', `sandbox` = '".$sandbox."', `adddate` = now() where id = '".$docdata['id']."' ");
	  
	  $iiid = $docdata['id'];
	
	}
	else
	{
	
	   $xxmmll = "";
	
	  /*$xxmmll = time().".xml";

      file_put_contents(plugin_dir_path(__FILE__)."/../xml/".$xxmmll, PHP_slashes($xml, "strip"));*/
	
	  //die("insert into `".$table."` set  `title` = '".$ptitle."', `artist` = '".$partist."', `description` = '".$description."', `url` = '".$url."', `image` = '".$pimage."', `facebook` = '".$facebook."', `twitter` = '".$twitter."', `link` = '".$link."', `size` = '".$size."', `email` = '".$email."', `brcolor` = '".$brcolor."', `bgcolor` = '".$bgcolor."', `width` = '".$width."', `height` = '".$height."', `popout` = '".$popout."', `autoplay` = '".$autoplay."', `shuffle` = '".$shuffle."', `embed` = '".$embed."', `playlist` = '".$playlist."', `scrollingtext`='".$scrollingtext."', `toggleplaylist` = '".$toggleplaylist."', `sourcetype` = '".$sourcetype."', `sourceurl` = '".$sourceurl."', `sourcedownload` = '".$sourcedownload."', `clientid` = '".$clientid."', `clientsecret` = '".$clientsecret."', `xml` = '".$xxmmll."', `sandbox` = '".$sandbox."', `adddate` = now() ");
	
	  mysql_query("insert into `".$table."` set  `title` = '".$ptitle."', `artist` = '".$partist."', `description` = '".$description."', `url` = '".$url."', `image` = '".$pimage."', `facebook` = '".$facebook."', `twitter` = '".$twitter."', `link` = '".$link."', `size` = '".$size."', `email` = '".$email."', `brcolor` = '".$brcolor."', `bgcolor` = '".$bgcolor."', `width` = '".$width."', `height` = '".$height."', `popout` = '".$popout."', `autoplay` = '".$autoplay."', `shuffle` = '".$shuffle."', `embed` = '".$embed."', `playlist` = '".$playlist."', `scrollingtext`='".$scrollingtext."', `toggleplaylist` = '".$toggleplaylist."', `sourcetype` = '".$sourcetype."', `sourceurl` = '".$sourceurl."', `sourcedownload` = '".$sourcedownload."', `clientid` = '".$clientid."', `clientsecret` = '".$clientsecret."', `xml` = '".$xxmmll."', `sandbox` = '".$sandbox."', `adddate` = now() ");
	  
	  $iiid = mysql_insert_id();	
	
	}
	
	
	
	//// Remove old data ....
	
	mysql_query("delete from `".$itable."` where `pid` = '".$iiid."' ");
	
	
	/// save to item table
	
	for($i=0;$i<$c;$i++)
	{
	
	
	$item_id = $_REQUEST['item_id'][$i];
	
	$qqqq = mysql_query("select id from `".$itable."` where pid = '".$iiid."' and id = '".$item_id."'  ");

    $docta = mysql_fetch_assoc($qqqq);
	
	$title = $_REQUEST['title'][$i];
	$artist = $_REQUEST['artist'][$i];
	$preview = $_REQUEST['preview'][$i];
	$amount = $_REQUEST['price'][$i];
	$song = $_REQUEST['song'][$i];
	$image = $_REQUEST['image'][$i];
	$download = $selectboxes[$i];
	$buynow = $selectbuys[$i];
	
	
	////All Time Insert
	
	 mysql_query("insert into `".$itable."` set  `pid` = '".$iiid."', `title` = '".$title."', `artist` = '".$artist."', `preview` = '".$preview."', `amount` = '".$amount."', `song` = '".$song."', `image` = '".$image."', `download` = '".$download."', `buynow` = '".$buynow."', `adddate` = now() ");
	
	
/*	if(isset($docta['id']) && $docta['id']!="")
	{
	
	  mysql_query("update `".$itable."` set `pid` = '".$iiid."', `title` = '".$title."', `artist` = '".$artist."', `preview` = '".$preview."', `amount` = '".$amount."', `song` = '".$song."', `image` = '".$image."', `download` = '".$download."', `buynow` = '".$buynow."', `adddate` = now() where id = '".$docta['id']."' ");
	  	
	}
	else
	{
	
	  mysql_query("insert into `".$itable."` set  `pid` = '".$iiid."', `title` = '".$title."', `artist` = '".$artist."', `preview` = '".$preview."', `amount` = '".$amount."', `song` = '".$song."', `image` = '".$image."', `download` = '".$download."', `buynow` = '".$buynow."', `adddate` = now() ");
	  	
	}*/
	
	
	
	}
	
	
	
	?>
    
    <script language="javascript">
    
	document.location = "<?php echo get_bloginfo('url')."/wp-admin/admin.php?page=html5mp3_playlist&isuccess=Playlist saved successfully"; ?>";
	
	</script>
    
    <?php
	
	
 	
    //wp_redirect(get_bloginfo('url')."/wp-admin/admin.php?page=html5mp3_add_playlist",301); exit; 
	
	$isuccess = "Playlist saved successfully ";

}
else
{

   $ierror = "Something went wrong... navigate back";

}

?>