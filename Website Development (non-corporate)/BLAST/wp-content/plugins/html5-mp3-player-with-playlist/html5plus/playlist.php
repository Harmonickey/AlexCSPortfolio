<?php

global $wpdb;
$table		=	$wpdb->prefix."html5mp3_playlist";
$itable		=	$wpdb->prefix."html5mp3_items";


$isuccess = isset($_REQUEST['isuccess'])?$_REQUEST['isuccess']:"";
$ierror = isset($_REQUEST['ierror'])?$_REQUEST['ierror']:"";


if(isset($_GET['id'])){
	$id		=	$_GET['id'];
}


$usql		=	"SELECT * FROM $table WHERE id='$id'";
$uresults 	= 	$wpdb->get_row( $usql  );


$action		=	"add";
if(isset($_GET['action'])){
	$action	=	$_GET['action'];	
}

if($action=="delete") {

$delete		=	$wpdb->query(
							"DELETE FROM $table WHERE id='$id'"
						);
						
						
$delete		=	$wpdb->query(
							"DELETE FROM $itable WHERE pid='$id'"
						);						
						
$isuccess	=	"Playlist deleted successfully";						
						
}						


if(isset($_REQUEST['width']) && $_REQUEST['width']!="")
{
  $width=$_REQUEST['width'];
}
else
{
  $width = '600';
}

if(isset($_REQUEST['height']) && $_REQUEST['height']!="")
{
  $height=$_REQUEST['height'];
}
else
{
  $height = '250';
}


if(isset($_REQUEST['stitle']) && $_REQUEST['stitle']!="")
{
  $stitle=$_REQUEST['stitle'];
}
else
{
  $stitle = '0';
}


if(isset($_REQUEST['size']) && $_REQUEST['size']!="")
{
  $size=$_REQUEST['size'];
}
else
{
  $size = 'full';
}


if(isset($_REQUEST['links']) && $_REQUEST['links']!="")
{
  $links=$_REQUEST['links'];
}
else
{
  $links = '0';
}


$c=array('DA1D1E', '497BF3', '13A536', 'F6C230', '343434');
$rnd = rand(0,4);


if(isset($_REQUEST['fcolor']) && $_REQUEST['fcolor']!="")
{
  $fcolor=$_REQUEST['fcolor'];
}
else
{
  $fcolor = '000000'; //$c[$rnd];
}


if(isset($_REQUEST['bcolor']) && $_REQUEST['bcolor']!="")
{
  $bcolor=$_REQUEST['bcolor'];
}
else
{
  $bcolor = 'ff0000'; //$c[$rnd];
}


if(isset($_REQUEST['tcolor1']) && $_REQUEST['tcolor1']!="")
{
  $tcolor1=$_REQUEST['tcolor1'];
}
else
{
  $tcolor1 = 'ffffff'; //$c[$rnd];
}

if(isset($_REQUEST['tcolor2']) && $_REQUEST['tcolor2']!="")
{
  $tcolor2=$_REQUEST['tcolor2'];
}
else
{
  $tcolor2 = 'a19b9b'; //$c[$rnd];
}


if(isset($_REQUEST['dlicon']) && $_REQUEST['dlicon']!="")
{
  $dlicon=$_REQUEST['dlicon'];
}
else
{
  $dlicon = ''; //$c[$rnd];
}



if(isset($_REQUEST['dlpos']) && $_REQUEST['dlpos']!="")
{
  $dlpos=$_REQUEST['dlpos'];
}
else
{
  $dlpos = '10'; //$c[$rnd];
}




if($width<150)
 $width = '150';

if(isset($_REQUEST['id']) && $_REQUEST['id']!="")
{
  $id=$_REQUEST['id'];
}
else
{
  $id = '0';  
}


if(isset($_REQUEST['host']) && $_REQUEST['host']!="")
{
  $host=$_REQUEST['host'];
  //add_host($host);
}
else
{
  $host = 'localhost';
}




?>





