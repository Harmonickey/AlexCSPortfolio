/* javascript for shepardrc.com design candidate */

$(function() {
	// enables the slideshow at an interval of 4500ms
	setInterval("slides()", 4500);
	
	// enables the back to top button
	var scrollDiv = document.createElement("div");
				$(scrollDiv).attr("id", "toTop").html("Back to Top").appendTo("body");    
				$(window).scroll(function () {
						if ($(this).scrollTop() > 600) {
							$("#toTop").fadeIn();
						} else {
							$("#toTop").fadeOut();
						}
					});
					$("#toTop").click(function () {
						$("body,html").animate({
							scrollTop: 0
						},
						800);
					});
});

/* this code drives the slideshow in the title */
/* credit to Jon Raasch for his helpful tutorial */
// I considered using a cookie to remember slideshow position,
// but then it's messy to save the elapsed display time.
// Without saving that, some photos are displayed for a long time.
var current_slide;
function slides() {
	var $active = $('#slideshow img.active');
	if ($active.length === 0) $active = $('#slideshow img:last');
	
	/* make slideshow wrap around */
	var $next = $active.next().length? $active.next()
		: $('#slideshow img:first');
	
	$active.addClass('last-active');
	$next.css({opacity: 0.0})
		.addClass('active')
		.animate({opacity: 1.0}, 1000, function() {
			$active.removeClass('active last-active');
		});
}

