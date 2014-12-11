<?php

/*
Plugin Name: HTML5 MP3 Playlist Player Free
Plugin URI: http://html5.svnlabs.com/
Description: Allows Wordpress users to easily use HTML5 <audio> the element enable native audio playback within the browser. It supports all browsers i.e. Firefox, Chrome, Safari, IE and Opera. HTML5 Audio Player with Playlist, Repeat, Random, Stream Seek, Volume Control, Timer, Next, Previous, Play-Pause option.
Date: 2013, June, 1
Author: Sandeep Verma
Author URI: http://www.svnlabs.com/
Version: 2.7.0

*/

/*
Author: Sandeep Verma
Website: http://www.svnlabs.com
Copyright 2012 SVN Labs Softwares, Jaipur, India All Rights Reserved.

*/


//Database table versions
global $html5mp3_player_db_table_version, $local_variables, $localvariables;
$html5mp3_player_db_table_version = "2.7.0";
$local_variables = array();
$localvariables = array();


//Create database tables
function html5mp3_db_create () {
    html5mp3_create_table_player();
}


function html5mp3_create_table_player(){
    //Get the table name with the WP database prefix
    global $wpdb;
    $table_name_playlist = $wpdb->prefix . "html5mp3_playlist";
	$table_name_items = $wpdb->prefix . "html5mp3_items";
	$table_name_sales = $wpdb->prefix . "html5mp3_sales";
	
    global $html5mp3_player_db_table_version;
    $installed_ver = get_option( "html5mp3_player_db_table_version" );
     
	//Check if the table already exists and if the table is up to date, if not create it
    if($wpdb->get_var("SHOW TABLES LIKE '$table_name_playlist'") != $table_name_playlist ||  $installed_ver != $html5mp3_player_db_table_version ) {
        $sql = "CREATE TABLE " . $table_name_playlist . " (
					`id` int(11) NOT NULL AUTO_INCREMENT,
					`title` varchar(150) CHARACTER SET utf8 NOT NULL,
					`artist` varchar(60) NOT NULL,
					`description` text CHARACTER SET utf8 NOT NULL,
					`url` varchar(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
					`image` text NOT NULL,
					`facebook` varchar(255) NOT NULL,
					`twitter` varchar(255) NOT NULL,
					`link` varchar(255) NOT NULL,
					`size` varchar(5) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
					`email` varchar(100) NOT NULL,
					`brcolor` varchar(6) NOT NULL,
					`bgcolor` varchar(6) NOT NULL,
					`width` varchar(4) NOT NULL,
					`height` varchar(4) NOT NULL,
					`popout` enum('1','0') NOT NULL DEFAULT '0',
					`autoplay` enum('1','0') NOT NULL DEFAULT '0',
					`shuffle` enum('1','0') NOT NULL DEFAULT '1',
					`embed` enum('1','0') NOT NULL DEFAULT '1',
					`playlist` enum('1','0') NOT NULL DEFAULT '1',
					`scrollingtext` enum('1','0') NOT NULL DEFAULT '1',
					`toggleplaylist` enum('1','0') NOT NULL DEFAULT '1',
					`sourcetype` varchar(15) NOT NULL DEFAULT 'default',
					`sourceurl` text NOT NULL,
					`sourcedownload` enum('1','0') NOT NULL DEFAULT '0',
					`clientid` varchar(100) NOT NULL,
					`clientsecret` varchar(100) NOT NULL,
					`xml` varchar(50) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
					`sandbox` enum('1','0') CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '1',
					`adddate` datetime NOT NULL,
					PRIMARY KEY (`id`)
				);";

        require_once(ABSPATH . 'wp-admin/includes/upgrade.php');
        dbDelta($sql);
		}
		
		
		if($wpdb->get_var("SHOW TABLES LIKE '$table_name_items'") != $table_name_items ||  $installed_ver != $html5mp3_player_db_table_version ) {
        $sql = "CREATE TABLE " . $table_name_items . " (
					`id` int(11) NOT NULL AUTO_INCREMENT,
					`pid` int(11) NOT NULL,
					`title` varchar(255) CHARACTER SET utf8 NOT NULL,
					`artist` varchar(255) CHARACTER SET utf8 NOT NULL,
					`preview` varchar(255) CHARACTER SET utf8 NOT NULL,
					`amount` varchar(5) NOT NULL,
					`song` varchar(255) CHARACTER SET utf8 NOT NULL,
					`image` varchar(255) CHARACTER SET utf8 NOT NULL,
					`download` enum('1','0') NOT NULL DEFAULT '0',
					`buynow` enum('1','0') NOT NULL DEFAULT '0',
					`adddate` datetime NOT NULL,
					PRIMARY KEY (`id`)
				);";

        //require_once(ABSPATH . 'wp-admin/includes/upgrade.php');
        dbDelta($sql);		
		
        //update_option( "html5mp3_player_db_table_version", $html5mp3_player_db_table_version );
        }
		
		
		
		if($wpdb->get_var("SHOW TABLES LIKE '$table_name_sales'") != $table_name_sales ||  $installed_ver != $html5mp3_player_db_table_version ) {
        $sql = "CREATE TABLE " . $table_name_sales . " (
					`id` int(11) NOT NULL AUTO_INCREMENT,
					`pid` varchar(20) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
					`uid` varchar(50) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
					`email` varchar(25) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
					`amount` varchar(10) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
					`currency` varchar(3) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
					`saledate` datetime NOT NULL,
					`transactionid` varchar(125) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
					PRIMARY KEY (`id`)
				);";

        //require_once(ABSPATH . 'wp-admin/includes/upgrade.php');
        dbDelta($sql);		
		
        update_option( "html5mp3_player_db_table_version", $html5mp3_player_db_table_version );
        }
		
		
		
		
    //Add database table versions to options
    add_option("html5mp3_player_db_table_version", $html5mp3_player_db_table_version);
}

