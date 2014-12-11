<?php 

?><style type="text/css">

#WraperMain<?php echo $id; ?>{width:566px; margin:0px auto; height:207px; padding:0px;  }

#WraperMainLarge<?php echo $id; ?>{width:566px; /*margin:0px auto;*/ height:207px; padding:0px;  }

#WraperMainSmall<?php echo $id; ?>{width:347px; /*margin:0px auto;*/ height:207px; padding:0px;  }


#MainPlayer<?php echo $id; ?>{ float:left; width:337px; margin:0px; height:200px; padding:0px; }
#MainPalylistbox<?php echo $id; ?>{ float:left; width:215px; margin:0 0 0 10px; height:200px; padding:0px; }

#Music-title-box<?php echo $id; ?>{ clear:both; padding:5px 15px; width:100%; height:auto; margin:0px; font-family:Arial, Helvetica, sans-serif; color:#fff; font-size:13px; }
#Player-display-screen<?php echo $id; ?>{ clear:both; width:316px; height:86px; margin:0px auto; }
#Player-progressbar<?php echo $id; ?>{ clear:both; width:322px; height:20px; margin:11px auto 0 11px;   }
#Player-progressbar01<?php echo $id; ?>{ clear:both; background:url(<?php echo  plugin_dir_url(__FILE__); ?>../images/progress-bar-bg.jpg) repeat-x; width:322px; height:7px; -webkit-border-radius: 10px;
-moz-border-radius: 10px; border-radius: 10px;}
#Player-progressbar01<?php echo $id; ?>:hover{ clear:both; width:322px; height:7px; -webkit-border-radius: 10px;-moz-border-radius: 10px; border-radius: 10px;}

#Player-Control<?php echo $id; ?>{ clear:both; width:325px; height:45px; margin:0px auto; }
#Play-back<?php echo $id; ?>{ float:left; width:30px; margin-top:5px; }
#Play-button<?php echo $id; ?>{ float:left; width:45px; }
#Play-next<?php echo $id; ?>{ float:left; width:30px; margin-top:5px; }

#Play-Revind<?php echo $id; ?>{ float:left; width:55px; height:13px; margin:10px 0 0 0; }
#Play-Timer<?php echo $id; ?>{ float:left; width:90px; height:13px; margin:13px 0 0 0;  font-family:Arial, Helvetica, sans-serif; color:#fff; font-size:11px; }
#Sound-bar<?php echo $id; ?>{ float:left; width:75px; height:15px; margin:13px 0 0 0px; background:url(<?php echo  plugin_dir_url(__FILE__); ?>../images/volume-bar-bg.png) no-repeat; position:relative; }
#Sound-bar<?php echo $id; ?> #sound01<?php echo $id; ?>{ position:absolute; width:10px; height:10px; top:3px; left:15px; }


.soundbar{ float:left; width:68px; height:5px; margin:17px 0 0 0px; position:relative; }


#Playlist-heading<?php echo $id; ?>{ clear:both; /*width:185px;*/ height:30px; padding:6px 15px; margin:3px 0 3px 0;  font-family:Arial, Helvetica, sans-serif; color:#faca14; font-size:13px; /*border:1px solid #252525;*/ }

#Playlist-block<?php echo $id; ?>{ clear: both;
    height: 166px;
    margin: 0;
   /* overflow: auto;*/
    /*width: 211px;*/ padding:0 3px 0 0;
	}
.Playlist01{ clear:both; width:auto; height:28px; padding:3px 15px; margin:0 0 3px 0;  font-family:Arial, Helvetica, sans-serif; color:#fff; font-size:12px; /*border:1px solid #252525;*/ }


.playing{ clear:both; width:auto; /*height:28px;*/ font-family:Arial, Helvetica, sans-serif; color:#fff; font-size:13px; font-stretch:extra-expanded; font-weight:bold; /*background:url(../images/song-list-bg.png) repeat-x; background-position:-10px;*/ }


.Playlist01 .Title01{  font-family:Arial, Helvetica, sans-serif; color:#a19b9b; font-size:10px; margin:0px 0 0 0px;  }
.Playlist01 ol li a{  font-family:Arial, Helvetica, sans-serif; color:#FFFFFF; font-size:11px;  }
.Playlist01 ol {padding:0; /*margin:0 0 0 32px;*/  }

.Playlist01 ol li { list-style:circle; }


/* Tiny Scrollbar */
#scrollbar1<?php echo $id; ?> { width: 215px;  }
#scrollbar1<?php echo $id; ?> .viewport { height: 164px; overflow: hidden; position: relative; }
#scrollbar1<?php echo $id; ?> .overview { list-style: none; position: absolute; left: 0; top: 0; padding: 0; margin: 0; width:209px;}
#scrollbar1<?php echo $id; ?> .scrollbar{ position: relative; background-position: 0 0; float: right; width: 15px; }
#scrollbar1<?php echo $id; ?> .track {height: 100%; width:13px; position: relative; padding: 0 1px; }


/*#scrollbar1 .thumb { background: transparent url(../images/bg-scrollbar-thumb-y.png) no-repeat 50% 100%; height: 20px; width: 20px; cursor: pointer; overflow: hidden; position: absolute; top: 0; left: 0px; }
#scrollbar1 .thumb .end { background: transparent url(../images/bg-scrollbar-thumb-y.png) no-repeat 50% 0; overflow: hidden; height: 5px; width: 20px; }
*/

#scrollbar1<?php echo $id; ?> .thumb .end {
    background: url("<?php echo  plugin_dir_url(__FILE__); ?>../images/bg-scrollbar-thumb-y.png") no-repeat scroll 50% 0 transparent;
    overflow: hidden;
    width: 20px;
}


#scrollbar1<?php echo $id; ?> .thumb {
    background: url("<?php echo  plugin_dir_url(__FILE__); ?>../images/bg-scrollbar-thumb-y.png") no-repeat scroll 50% 95% transparent;
    cursor: pointer;
    height: 20px;
    left: 0;
    overflow: hidden;
    position: absolute;
    top: 0;
    width: 20px;
	
	}


#scrollbar1<?php echo $id; ?> .disable { display: none; }

.audiojs {
	/*visibility:hidden !important;
	position:absolute  !important;*/
	width:0px;
	height:0px;
	
	
}

</style>