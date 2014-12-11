var current_image = 1;
var next_image;
var image_delay = 5000;
var click_transition_time = 300;
var noclick_transition_time = 800;

// this is not a good solution, but it's not good 
// to embed all of the photos in the html either
// (slow page load)

var captions = [
"Awesome people on the third floor!",
"Residents of Shepard visit the garden of the Bahá'í temple.",
"A second floor hallway decorating party!",
"Alex and Matt play foosball in the TV Lounge.",
"Emai and Jane perform at Shepard coffeehouse.",
"Sheng and Greg perform at Shepard coffeehouse.",
"Classy sheep right before RCB Yacht Formal.",
"Classy sheep at the Mad Men-themed Shepard Formal.",
"Classy sheep right after Shepard's Mad Men-themed spring formal.",
"\"This is a holdup!\" - D'Weston and Ben Foster",
"A guest speaker discusses business attire at a student-run Shepard fashion show.",
"Assistant Master D'Weston Haywood demonstrates impeccable taste at the Shepard fashion show.",
"Sheep congratulate their friends on the Shepard Dance Marathon team after 30 hours.",
"Sheep prepare to compete in a quiz bowl in the Woo-Shep olympics.",
"Lee and Andrew after winning a round of the eating competition in the Woo-Shep olympics.",
"Residents of Shepard celebrate Joe's birthday [center].",
"Residents compete at a \"Minute-to-win-it\"-themed first floor event.",
"Ben battles for the pride of Shepard at RCB Field Day 2011.",
"SHEPARD BLOODKILL",
"Finishing the master's mile with D'Weston intact.",
"",
"Sheep dine in the Shepard dining room.",
"Professor Garthoff gives a fireside about the art of playing poker.",
"at a third floor classy christmas party",
"",
"Don't fall asleep with your MacBook in bed, folks.",
"Shepard paints the rock!",
"We are proud of ourselves.",
"Yay Munchies!",
"Sheep ice skating at Millenium Park."
];
var current_max = captions.length;

$(function() {
	$('#photoviewernext,#photoviewerprev,#lightsoff,#lightson').hover(function() {
		$(this).css('cursor', 'pointer');
	}, function() {
		$(this).css('cursor', 'default');
	});
	
	$('#photoviewernext,#photoviewerprev').hover(function() {
		$(this).stop();
		$(this).animate({opacity: 1.0}, 220);
	}, function() {
		$(this).stop();
		$(this).animate({opacity: 0.0}, 220);
	});
	
	$('#photoviewernext').click(function() {nextphoto(click_transition_time);});
	$('#photoviewerprev').click(function() {prevphoto(click_transition_time);});
	
	timeout = setTimeout(function(){nextphoto(noclick_transition_time)}, image_delay);
});

function nextphoto(transition_time) {
	if (current_image < current_max)
		next_image = current_image + 1;
	else
		next_image = 1;
	
	tophoto(next_image, transition_time);
	clearTimeout(timeout);
	timeout = setTimeout(function(){nextphoto(noclick_transition_time)}, image_delay);
	current_image = next_image;
}

function prevphoto(transition_time) {
	if (current_image > 1)
		next_image = current_image - 1;
	else
		next_image = current_max;
	
	tophoto(next_image, transition_time);
	clearTimeout(timeout);
	timeout = setTimeout(function(){nextphoto(noclick_transition_time)}, image_delay);
	current_image = next_image;
}	

// transition to a specific photo
function tophoto(next, transition_time) {
	$('#photoviewerphoto').append("<img height=\"640\" src=\"/htdocs/images/media/"
	+ String(next) 
	+ ".jpg\" />");
	// transition once image is loaded
	$("<img height=\"640\" src=\"/htdocs/images/media/"
	+ String(next) 
	+ ".jpg\" />").load(function() {
		$('#photoviewerphoto img:last').css({opacity: 0.0}).css('z-index', '7')
			.animate({opacity: 1.0}, transition_time, function() {
				$('#photoviewerphoto img:first').remove();
			});
	});
	
	var caption = $('#photoviewercaption');
	if (captions[next-1])
	{
		var caption_text = $('#photoviewercaptiontext');
		$(caption_text).stop(true);
		if ($(caption).is(':hidden'))
		{
			$(caption_text).text(captions[next-1]);
			$(caption).stop(true);
			$(caption).slideDown(transition_time/2);
		}
		else
		{
			$(caption_text).fadeOut(transition_time/2-40,
				function() {$(caption_text).text(captions[next-1]);});
			$(caption_text).fadeIn(transition_time/2-40);
		}
	}
	else
	{
		$(caption).slideUp(transition_time/2);
	}
}