<?php //$mm = 2; ?>

<script type="text/javascript">
//var idno = <?php //echo $mm; ?>; // It start from id 2 

function addNewElement()
{
	// mainDiv is a variable to store the object of main area Div.
	var mainDiv = document.getElementById('more_element_area');
	// Create a new div 
	var innerDiv = document.createElement('div');
	// Set the attribute for created new div like here I am assigning Id attribure. 
	innerDiv.setAttribute('id', 'divId' + idno);
	// Create text node to insert in the created Div
	//var generatedContent = '<input type="text" name="new_element' + idno + '"	id="new_element' + idno + '" value="" />: <input type="text" size="70" name="new_element_' + idno + '"	id="new_element_' + idno + '" value="" />&nbsp;<a href="javascript:void(0)" onclick="return removeThisElement(' + idno + ')">Remove This</a>';
	
	//var generatedContent = '<input type="checkbox" name="download[' + (idno-1) + ']" id="download' + idno + '" value="1" />&nbsp;<input type="text" name="title[]"	id="title' + idno + '" value="" placeholder="title" />&nbsp;<input type="text" name="artist[]"	id="artist' + idno + '" value="" placeholder="artist" />&nbsp;<input type="text" size="70" name="song[]"	id="song' + idno + '" value="" placeholder="mp3 song" />&nbsp;<input type="text" size="70" name="image[]"	id="image' + idno + '" value="" placeholder="image" />&nbsp;<a href="javascript:void(0)" onclick="return removeThisElement(' + idno + ')">Remove This</a><br><br><br>';
	
	
	var generatedContent = '<input type="hidden" name="item_id[' + (idno-1) + ']" id="item_id' + idno + '" value="" /><input type="checkbox" name="download[' + (idno-1) + ']" id="download' + idno + '" value="1" />&nbsp;Download&nbsp;<br><input type="text" name="title[]"	id="title' + idno + '" value="" placeholder="title" />&nbsp;<br><input type="text" name="artist[]"	id="artist' + idno + '" value="" placeholder="artist" />&nbsp;<br><input type="text" size="60" name="song[]"	id="song' + idno + '" value="" placeholder="media" />&nbsp;<br><input type="text" size="60" name="image[]"	id="image' + idno + '" value="" placeholder="image" />&nbsp;&nbsp;<a href="javascript:void(0)" onclick="return addNewElement()">+ Add More</a>&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="return removeThisElement(' + idno + ')">- Remove This</a><br><br><br>';
	
	
	// Inserting content to created Div by innerHtml
	innerDiv.innerHTML = generatedContent;
	// Appending this complete div to main div area.
	mainDiv.appendChild(innerDiv);
	// increment the id number by 1.
	idno++;
}

function removeThisElement(idnum)
{
	
	if(confirm("Are you Sure?"))
	{
	// mainDiv is a variable to store the object of main area Div.
	var mainDiv = document.getElementById('more_element_area');
	// get the div object with get Id to remove from main div area.
	var innerDiv = document.getElementById('divId' + idnum);
	// Removing element from main div area.
	mainDiv.removeChild(innerDiv);
	}
 
}


function popitup(id, size) {

var url = "<?php bloginfo('url'); ?>/wp-content/plugins/html5-mp3-player-with-playlist/html5plus/html5"+size+".php?id="+id+"&iframe=1";
 
 url = addParameterToURL(url, "rand=2");

        LeftPosition = (screen.width) ? (screen.width - 800) / 2 : 0;
        TopPosition = (screen.height) ? (screen.height - 700) / 2 : 0;
        //var sheight = 450;;/*(screen.height) * 0.9;*/
        //var swidth = 370;;/*(screen.width) * 0.8;*/  
		
		if(size=="full")
		{
		  var sheight = 227; var swidth = 587;
		}
		
		if(size=="big")
		{
		  var sheight = 450; var swidth = 370;
		}
		
		if(size=="small")
		{
		  var sheight = 227; var swidth = 367;
		}	
		        

        settings = 'height='+ sheight + ',width='+ swidth + ',top=' + TopPosition + ',left=' + LeftPosition + ',scrollbars=yes,resizable=yes,toolbar=no,status=no,menu=no, directories=no,titlebar=no,location=no,addressbar=no'
		
		
		
        newwindow = window.open('', '', settings);
        if (window.focus) { newwindow.focus(); newwindow.document.location.href=url; /*newwindow.location.reload()*/ 
		
		/*$('audio#player22').mediaelementplayer({
		features: []	
		});
		
		setTimeout("newPlayer();", 10000);*/
		
		}
        return false;
    }
	
	
		function addParameterToURL(_url, param){
    //_url = location.href;
    _url += (_url.split('?')[1] ? '&':'?') + param;
    return _url;
}


