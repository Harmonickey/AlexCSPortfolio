<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Post Form</title>

<link rel="stylesheet" type="text/css" href="entry.css">
<script type="text/javascript" src="entry.js"></script>
</head>

<body>

<div id="forminsert">
    <form class="entry" action="insert.php" method="post" name="blog_post_form" onsubmit="return validateForm()">
    
    
    		<h3 class="text"> Title: </h3>
        <input class="title" type="text" name="title" />
        
        
        	<h3 class="text" style="margin-right: 10px;">Media Type:</h3>
        <select class="mediatype" name="mediatype">
            <option selected>Movie
            <option>TV Show
            <option>Music
            <option>Podcast
		</select>
        
        	<h3 class="text" style="margin-right: 10px;">Report:</h3>
        <select class="Report" name="Report">
            <option selected>Watch it!
            <option>Meh
            <option>Avoid
        </select>
        
        	<h3 class="text" style="clear: both;">Review/Summary:</h3>
		<textarea class="post" name="post" rows="5" cols="80"></textarea>
        
        
        	<h3 class="text" style="clear: both;">Spoiler:</h3>
        <textarea class="post" name="spoiler" rows="5" cols="80"></textarea>
        
        	<h3 class="text" style="clear: both;">IMDB URL:</h3>
        <textarea class="post" name="IMDB_URL" rows="1" cols="80"></textarea>
        
        </br>
        </br>
        
		<input class="submit" style="clear: both; float: left;" type="submit" name="submit" value="Submit">
    
    </form>
</div>
</body>
</html>

