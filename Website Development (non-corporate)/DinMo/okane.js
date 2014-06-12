// JavaScript Document

var keys = [];

$(function () { keys = getClientIDs(); });

function getEverything() {
	var zip = getZipCode();
	if (!validZip(zip))
		return;
	
	$("#suggestion h2").prop("hidden", false);
	$("suggestion h4").prop("hidden", false);
	$("#zip").prop("hidden", true);
	$("#search").prop("hidden", true);
	$("#greeting").prop("hidden", true);
	$("#extra").prop("hidden", true);
	$("#leftText").prop("hidden", false);
	$("#rightText").prop("hidden", false);
	$("#leftTitle").prop("hidden", false);
	$("#rightTitle").prop("hidden", false);
	
	$.ajax(
	{
		type: 'GET',
		url: 'http://api.rottentomatoes.com/api/public/v1.0/lists/movies/in_theaters.json',
		data:
		{
			page_limit: '16',
			page: '1',
			country: 'us',
			apikey: keys[0]
		},
		dataType: 'jsonp',
		success: function(res)
		{
			//get the result of restaurants
			var result_array = res['movies'];
			//sort by rating
			result_array.sort(function(obj1, obj2)
			{
				return obj2['ratings']['audience_score'] - obj1['ratings']['audience_score']; 
			});
		  
		    var x = document.cookie.split(';');
			var to_cont = false;
			var offset = 0;
			var suggestion = '';
			//append to the table
			for (var i = 0; i < 10 + offset; i++)
			{
				for (var j = 0; j < x.length && x[0] != ""; j++) 
				{
				  if (x[j].split('=')[1].indexOf(result_array[i]['title']) != -1) 
				  {
					  to_cont = true;
					  offset += 1;
					  break;
				  }
				}
				
				if (to_cont) {
				    to_cont = false;
					continue;
				}
				
				if (suggestion == '') {
					suggestion = result_array[i]['title'];
				}
					  
				$("#body_result2").append("<tr><td>" + (i + 1 - offset) + "</td><td><a href='" + result_array[i]['links']['alternate'] + "' onclick='saveCookie(this);'>" + result_array[i]['title'] + "</a></td><td>" + result_array[i]['ratings']['audience_score'] + "</td></tr>");
			}
		  
			$("#movie").append(suggestion);
		}
	});
	  
    $.ajax(
	{
		type: 'GET',
		url: 'https://query.yahooapis.com/v1/public/yql',
		data:
		{
			q: 'select * from local.search(0, 100) where query="restaurants" and zip=' + zip,
			format: 'json'
		},
		dataType: 'json',
		success: function(res)
		{
			//get the result of restaurants
			var restaurants = res['query']['results']['Result'];
		    
			//sort by rating
			restaurants.sort(function(obj1, obj2)
			{
				return (obj2['Rating']['AverageRating'] != "NaN" ? obj2['Rating']['AverageRating'] : obj2['Rating']['TotalRatings']) - (obj1['Rating']['AverageRating'] != "NaN" ? obj1['Rating']['AverageRating'] : obj1['Rating']['TotalRatings']); 
			});
		    
			var x = document.cookie.split(';');
			var to_cont = false;
			var offset = 0;
			var suggestion = '';
			//append to the table
			for (var i = 0; i < 10 + offset; i++)
			{
				for (var j = 0; j < x.length && x[0] != ""; j++) 
				{
				  if (x[j].split('=')[1].indexOf(restaurants[i]['Title']) != -1) 
				  {
					  to_cont = true;
					  offset += 1;
					  break;
				  }
				}
				
				if (to_cont) {
					to_cont = false;
					continue;
				}
				
				if (suggestion == '') {
					suggestion = restaurants[i]['Title'];
				}
				
				$("#body_result").append("<tr><td>" + (i + 1 - offset) + "</td><td><a href='" + (restaurants[i]['BusinessUrl'] != null ? restaurants[i]['BusinessUrl'] : restaurants[i]['Url']) + "' onclick='saveCookie(this);'>" + restaurants[i]['Title'] + "</a></td><td>" + (restaurants[i]['Rating']['AverageRating'] != "NaN" ? restaurants[i]['Rating']['AverageRating'] : restaurants[i]['Rating']['TotalRatings']) + "</td></tr>");
			}
		    
			$("#dinner").append(suggestion);
		}
	});
	  /*  failed Groupon integration for coupons to dinner places...
	$.ajax(
	{
		type: 'GET',
		url: 'http://api.groupon.com/v2/deals.json',
		data:
		{
			radius: '20',
			postal_code: zip,
			client_id: keys[1]
		},
		dataType: 'jsonp',
		success: function(res)
		{
			//get the result of restaurants
			var result_array = res['deals'];
			
			for (var i = 0; i < restaurants.length; i++)
			{
				for (var j = 0; j < result_array.length; j++)
				{
					var merchant = result_array[j]['merchant']['name'];
				
					if (merchant == restaurants[i])
					{
					console.log(merchant);	
					}
				}
			}
		  
			for (var i = 0; i < result_array.length; i++)
			{
				var merchant = result_array[i]['merchant']['name'];
			
			
				if (merchant == number_one)
				{
				$("#deal").append(result_array[i]['announcementTitle']);
				}
			}	
		}
	});
	*/
}

function getClientIDs()
{
	var retVal = new Array();
	$.ajax(
	{
		type: 'GET',
		url: 'https://okane.firebaseio.com/.json',
		dataType: 'json',
		async: false,
		success: function(res)
		{
			retVal = [res['rot_tom'], res['groupon']];
		}
	});
	return retVal;
}

function getZipCode() {
	return $("#zip").val();	
}

function validZip(zip) {
	var parts = zip.split('');
	if (parts.length != 5)
		return false;
	for (var i = 0; i < parts.length; i++) {
	  if (isNaN(parts[i]))
	  	return false;	
	}
	return true;
}

function saveCookie(element) {
  var title = $(element).html();
  var num = document.cookie.split(';');
  document.cookie="visited" + (num[0] == "" ? 0 : num.length) + "=" + title;
}