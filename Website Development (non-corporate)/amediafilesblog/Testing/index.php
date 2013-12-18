<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Snmp Execution Report Tool</title>
</head>

	<div id="top-section">
    	<div id="load_session">
        	<label for="load_list">Load Session </label>
            <select id="load_list" name="load_list">
            	<?php
					//search in the user id folder
					//then take out all the file names save in there
					$user = $_SESSION['user_id'];
					$exists = "";
					exec("if test -d $user; then echo 'okay'; fi", $exists);
					if ($exists == 'okay')
					{
						exec("cd $user");
						$files = array();
						exec("ls | tr ' ' '\n'", $files);
						for ($file=0; $file < count($files); $file++)
						{
							echo "$files[$file]";	
						}
					}
					
				?>
			</select>
        </div>
        <div id="ip_file_load">
        </div>
    </div>
    
    <div id="middle-section">
    	<div id="create_query">
        </div>
    </div>
    
    <div id="bottom-section">
    	<div id="save_session">
        </div>
        <div id="execute_query">
        </div>
    </div>

<body>
</body>
</html>