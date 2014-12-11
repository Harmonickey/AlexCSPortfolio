// JavaScript Document

$(function() {
	$('.hoverimageleft,.hoverimageright,.hoverimage').hover(function() {
		$(this).children('div').stop();
		$(this).children('div').animate({opacity:1},200);
	}, function() {
		$(this).children('div').stop();
		$(this).children('div').animate({opacity:0},200);
	});
});