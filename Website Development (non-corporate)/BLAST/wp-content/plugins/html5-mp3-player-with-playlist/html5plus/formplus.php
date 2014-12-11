<?php
//include_once("db.php");
include_once("function.php");

global $wpdb;
$itable		=	$wpdb->prefix."html5mp3_items";

if(isset($_REQUEST['id']) && $_REQUEST['id']!="")
 $id = $_REQUEST['id'];
else
 $id = 0;

$qq = mysql_query("select * from `".$table."` where id = '".$id."' ");

$docdata = mysql_fetch_assoc($qq);



/* Save Playlist */

if(isset($_REQUEST['submit']) && $_REQUEST['submit']=="Save Playlist")
{

include("saveplus.php");

}


/* Save Playlist */


$ele = '';

if($id)
{

//echo "select * from `".$itable."` where pid = '".$id."'  ";

$qqqq = mysql_query("select * from `".$itable."` where pid = '".$id."' order by id ");

$mm=1;

while( $docta = mysql_fetch_assoc($qqqq) )
{

 
	
	$mp3i = $docta['id'];
	
	$mp3p = $docta['image'];						
		
	$mp3s = $docta['song'];  
	 
	$mp3t = $docta['title']; 
	
	$mp3a = $docta['artist'];
	
	$mp3d = $docta['download'];
	
	$mp3b = $docta['buynow'];
	
	$mp3amt = $docta['amount'];
	
	$mp3pre = $docta['preview'];
	
	if($mp3d=='1')
	{  
	  $ddd = ' checked="checked" '; 
	  $vvv = 1; 
	}  
	else
	{
	 $ddd = '';
	 $vvv = 0;
	} 
	
	
	if($mp3b=='1')
	{  
	  $bbb = ' checked="checked" '; 
	  $vvv = 1; 
	}  
	else
	{
	 $bbb = '';
	 $vvv = 0;
	} 
	
			
	 $ele .= '<div id="divId' .$mm. '">';
				
	$ele .= '<input type="hidden" name="item_id['.($mm-1).']" id="item_id' .$mm. '" value="'.$mp3i.'" /><input type="checkbox" name="download['.($mm-1).']" id="download' .$mm. '" '.$ddd.' value="1" />&nbsp;Download&nbsp;<br /><input type="text" size="60" name="title[]"	id="title' .$mm. '" value="'.$mp3t.'" placeholder="title" />&nbsp;<br /><input type="text" size="60" name="artist[]"	id="artist' .$mm. '" value="'.$mp3a.'" placeholder="artist" />&nbsp;<br /><input type="text" size="60" name="song[]"	id="song' .$mm. '" value="'.$mp3s.'" placeholder="media" />&nbsp;<br /><input type="text" size="60" name="image[]"	id="image' .$mm. '" value="'.$mp3p.'" placeholder="image" />';
				
				/*if($mm==1)
				  $ele .= '&nbsp;&nbsp;<a href="javascript:void(0)" onclick="return addNewElement()">+ Add More</a><br><br><br>';
				else
				  $ele .= '&nbsp;&nbsp;<a href="javascript:void(0)" onclick="return removeThisElement(' .$mm. ')">- Remove This</a><br><br><br>'; */ 
				  
				$ele .= '&nbsp;&nbsp;<a href="javascript:void(0)" onclick="return addNewElement()">+ Add More</a>&nbsp;&nbsp;<a href="javascript:void(0)" onclick="return removeThisElement(' .$mm. ')">- Remove This</a><br><br><br>';  
				  
				$ele .= '</div>';
				  
				
				$mm++;
				
			  }
			  
			  
			
 		


}


?>
	 
<?php if(isset($_REQUEST['page']) && $_REQUEST['page']=="html5mp3-options") { ?>
<h2>Manage Playlist</h2>	 <br />
<?php } ?>		
	 
 <form name="frm" action="<?php bloginfo('url'); ?>/wp-admin/admin.php?page=html5mp3_add_playlist&action=update&id=<?php echo $_REQUEST['id']; ?>" method="post">
		

