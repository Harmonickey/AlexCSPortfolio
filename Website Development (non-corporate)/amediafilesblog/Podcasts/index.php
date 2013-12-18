<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>A MediaPhile's Blog | Podcasts</title>
		<meta name="keywords" content="Media, Movies, Videos, Music, TV, Television, Shows">
		<meta name="description" content="A MediaPhile's Blog is a blog about Movies, T.V. Shows, Music, and more. Any consumable Media has a review here. Review missing, want to see one? Just suggest it">
		<meta name="author" content="Saurutobi">
		<link rel="stylesheet" type="text/css" href="../main.css">
        <script type="text/javascript" src="../main.js"></script>
        <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
  		<script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
        <script>
		  $(function() {
			$( "#accordion" ).accordion();
		  });
		</script>
	</head>
	<body>
    <script type="text/javascript">
  		var _gaq = _gaq || [];
  		_gaq.push(['_setAccount', 'UA-40112954-1']);
  		_gaq.push(['_trackPageview']);
  		(function()
		{
    		var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    		ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    		var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  		})();
	</script>
    <div id="container">
    
    	<div id="header">
        	<p>Welcome to A MediaPhile's Blog</p>
        </div>
        
        <div id="nav">
			<ul>
            	<li><a href="../">Home</a></li>
                <li><a href="../Movies">Movies</a></li>
                <li><a href="../TV Shows">TV Shows</a></li>
                <li><a href="../Music">Music</a></li>
                <li><a href="../Podcasts">Podcasts</a></li>
                <li><a href="../About">About</a></li>
            </ul>
        </div>
        
        <div id="main">
        	<div class="left">
            	<script type="text/javascript"><!--
					google_ad_client = "ca-pub-7999659877433575";
					/* Left Side */
					google_ad_slot = "3813931644";
					google_ad_width = 160;
					google_ad_height = 600;
					//-->
				</script>
				<script type="text/javascript"
					src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
				</script>
            </div>
        	<div id="middle">
				<?php
					function getRows($limit) 
					{
						//host data
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
						
						//make the query for all the blog posts
						$sql = "SELECT * FROM BlogPosts 
									WHERE MediaType = 'Podcast'
									ORDER BY Date DESC;";
						$result = mysqli_query($con, $sql);  //send the query by the server
						$counter = 0;  //keep track of how many entries we receive from the server
						
						echo '<div id=accordion>';
						while($row = mysqli_fetch_array($result))  //catch rows from the query
						{
							//create an entry
							echo '<h3>' . $row['Title'] . '</h3>';
							echo '<div class="entry">';
								echo '<div class="mediatype"><p>' . $row['MediaType'] . '</p></div>';
								echo '<div class="title"><p>' . $row['Title'] . '</p>';
									$postdate = date("d-m-Y", strtotime($row['Date']));  //grab the date a reset to more userfriendly format
									echo '<div class="date"><p>' . $postdate . '</p></div>';
									echo '<div class="report"><p>' . $row['Report'] . '</p></div>';
								echo '</div>';
								echo '<div class="post"><p>' . $row['Post'] . '</p></div>';
								echo '<div class="spoilerexpand"></div>';
								echo '<div class="post spoiler">';
									echo '<p>' . $row['Spoiler'] . '</p>';
								echo '</div>';
							echo '</div>';
							$counter++;  //increase by one entry created
						}
						echo '</div>';
						
						//if we encounter more than the limit for the initial page, then include a showmore button
						//also call setProperties to make all the extra entries invisible
						if ($counter > $limit) 
						{
								echo '<div id="showmore" onclick="showMore()"> Show More </div>';
								echo '<script> setProperties() </script>';
						}
						
						mysqli_close($con);
						
						//call the extendDiv function
						echo '<script> extendMiddle() </script> ';
					}
					
					//grab all the rows, but with a limit of 5 to be displayed
					getRows(5);
					
                ?>
                <script>
					$(".spoilerexpand").click(function() {
						//first get the actual height the particular paragraph
						if ($(this).parent('.entry').hasClass('showing'))
						{
							var paragraphHeight = $(this).siblings('.spoiler').find('p').height();  //grab the height
							console.log(paragraphHeight);
							$('.spoiler').height(paragraphHeight);
						}
						else
						{
							$(this).siblings('.spoiler').css('display', 'block');  //turn visible
								var paragraphHeight = $(this).siblings('.spoiler').find('p').height();  //grab the height
								console.log(paragraphHeight);
								$(this).siblings('.spoiler').height(paragraphHeight);	
							$(this).siblings('.spoiler').css('display', 'none');   //turn back invisible
						}
						
						//then toggle the spoiler post while also adjusting the height of the entry
						var heightChange = $(this).siblings('.spoiler').height();
						$(this).siblings('.spoiler').slideToggle("slow");
						
						if ($(this).parent('.entry').hasClass('showing'))  //if we're showing the spoiler
						{
							$(this).parent('.entry').removeClass('showing');
							$(this).css('background-image', 'url(../Images/spoileropen.png) no-repeat');  //use the reverse image
							$(this).parent('.entry').animate({height: "-=" + heightChange}, 'slow');
						}
						else  //we're not showing the spoiler
						{
							$(this).parent('.entry').addClass('showing');
							$(this).css('background-image', 'url(../Images/spoileclose.png) no-repeat'); //use the forward image
							$(this).parent('.entry').animate({height: "+=" + heightChange}, 'slow');
						}
					});
				</script>
          </div>
        	
            <div class="search">
            	<p>Search</p>
            
            </div>
            
            <div class="right">
            
            	<script type="text/javascript"><!--
					google_ad_client = "ca-pub-7999659877433575";
					/* right side */
					google_ad_slot = "8244131245";
					google_ad_width = 160;
					google_ad_height = 600;
					//-->
				</script>
				<script type="text/javascript"
					src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
				</script>
            </div>
        </div>
    
    </div>
    
    <div id="footer">
        <span>
            <p>Copyright 2013 by Saurutobi. Created by LbKStudios
            <a href="http://www.amediaphilesblog.com/DMCA/">DMCA Compliance</a>
            </p>
        </span>
    </div>
    
	</body>
</html>