register_activation_hook( __FILE__, 'html5mp3_db_create' );


add_action( 'admin_menu', 'html5mp3_plugin_menu' );


function html5mp3_plugin_menu() {
	add_menu_page( 'HTML5 MP3 Player', 'HTML5 MP3 Player', 'manage_options', 'html5mp3_playlist', 'wp_html5mp3_playlist',plugin_dir_url( __FILE__ )."/html5mp3.png" );
	add_submenu_page('html5mp3_playlist','','','manage_options','html5mp3-options','wp_html5mp3_options');
	add_submenu_page('html5mp3_playlist', 'Manage Playlist', 'Manage Playlist', 'manage_options', 'html5mp3_playlist', 'wp_html5mp3_playlist' );
	add_submenu_page('html5mp3_playlist', 'Add Playlist', 'Add Playlist', 'manage_options', 'html5mp3_add_playlist', 'wp_add_html5mp3_playlist' );
	//add_submenu_page('html5mp3_playlist', 'PayPal', 'PayPal', 'manage_options', 'html5mp3_paypal', 'wp_add_html5mp3_paypal' );		
	add_submenu_page('html5mp3_playlist','Help','Help','manage_options','html5mp3_help','wp_html5mp3_help');
	
}


add_action( 'admin_init', 'register_html5mp3settings' );

function register_html5mp3settings() {
	/*register_setting( 'baw-settings-group', 'buy_text' );
	register_setting( 'baw-settings-group', 'color' );
	register_setting( 'baw-settings-group', 'showlist' );
	register_setting( 'baw-settings-group', 'showbuy' );
	register_setting( 'baw-settings-group', 'html5mp3_description' );
	register_setting( 'baw-settings-group', 'currency' );
	register_setting( 'baw-settings-group', 'tracks' );
	register_setting( 'baw-settings-group', 'tcolor' );*/
}



function wp_html5mp3_help() {

include 'html5plus/help.php';

}

function wp_html5mp3_preview() {

include 'html5plus/preview.php';

}


function wp_html5mp3_options() {

 global $wpdb;
	$table		=	$wpdb->prefix.'html5mp3_playlist';
	$itable		=	$wpdb->prefix.'html5mp3_items';

//include 'player/settings.php';
include 'html5plus/formplus.php';

}



function wp_html5mp3_playlist(){
		
include('html5plus/playlist.php');
		
}


function wp_add_html5mp3_playlist(){
		
include('html5plus/addplaylist.php');
		
}


function html5mp3_player1($content){
	
	
    global $wpdb;
	$table		=	$wpdb->prefix.'html5mp3_playlist';	
	  
	$pluginurl	=	plugin_dir_url( __FILE__ );

    //$regex = '/\[html5mp3 (.*?)]/i';
	
	$regex = '/\[html5mp3(\s+id=([0-9]+))?(\s+type=([a-z]+))?\s*}(.*)\]/i';
    preg_match_all( $regex, $content, $matches );
	//echo "<pre>";
	//print_r($matches);

    //include('html5plus/html5.php');

    $player_div	=	'<div id="myplayer">'.$content.'</div>';
    return $player_div;

}


