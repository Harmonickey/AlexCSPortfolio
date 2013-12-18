function removeParentHover(that) {
	//assign the element
	var element = $(that);
	//get the parent list item
	var parentElement = $(element.parent('li'));
	
	//change it's css
	var originalID = parentElement.attr('id');
	parentElement.addClass(originalID);
	
	parentElement.attr('id', originalID + '_revert');
	
}

function revertParentHover(that) {
	var element = that;
	
	var parentElement = $($(element).parent('li'));
	//revert back to original
	var originalID = parentElement.attr('class');
	parentElement.removeAttr('class');
	
	parentElement.attr('id', originalID);
	
}