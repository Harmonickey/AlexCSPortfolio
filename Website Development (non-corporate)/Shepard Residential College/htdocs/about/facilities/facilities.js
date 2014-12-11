// a list apart style photo viewer 
var oldpic = '';

function showPic (whichpic) {
 if (document.getElementById) {
  document.getElementById('photoselect').src = whichpic.href;
  return false;
 } else {
  return true;
 }
}