function onTick1(value) {
		
            //jQuery('#text1').html("Current Value: " + value);
			
			//audio.setVolume(value*10);
			
			var  ids = ['vol-0', 'vol-10', 'vol-20', 'vol-30', 'vol-40', 'vol-50', 'vol-60', 'vol-70', 'vol-80', 'vol-90'];
			
			if(value>=10) value=9;
			if(value<=0) value=0;		
			
			//var elem = document.getElementById(ids[value]);
			
			//elem.click();
			jQuery('#'+ids[value]+'').click();
			
			
        }
		
		
		function SocialShareFB() 
		{
		
		
         var str = jQuery('#current-track'+id).html();
		 var mp3p = jQuery('#current-play-pic'+id).attr('src');
		 
		 var currt = str.replace("&"," ");
		
         var atitle = encodeURI( "I listen to " + currt  ); 
         var bhref = encodeURI(document.location.href);
		 
	
		 
		 var url = 'http://www.facebook.com/sharer.php?s=100&p[title]='+atitle+'&p[url]='+bhref+'&p[images][0]='+mp3p+'&';
		 
		 window.open(url,"Mywindow","location=yes,menubar=yes");
		
		}
		
		
		function SocialShareTwitter() 
		{
		
		
		var str = jQuery('#current-track'+id).html();
		var mp3p = jQuery('#current-play-pic'+id).attr('src');
		
		 var currt = str.replace("&"," ");
		
		var atitle = encodeURI( "I listen to " + currt  ); 
         var bhref = encodeURI(document.location.href);
		 
		 var url = 'https://twitter.com/intent/tweet?status='+atitle+' '+bhref+'&amp;url='+bhref;
		 
		 window.open(url,"Mywindow","location=yes,menubar=yes");
		
		
		
		}
		
		

	function addParameterToURL(_url, param){
    //_url = location.href;
    _url += (_url.split('?')[1] ? '&':'?') + param;
    return _url;
}