</script>

<h2>Manage Playlist</h2>


<strong>HTML5 Audio Player with Playlist&nbsp;<a href="<?php bloginfo('url'); ?>/wp-admin/admin.php?page=html5mp3_add_playlist&action=update&id=0" style="background-color:#D84937; padding:5px; height:35px; color:#ffffff; font-weight:bold;">Add New</a></strong> <br /><br />



<?php if(!empty($isuccess)): ?>
        
<span style="color:green;"><?php echo $isuccess; ?></span>

<?php elseif(!empty($ierror)): ?>

<span style="color:red;"><?php echo $ierror; ?></span>
       
<?php endif ?>

<?php if($action=="preview") { ?>

<?php include("preview.php"); ?>

<?php } else { ?>

<table class="wp-list-table widefat fixed" cellspacing="0" style="margin-top:20px;">
	<thead>
	<tr>		
        <th scope="col" width="10%"><a href="#">Title</a></th>
        <th scope="col" width="10%" ><a href="#">Size</a></th>
        <th scope="col" width="15%"><a href="#">Items</a></th>
        <th scope="col" width="10%"><a href="#">Shortcode</a></th>
         
        <th scope="col" width="10%"><a href="#">Preview</a></th>	
        <th scope="col" width="10%"><a href="#">Edit</a></th>	
        <th scope="col" width="10%"><a href="#">Delete</a></th>	
     </tr>
	</thead>

	<tfoot>
	<tr>
	    <th scope="col" width="10%"><a href="#">Title</a></th>
        <th scope="col" width="10%" ><a href="#">Size</a></th>
        <th scope="col" width="15%"><a href="#">Items</a></th>
        <th scope="col" width="10%"><a href="#">Shortcode</a></th>
        
        <th scope="col" width="10%"><a href="#">Preview</a></th>	 
        <th scope="col" width="10%"><a href="#">Edit</a></th>	
        <th scope="col" width="10%"><a href="#">Delete</a></th>		
     </tr>
	</tfoot>

	<tbody id="the-list">
    
    <?php
		//$sql		=	"SELECT * FROM $table";

		//$results 	= 	$wpdb->get_results( $wpdb->prepare( $sql ) );
		
		$sql		=	mysql_query("SELECT * FROM $table");
		
		$mmm = 0;
		
	?>
	
    <?php while( $result = mysql_fetch_object($sql) ) { 
	
		
	$isql		=	mysql_num_rows(mysql_query("SELECT * FROM $itable where pid = '".$result->id."'")); 
	
	
	?>
    <tr>
        <td><a href="<?php bloginfo('url'); ?>/wp-admin/admin.php?page=html5mp3_playlist&action=preview&id=<?php echo $result->id; ?>"><?php echo $result->title; ?></a></td>
        <td width="10%"><?php echo $result->size; ?></td>
        <td width="10%"><?php echo $isql; ?></td>
        <td width="10%">[html5mp3 id=<?php echo $result->id; ?>]</td>
         
        <td width="10%"><a onclick="popitup('<?php echo $result->id; ?>', '<?php echo $result->size; ?>')" href="javascript:void(0);">Preview</a></td>  
        <td width="10%"><a href="<?php bloginfo('url'); ?>/wp-admin/admin.php?page=html5mp3_add_playlist&action=update&id=<?php echo $result->id; ?>">Update</a></td>
        <td width="10%"><a onclick="return confirm('Are you sure?');" href="<?php bloginfo('url'); ?>/wp-admin/admin.php?page=html5mp3_playlist&action=delete&id=<?php echo $result->id; ?>">Delete</a></td>
	</tr>
	<?php $mmm=1; } ?>
	
	<?php if($mmm==0) { ?>
    
    <td class="posts column-posts num" colspan="6"><a href="admin.php?page=html5mp3_add_playlist">Please add playlist</a></td>
    
	<?php } ?>
  	
  </tbody>
</table>   

<?php } ?>      
<br />
<br />

