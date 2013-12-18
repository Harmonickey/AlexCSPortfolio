<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>A MediaPhile's Blog | T.V. Shows</title>
		<meta name="keywords" content="Media, Movies, Videos, Music, TV, Television, Shows">
		<meta name="description" content="A MediaPhile's Blog is a blog about Movies, T.V. Shows, Music, and more. Any consumable Media has a review here. Review missing, want to see one? Just suggest it">
		<meta name="author" content="Saurutobi">
		<link rel="stylesheet" type="text/css" href="../main.css">
        <script type="text/javascript" src="../main.js"></script>
        <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
  		<script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
        <script src="http://www.adipalaz.com/scripts/jquery/jquery.nestedAccordion.js"></script>
        <script>
		  
		  $(function() {
			$( "#accordion" ).accordion({
				collapsible: true
			});
		  });
		  
		  //$('#post-container').accordion({el:'.h', head:'h2, h3', collapsible: true, next:'div', initShow:'div.outer:eq(1)'});
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
									WHERE MediaType = 'TV Show'
									ORDER BY Date DESC;";
						$result = mysqli_query($con, $sql);  //send the query by the server
						
						/*
						$sql = "SELECT * FROM Blogposts
									GROUP BY Season
									ORDER BY Season;";  //should be alphabetical
						$seasons = mysqli_query($con, $sql);
						
						*/
						//echo <div id="post-container">
						//echo '<ul class="accordion">';
						echo '<div id="accordion">';
						//while ($row_se = mysqli_fetch_array($seasons))
						while($row = mysqli_fetch_array($result))  //catch rows from the query
						{
							/*
							//create an entry
							echo '<li>';
								echo '<h2>' . $row_se['Season'] . '</h2>';   //******Title of Season  $row_se['Season']
								echo '<div>';
									echo '<ul>';
										//********while loop for the episodes.....
										
										$sql = "SELECT * FROM Blogposts
											GROUP BY Episode
											WTIH Season = " . $row['Season'] . "
											ORDER BY Episode;";
										$episodes = mysqli_query($con, $sql); 
										while ($row_ep = mysqli_fetch_array($episodes))
										{
											echo '<li>';
												echo '<h3>' . $row_ep['Title'] . '</h3>';
												echo '<div class="entry" style="background-color: #B165D4;">';
													echo '<div class="mediatype"><p>' . $row_ep['MediaType'] . '</p></div>';  //$row_ep
													echo '<div class="title" ><p class="titleparagraph">' . $row_ep['Title'] . '</p>';  //$row_ep
														$postdate = date("d-m-Y", strtotime($row_ep['Date']));  //grab the date a reset to more  userfriendly format
														echo '<script> fixTitles() </script>';
														echo '<div class="date"><p>' . $postdate . '</p></div>';
														echo '<div class="report"><p>' . $row_ep['Report'] . '</p></div>';  //$row_ep
													echo '</div>';
													echo '<div class="button"><a href="' . $row_ep['IMDB'] . '" target="_blank"><img src="../Images/IMDB Button image.png" /></a></div>';  //$row_ep
													echo '<div class="post info"><p class="postparagraph">' . $row_ep['Post'] . '</p></div>';  //$row_ep
													echo '<div class="showmorepost" onclick="showMorePost(this)" height="10px"> Show More </div>';
													echo '<script> createPostShowMore() </script>';
													echo '<div class="spoilerexpand" style="margin-top: 0px"></div>';
													echo '<div class="post spoiler">';
														echo '<p>' . $row_ep['Spoiler'] . '</p>';
													echo '</div>';
												echo '</div>';
											echo '</li>';
										}
										//******** end of while loop
									echo '</ul>';
								echo '</div>';
							*/
							echo '<h3>' . $row['Title'] . '</h3>';
							echo '<div class="entry" style="background-color: #B165D4;">';
								echo '<div class="mediatype"><p>' . $row['MediaType'] . '</p></div>';
								echo '<div class="title" ><p class="titleparagraph">' . $row['Title'] . '</p>';
									$postdate = date("d-m-Y", strtotime($row['Date']));  //grab the date a reset to more userfriendly format
									echo '<script> fixTitles() </script>';
									echo '<div class="date"><p>' . $postdate . '</p></div>';
									echo '<div class="report"><p>' . $row['Report'] . '</p></div>';
								echo '</div>';
								echo '<div class="button"><a href="' . $row['IMDB'] . '" target="_blank"><img src="../Images/IMDB Button image.png" /></a></div>';
								echo '<div class="post info"><p class="postparagraph">' . $row['Post'] . '</p></div>';
								echo '<div class="showmorepost" onclick="showMorePost(this)" height="10px"> Show More </div>';
								echo '<script> createPostShowMore() </script>';
								echo '<div class="spoilerexpand" style="margin-top: 0px"></div>';
								echo '<div class="post spoiler">';
									echo '<p>' . $row['Spoiler'] . '</p>';
								echo '</div>';
							echo '</div>';
							
						}
						echo '</div>';
						//echo '</div>';
						mysqli_close($con);
						
						//call the extendDiv function
						echo '<script> extendMiddle() </script> ';
					}
					
					//grab all the rows, but with a limit of 5 to be displayed
					getRows(0);
					
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