<?php
$HTTP_user = 'Exec';
$HTTP_password = 'mischiefmayhemsoap';

list($_SERVER['PHP_AUTH_USER'], $_SERVER['PHP_AUTH_PW']) = 
  explode(':', base64_decode(substr($_SERVER['HTTP_AUTHORIZATION'], 6)));

if (ucfirst($_SERVER['PHP_AUTH_USER']) != $HTTP_user or $_SERVER['PHP_AUTH_PW'] != $HTTP_password) {
    header('WWW-Authenticate: Basic realm="Exec"');
    header('HTTP/1.0 401 Unauthorized');
    echo 'You are not authorized to enter this area';
    exit;
}
?>