<script type='text/javascript' src='<?php bloginfo('url'); ?>/wp-content/plugins/html5-mp3-player-with-playlist/html5plus/js/html5mp3playlist-min.js?ver=3.6.1'></script>

<script language="javascript">

function getSC()
{

//var currentTime = Date.now() || +new Date();

var thestring = document.getElementById('oldsc').value;
var thenum = thestring.match(/\d+/g)[2];

var varss = [{"html5mp3playlistid":thenum,"html5mp3playlistspan":"fullcode","html5mp3playlistsize":"full"}, {"html5mp3playlistid":thenum,"html5mp3playlistspan":"smallcode","html5mp3playlistsize":"small"}];

//var fcode = '<iframe src="http://html5player.svnlabs.com/v1/html5full.html?id='+thenum+'" frameborder="0" marginheight="0" marginwidth="0" scrolling="no" width="566" height="207"></iframe>';
//var scode = '<iframe src="http://html5player.svnlabs.com/v1/html5small.html?id='+thenum+'" frameborder="0" marginheight="0" marginwidth="0" scrolling="no" width="347" height="207"></iframe>';

html5mp3player(varss);

document.getElementById('fullcode').style.display='block';
document.getElementById('smallcode').style.display='block';

//document.getElementById('fullcode').innerHTML=fcode;
//document.getElementById('smallcode').innerHTML=scode;



}


function OldPlaylists()
{

var thestring = document.getElementById('oldscid').value;

var currt = thestring.replace("http://"," ");

var scode = '<a target="_blank" onclick="return confirm(&quot;This will take you to plugin site?&quot;);" href="http://html5.svnlabs.com/form.php?host='+currt+'"><strong>Please click on this URL to get old playlists</strong></a>';

document.getElementById('oldpl').innerHTML=scode;




}

</script>



<?php 

$domhost = parse_url(plugin_dir_url(__FILE__));

?>


<h2>Get Old ShortCode</h2>

<input type="text"  name="oldsc" id="oldsc" size="30" value="[html5mp3full:1]" />&nbsp;<input onclick="getSC();" type="button" name="but" value="Get Widget" /> <br />

<textarea cols="60" rows="5" id="fullcode" onFocus="this.select();" style="border:1px dotted #343434; display:none;" ></textarea>&nbsp;<textarea cols="60" rows="5" id="smallcode" onFocus="this.select();" style="border:1px dotted #343434; display:none;" ></textarea>


<h2>Get Old Playlists</h2>

<input type="text"  name="oldscid" id="oldscid" size="30" value="<?php echo $domhost['host']; ?>" />&nbsp;<input onclick="OldPlaylists();" type="button" name="but" value="Get Playlists" /> <br />
(i.e. html5.svnlabs.com without http://)

<br />
<br />

<span id="oldpl"><a target="_blank" onclick="return confirm(&quot;This will take you to plugin site?&quot;);" href="http://html5.svnlabs.com/form.php?host=<?php echo $domhost['host']; ?>"><strong>Please click on this URL to get old playlists</strong></a></span>

<br /><br /><br />

<strong>Note:</strong> If you have problem in using player try option "Use iFrame" in Manage Playlist

    
