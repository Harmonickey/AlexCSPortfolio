<?php function page_status ($page, $array){
	$i=0;
	$length = count($array);
	$return_array = 0;
	while ($i < $length && $return_array == 0){
		if(in_array($page, $array[$i])){
			$return_array = $array[$i];
		}
		$i = ++$i;
	}
	if ($return_array == 0){
		$return_array = array (
		'page' => '404',
		'folder' => '404',
		'css' => array (),
		'js' => array(),
		);
	}
	if ($return_array['page']==$return_array['folder']){
		$return_array['subfolder'] = false;
	}
	else {
		$return_array['subfolder'] = $return_array['page'] . '/';
	}
	return $return_array;
}
?>