<br />
Manage HTML5 MP3 Audio link and other information<br /><br />


<h3>Manage Info</h3>


<br />

Title: <input type="text" name="ptitle" value="<?php echo $docdata['title']; ?>" placeholder="title" />&nbsp;&nbsp;&nbsp;&nbsp;

Artist: <input type="text" name="partist" value="<?php echo $docdata['artist']; ?>" placeholder="artist" />&nbsp;&nbsp;&nbsp;&nbsp;

<br />
<br />

Facebook: <input type="text" name="facebook" value="<?php echo $docdata['facebook']; ?>" placeholder="facebook link" />&nbsp;&nbsp;&nbsp;&nbsp;
Twitter: <input type="text" name="twitter" value="<?php echo $docdata['twitter']; ?>" placeholder="twitter link" />&nbsp;&nbsp;&nbsp;&nbsp;
<?php /*?>Link: <input type="text" name="link" value="<?php echo $docdata['link']; ?>" placeholder="link" />&nbsp;&nbsp;&nbsp;&nbsp;<?php */?>

<br />
<br />

<?php /*?>Background Color: #<input type="text" size="7" name="brcolor" value="<?php echo $docdata['brcolor']; ?>" placeholder="color code" />&nbsp;&nbsp;&nbsp;&nbsp;<?php */?>
Background Color: #<input type="text" size="7" name="bgcolor" value="<?php echo $docdata['bgcolor']; ?>" placeholder="color code" />&nbsp;&nbsp;&nbsp;&nbsp;
 

Artwork: <input type="text" name="pimage" value="<?php echo $docdata['image']; ?>" placeholder="image" />&nbsp;&nbsp;&nbsp;&nbsp;

 

<?php /*?>Width: <input type="text" name="width" value="<?php echo $docdata['width']; ?>" placeholder="width" />&nbsp;&nbsp;&nbsp;&nbsp;
Height: <input type="text" name="height" value="<?php echo $docdata['height']; ?>" placeholder="height" />&nbsp;&nbsp;&nbsp;&nbsp;<?php */?>
<br />
<br />


<?php /*?>Shuffle: <input type="checkbox" name="shuffle" value="1" <?php if(isset($docdata['shuffle']) && $docdata['shuffle']=="1") { ?> checked="checked" <?php } ?>  />&nbsp;&nbsp;&nbsp;&nbsp;

Popout: <input type="checkbox" name="popout" value="1" <?php if(isset($docdata['popout']) && $docdata['popout']=="1") { ?> checked="checked" <?php } ?>  />&nbsp;&nbsp;&nbsp;&nbsp;
Embed: <input type="checkbox" name="embed" value="1" <?php if(isset($docdata['embed']) && $docdata['embed']=="1") { ?> checked="checked" <?php } ?> />&nbsp;&nbsp;&nbsp;&nbsp;<?php */?>

<input type="hidden" name="embed" value="0" />
<input type="hidden" name="popout" value="0" />
<input type="hidden" name="shuffle" value="0" />
<input type="hidden" name="link" value="1" />
<input type="hidden" name="size" value="full" />


Autoplay: <input type="checkbox" name="autoplay" value="1" <?php if(isset($docdata['autoplay']) && $docdata['autoplay']=="1") { ?> checked="checked" <?php } ?> />&nbsp;&nbsp;&nbsp;&nbsp;
Scrolling Title: <input type="checkbox" name="scrollingtext" value="1" <?php if(isset($docdata['scrollingtext']) && $docdata['scrollingtext']=="1") { ?> checked="checked" <?php } ?> />&nbsp;&nbsp;&nbsp;&nbsp;


<?php /*?>Toggle Playlist: <input type="checkbox" name="toggleplaylist" value="1" <?php if(isset($docdata['toggleplaylist']) && $docdata['toggleplaylist']=="1") { ?> checked="checked" <?php } ?> />&nbsp;&nbsp;&nbsp;&nbsp;<?php */?>

