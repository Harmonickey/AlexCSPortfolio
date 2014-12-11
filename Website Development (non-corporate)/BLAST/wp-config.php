<?php
/**
 * The base configurations of the WordPress.
 *
 * This file has the following configurations: MySQL settings, Table Prefix,
 * Secret Keys, WordPress Language, and ABSPATH. You can find more information
 * by visiting {@link http://codex.wordpress.org/Editing_wp-config.php Editing
 * wp-config.php} Codex page. You can get the MySQL settings from your web host.
 *
 * This file is used by the wp-config.php creation script during the
 * installation. You don't have to use the web site, you can just copy this file
 * to "wp-config.php" and fill in the values.
 *
 * @package WordPress
 */

// ** MySQL settings - You can get this info from your web host ** //
/** The name of the database for WordPress */
define('DB_NAME', 'blast');

/** MySQL database username */
define('DB_USER', 'blast');

/** MySQL database password */
define('DB_PASSWORD', '9yVs7e9e7fAEd3rw');

/** MySQL hostname */
define('DB_HOST', 'localhost');

/** Database Charset to use in creating database tables. */
define('DB_CHARSET', 'utf8');

/** The Database Collate type. Don't change this if in doubt. */
define('DB_COLLATE', '');

/**#@+
 * Authentication Unique Keys and Salts.
 *
 * Change these to different unique phrases!
 * You can generate these using the {@link https://api.wordpress.org/secret-key/1.1/salt/ WordPress.org secret-key service}
 * You can change these at any point in time to invalidate all existing cookies. This will force all users to have to log in again.
 *
 * @since 2.6.0
 */
define('AUTH_KEY',         'NgRyOxJS=`ZBPD0[;x<_cMliaoSR&+<d6OT1`qPP :>),a5w*$TNsB47uVxeb]28');
define('SECURE_AUTH_KEY',  '#p|lQA!XT;/9>BiU|[zIExp=-NV[x2@):-j+@ MW[FT%0Q*6CeGXT7^X+h;=uL+e');
define('LOGGED_IN_KEY',    'U}+;L|mXv+U>HAj{erV)30kG$0o-k+FE0(U%3FfVo0Q!&/L{ la<.<~r<bTxRV94');
define('NONCE_KEY',        'xC9A$?-*L0-<O_FP) ,4wP?5xJy8.2(I9pFXbvKwH8b-m^qcG9,l@#w6-y|b-+WA');
define('AUTH_SALT',        'Svqn6a+U&Tj[aHe ;_LKriuauL{&n`^07@<b9jAPY|,,7hEY(bwPe+pK.TdU[gKM');
define('SECURE_AUTH_SALT', '-G[]t-[ExsuQ/!XQtUK,XqG?R^Z~*E^scxSY<XkpD]*`vlNrht$W`3:@[ul<<Kba');
define('LOGGED_IN_SALT',   '%jgOBB,`Giw}xD2.=p{*#6+*bhC5;Xs<;J;.xp-I8fk-,v~DK.+bcyOm1GVGcYU*');
define('NONCE_SALT',       'UY/G_b|< 6$%2{MYIA3Vj%H$.QDKkWd+e8w&:][6;38 NMJIj?I*m%egFX:&^Ac1');

/**#@-*/

/**
 * WordPress Database Table prefix.
 *
 * You can have multiple installations in one database if you give each a unique
 * prefix. Only numbers, letters, and underscores please!
 */
$table_prefix  = 'wp_blast';

/**
 * WordPress Localized Language, defaults to English.
 *
 * Change this to localize WordPress.  A corresponding MO file for the chosen
 * language must be installed to wp-content/languages. For example, install
 * de.mo to wp-content/languages and set WPLANG to 'de' to enable German
 * language support.
 */
define ('WPLANG', '');

/**
 * For developers: WordPress debugging mode.
 *
 * Change this to true to enable the display of notices during development.
 * It is strongly recommended that plugin and theme developers use WP_DEBUG
 * in their development environments.
 */
define('WP_DEBUG', false);

/* That's all, stop editing! Happy blogging. */

/** Absolute path to the WordPress directory. */
if ( !defined('ABSPATH') )
	define('ABSPATH', dirname(__FILE__) . '/');

/** Sets up WordPress vars and included files. */
require_once(ABSPATH . 'wp-settings.php');
