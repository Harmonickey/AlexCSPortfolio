var current_image = 1;
var next_image;
var current_max = 2;
var current_room_id = "1";
var room_delay = 5000;
var current_room;
var timeout;

$(function() {
	$('.roomsel,#roomnext,#roomprev').hover(function() {
		$(this).css('cursor', 'pointer');
	}, function() {
		$(this).css('cursor', 'default');
	});
	
	$('#roomnext,#roomprev').hover(function() {
		$(this).stop();
		$(this).animate({opacity: 1.0}, 220);
	}, function() {
		$(this).stop();
		$(this).animate({opacity: 0.0}, 220);
	});
	$('#roomnext').click(function() {nextroom(400);});
	$('#roomprev').click(function() {prevroom(400);});
	
	$('.roomsel').click(function() {switchroom(this, 400);});
	
  timeout = setTimeout(function(){nextroom(800)}, room_delay);
});


function nextroom(delay) {
	if (current_image < current_max)
		next_image = current_image + 1;
	else
		next_image = 1;
	
	toroom(next_image, delay);
	clearTimeout(timeout);
	timeout = setTimeout(function(){nextroom(800)}, room_delay);
	current_image = next_image;
}

function prevroom(delay) {
	if (current_image > 1)
		next_image = current_image - 1;
	else
		next_image = current_max;
	
	toroom(next_image, delay);
	clearTimeout(timeout);
	timeout = setTimeout(function(){nextroom(800)}, room_delay);
	current_image = next_image;
}	

// transition to a specific image #
function toroom(next, delay) {
	$('#roomphoto').append("<img height=\"480\" src=\"/images/rooms/"
	+ current_room_id
	+ "-" 
	+ String(next) 
	+ ".jpg\" />");
	$("<img height=\"480\" src=\"/images/rooms/"
	+ current_room_id
	+ "-" 
	+ String(next) 
	+ ".jpg\" />").load(function() {
		$('#roomphoto img:last').css({opacity: 0.0}).css('z-index', '9').animate({opacity: 1.0}, delay, function() {
			$('#roomphoto img:first').remove();
	});
	});
}

// switch to this person's room
function switchroom(new_room, delay) {
	if (current_room === $(new_room).text())
		return;
	
	current_image = 1;
	current_max = parseInt($(new_room).attr('num_images'));
	current_room_id = $(new_room).attr('room_id');
	
	// transition to selected room
	toroom(current_image, delay);
	clearTimeout(timeout);
	timeout = setTimeout(function(){nextroom(800)}, room_delay);
	
	var room_title = $('#roomtitle h2');
	$(room_title).stop(true);
	$(room_title).animate({opacity: 0}, delay/2, function() {$(room_title).text($(new_room).text())});
	$(room_title).animate({opacity: 1.0}, delay/2);
	
	var room_comments = $('#roomcomments p');
	$(room_comments).stop(true);
	$(room_comments).animate({opacity: 0}, delay/2, function() {
		$(room_comments).text('');
		$(room_comments).append($(new_room).attr('comments'));
	});
	$(room_comments).animate({opacity: 1.0}, delay/2);
	
	current_room = $(new_room).text();
}