Use iFrame: <input type="checkbox" name="embed" value="1" <?php if(isset($docdata['embed']) && $docdata['embed']=="1") { ?> checked="checked" <?php } ?> />&nbsp;&nbsp;&nbsp;&nbsp;

<br />
<?php /*?>Description: <br /><textarea name="description" cols="100" rows="5"><?php echo $docdata['description']; ?></textarea>&nbsp;&nbsp;&nbsp;&nbsp;<?php */?>






<?php /*?>PayPal Sandbox?</strong> <input type="checkbox" name="sandbox" value="1" <?php if($docdata['sandbox']=="1") { ?> checked="checked" <?php } ?>>&nbsp;&nbsp;&nbsp;&nbsp;

PayPal Email: <input type="text" name="email" value="<?php echo $docdata['email']; ?>" />&nbsp;&nbsp;&nbsp;&nbsp;<?php */?>

<br /><br /><h3>Manage Items</h3>

<?php /*?>Source Type: <select name="sourcetype" onchange="sel_source(this.value);">
<option value="default" <?php if($docdata['sourcetype']=="default") { ?>selected="selected"<?php } ?>>Default</option>
<option value="folder" <?php if($docdata['sourcetype']=="folder") { ?>selected="selected"<?php } ?>>MP3 Folder</option>
<option value="feed" <?php if($docdata['sourcetype']=="feed") { ?>selected="selected"<?php } ?>>Feed Burner</option>
<option value="soundcloud" <?php if($docdata['sourcetype']=="soundcloud") { ?>selected="selected"<?php } ?>>Sound Cloud</option>
</select>
<?php */?>

<input type="hidden" name="sourcetype" value="default" />


<br />


<div id="default" style="display:<?php if($docdata['sourcetype']=="default" || $docdata['sourcetype']=="") { ?>block<?php } else { ?>none<?php } ?>">


<div id="more_element_area">
  
  <script language="javascript">
  
  var idno = <?php echo isset($mm)?$mm:'2'; ?>;
  
  </script>
  
  <?php if($ele!="") { ?>
  
  <?php echo $ele; ?>
  
  <?php }else{ ?>
  
    <input type="hidden" name="item_id[0]" id="item_id1"  value="" /><input type="checkbox" name="download[0]" id="download1"  value="1" />&nbsp;Download&nbsp;<br /><input type="text" size="60" name="title[]" id="title1" value="" placeholder="title" />&nbsp;<br /><input type="text" size="60" name="artist[]" id="artist1" value="" placeholder="artist" />&nbsp;<br /><input type="text" size="60" name="song[]" id="song1" value="" placeholder="media" />&nbsp;<br /><input type="text" name="image[]" id="image1" size="60" value="" placeholder="image" />&nbsp;&nbsp;<a href="javascript:void(0)" onclick="return addNewElement()">+ Add More</a>&nbsp;<br><br><br>
    
  <?php } ?>  
    
  
</div>

</div>



<br />

<input type="hidden" name="host" value="<?php echo $_REQUEST['host']; ?>" />
 <input type="hidden" name="url" value="<?php echo $docdata['url']; ?>" />
 
<?php /*?> <input type="hidden"   name="size" value="full" /><?php */?>


<input type="submit" name="submit" style="background-color:#D84937; height:35px; color:#ffffff; font-weight:bold;" value="Save Playlist" />


</form>

<br />

<?php



?>

  <div>
        		
 </div>
 
 <script language="javascript">
 
  function sel_source(v)
  {
  
    if(v == "default")
	{
	  document.getElementById("default").style.display = 'block';
	  document.getElementById("other").style.display = 'none';
	  document.getElementById("osoundcloud").style.display = 'none';
	}
	else if(v == "soundcloud")
	{
	  document.getElementById("default").style.display = 'none';
	  document.getElementById("other").style.display = 'block';
	  document.getElementById("osoundcloud").style.display = 'block';
	  
	}
	else
	{
	  document.getElementById("default").style.display = 'none';
	  document.getElementById("other").style.display = 'block';
	  document.getElementById("osoundcloud").style.display = 'none';
	}  
  
  }
  
 </script>
 
 
