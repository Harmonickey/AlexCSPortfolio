<?php
ob_start();
error_reporting(0);

$iframe = isset($_REQUEST['iframe'])?$_REQUEST['iframe']:"0";

if($iframe=="1")
{

$id = isset($_REQUEST['id'])?$_REQUEST['id']:"";

include("db.php");

$inurl = str_replace("wp-content/plugins/html5-mp3-player-with-playlist/html5plus/", "wp-includes/js/jquery/jquery.js?ver=1.10.2", plugin_dir_url(__FILE__));

echo '<script type="text/javascript" src="'.$inurl.'"></script>';
}

/*if(isset($_REQUEST['id']) && $_REQUEST['id']!="")
  $id = $_REQUEST['id'];
else
  $id = 1;  */


$qqqq = mysql_query("select * from `".$table."` where id = '".$id."' ");
$docdata = mysql_fetch_assoc($qqqq);  


$title = isset($docdata['title'])?$docdata['title']:"";
$brcolor = isset($docdata['brcolor'])?$docdata['brcolor']:"";
$bgcolor = isset($docdata['bgcolor'])?$docdata['bgcolor']:"000";
$autoplay = isset($docdata['autoplay'])?$docdata['autoplay']:"";
$size = isset($docdata['size'])?$docdata['size']:"";
$image = isset($docdata['image'])?$docdata['image']:"";
$scrollingtext = isset($docdata['scrollingtext'])?$docdata['scrollingtext']:"";
$facebook = isset($docdata['facebook'])?$docdata['facebook']:"1";
$twitter = isset($docdata['twitter'])?$docdata['twitter']:"1";
$link = isset($docdata['link'])?$docdata['link']:"1";




if($bgcolor=="") $bgcolor = '000';

?><script language="javascript">

<?php if($autoplay==1) { ?>
var autoplayz = true;
<?php } else { ?>
var autoplayz = false;
<?php } ?>

var plugin_url = '<?php echo plugin_dir_url(__FILE__); ?>';
var id = '<?php echo $id; ?>';

var $ = jQuery;

</script>

<?php include("css/player.css.php"); ?>

<script type="text/javascript" src="<?php echo plugin_dir_url(__FILE__); ?>js/jquery.tinyscrollbar.min.js"></script>

<script src="<?php echo plugin_dir_url(__FILE__); ?>audiojs/audio.js"></script>
<script src="<?php echo plugin_dir_url(__FILE__); ?>js/coresd-min.js"></script>	  
	  

<script type="text/javascript">
		jQuery(document).ready(function(){
			$('#scrollbar1<?php echo $id; ?>').tinyscrollbar();
			
			$('#trackbar1<?php echo $id; ?>').strackbar({ callback: onTick1, defaultValue: 3, sliderHeight: 4, sliderWidth: 68, style: 'style1', animate: true, ticks: true, labels: false, trackerHeight: 20, trackerWidth: 19 });
							
		});
</script>

<script src="<?php echo plugin_dir_url(__FILE__); ?>js/smartslider.js" type="text/javascript"></script>
<script src="<?php echo plugin_dir_url(__FILE__); ?>js/main.js" type="text/javascript"></script>
<link href="<?php echo plugin_dir_url(__FILE__); ?>css/smartslider.css" rel="stylesheet" type="text/css" />


<a href="#" id="vol-0"></a> <a href="#" id="vol-10"></a> <a href="#" id="vol-20"></a> <a href="#" id="vol-30"></a> <a href="#" id="vol-40"></a> <a href="#" id="vol-50"></a> <a href="#" id="vol-60"></a> <a href="#" id="vol-70"></a> <a href="#" id="vol-80"></a> <a href="#" id="vol-90"></a>

