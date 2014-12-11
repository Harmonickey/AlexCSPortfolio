/* js for faq page - expanding questions */

$(function() {
	/* answers hidden by default; expand when clicked */
	$('dl#faq').find('dd').hide().end().find('dt').click(function() {
		$(this).next().slideToggle();
		$(this).toggleClass('expanded');
	});
	
	/* user affordances: pointer is link pointer when hovering over clickable question */
	$('dl#faq').find('dd').hide().end().find('dt').hover(function() {
		$(this).css('cursor', 'pointer');
	}, function() {
		$(this).css('cursor', 'default');
	});
	$('b#exp, b#col').hover(function() {
		$(this).css('cursor', 'pointer');
	}, function() {
		$(this).css('cursor', 'default');
	});
	
	/* "expand all" option */
	$('b#exp').click(function() {
		$("dl#faq").find("dt").each(function() {
     $(this).next().slideDown();
		 $(this).addClass('expanded');
		});
  });
	/* "collapse all" option */
	$('b#col').click(function() {
		$("dl#faq").find("dt").each(function() {
     $(this).next().slideUp();
		 $(this).removeClass('expanded');
		});
  });
	
});