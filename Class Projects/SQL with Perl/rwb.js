//
// Global state
//
// map     - the map object
// usermark- marks the user's position on the map
// markers - list of markers on the current map (not including the user position)
// 
//

//
// First time run: request current location, with callback to Start

var $committee;
var $candidate;
var $individual;
var $opinion;


$(document).ready(function() {

  $committee = $('#committee');
  $candidate = $('#candidate');
  $individual = $('#individual');
  $opinion = $('#opinion');

});


if (navigator.geolocation)  {
    navigator.geolocation.getCurrentPosition(Start);
}


function UpdateMapById(id, tag) {

    var target = document.getElementById(id);
    var data = target.innerHTML;

    var rows  = data.split("\n");
   
    for (i in rows) {
	var cols = rows[i].split("\t");
	var lat = cols[0];
	var long = cols[1];

	markers.push(new google.maps.Marker({ map:map,
						    position: new google.maps.LatLng(lat,long),
						    title: tag+"\n"+cols.join("\n")}));
	
    }
}

function ClearMarkers()
{
    // clear the markers
    while (markers.length>0) { 
	markers.pop().setMap(null);
    }
}

ShowOpiniondata()
{


}


function UpdateMap()
{
    var color = document.getElementById("color");
    
    color.innerHTML="<b><blink>Updating Display...</blink></b>";
    color.style.backgroundColor='white';

    ClearMarkers();

    if ($committee.is(':checked'))
    {
    	UpdateMapById("committee_data","COMMITTEE");

    }
    if ($candidate.is(':checked'))
    	UpdateMapById("candidate_data","CANDIDATE");
    if ($individual.is(':checked'))
    {
    	UpdateMapById("individual_data", "INDIVIDUAL");
      
    
    }
    if ($opinion.is(':checked'))
    {
      UpdateMapById("opinion_data","OPINION");
       results.innerHTML= "Average: " +
        parseFloat(document.getElementById('opinionAverage').value).toFixed(5) + "<br> Standard Deviation: " + 
        parseFloat(document.getElementById('opinionStddev').value).toFixed(5) + "<br>";
    
    
    }
    	summaries.innerHTML= "Democrat Contributions: " +
        parseFloat(document.getElementById('demIndv').value) + "<br> Republican Contributions: " + 
        parseFloat(document.getElementById('repIndv').value) + "<br>";


    color.innerHTML="Ready <br>";

   if ((parseFloat(document.getElementById('demIndv').value))>0)
   {
      color.style.backgroundColor='red';
      summaries.style.backgroundColor='red';
      results.style.backgroundColor='green';
   }
   else 
   {
      color.style.backgroundColor='blue';
      summaries.style.backgroundColor='blue';
      results.style.backgroundColor='green';
   }
   
    
   
}

function NewData(data)
{
  var target = document.getElementById("data");
  
  target.innerHTML = data;

  UpdateMap();

}

function ViewShift()
{
    var bounds = map.getBounds();

    var ne = bounds.getNorthEast();
    var sw = bounds.getSouthWest();

    var color = document.getElementById("color");

    color.innerHTML="<b><blink>Querying...("+ne.lat()+","+ne.lng()+") to ("+sw.lat()+","+sw.lng()+")</blink></b>";
    color.style.backgroundColor='white';
   
    var what = "";

    // debug status flows through by cookie
    var perlString = "rwb.pl?act=near&latne="+ne.lat()+"&longne="+ne.lng()+"&latsw="+sw.lat()+"&longsw="+sw.lng()+"&format=raw&what=";

    if ($committee.is(':checked'))
	perlString += "committees,";
    if ($candidate.is(':checked'))
	perlString += "candidates,";
    if ($individual.is(':checked'))
	perlString += "individuals,";
    if ($opinion.is(':checked'))
	perlString += "opinions,";
    
    // slice off the last comma
    perlString = perlString.slice(0, -1);

    perlString += "&cycle=" + selectedCycles();

    $.get(perlString, NewData);
}

function selectedCycles()
{
  var $cycles = $('.cycles');
  var cycles_checked = "";

  for (var i = 0; i < $cycles.length; i++)
  {
    if ($($cycles[i]).is(':checked'))
    {
      cycles_checked += '"' + $($cycles[i]).attr('id') + '",';
    }
  }

  cycles_checked = cycles_checked.slice(0, -1);

  return cycles_checked;

}


function Reposition(pos)
{
    var lat=pos.coords.latitude;
    var long=pos.coords.longitude;

    map.setCenter(new google.maps.LatLng(lat,long));
    usermark.setPosition(new google.maps.LatLng(lat,long));
}


function Start(location) 
{
  var lat = location.coords.latitude;
  var long = location.coords.longitude;
  var acc = location.coords.accuracy;
  
  var mapc = $( "#map");

  map = new google.maps.Map(mapc[0], 
			    { zoom:16, 
				center:new google.maps.LatLng(lat,long),
				mapTypeId: google.maps.MapTypeId.HYBRID
				} );

  usermark = new google.maps.Marker({ map:map,
					    position: new google.maps.LatLng(lat,long),
					    title: "You are here"});

  markers = new Array;

  var color = document.getElementById("color");
  color.style.backgroundColor='white';
  color.innerHTML="<b><blink>Waiting for first position</blink></b>";

  google.maps.event.addListener(map,"bounds_changed",ViewShift);
  google.maps.event.addListener(map,"center_changed",ViewShift);
  google.maps.event.addListener(map,"zoom_changed",ViewShift);

  navigator.geolocation.watchPosition(Reposition);

}