function html5mp3playlist_content($content) {
    global $html5mp3playlist_sizes, $current_site, $local_variables;
     
	//echo $current_site; 
	
	//$local_variables = array();
	 
    $size     = intval(get_option('html5mp3playlist_size'));
    
     
	$regex = '/\[html5mp3(full|big|small):(.*?)]/i';
    preg_match_all( $regex, $content, $matches );
	//echo "<pre>";
	//print_r($matches);
	
	$sc = count($matches[0]);
	
	
	
	wp_register_script( 'html5mp3playlist', plugins_url('html5plus/js/html5mp3playlist-min.js', __FILE__) , array('jquery'));
	wp_enqueue_script( 'html5mp3playlist' );
	
	
		
	
	for($ij=0;$ij<$sc;$ij++)
	{
	
     
	if($matches[1][$ij]=="full") 
	{ 
	 
	$local_variables[] = array('html5mp3playlistid' => $matches[2][$ij], 'html5mp3playlistspan' => 'html5mp3playlist'.$matches[2][$ij], 'html5mp3playlistsize' => 'full');
	 
	
	$replace = '<span id="html5mp3playlist'.$matches[2][$ij].'"></span>';
	 
	}
    else if($matches[1][$ij]=="big") 
	{
	
	$local_variables[] = array('html5mp3playlistid' => $matches[2][$ij], 'html5mp3playlistspan' => 'html5mp3playlist'.$matches[2][$ij], 'html5mp3playlistsize' => 'big');
	
	
	$replace = '<span id="html5mp3playlist'.$matches[2][$ij].'"></span>';
	
	}
	else
	{
	
	$local_variables[] = array('html5mp3playlistid' => $matches[2][$ij], 'html5mp3playlistspan' => 'html5mp3playlist'.$matches[2][$ij], 'html5mp3playlistsize' => 'small');
	 
	
	$replace = '<span id="html5mp3playlist'.$matches[2][$ij].'"></span>';
	
	}
	
	
	
    $content = str_replace($matches[0][$ij], $replace, $content);
	
	}
	
	wp_localize_script( 'html5mp3playlist', 'vars', $local_variables );
    
    
    return $content;
}


function wp_html5mp3_player( $atts, $content = null ) {

    global $wpdb, $localvariables;
	$table		=	$wpdb->prefix.'html5mp3_playlist';
    $itable	=	$wpdb->prefix.'html5mp3_items';
    $stable	=	$wpdb->prefix.'html5mp3_sales';	
	  
	$pluginurl	=	plugin_dir_url( __FILE__ );
	
	$docdata = $wpdb->get_results( "select * from `".$table."` where id = '".$atts['id']."' " );
	
	//print_r($docdata);
	
	//$localvariables = array();

   extract( shortcode_atts( array(
		'id' => '1',
		'width' => '600',
		'height' => '250',
		'fcolor' => '343434',
		'bcolor' => 'ff0000',
		'tcolor1' => 'ffffff',
		'tcolor2' => 'a19b9b',
		'dlicon' => '',
		'dlpos' => '10',
		'links' => '0',
		'stitle' => '0',
		'size' => $docdata[0]->size,
		'embed' => $docdata[0]->embed
	), $atts ) );

 
	
	/* Actual Player code */
	
	//echo $size;
	
	
	if( $embed == "1" )
	{
	 
	 $localvariables[] = array('html5mp3playlistid' => $atts['id'], 'html5mp3playlisturl' => $pluginurl.'html5plus/', 'html5mp3playlistspan' => 'html5mp3playlists'.$atts['id'], 'html5mp3playlistsize' => 'full');
	 
	
	 $mp3content = '<span id="html5mp3playlists'.$atts['id'].'"></span>';
	 
	 //print_r($local_variables);
	 
	}
	else
	{
     include("html5plus/html5full.php");
    }
	
	wp_localize_script( 'html5mp3playlist', 'varss', $localvariables );
	
	
	/* Actual Player code */

    return '<span>' . $mp3content . '</span>';
}


add_shortcode('html5mp3','wp_html5mp3_player');

add_filter('the_content','html5mp3playlist_content');

//add_filter('the_content','wp_html5mp3_player');

function html5mp3_init() {

	if (!is_admin()) {
		wp_enqueue_script('jquery');
	}
}
add_action('init', 'html5mp3_init');


?>