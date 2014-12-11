<?php
//initial directories to get to htdocs
$uno = '/home/users/web/b277/pow.shepardadmin/htdocs/htdoc_dir.php';
include $uno;
//array of possible pages
$duos = '/home/users/web/b277/pow.shepardadmin/htdocs/page_array.php';
include $duos;
//functions
include 'functions.php';
$called_page = $_GET['page'];
$return_page = page_status($called_page,$page_array);

?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml"><!-- InstanceBegin template="/Templates/main.dwt" codeOutsideHTMLIsLocked="false" -->
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<meta http-equiv="cache-control" content="no-cache">
		<meta http-equiv="pragma" content="no-cache">
		
		<!-- InstanceBeginEditable name="doctitle" -->
		<title><?php echo ucfirst($return_page['page']);?> | Shepard Residential College</title>
		<!-- InstanceEndEditable -->
		<link rel="icon" type="image/ico" href="<?php echo $html_htdocs; ?>favicon.ico"/>
		<link rel="stylesheet" type="text/css" media="screen" href="<?php echo $html_htdocs; ?>main.css" title="CSS" />
		<link rel="stylesheet" type="text/css" media="screen" href="<?php echo $html_htdocs; ?>mouseover.css" title="CSS" />
		<script src="<?php echo $html_htdocs; ?>jquery-1.7.2.min.js" type="text/javascript"></script>
		<script src="<?php echo $html_htdocs; ?>main.js" type="text/javascript"></script>
		
		<!-- InstanceBeginEditable name="head" -->
		
		<?php
		//inputting css
		foreach ($return_page['css'] as $value){
			?>
			<link rel="stylesheet" type="text/css" media="screen" href="<?php echo $html_htdocs . $return_page['folder'] . '/' . $return_page['subfolder'] . $value; ?>" />
			<?php
			//echo '<link rel="stylesheet" type="text/css" href="' . $html_htdocs . $return_page['folder'] . '/' . $return_page['subfolder'] . $value . '"/>';
		}
		//inputting js
		foreach ($return_page['js'] as $value){
			echo '<script src="' . $html_htdocs . $return_page['folder'] . '/' . $return_page['subfolder'] . $value . '" type="text/javascript"></script>';
		}
		?>

		<!-- InstanceEndEditable -->
		<script type="text/javascript">

			var _gaq = _gaq || [];
			_gaq.push(['_setAccount', 'UA-31492000-1']);
			_gaq.push(['_trackPageview']);

			(function() {
				var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
				ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
				var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
			})();

		</script>
	</head>

	<body>
		<div id="container">
			<div id="header">
				<div id="slideshow">
					<img src="<?php echo $html_htdocs; ?>images/shep1.jpg" class="active" />
					<img src="<?php echo $html_htdocs; ?>images/shep2.jpg" />
					<img src="<?php echo $html_htdocs; ?>images/shep3.jpg" />
					<img src="<?php echo $html_htdocs; ?>images/shep4.jpg" />
					<img src="<?php echo $html_htdocs; ?>images/shep5.jpg" />
					<img src="<?php echo $html_htdocs; ?>images/shep6.jpg" />
					<img src="<?php echo $html_htdocs; ?>images/shep7.jpg" />
				</div>
				<span></span>
				<a href="../"><h1><blue>Shepard</blue> Residential College</h1></a>
			</div>
  <!-- Dropdown Menu -->
			<div id="nav">
				<ul>
					<li><a href="<?php echo $html_htdocs; ?>v/home">Home</a></li>
					<li><a href="about">About &#187;</a><ul>
						<li><a href="activities">Activities</a></li>
						<li><a href="facilities">Facilities</a></li>
						<li><a href="traditions">Traditions</a></li></ul></li>
					<li><a href="events">Events</a></li>
					<li><a href="government">Government &#187;</a><ul>
						<li><a href="exec">Exec Board</a></li>
						<li><a href="scc">SCC</a></li>
						<li><a href="masters">Masters</a></li>
						<li><a href="policies">Policies</a></li></ul></li> 
					<li><a href="residents">Residents &#187;</a><ul>
						<li><a href="ca">CAs &amp; RHC</a></li>
						<li><a href="directory">Directory</a></li>
						<li><a href="intellectuwool">Intellectuwool</a></li>
						<li><a href="listserv">Listserv Info</a></li>
						<li><a href="minutes">Minutes</a></li></ul></li>
					<li><a href="fellows">Fellows</a></li>
					<li><a href="media">Media</a></li>
					<li><a href="prosp">For prospectives &#187;</a><ul>
						<li><a href="faq">FAQ</a></li>
						<li><a href="nonres">Non-Res</a></li>
						<li><a href="resources">Resources</a></li>
						<li><a href="rooms">Sample Rooms</a></li></ul></li>
				</ul>
			</div>

			<div id="main"><!-- InstanceBeginEditable name="EditRegion3" -->
			
		

<?php
//inserting main content
$meh = $php_htdocs . $return_page['folder'] . '/' . $return_page['subfolder'] . $return_page['page'] . '.php';
include "$meh";
?>
<!-- InstanceEndEditable --></div>
  <!-- to extend the container to the bottom -->
<div id="spacer"></div>
</div>

<div id="footer">
<img id="footleft" src="<?php echo $html_htdocs; ?>sheplogowhite.png" />
<div>
<p>&#169;2013 Shepard Residential College</p>
<p>626 University Place, Evanston, IL 60201</p>
<p><a href="http://www.northwestern.edu/residentialcolleges/" target="_blank">Northwestern University Residential Colleges</a> | <a href="http://www.wildcats.northwestern.edu/rcb/" target="_blank">Residential College Board</a></p>
<p>Questions? Comments? Suggestions? Email <a href="mailto:tech@shepardrc.com" target="_blank">tech@shepardrc.com</a>.</p>
</div>
<img id="footright" src="<?php echo $html_htdocs; ?>nulogo.png" />
</div>


</body>
<!-- InstanceEnd -->
</html>
