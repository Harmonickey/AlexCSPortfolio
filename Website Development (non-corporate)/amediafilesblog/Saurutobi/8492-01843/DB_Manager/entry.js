// JavaScript Document

function validateForm()
{
	var fields = ["title", "post", "spoiler", "IMDB_URL"];
	var form = "blog_post_form";
	
	for (i = 0; i < len(fields); i++)
	{
		var x = document.forms[form][fields[i]].value;
		if (x == null || x == "")
		{
			alert(fields[i] + " must be filled out");
			return false;	
		}
	}
}