<div <?php if($size=="small") { ?>id="WraperMainSmall<?php echo $id; ?>"<?php } else { ?>id="WraperMainLarge<?php echo $id; ?>"<?php } ?> style="background-color:#<?php echo $bgcolor; ?>">
  <div id="MainPlayer<?php echo $id; ?>">
    <audio id="audio-palayer<?php echo $id; ?>" preload></audio>
    <div id="Music-title-box<?php echo $id; ?>"> <img src="<?php echo plugin_dir_url(__FILE__); ?>images/audio.png" width="13" height="16" alt="play" align="absmiddle" /> &nbsp; <?php /*?>Part <?php echo $day; ?>:<?php */?><?php if(isset($scrollingtext) && $scrollingtext=="1") { ?> <marquee scrollamount="2" behavior="scroll" direction="right" width="280"><?php } ?> <span id="current-track<?php echo $id; ?>"></span><?php if(isset($scrollingtext) && $scrollingtext=="1") { ?></marquee><?php } ?> </div>
    <div id="Player-display-screen<?php echo $id; ?>"> <img src="<?php echo plugin_dir_url(__FILE__); ?>images/spacer.gif" alt="display" id="current-play-pic<?php echo $id; ?>" style="width:325px; height:86px;" /></div>
    
    <div  id="Player-progressbar<?php echo $id; ?>">
      <div class="scrubber" id="Player-progressbar01<?php echo $id; ?>">
        <div id="loaded-bar<?php echo $id; ?>" class="progress" style=" background:#666666; width:0;  height: 7px;">
          <div id="progress-bar<?php echo $id; ?>" class="loaded" style="width:0; height:7px; background:#e5cf58;"></div>
        </div>
      </div>
    </div>
    
    
    <div class="Clear"></div>
    <div id="Player-Control<?php echo $id; ?>">
      <div id="Play-back<?php echo $id; ?>"><a style="cursor:pointer;" id="prev_track<?php echo $id; ?>" title="previous track"><img src="<?php echo plugin_dir_url(__FILE__); ?>images/back-img.png" width="26" height="26" alt="back" /></a></div>
      <div id="Play-button<?php echo $id; ?>"><a style="cursor:pointer;" id="play-pause<?php echo $id; ?>"><img src="<?php echo plugin_dir_url(__FILE__); ?>images/pause-btn.png" width="41" height="41" alt="play" id="play-pause-btn<?php echo $id; ?>" /></a></div>
      <div id="Play-next<?php echo $id; ?>"><a style="cursor:pointer;" id="next_track<?php echo $id; ?>" title="next track"><img src="<?php echo plugin_dir_url(__FILE__); ?>images/next-btn.png" width="26" height="26" alt="next" /></a></div>
      <div id="Play-Revind<?php echo $id; ?>"> <span id="loop-track-box<?php echo $id; ?>"> <a style="cursor:pointer;" title="loop on" id="loop-track<?php echo $id; ?>"><img src="<?php echo plugin_dir_url(__FILE__); ?>images/noloop-btn.png"  alt="refresh" /></a> </span> <span id="noloop-track-box<?php echo $id; ?>" style="display:none" > <a style="cursor:pointer;;" id="noloop-track<?php echo $id; ?>" title="loop off"><img src="<?php echo plugin_dir_url(__FILE__); ?>images/loop-btn.png" alt="refresh" /></a> </span><?php if(isset($shuffle) && $shuffle==1){ ?><span id="shuffle-track-box<?php echo $id; ?>"> <a style="cursor:pointer;" id="shuffle-track<?php echo $id; ?>"><img src="<?php echo plugin_dir_url(__FILE__); ?>images/noshuffle-btn.png" width="27" height="19" alt="revid" /></a> </span> <span id="noshuffle-track-box<?php echo $id; ?>" style="display:none;"> <a style="cursor:pointer;" id="noshuffle-track<?php echo $id; ?>"><img src="<?php echo plugin_dir_url(__FILE__); ?>images/shuffle-btn.png" width="27" height="19" alt="revid" /></a> </span><?php } ?> </div>
      <div id="Play-Timer<?php echo $id; ?>" style="padding-left:7px"><span id="palyed-time<?php echo $id; ?>">00.00</span> &nbsp;  / &nbsp; <span id="duration-time<?php echo $id; ?>">00:00</span> </div>
      <!--<div id="Sound-bar">
        <div id="sound01"><img src="images/volume-bar-btn.png" width="12" height="11" /></div>
      </div>-->
      <div>
        <div class="soundbar" id="trackbar1<?php echo $id; ?>"> </div>
        <!--<div id="text1" style="clear: both; text-align: center;">
            </div>-->
      </div>
    </div>
  </div>
  <div <?php if($size=="small") { ?>style="display:none"<?php } ?> id="MainPalylistbox<?php echo $id; ?>">
    <div id="Playlist-heading<?php echo $id; ?>" style="height:16px;">
    
    <div style="float:left"><strong>Playlist</strong></div>
    <div style="float:right; margin-right:-10px;"><a href="<?php echo $twitter; ?>" target="_blank" title="Twitter"><img src="<?php echo plugin_dir_url(__FILE__); ?>images/twitter.png" border="0" width="18" /></a><a href="<?php echo $facebook; ?>" target="_blank" title="Facebook"><img src="<?php echo plugin_dir_url(__FILE__); ?>images/facebook.png" border="0" width="18" /></a>
    
    
    </div>
    
    </div>
    <div id="Playlist-block<?php echo $id; ?>" class="Playlist01">
      <div id="scrollbar1<?php echo $id; ?>">
        <div class="scrollbar">
          <div class="track">
            <div class="thumb">
              <div class="end"></div>
            </div>
          </div>
        </div>
        <div class="viewport">
          <div class="overview">
          
          
            <ol id="tracks-list<?php echo $id; ?>">
              
              <?php
              
			  //$xml = simplexml_load_file($xml_file);
			  
			  $mm = 0;
			  
			  $qqqq = mysql_query("select * from `".$itable."` where pid = '".$id."' order by id ");

              while( $songs = mysql_fetch_assoc($qqqq) )
			  {
					
				$mp3sid = $songs['id'];
				
				$mp3pid = $songs['pid'];
				
				$mp3p = $songs['image'];						
					
				$mp3s = $songs['song'];
				
				$mp3prev = $songs['preview'];  
                 
				$mp3t = $songs['title']; 
				
				$mp3a = $songs['artist'];
				
				$mp3d = $songs['download'];
				
				$mp3b = $songs['buynow'];
				
				
				if(isset($mp3b) && $mp3b=="1")
				{
				  $sng = $mp3prev;
				  $ttl = "Preview - ".$mp3t;
				}
				else
				{
				  $sng = $mp3s; 
				  $ttl = $mp3t;
			    }
				
				
			  $prms = 'id='.$id.'&sid='.$mp3sid.'&size='.$size.'&width='.$width.'&height='.$height.'&links='.$links.'&stitle='.$stitle.'&fcolor='.$fcolor.'&bcolor='.$bcolor.'&host='.$host; 
			  
			  $dlink = preg_replace('/ /i', '%20', $mp3s);
			  
			  ?>
              
                           
               
                <?php if(isset($mp3d) && $mp3d=="1") { ?>
              <span style="right:27px; position:absolute;">&nbsp;&nbsp;<a id="source<?php echo $id; ?><?php echo $mm; ?>download" href="<?php echo $dlink; ?>" download="<?php echo basename($mp3s); ?>" style="z-index:1000; position:absolute;" target="_blank" title="Download"><img src="<?php echo plugin_dir_url(__FILE__); ?>images/download.jpg" border="0" width="17" /></a></span>
              <?php }else{ ?>
              <span style="right:27px; position:absolute;">&nbsp;&nbsp;<a id="source<?php echo $id; ?><?php echo $mm; ?>download" href="<?php echo $dlink; ?>" download="<?php echo basename($mp3s); ?>" style="z-index:1000; position:absolute;" target="_blank" title="Download"><img src="<?php echo plugin_dir_url(__FILE__); ?>images/download.jpg" border="0" width="0" /></a></span>              
              <?php } ?>
              
            <?php /*?>  <?php if(isset($mp3b) && $mp3b=="1") { ?>
              <span style="right:27px; position:absolute;">&nbsp;&nbsp;<a href="buynowfull.php?<?php echo $prms; ?>" style="z-index:1000; position:absolute;" title="Buynow"><img src="buynow.png" border="0" width="17" /></a></span>
              <?php }else{ ?>
              <span style="right:27px; position:absolute;">&nbsp;&nbsp;<a href="buynowfull.php?<?php echo $prms; ?>" style="z-index:1000; position:absolute;" title="Buynow"><img src="buynow.png" border="0" width="0" /></a></span>              
              <?php } ?><?php */?>
              
              
              <li>
                <a id="source<?php echo $id; ?><?php echo $mm; ?>" href="#" data-src="<?php echo $mp3s; ?>" title="<?php echo $mp3t; ?>" class="track-name" name="<?php echo $mp3p; ?>"><?php echo $mp3t; ?></a>
                <p class="Title01" style="font-family:Arial, Helvetica, sans-serif; color:#a19b9b; font-size:10px; margin:0px 0 0 0px; "><?php echo $mp3a; ?></p>
              </li>
              
              
              <?php $mm++; } ?>
              
              
            </ol>
            
            
          </div>
        </div>
      </div>
    </div>
  </div>
</div>

<?php

$mp3content .= ob_get_contents();

ob_end_clean();

if($iframe=="1")
{
echo $mp3content;
}

?>