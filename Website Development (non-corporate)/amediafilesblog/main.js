// JavaScript Document

function showMore() 
{	
	var length = $('#middle').find('.entry-container').length;
	var counter = 0;
	
	//loop through each of the .entry children of #middle
	$('#middle').find('.entry-container').each(function(i) //pass through the index
	{ 
    	if($(this).css('display') === 'none') //if we find an invisible entry...
		{
			if (counter === 5) return false;  //if we found 5 more after that...
			counter++;  
			
			$(this).css('display', 'block');  //change to visible
			if (i === length - 1)
			{
				$('.showmore').css('display', 'none');  //delete the showmore button if we don't have any more entries...
			}
		}
	});
	
	extendMiddle(); //extend the page to hold the extra entries
	fixTitles();
	createPostShowMore();
}

function showMorePost(entry)
{
	var $paragraph = $(entry).parent('.entry').find('.postparagraph');
	if ($paragraph.height() > 98)
	{
		if ($paragraph.hasClass('expanded'))  //if the paragraph has been expanded
		{
			//if the spoiler notes are showing then don't detract quite as much...
			if ($(entry).parent('.entry').hasClass('showing'))
			{
				var $spoilerParagraphHeight = $(entry).parent('.entry').find('.spoiler > p').height();
				var newHeight = 200 + $spoilerParagraphHeight;
				$(entry).parent('.entry').animate({height: newHeight}, 'slow');
			}
			else
			{
				$(entry).parent('.entry').animate({height: "200px"}, 'slow');
			}
			var heightChange = $paragraph.height() - 98;
			$(entry).parent('.entry').find('.info').animate({height: "98px"}, 'slow');
			$('#main').animate({height: "-=" + heightChange}, 'slow');
			$paragraph.removeClass('expanded');
		}
		else  //if the paragraph has not been expanded
		{
			var heightChange = $paragraph.height() - 98;
			$(entry).parent('.entry').animate({height: "+=" + heightChange}, 'slow');
			$(entry).parent('.entry').find('.info').animate({height: "+=" + heightChange}, 'slow');
			$('#main').animate({height: "+=" + heightChange}, 'slow');
			$paragraph.addClass('expanded');
		}
	}
}

function setProperties() {
	var length = $('#middle').find('.entry-container').length;
	
	$('#middle').find('.entry-container').each(function(i) //loop through each entry and pass through its index
	{ 
    	if(i > 4) //if we have more than five, set those to invisible...
		{
			$(this).css('display', 'none');
		}
	});
	
}

function extendMiddle() {
	
	var $middle = $('#middle');
	var $main = $('#main');
	var $intendedHeight = $middle.height();  //get the height dynamically created by #middle
	$main.height($intendedHeight + 40);  //assign that to main, to adjust
}

function fixTitles() {
	$('.entry').each(function() {
		var $paragraph = $(this).find('.titleparagraph');
		while ($paragraph.height() > 27)
		{
			$paragraph.css('font-size', '-=' + 1);
		}
	});
}

function createPostShowMore() {
	$('.entry').each(function() {
		var $paragraph = $(this).find('.postparagraph');
		if ($paragraph.height() > 98)
		{
			$(this).find('.showmorepost').css('display', 'block');
			$(this).find('.info').height(98);
		}
	});
}

