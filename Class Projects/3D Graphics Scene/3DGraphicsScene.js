//Alex Ayerdi
// Modified script that uses some shapes and phong shading from Jack Tumblin's scripts.  
//      This program includes
//         All the shapes from Proj C, except some extra joints on the sponge and pyramid shapes
//         Phong Shading, with Blinn-Phong H value calculation.
//         Two light sources, the non-headlight one being the controllable.
//         One camera that can fly through the scene from user controls.
//         Adjustable orbit speed.
//         Different material reflectance types for various objects.
//
// Vertex shader program----------------------------------

var VSHADER_SOURCE = 
  'uniform mat4 u_ViewMatrix;\n' +
  'uniform mat4 u_ProjMatrix;\n' +
  'uniform mat4 u_NormalMatrix; \n' +
  'uniform vec3 u_Kd; \n' +
  
  'attribute vec4 a_Position;\n' +
  'attribute vec4 a_Normal;\n' +
  
  'varying vec4 v_Position; \n' +
  'varying vec3 v_Normal; \n' +
  'varying vec3 v_Kd; \n' +
  
  'void main() {\n' +
  '  gl_Position = u_ProjMatrix * u_ViewMatrix * a_Position;\n' +
  '  v_Position = u_ViewMatrix * a_Position; \n' +
  '  v_Normal = normalize(vec3(u_NormalMatrix * a_Normal));\n' +
  '  v_Kd = u_Kd; \n' +	
  '}\n';

// Fragment shader program----------------------------------
var FSHADER_SOURCE = 
  '#ifdef GL_ES\n' +
  'precision mediump float;\n' +
  '#endif GL_ES\n' +
  //first light source 
  'uniform vec4 u_Lamp0Pos;\n' + 	 // Phong Illum: position
  'uniform vec3 u_Lamp0Amb;\n' +   	 // Phong Illum: ambient
  'uniform vec3 u_Lamp0Diff;\n' +    // Phong Illum: diffuse
  'uniform vec3 u_Lamp0Spec;\n' +	 // Phong Illum: specular
  
  //second light source	
  'uniform vec4 u_Lamp1Pos;\n' + 	 // Phong Illum: position
  'uniform vec3 u_Lamp1Amb;\n' +   	 // Phong Illum: ambient
  'uniform vec3 u_Lamp1Diff;\n' +    // Phong Illum: diffuse
  'uniform vec3 u_Lamp1Spec;\n' +	 // Phong Illum: specular	
	
  'uniform vec3 u_Ke;\n' +							// Phong Reflectance: emissive
  'uniform vec3 u_Ka;\n' +							// Phong Reflectance: ambient
  'uniform vec3 u_Ks;\n' +							// Phong Reflectance: specular
  'uniform float u_Kshiny;\n' +	                    // Phong Reflectance: 1 < shiny < 200
  
  'uniform float u_ambientOn;\n' +
  'uniform float u_diffuseOn;\n' +
  'uniform float u_specularOn;\n' +
  'uniform float u_emissiveOn;\n' +
  
  'uniform vec4 u_eyePosWorld; \n' + 		// Camera/eye location in world coords.
  'uniform vec4 u_light2PosWorld; \n' +     // location for second light, similar to eyePosWorld
  
  'varying vec3 v_Normal;\n' +				// Find 3D surface normal at each pix
  'varying vec4 v_Position;\n' +			// pixel's 3D pos too -- in 'world' coords
  'varying vec3 v_Kd;	\n' +
  
  'void main() {\n' +
  '  vec3 normal = normalize(v_Normal); \n' +

  '  vec3 lightDirection = normalize(u_Lamp0Pos.xyz - v_Position.xyz);\n' +
  '  vec3 lightDirection2 = normalize(u_Lamp1Pos.xyz - v_Position.xyz);\n' +
  
  '  float nDotL = max(dot(lightDirection, normal), 0.0); \n' +
  '  float nDotL2 = max(dot(lightDirection2, normal), 0.0); \n' + 
  
  '  vec3 eyeDirection = normalize(u_eyePosWorld.xyz - v_Position.xyz); \n' +
  '  vec3 eyeDirection2 = normalize(u_light2PosWorld.xyz - v_Position.xyz); \n' +
  
  '  vec3 H = normalize(lightDirection + eyeDirection); \n' +
  '  vec3 H2 = normalize(lightDirection2 + eyeDirection2); \n' +
 
  '  float nDotH = pow(max(dot(H, normal), 0.0), u_Kshiny); \n' +
  '  float nDotH2 = pow(max(dot(H2, normal), 0.0), u_Kshiny); \n' +
  
  '  float e02 = nDotH*nDotH; \n' +
  '  float e04 = e02*e02; \n' +
  '  float e08 = e04*e04; \n' +
  '  float e16 = e08*e08; \n' +
  '  float e32 = e16*e16; \n' +
  '  float e64 = e32*e32;	\n' +
  
  '  float e02_1 = nDotH2*nDotH2; \n' +
  '  float e04_1 = e02_1*e02_1; \n' +
  '  float e08_1 = e04_1*e04_1; \n' +
  '  float e16_1 = e08_1*e08_1; \n' +
  '  float e32_1 = e16_1*e16_1; \n' +
  '  float e64_1 = e32_1*e32_1;	\n' +

  '	 vec3 emissive = u_Ke * u_emissiveOn;' +
  
  '  vec3 ambient = u_Lamp0Amb * u_Ka * u_ambientOn;\n' +
  '  vec3 diffuse = u_Lamp0Diff * v_Kd * nDotL * u_diffuseOn;\n' +
  '	 vec3 speculr = u_Lamp0Spec * u_Ks * e64 * e64 * u_specularOn;\n' +
  
  '  vec3 ambient2 = u_Lamp1Amb * u_Ka * u_ambientOn;\n' +
  '  vec3 diffuse2 = u_Lamp1Diff * v_Kd * nDotL2 * u_diffuseOn;\n' +
  '	 vec3 speculr2 = u_Lamp1Spec * u_Ks * e64_1 * e64_1 * u_specularOn;\n' +
  
  '  gl_FragColor = vec4(emissive + ambient + diffuse + speculr + ambient2 + diffuse2 + speculr2 , 1.0);\n' +
  '}\n';

var material;
  
// Global Variables
var sphVerts = {};
var normals = {};
var torVerts = [];
var cylVerts = [];
var cubeVerts = [];
var axisVerts = [];
var ANGLE_STEP = 45.0;	// Rotation angle rate (degrees/second)
var ANGLE_STEP_SECOND = 45.0;
var floatsPerVertex = 7; // x,y,z,w, r,g,b # of Float32Array elements used for each vertex
var isDrag = false;
var xMclik=0.0;			// last mouse button-down position (in CVV coords)
var yMclik=0.0;   
var xMdragTot=0.0;	// total (accumulated) mouse-drag amounts (in CVV coords).
var yMdragTot=0.0;
var qNew = new Quaternion(0,0,0,1); // most-recent mouse drag's rotation
var qTot = new Quaternion(0,0,0,1);	// 'current' orientation (made from qNew)
var quatMatrix = new Matrix4();	
var g_EyeX = 0.0, g_EyeY = 0.0, g_EyeZ = 2.5;
var g_LookAtX = g_EyeX;
var g_LookAtY = g_EyeY;
var g_LookAtZ = g_EyeZ - 2.5;

var ambientOn = true;
var diffuseOn = true;
var specularOn = true;
var emissiveOn = true;
var shinyOn = true;

var lightX = -6.0;
var lightY =  0.0;
var lightZ =  0.0;

var lampX = -6.0;
var lampY = 0.0;
var lampZ = 0.0;

var lamp0Amb = [0.4, 0.4, 0.4];
var lamp0Diff = [1.0, 1.0, 1.0];
var lamp0Spec = [1.0, 1.0, 1.0];

var lamp1Amb = [0.1, 0.1, 0.1];
var lamp1Diff = [0.8, 0.8, 0.8];
var lamp1Spec = [0.8, 0.8, 0.8];

function main() {
//==============================================================================
	// Retrieve <canvas> element
	setLampValues();
	canvas = document.getElementById('webgl');
	resizeCanvas();
	// Get the rendering context for WebGL
	var gl = getWebGLContext(canvas);
	if (!gl) {
	console.log('Failed to get the rendering context for WebGL');
	return;
	}

	// Initialize shaders
	if (!initShaders(gl, VSHADER_SOURCE, FSHADER_SOURCE)) {
	console.log('Failed to intialize shaders.');
	return;
	}

	// 
	var n = initVertexBuffer(gl);
	if (n < 0) {
	console.log('Failed to set the vertex information');
	return;
	}

	//capture mouse clicks or space bar events
	canvas.onmousedown =	function(ev){ myMouseDown( ev, gl, canvas) }; 
	canvas.onmousemove = 	function(ev){ myMouseMove( ev, gl, canvas) };
	canvas.onmouseup = function(ev){ myMouseUp( ev, gl, canvas)};
	$('body').mouseleave(function() { myMouseUp(null, gl, canvas); });
	$(window).mouseleave(function() { myMouseUp(null, gl, canvas); });
	
	$(window).on('resize', resizeCanvas);

	// Specify the color for clearing <canvas>
	gl.clearColor(0.0, 0.0, 0.0, 1.0);

	gl.enable(gl.DEPTH_TEST); 	  

	var u_eyePosWorld = gl.getUniformLocation(gl.program, 'u_eyePosWorld');
	var u_light2PosWorld = gl.getUniformLocation(gl.program, 'u_light2PosWorld');

	var u_ViewMatrix = gl.getUniformLocation(gl.program, 'u_ViewMatrix');
	var u_ProjMatrix = gl.getUniformLocation(gl.program, 'u_ProjMatrix');
	var u_NormalMatrix = gl.getUniformLocation(gl.program, 'u_NormalMatrix');
	if (!u_ViewMatrix || !u_ProjMatrix || !u_NormalMatrix || 
	    !u_eyePosWorld || !u_light2PosWorld) { 
		console.log('Failed to get u_ViewMatrix or u_ProjMatrix or lights');
		return;
	}
  
	//  ... for Phong light source:
	var u_Lamp0Pos  = gl.getUniformLocation(gl.program, 'u_Lamp0Pos');
	var u_Lamp0Amb  = gl.getUniformLocation(gl.program, 'u_Lamp0Amb');
	var u_Lamp0Diff = gl.getUniformLocation(gl.program, 'u_Lamp0Diff');
	var u_Lamp0Spec	= gl.getUniformLocation(gl.program,	'u_Lamp0Spec');
	
	var u_Lamp1Pos  = gl.getUniformLocation(gl.program, 'u_Lamp1Pos');
	var u_Lamp1Amb  = gl.getUniformLocation(gl.program, 'u_Lamp1Amb');
	var u_Lamp1Diff = gl.getUniformLocation(gl.program, 'u_Lamp1Diff');
	var u_Lamp1Spec	= gl.getUniformLocation(gl.program,	'u_Lamp1Spec');
	if( !u_Lamp0Pos || !u_Lamp0Amb || !u_Lamp0Diff || !u_Lamp0Spec ||
	    !u_Lamp1Pos || !u_Lamp1Amb || !u_Lamp1Diff || !u_Lamp1Spec) {
		console.log('Failed to get the Lamp0 or Lamp1 storage locations');
		return;
	}
	// ... for Phong material/reflectance:
	var u_Ke = gl.getUniformLocation(gl.program, 'u_Ke');
	var u_Ka = gl.getUniformLocation(gl.program, 'u_Ka');
	var u_Kd = gl.getUniformLocation(gl.program, 'u_Kd');
	var u_Ks = gl.getUniformLocation(gl.program, 'u_Ks');
	var u_Kshiny = gl.getUniformLocation(gl.program, 'u_Kshiny');

	if(!u_Ke || !u_Ka || !u_Kd || !u_Ks || !u_Kshiny) {
		console.log('Failed to get the Phong Reflectance storage locations');
		return;
	}
	
	var lightReflectances = [u_Ke, u_Ka, u_Kd, u_Ks, u_Kshiny];
	
	var u_ambientOn = gl.getUniformLocation(gl.program, 'u_ambientOn');
	var u_diffuseOn = gl.getUniformLocation(gl.program, 'u_diffuseOn');
	var u_specularOn = gl.getUniformLocation(gl.program, 'u_specularOn');
	var u_emissiveOn = gl.getUniformLocation(gl.program, 'u_emissiveOn');
	
	// Position the first light source in World coords: 
	// Set its light output:  
  
    // Create a local version of our model matrix in JavaScript 
    var viewMatrix = new Matrix4();
    var persMatrix = new Matrix4();
    var normalMatrix = new Matrix4();
  
    // for moving the camera
    document.onkeydown = function(ev){ keydown(ev); };
  
    persMatrix.setPerspective(40, canvas.width/canvas.height, 0.01, 100);
  
    // Create, init current rotation angle value in JavaScript
    var currentAngle = 0.0;
    var topAngle = 0.0;
	
    // Start drawing: create 'tick' variable whose value is this function:
    var tick = function() {
	
		//update rotation angles for solar system
		currentAngle = animate(currentAngle);
		topAngle = animateSecond(topAngle);
	
		gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
	
		//perspective view
		drawCamera(gl, viewMatrix, u_ViewMatrix, persMatrix, u_ProjMatrix, 
		           normalMatrix, u_NormalMatrix);
		
		drawScene(gl, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix, 
		           lightReflectances, currentAngle, topAngle);
		
		getLight(gl, u_ambientOn, u_diffuseOn, u_specularOn, u_emissiveOn,
		         u_eyePosWorld, u_light2PosWorld, 
				 u_Lamp0Pos, u_Lamp0Amb, u_Lamp0Diff, u_Lamp0Spec,
				 u_Lamp1Pos, u_Lamp1Amb, u_Lamp1Diff, u_Lamp1Spec);
		
		requestAnimationFrame(tick, canvas); // redraw webpage
    };
    tick();	// start (and continue) animation: draw current image
}

function resizeCanvas() {
   var width = $(window).width();
   var height = $(window).height();
   if (canvas.width != width || canvas.height != height) {
	 canvas.width = width;
	 canvas.height = height;
   }
}

function initVertexBuffer(gl) {
//==============================================================================
// Create one giant vertex buffer object (VBO) that 
// holds all vertices for all shapes.
	
 	// Make each 3D shape in its own array of vertices:
    makeSun();	// create, fill the sunVerts array
	makeEarth(); // create, fill the earthVerts array
	makeMoon(); // create, fill the moonVerts array
	
	makeTorus(); // create, fill the torVerts array
	makeCylinder(); // create, fill the cylVerts array
	makePyramid(); //create, fill the pyrVerts array
	makeCube(); //create, fill the cubeVerts array
	
	makeGroundGrid();
	
    // how many floats total needed to store all shapes?
	var mySiz = (sphVerts["sun"].length + 
	             sphVerts["earth"].length +
				 sphVerts["moon"].length +
				 torVerts.length + 
				 cylVerts.length + 
				 pyrVerts.length +
				 cubeVerts.length +
				 gndVerts.length);
	
	// How many vertices total?
	var nn = mySiz / floatsPerVertex;
	
	// Copy all shapes into one big Float32 array:
    var colorShapes = new Float32Array(mySiz);
	
	// Copy them:  remember where to start for each shape:
	sunStart = 0;
	for(i=0,j=0; j< sphVerts["sun"].length; i++,j++) {
		colorShapes[i] = sphVerts["sun"][j];
	}
	earthStart = i;
	for(j=0; j< sphVerts["earth"].length; i++,j++) {
		colorShapes[i] = sphVerts["earth"][j];
	}
	moonStart = i;
	for(j=0; j< sphVerts["moon"].length; i++,j++) {
		colorShapes[i] = sphVerts["moon"][j];
	}
	torStart = i;
	for(j=0; j< torVerts.length; i++,j++) {
		colorShapes[i] = torVerts[j];
	}
	cylStart = i;
	for(j=0; j< cylVerts.length; i++,j++) {
		colorShapes[i] = cylVerts[j];
	}
	pyrStart = i;
	for(j=0; j< pyrVerts.length; i++,j++) {
		colorShapes[i] = pyrVerts[j];
	}
	cubeStart = i;
	for(j=0; j< cubeVerts.length; i++,j++) {
		colorShapes[i] = cubeVerts[j];
	}
	gndStart = i; 
	for(j=0; j< gndVerts.length; i++, j++) {
		colorShapes[i] = gndVerts[j];
	}
		
  // Create a buffer object on the graphics hardware:
  var shapeBufferHandle = gl.createBuffer();  
  if (!shapeBufferHandle) {
    console.log('Failed to create the shape buffer object');
    return false;
  }
  
  // how many bytes per stored value?
  var FSIZE = colorShapes.BYTES_PER_ELEMENT;
  
  // Bind the the buffer object to target:
  gl.bindBuffer(gl.ARRAY_BUFFER, shapeBufferHandle);
  // Transfer data from Javascript array colorShapes to Graphics system VBO
  gl.bufferData(gl.ARRAY_BUFFER, colorShapes, gl.STATIC_DRAW);
  

  //---------------------POSITION-----------------------
  //Get graphics system's handle for our Vertex Shader's position-input variable 
  var a_Position = gl.getAttribLocation(gl.program, 'a_Position');
  if (a_Position < 0) {
    console.log('Failed to get the storage location of a_Position');
    return -1;
  }

  gl.vertexAttribPointer(a_Position, 4, gl.FLOAT, false, FSIZE * floatsPerVertex, 0);
  gl.enableVertexAttribArray(a_Position); 
  
  //------------------------NORMAL------------------------------
  // Get graphics system's handle for our Vertex Shader's normal-input variable;
  var a_Normal = gl.getAttribLocation(gl.program, 'a_Normal');
  if (a_Normal < 0) {
	console.log('Failed to get the storage location of a_Normal');
    return -1;  
  }
  
  gl.vertexAttribPointer(a_Normal, 3, gl.FLOAT, false, FSIZE * floatsPerVertex, 0);
  gl.enableVertexAttribArray(a_Normal);
  
  // Unbind the buffer object 
  gl.bindBuffer(gl.ARRAY_BUFFER, null);

  return nn;
}

function makeGroundGrid() {
//==============================================================================
// Create a list of vertices that create a large grid of lines in the x,y plane
// centered at x=y=z=0.  Draw this shape using the GL_LINES primitive.

    var xcount = 100;           // # of lines to draw in x,y to make the grid.
    var ycount = 100;       
    var xymax   = 50.0;         // grid size; extends to cover +/-xymax in x and y.
    var xColr = new Float32Array([1.0, 0.4, 0.0]);  // bright yellow
    var yColr = new Float32Array([1.0, 0.2, 0.2]);  // bright green.
    
    // Create an (global) array to hold this ground-plane's vertices:
    gndVerts = new Float32Array(floatsPerVertex*2*(xcount+ycount));
                        // draw a grid made of xcount+ycount lines; 2 vertices per line.
                        
    var xgap = xymax/(xcount-1);        // HALF-spacing between lines in x,y;
    var ygap = xymax/(ycount-1);        // (why half? because v==(0line number/2))
    
    // First, step thru x values as we make vertical lines of constant-x:
    for(v=0, j=0; v<2*xcount; v++, j+= floatsPerVertex) {
        if(v%2==0) {    // put even-numbered vertices at (xnow, -xymax, 0)
            gndVerts[j  ] = -xymax + (v  )*xgap;    // x
            gndVerts[j+1] = -xymax;                             // y
            gndVerts[j+2] = 0.0;                                    // z
			gndVerts[j+3] = 1.0;
			
        }
        else {              // put odd-numbered vertices at (xnow, +xymax, 0).
            gndVerts[j  ] = -xymax + (v-1)*xgap;    // x
            gndVerts[j+1] = xymax;                              // y
            gndVerts[j+2] = 0.0;                                    // z
			gndVerts[j+3] = 1.0;
			
        }
        gndVerts[j+4] = xColr[0];           // red
        gndVerts[j+5] = xColr[1];           // grn
        gndVerts[j+6] = xColr[2];           // blu
    }
    // Second, step thru y values as wqe make horizontal lines of constant-y:
    // (don't re-initialize j--we're adding more vertices to the array)
    for(v=0; v<2*ycount; v++, j+= floatsPerVertex) {
        if(v%2==0) {        // put even-numbered vertices at (-xymax, ynow, 0)
            gndVerts[j  ] = -xymax;                             // x
            gndVerts[j+1] = -xymax + (v  )*ygap;    // y
            gndVerts[j+2] = 0.0;                                    // z
			gndVerts[j+3] = 1.0;
			
        }
        else {                  // put odd-numbered vertices at (+xymax, ynow, 0).
            gndVerts[j  ] = xymax;                              // x
            gndVerts[j+1] = -xymax + (v-1)*ygap;    // y
            gndVerts[j+2] = 0.0;                                    // z
			gndVerts[j+3] = 1.0;   //w

        }
        gndVerts[j+4] = yColr[0];           // red
        gndVerts[j+5] = yColr[1];           // grn
        gndVerts[j+6] = yColr[2];           // blu
    }
}

function makeEarth() {
	var topColor = new Float32Array([0.3, 0.3, 0.8]);	// blue top
	var bottomColor = new Float32Array([0.3, 0.3, 0.8]);	// blue bottom
	
	makeSphere({top: topColor, bot: bottomColor, "planet": "earth"});
}

function makeMoon() {
	var topColor = new Float32Array([0.5, 0.5, 0.5]);	// blue top
	var bottomColor = new Float32Array([0.5, 0.5, 0.5]);	// blue bottom
	
	makeSphere({top: topColor, bot: bottomColor, "planet": "moon"});
}

function makeSun() {
	var topColor = new Float32Array([0.8, 0.8, 0.3]);	// yellow top
	var bottomColor = new Float32Array([0.8, 0.8, 0.3]);	// yellow bottom
	
	makeSphere({"top": topColor, "bot": bottomColor, "planet": "sun"});
}

function getColor(planet) {
	var color;
	switch (planet) {
		case "sun":
			color = new Float32Array([ 
	                0.8,
	               (Math.floor(Math.random() * 8) + 4) / 10, 
				    0.2]);
			break;
		case "earth":
			color = new Float32Array([ 
	                0.2,
	               (Math.floor(Math.random() * 8) + 4) / 10, 
				   (Math.floor(Math.random() * 8) + 4) / 10]); 
			break;
		case "moon":
			var sameGrey = (Math.floor(Math.random() * 8) + 4) / 10;
			color = new Float32Array([sameGrey, sameGrey, sameGrey]);
			break;		
	}
	return color;
}

function makeSphere(colors) {
//==============================================================================

	var slices = 13;
	var sliceVerts = 27;
	
	var sliceAngle = Math.PI/slices;

	var verts = new Float32Array(((slices * 2* sliceVerts) - 2) * floatsPerVertex);
										
	// Create dome-shaped top slice of sphere at z=+1
	// s counts slices; v counts vertices; 
	// j counts array elements (vertices * elements per vertex)
	var cos0 = sin0 = cos1 = sin1 = 0.0;	// sines,cosines of slice's top, bottom edge.	
	var j = 0;	// initialize our array index
	var isLast = 0;
	var isFirst = 1;
	for(s=0; s<slices; s++) {	// for each slice of the sphere,
		// find sines & cosines for top and bottom of this slice
		if(s==0) {
			isFirst = 1;	// skip 1st vertex of 1st slice.
			cos0 = 1.0; 	// initialize: start at north pole.
			sin0 = 0.0;
		}
		else {					// otherwise, new top edge == old bottom edge
			isFirst = 0;	
			cos0 = cos1;
			sin0 = sin1;
		}								// & compute sine,cosine for new bottom edge.
		cos1 = Math.cos((s+1)*sliceAngle);
		sin1 = Math.sin((s+1)*sliceAngle);
		// go around the entire slice, generating TRIANGLE_STRIP verts
		// (Note we don't initialize j; grows with each new attrib,vertex, and slice)
		if(s == slices - 1) 
			isLast = 1;	// skip last vertex of last slice.
		
		for(v=isFirst; v< 2*sliceVerts-isLast; v++, j+=floatsPerVertex) {	
			if( v % 2 == 0) {  
				verts[j]   = sin0 * Math.cos(Math.PI*(v)/sliceVerts); 	
				verts[j+1] = sin0 * Math.sin(Math.PI*(v)/sliceVerts);	
				verts[j+2] = cos0;		
				verts[j+3] = 1.0;	
			}
			else {
				verts[j] = sin1 * Math.cos(Math.PI*(v-1)/sliceVerts); //x
				verts[j+1] = sin1 * Math.sin(Math.PI*(v-1)/sliceVerts); //y
				verts[j+2] = cos1; //z
				verts[j+3] = 1.0; //w
			}
			
			if(s==0) {	//set the colors
				verts[j+4] = colors["top"][0]; 
				verts[j+5] = colors["top"][1]; 
				verts[j+6] = colors["top"][2];	
			}
			else if(s==slices-1) {
				verts[j+4] = colors["bot"][0]; 
				verts[j+5] = colors["bot"][1]; 
				verts[j+6] = colors["bot"][2];	
			}
			else {
				var equColor = getColor(colors["planet"]);
				verts[j+4] = equColor[0];
				verts[j+5] = equColor[1]; 
				verts[j+6] = equColor[2];
			}
		}
	}
	
	sphVerts[colors["planet"]] = verts;
}

function makeTorus() {
//==============================================================================

var rbend = 1.0;										// Radius of circle formed by torus' bent bar
var rbar = 0.5;											// radius of the bar we bent to form torus
var barSlices = 23;									// # of bar-segments in the torus: >=3 req'd;
																		// more segments for more-circular torus
var barSides = 13;										// # of sides of the bar (and thus the 
																		// number of vertices in its cross-section)
																		// >=3 req'd;
																		// more sides for more-circular cross-section
// for nice-looking torus with approx square facets, 
//			--choose odd or prime#  for barSides, and
//			--choose pdd or prime# for barSlices of approx. barSides *(rbend/rbar)
// EXAMPLE: rbend = 1, rbar = 0.5, barSlices =23, barSides = 11.

	// Create a (global) array to hold this torus's vertices:
 torVerts = new Float32Array(floatsPerVertex*(2*barSides*barSlices +2));
//	Each slice requires 2*barSides vertices, but 1st slice will skip its first 
// triangle and last slice will skip its last triangle. To 'close' the torus,
// repeat the first 2 vertices at the end of the triangle-strip.  Assume 7

var phi=0, theta=0;										// begin torus at angles 0,0
var thetaStep = 2*Math.PI/barSlices;	// theta angle between each bar segment
var phiHalfStep = Math.PI/barSides;		// half-phi angle between each side of bar
																			// (WHY HALF? 2 vertices per step in phi)
	// s counts slices of the bar; v counts vertices within one slice; j counts
	// array elements (Float32) (vertices*#attribs/vertex) put in torVerts array.
	for(s = 0,j = 0; s < barSlices; s++) {		// for each 'slice' or 'ring' of the torus:
		for(v = 0; v < 2 * barSides; v++, j += floatsPerVertex) {		// for each vertex in this slice:
			if( v % 2 == 0)	{	// even #'d vertices at bottom of slice,
				torVerts[j  ] = (rbend + rbar*Math.cos((v)*phiHalfStep)) * 
																						 Math.cos((s)*thetaStep);
							  //	x = (rbend + rbar*cos(phi)) * cos(theta)
				torVerts[j+1] = (rbend + rbar*Math.cos((v)*phiHalfStep)) *
																						 Math.sin((s)*thetaStep);
								//  y = (rbend + rbar*cos(phi)) * sin(theta) 
				torVerts[j+2] = -rbar*Math.sin((v)*phiHalfStep);
								//  z = -rbar  *   sin(phi)
				torVerts[j+3] = 1.0;		// w
				
			}
			else {				// odd #'d vertices at top of slice (s+1);
										// at same phi used at bottom of slice (v-1)
				torVerts[j  ] = (rbend + rbar*Math.cos((v-1)*phiHalfStep)) * 
																						 Math.cos((s+1)*thetaStep);
							  //	x = (rbend + rbar*cos(phi)) * cos(theta)
				torVerts[j+1] = (rbend + rbar*Math.cos((v-1)*phiHalfStep)) *
																						 Math.sin((s+1)*thetaStep);
								//  y = (rbend + rbar*cos(phi)) * sin(theta) 
				torVerts[j+2] = -rbar*Math.sin((v-1)*phiHalfStep);
								//  z = -rbar  *   sin(phi)
				torVerts[j+3] = 1.0;		// w
				
			}
			torVerts[j+4] = Math.random();		// random color 0.0 <= R < 1.0
			torVerts[j+5] = Math.random();		// random color 0.0 <= G < 1.0
			torVerts[j+6] = Math.random();		// random color 0.0 <= B < 1.0
		}
	}
	// Repeat the 1st 2 vertices of the triangle strip to complete the torus:
			torVerts[j  ] = rbend + rbar;	// copy vertex zero;
						  //	x = (rbend + rbar*cos(phi==0)) * cos(theta==0)
			torVerts[j+1] = 0.0;
							//  y = (rbend + rbar*cos(phi==0)) * sin(theta==0) 
			torVerts[j+2] = 0.0;
							//  z = -rbar  *   sin(phi==0)
			torVerts[j+3] = 1.0;		// w
			
			torVerts[j+4] = Math.random();		// random color 0.0 <= R < 1.0
			torVerts[j+5] = Math.random();		// random color 0.0 <= G < 1.0
			torVerts[j+6] = Math.random();		// random color 0.0 <= B < 1.0
			
			j+=floatsPerVertex; // go to next vertex:
			
			torVerts[j  ] = (rbend + rbar) * Math.cos(thetaStep);
						  //	x = (rbend + rbar*cos(phi==0)) * cos(theta==thetaStep)
			torVerts[j+1] = (rbend + rbar) * Math.sin(thetaStep);
							//  y = (rbend + rbar*cos(phi==0)) * sin(theta==thetaStep) 
			torVerts[j+2] = 0.0;
							//  z = -rbar  *   sin(phi==0)
			torVerts[j+3] = 1.0;		// w
			
			torVerts[j+4] = Math.random();		// random color 0.0 <= R < 1.0
			torVerts[j+5] = Math.random();		// random color 0.0 <= G < 1.0
			torVerts[j+6] = Math.random();		// random color 0.0 <= B < 1.0
}

function makeCylinder() {
//==============================================================================
// Make a cylinder shape from one TRIANGLE_STRIP drawing primitive, using the
// 'stepped spiral' design described in notes.
// Cylinder center at origin, encircles z axis, radius 1, top/bottom at z= +/-1.
//
 var ctrColr = new Float32Array([0.2, 0.2, 0.2]);	// dark gray
 var topColr = new Float32Array([0.4, 0.7, 0.4]);	// light green
 var botColr = new Float32Array([0.5, 0.5, 1.0]);	// light blue
 var capVerts = 16;	// # of vertices around the topmost 'cap' of the shape
 var botRadius = 1.6;		// radius of bottom of cylinder (top always 1.0)
 
 // Create a (global) array to hold this cylinder's vertices;
 cylVerts = new Float32Array(  ((capVerts*6) -2) * floatsPerVertex);
										// # of vertices * # of elements needed to store them. 

	// Create circle-shaped top cap of cylinder at z=+1.0, radius 1.0
	// v counts vertices: j counts array elements (vertices * elements per vertex)
	for(v=1,j=0; v<2*capVerts; v++,j+=floatsPerVertex) {	
		// skip the first vertex--not needed.
		if(v%2==0)
		{				// put even# vertices at center of cylinder's top cap:
			cylVerts[j  ] = 0.0; 			// x,y,z,w == 0,0,1,1
			cylVerts[j+1] = 0.0;	
			cylVerts[j+2] = 1.0; 
			cylVerts[j+3] = 1.0;			// r,g,b = topColr[]
			
			cylVerts[j+4]=ctrColr[0]; 
			cylVerts[j+5]=ctrColr[1]; 
			cylVerts[j+6]=ctrColr[2];
		}
		else { 	// put odd# vertices around the top cap's outer edge;
						// x,y,z,w == cos(theta),sin(theta), 1.0, 1.0
						// 					theta = 2*PI*((v-1)/2)/capVerts = PI*(v-1)/capVerts
			cylVerts[j  ] = Math.cos(Math.PI*(v-1)/capVerts);			// x
			cylVerts[j+1] = Math.sin(Math.PI*(v-1)/capVerts);			// y
			//	(Why not 2*PI? because 0 < =v < 2*capVerts, so we
			//	 can simplify cos(2*PI * (v-1)/(2*capVerts))
			cylVerts[j+2] = 1.0;	// z
			cylVerts[j+3] = 1.0;	// w.
			
			// r,g,b = topColr[]
			cylVerts[j+4]=topColr[0]; 
			cylVerts[j+5]=topColr[1]; 
			cylVerts[j+6]=topColr[2];			
		}
	}
	// Create the cylinder side walls, made of 2*capVerts vertices.
	// v counts vertices within the wall; j continues to count array elements
	for(v=0; v< 2*capVerts; v++, j+=floatsPerVertex) {
		if(v%2==0)	// position all even# vertices along top cap:
		{		
				cylVerts[j  ] = Math.cos(Math.PI*(v)/capVerts);		// x
				cylVerts[j+1] = Math.sin(Math.PI*(v)/capVerts);		// y
				cylVerts[j+2] = 1.0;	// z
				cylVerts[j+3] = 1.0;	// w.
				
				// r,g,b = topColr[]
				cylVerts[j+4]=topColr[0]; 
				cylVerts[j+5]=topColr[1]; 
				cylVerts[j+6]=topColr[2];			
		}
		else		// position all odd# vertices along the bottom cap:
		{
				cylVerts[j  ] = botRadius * Math.cos(Math.PI*(v-1)/capVerts);		// x
				cylVerts[j+1] = botRadius * Math.sin(Math.PI*(v-1)/capVerts);		// y
				cylVerts[j+2] =-1.0;	// z
				cylVerts[j+3] = 1.0;	// w.
				
				// r,g,b = topColr[]
				cylVerts[j+4]=botColr[0]; 
				cylVerts[j+5]=botColr[1]; 
				cylVerts[j+6]=botColr[2];			
		}
	}
	// Create the cylinder bottom cap, made of 2*capVerts -1 vertices.
	// v counts the vertices in the cap; j continues to count array elements
	for(v=0; v < (2*capVerts -1); v++, j+= floatsPerVertex) {
		if(v%2==0) {	// position even #'d vertices around bot cap's outer edge
			cylVerts[j  ] = botRadius * Math.cos(Math.PI*(v)/capVerts);		// x
			cylVerts[j+1] = botRadius * Math.sin(Math.PI*(v)/capVerts);		// y
			cylVerts[j+2] =-1.0;	// z
			cylVerts[j+3] = 1.0;	// w.;
			
			// r,g,b = topColr[]
			cylVerts[j+4]=botColr[0]; 
			cylVerts[j+5]=botColr[1]; 
			cylVerts[j+6]=botColr[2];		
		}
		else {				// position odd#'d vertices at center of the bottom cap:
			cylVerts[j  ] = 0.0; 			// x,y,z,w == 0,0,-1,1
			cylVerts[j+1] = 0.0;	
			cylVerts[j+2] =-1.0; 
			cylVerts[j+3] = 1.0;			// r,g,b = botColr[]
			
			cylVerts[j+4]=botColr[0]; 
			cylVerts[j+5]=botColr[1]; 
			cylVerts[j+6]=botColr[2];
		}
	}
}

function makePyramid() {
	var c30 = Math.sqrt(0.75);
	var sq2	= Math.sqrt(2.0);						 

  pyrVerts = new Float32Array([
   
     0.0,	 0.0, sq2, 1.0,		0.0, 	0.0,	1.0,	// Node 0 (apex, +z axis;  blue)
     c30, -0.5, 0.0, 1.0, 		1.0,  0.0,  0.0, 	// Node 1 (base: lower rt; red)
     0.0,  1.0, 0.0, 1.0,  		0.0,  1.0,  0.0,	// Node 2 (base: +y axis;  grn)
			// Face 1: (right side)
		 0.0,	 0.0, sq2, 1.0,			0.0, 	0.0,	1.0,	// Node 0 (apex, +z axis;  blue)
     0.0,  1.0, 0.0, 1.0,  		0.0,  1.0,  0.0,	// Node 2 (base: +y axis;  grn)
    -c30, -0.5, 0.0, 1.0, 		1.0,  1.0,  1.0, 	// Node 3 (base:lower lft; white)
    	// Face 2: (lower side)
		 0.0,	 0.0, sq2, 1.0,			0.0, 	0.0,	1.0,	// Node 0 (apex, +z axis;  blue) 
    -c30, -0.5, 0.0, 1.0, 		1.0,  1.0,  1.0, 	// Node 3 (base:lower lft; white)
     c30, -0.5, 0.0, 1.0, 		1.0,  0.0,  0.0, 	// Node 1 (base: lower rt; red) 
     	// Face 3: (base side)  
    -c30, -0.5, 0.0, 1.0, 		1.0,  1.0,  1.0, 	// Node 3 (base:lower lft; white)
     0.0,  1.0, 0.0, 1.0,  		0.0,  1.0,  0.0,	// Node 2 (base: +y axis;  grn)
     c30, -0.5, 0.0, 1.0, 		1.0,  0.0,  0.0, 	// Node 1 (base: lower rt; red)
  ]);
  
}

function makeCube() {
	
	cubeVerts = new Float32Array([
	 -1.0, 0.0, 1.0, 1.0,  0.4, 0.4, 1.0, 
	  1.0, 0.0, 1.0, 1.0,  0.4, 0.4, 1.0,
	 -1.0, 1.0, 1.0, 1.0,  0.4, 0.4, 1.0,
	  1.0, 1.0, 1.0, 1.0,  0.4, 0.4, 1.0,
	  
	 -1.0, 1.0, -1.0, 1.0, 0.4, 1.0, 0.4, 
	  1.0, 1.0, -1.0, 1.0, 0.4, 1.0, 0.4,
	  
	 -1.0, 0.0, -1.0, 1.0, 1.0, 0.4, 0.4,
	  1.0, 0.0, -1.0, 1.0, 1.0, 0.4, 0.4, 
	  
	 -1.0, 0.0, 1.0, 1.0,  0.4, 0.4, 1.0, 
	  1.0, 0.0, 1.0, 1.0,  0.4, 0.4, 1.0,
	  
	  1.0, 0.0, -1.0, 1.0, 1.0, 0.4, 0.4,
	  1.0, 1.0, -1.0, 1.0, 0.4, 1.0, 0.4,
	  
	  1.0, 0.0, 1.0, 1.0,  0.4, 0.4, 1.0, 
	  1.0, 1.0, 1.0, 1.0,  0.4, 0.4, 1.0,
	  
	  -1.0, 0.0, -1.0, 1.0, 1.0, 0.4, 0.4,
	  -1.0, 0.0, 1.0, 1.0,  0.4, 0.4, 1.0, 
	  
	  -1.0, 1.0, -1.0, 1.0, 0.4, 1.0, 0.4, 
	  -1.0, 1.0, 1.0, 1.0,  0.4, 0.4, 1.0,
	  
	]);
	
}

function drawScene(gl, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix, lightReflectances, currentAngle, topAngle) {
	
	//-------- model our world into view
	viewMatrix.rotate(-90.0, 1,0,0); // rotate so the ground is parallel
	viewMatrix.translate(0.2, 0.0, -0.6);	// move us upwards
	viewMatrix.scale(0.4, 0.4, 0.4); //scale the world a bit
	
	//-------- GROUND PLANE
	SetMaterialReflectance(gl, lightReflectances[0], lightReflectances[1], 
	                           lightReflectances[2], lightReflectances[3],
							   lightReflectances[4],
							   DEFAULT);
							   
	drawGround(gl, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix);
	
	pushMatrix(viewMatrix);
	pushMatrix(viewMatrix);
	pushMatrix(viewMatrix);
	pushMatrix(viewMatrix);
	pushMatrix(viewMatrix);
	
	//---------ORBITS
	drawOrbits(gl, currentAngle, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix, lightReflectances);
	
	//---------TORUS
	SetMaterialReflectance(gl, lightReflectances[0], lightReflectances[1], 
	                           lightReflectances[2], lightReflectances[3], 
							   lightReflectances[4],
							   MATL_RED_PLASTIC);
	viewMatrix = popMatrix();
	drawTorus(gl, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix);
	
	//---------CYLINDER
	SetMaterialReflectance(gl, lightReflectances[0], lightReflectances[1], 
	                           lightReflectances[2], lightReflectances[3],
							   lightReflectances[4],							   
							   MATL_BRASS);
	viewMatrix = popMatrix();
	drawCylinder(gl, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix);
	
	//---------PYRAMID
	SetMaterialReflectance(gl, lightReflectances[0], lightReflectances[1], 
	                           lightReflectances[2], lightReflectances[3], 
							   lightReflectances[4],
							   MATL_CHROME);
	viewMatrix = popMatrix();
	drawPyramid(gl, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix, currentAngle, topAngle);
	
	//---------SPONGE-ALIEN-THING
	SetMaterialReflectance(gl, lightReflectances[0], lightReflectances[1], 
	                           lightReflectances[2], lightReflectances[3],
							   lightReflectances[4],							   
							   MATL_JADE);
	viewMatrix = popMatrix();
	viewMatrix.rotate(90.0, 0.0, 0.0, 1.0);
	viewMatrix.translate(0.0, -2.0, 0.0);
	drawSponge(gl, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix, currentAngle);
}

function drawGround(gl, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix) {
	
	computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	normalMatrix = new Matrix4();
	
	// Pass the modified view matrix to our shaders:
	gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);
	
	// Now, using these drawing axes, draw our ground plane: 
	gl.drawArrays(
		gl.LINES, 
		gndStart/floatsPerVertex, 
		gndVerts.length/floatsPerVertex);
}

function drawCamera(gl, viewMatrix, u_ViewMatrix, persMatrix, u_ProjMatrix, normalMatrix, u_NormalMatrix) {
	
	gl.uniformMatrix4fv(u_ProjMatrix, false, persMatrix.elements);
	
	// Draw in the Perspective viewports
	//------------------------------------------
	gl.viewport(0, // Viewport lower-left corner
				0, // location(in pixels)
				gl.drawingBufferWidth, // viewport width, height.
				gl.drawingBufferHeight);			
				
	// but use a different 'view' matrix:
	viewMatrix.setLookAt(
		g_EyeX, g_EyeY, g_EyeZ, // eye position
		//g_LookAtX, g_LookAtY, g_LookAtZ,
		g_EyeX + xMdragTot, g_EyeY + yMdragTot, g_EyeZ - 2.5, // look-at point 
		0, 1, 0 // rotation (up default)  x is left/right, y is up/down
	);
	//console.log(xMdragTot);
	
	// Pass the view matrix to our shaders:
	gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);
}

function drawOrbits(gl, currentAngle, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix, lightReflectances) {
//==============================================================================
	
	//-------- Place our solar system a bit
	viewMatrix.translate(0.0, 0.0, 1.5);
	viewMatrix.scale(1,1,-1); // convert to left-handed coord system

	//-------- Draw spinning sun and position it
	viewMatrix.rotate(currentAngle, 0, 0, 1); //rotate on z-axis
	viewMatrix.rotate(180, 1, 0, 0);
	
	viewMatrix.scale(0.3, 0.3, 0.3); // make smaller

	viewMatrix.rotate(0, 0, 180, 1);
	
	computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	normalMatrix = new Matrix4();
	SetMaterialReflectance(gl, lightReflectances[0], lightReflectances[1], 
	                           lightReflectances[2], lightReflectances[3],
							   lightReflectances[4],
							   MATL_GOLD_DULL);
							   
	// Drawing:		
	// Pass our current matrix to the vertex shaders:
	gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);
	
	// Draw just the sun's vertices
	gl.drawArrays(gl.TRIANGLE_STRIP,
							sunStart/floatsPerVertex, 
							sphVerts["sun"].length/floatsPerVertex);
	
	//--------Draw Spinning Earth
	viewMatrix.translate(2.0, 0, 0); //but translate outwards from sun
	viewMatrix.rotate(currentAngle*2, 0, 0, 1);
	
	pushMatrix(viewMatrix); //save before scaling
	
	viewMatrix.scale(0.5, 0.5, 0.5); //make smaller
		
	computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	normalMatrix = new Matrix4();
	
	SetMaterialReflectance(gl, lightReflectances[0], lightReflectances[1], 
	                           lightReflectances[2], lightReflectances[3],
							   lightReflectances[4],
							   MATL_TURQUOISE);
							   
	// Drawing:		
	// Pass our current matrix to the vertex shaders:
	gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);
		
	gl.drawArrays(gl.TRIANGLE_STRIP,
						  earthStart/floatsPerVertex,
						  sphVerts["earth"].length/floatsPerVertex);
						  
	//--------Draw ring
	viewMatrix.rotate(currentAngle, 1, 45, 1);
	
	viewMatrix.scale(1.0, 1.0, 0.1);
	
	computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	normalMatrix = new Matrix4();
	
	gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);
	
	gl.drawArrays(gl.TRIANGLE_STRIP, 
  						  torStart/floatsPerVertex,	
  						  torVerts.length/floatsPerVertex);	
				  					  
	//--------Draw Spinning Moon
	viewMatrix = popMatrix();
	
	viewMatrix.translate(0.85, 0, 0); //move drawing axis to earth
	
	//orbit moon with new drawing axis
	viewMatrix.rotate(currentAngle, 0, 0, 1); //rotate
	
	viewMatrix.scale(0.1, 0.1, 0.1); //make smaller
		
	computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	normalMatrix = new Matrix4();
		
	SetMaterialReflectance(gl, lightReflectances[0], lightReflectances[1], 
	                           lightReflectances[2], lightReflectances[3],
							   lightReflectances[4],
							   MATL_BRONZE_SHINY);
							   
	// Drawing:		
	// Pass our current matrix to the vertex shaders:
	gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);
	
	gl.drawArrays(gl.TRIANGLE_STRIP, 
						  moonStart/floatsPerVertex,
						  sphVerts["moon"].length/floatsPerVertex);

	//--------Draw ring
	viewMatrix.rotate(currentAngle, 1, 45, 1);
	viewMatrix.scale(1.0, 1.0, 0.1);
	
	computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	normalMatrix = new Matrix4();
	
	gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);
	
	gl.drawArrays(gl.TRIANGLE_STRIP, 
  						  torStart/floatsPerVertex,	
  						  torVerts.length/floatsPerVertex);	
	
}

function drawTorus(gl, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix) {
	
	viewMatrix.translate(0.0, 0.0, 2.5);
	viewMatrix.rotate(180, 1, 0, 0);
    viewMatrix.scale(1,1,-1);							// convert to left-handed coord sys
  																				// to match WebGL display canvas.
    viewMatrix.scale(0.6, 0.6, 0.6);
  
    viewMatrix.translate(-0.1, 0.0, 0.1);
  						// Make it smaller:
						
	computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	normalMatrix = new Matrix4();
	
    // Drawing:		
	// Pass our current matrix to the vertex shaders:
    gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);
	
  		// Draw just the torus's vertices
    gl.drawArrays(gl.TRIANGLE_STRIP, 				// use this drawing primitive, and
  						  torStart/floatsPerVertex,	// start at this vertex number, and
  						  torVerts.length/floatsPerVertex);	// draw this many vertices.			  
						  
}

function drawCylinder(gl, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix) {
	
	viewMatrix.translate(1.5, 0.0, 0.5);
	//viewMatrix.scale(1,1,-1);
	viewMatrix.scale(0.3, 0.3, 0.3);

	computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	normalMatrix = new Matrix4();

	// Drawing:
	// Pass our current matrix to the vertex shaders:
	gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);

	// Draw just the the cylinder's vertices:
	gl.drawArrays(gl.TRIANGLE_STRIP,
							cylStart/floatsPerVertex,
							cylVerts.length/floatsPerVertex);			
}

function drawPyramid(gl, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix, currentAngle, topAngle) {
  
    viewMatrix.translate(-1.6, 0.0, 1.3);
	viewMatrix.scale(0.2, 0.2, 0.2);
	
	computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	normalMatrix = new Matrix4();

	// Drawing:
	// Pass our current matrix to the vertex shaders:
	gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);

	// Draw just the the cylinder's vertices:
	gl.drawArrays(gl.TRIANGLE_STRIP,
							cylStart/floatsPerVertex,
							cylVerts.length/floatsPerVertex);	
  
	viewMatrix.translate(0.0, 0.0, 1.0);
	viewMatrix.rotate(currentAngle, 0, 0, 1);
	viewMatrix.scale(1.6, 1.6, 1.6);

	computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	normalMatrix = new Matrix4();

	gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);

	gl.drawArrays(gl.TRIANGLES, 
				pyrStart/floatsPerVertex,
				pyrVerts.length/floatsPerVertex);	
				
	viewMatrix.translate(0.0, 0.0, 1.5);
	viewMatrix.rotate(topAngle, 1, 1, 0);
	viewMatrix.rotate(115, 1, 0, 0);
	viewMatrix.scale(0.1, 0.7, 0.1);

	computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	normalMatrix = new Matrix4();

	gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);

	gl.drawArrays(gl.TRIANGLE_STRIP, 
			cubeStart/floatsPerVertex,
			cubeVerts.length/floatsPerVertex);
							
}

function drawSponge(gl, viewMatrix, u_ViewMatrix, normalMatrix, u_NormalMatrix, currentAngle) {
  
	viewMatrix.translate(1.6, 0.0, 1.3);
	viewMatrix.rotate(45, 0, 1, 1);

	pushMatrix(viewMatrix);

	viewMatrix.scale(0.1, 0.7, 0.1);

	computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	normalMatrix = new Matrix4();

	gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);

	gl.drawArrays(gl.TRIANGLE_STRIP, 
			cubeStart/floatsPerVertex,
			cubeVerts.length/floatsPerVertex);
	
  for (var i = 1; i < 9; i++)
  {
  
	  viewMatrix = popMatrix();
	  
	  if (i != 8) pushMatrix(viewMatrix);
	  
	  viewMatrix.rotate(45 * i, 0, 1, 1);
	  viewMatrix.scale(0.1, 0.7, 0.1);
	  
	  computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	  normalMatrix = new Matrix4();
	
	  gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);
			
	  gl.drawArrays(gl.TRIANGLE_STRIP, 
					cubeStart/floatsPerVertex, // start at this vertex number, and
					cubeVerts.length/floatsPerVertex);	// draw this many
					
	  viewMatrix.translate(0.0, 1.0, 0.0); 	
	  viewMatrix.scale(0.6,0.6,0.6);	
	  viewMatrix.rotate(currentAngle, 1.0 ,0,0);	
	  
	  computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	  normalMatrix = new Matrix4();
	  
	  gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);
			
	  gl.drawArrays(gl.TRIANGLE_STRIP, 
					cubeStart/floatsPerVertex,
					cubeVerts.length/floatsPerVertex);
					
	  viewMatrix.translate(0.0, 1.0, 0.0); 	
	  viewMatrix.scale(0.6,0.6,0.6);	
	  viewMatrix.rotate(currentAngle, 1.0 ,0,0);	
	  
	  computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix);
	  normalMatrix = new Matrix4();
	  
	  gl.uniformMatrix4fv(u_ViewMatrix, false, viewMatrix.elements);
			
	  gl.drawArrays(gl.TRIANGLE_STRIP, 
					cubeStart/floatsPerVertex,
					cubeVerts.length/floatsPerVertex);
  }
}

var g_last = Date.now();
function animate(angle) {
  var now = Date.now();
  var elapsed = now - g_last;
  g_last = now;
  
  var newAngle = angle + (ANGLE_STEP * elapsed) / 1000.0;
  return (newAngle %= 360);
}

var g_last2 = Date.now();
function animateSecond(angle) {
  var now = Date.now();
  var elapsed = now - g_last2;
  g_last2 = now;
  
  //keep the whip between 0 and -90 degrees
  
  if((angle > 0 && ANGLE_STEP_SECOND > 0) || 
     (angle < -90 && ANGLE_STEP_SECOND < 0))
	ANGLE_STEP_SECOND = -ANGLE_STEP_SECOND;
   
  var newAngle = angle + (ANGLE_STEP_SECOND * elapsed) / 1000.0;

  return newAngle %= 360;
}

function keydown(ev) {
//------------------------------------------------------

    if(ev.keyCode == 39 || ev.keyCode == 68) { // The right or D arrow key was pressed

		//var deltx = (xMdragTot - g_EyeX);
		//var delty = (yMdragTot - g_EyeY);
		//var deltz = (xMdragTot - g_EyeZ);
		//g_EyeX  += (deltx == 0 ? 0.01 : 0.01 * deltx);
		//xMdragTot += (deltx == 0 ? 0.01 : 0.01 * deltx);
		//g_EyeZ  += 0.01 * deltz;
		g_EyeX  += 0.1;
		
    } else if (ev.keyCode == 37 || ev.keyCode == 65) { // The left or A arrow key was pressed
        
		//var deltx = (xMdragTot - g_EyeX);
		//var delty = (yMdragTot - g_EyeY);
		//var deltz = (xMdragTot - g_EyeZ);
		//g_EyeX  -= (deltx == 0 ? 0.01 : 0.01 * deltx);
		//xMdragTot -= (deltx == 0 ? 0.01 : 0.01 * deltx);
		//g_EyeZ  -= 0.01 * deltz;
		
		g_EyeX  -= 0.1;
	
    } else if (ev.keyCode == 38 || ev.keyCode == 87) { // up or W arrow
        
		var deltx = (xMdragTot - g_EyeX);
		var delty = (yMdragTot - g_EyeY);
		var deltz = ((g_EyeZ - 2.5) - g_EyeZ);
		g_EyeX  += 0.01 * deltx;
		g_EyeY  += 0.01 * delty;
		g_EyeZ  += 0.01 * deltz;

    } else if (ev.keyCode == 40 || ev.keyCode == 83) { // down or S arrow
	
        var deltx = (xMdragTot - g_EyeX);
		var delty = (yMdragTot - g_EyeY);
		var deltz = ((g_EyeZ - 2.5) - g_EyeZ);
		g_EyeX  -= 0.01 * deltx;
		g_EyeY  -= 0.01 * delty;
		g_EyeZ  -= 0.01 * deltz;
		//g_LookAtZ -= 0.01 * deltz;
	} else if (ev.keyCode == 85) {  // u
		g_EyeY += 0.1;
		
	} else if (ev.keyCode == 74) {
		
		g_EyeY -= 0.1;
    } else if (ev.keyCode == 33) {
		spinUp();
	} else if (ev.keyCode == 34) {
		spinDown();
	}
}

function myMouseDown(ev, gl, canvas) {
//==============================================================================
// Called when user PRESSES down any mouse button;
	
	// Create right-handed 'pixel' coords with origin at WebGL canvas LOWER left;
	var rect = ev.target.getBoundingClientRect();
	var xp = ev.clientX - rect.left; 
	var yp = canvas.height - (ev.clientY - rect.top);
	
	// Convert to Canonical View Volume (CVV) coordinates too:
	var x = (xp - canvas.width/2) / (canvas.width/2);
	var y = (yp - canvas.height/2) / (canvas.height/2);
	 
	if (x < 0 && y < 0) {
		lowerLeftShape = true;
		upperRightShape = false;
	}
	if (x > 0 && y > 0) {
		upperRightShape = true;
		lowerLeftShape = false;
	}
	
	isDrag = true;		// set our mouse-dragging flag
	xMclik = x;	// record where mouse-dragging began
	yMclik = y;
};

function myMouseMove(ev, gl, canvas) {
//==============================================================================
// Called when user MOVES the mouse with a button already pressed down.
	
	if(isDrag==false) return;				// IGNORE all mouse-moves except 'dragging'

	// Create right-handed 'pixel' coords with origin at WebGL canvas LOWER left;
	var rect = ev.target.getBoundingClientRect();	// get canvas corners in pixels
	var xp = ev.clientX - rect.left;									// x==0 at canvas left edge
	var yp = canvas.height - (ev.clientY - rect.top);	// y==0 at canvas bottom edge
	//  console.log('myMouseMove(pixel coords): xp,yp=\t',xp,',\t',yp);

	// Convert to Canonical View Volume (CVV) coordinates too:
	var x = (xp - canvas.width/2)  / (canvas.width/2);
	var y = (yp - canvas.height/2) / (canvas.height/2);
						

	// find how far we dragged the mouse:
	xMdragTot += (x - xMclik);					// Accumulate change-in-mouse-position,&
	yMdragTot += (y - yMclik);
	
	dragQuat(x - xMclik, y - yMclik);
	
	xMclik = x;													// Make NEXT drag-measurement from here.
	yMclik = y;	
	
	$("#instructions").addClass("hidden");
};

function myMouseUp(ev, gl, canvas) {
//==============================================================================

	// Create right-handed 'pixel' coords with origin at WebGL canvas LOWER left;
	if (ev == null)
	{
		isDrag = false;
		$("#instructions").removeClass("hidden");
		return;
	}
		
	var rect = ev.target.getBoundingClientRect();	// get canvas corners in pixels
	var xp = ev.clientX - rect.left;									// x==0 at canvas left edge
	var yp = canvas.height - (ev.clientY - rect.top);	// y==0 at canvas bottom edge

	// Convert to Canonical View Volume (CVV) coordinates too:
	var x = (xp - canvas.width/2) / (canvas.width/2);
	var y = (yp - canvas.height/2) / (canvas.height/2);

	isDrag = false;	 // CLEAR our mouse-dragging flag, and
	// accumulate any final bit of mouse-dragging we did:
	xMdragTot += (x - xMclik);
	yMdragTot += (y - yMclik);
	
	dragQuat(x - xMclik, y - yMclik);
	
	$("#instructions").removeClass("hidden");
}

function dragQuat(xdrag, ydrag) {
//==============================================================================

	var qTmp = new Quaternion(0,0,0,1);
	
	var dist = Math.sqrt(xdrag*xdrag + ydrag*ydrag);
	
	qNew.setFromAxisAngle(-ydrag + 0.0001, xdrag + 0.0001, 0.0, dist*150.0);
							
	qTmp.multiply(qNew,qTot);			// apply new rotation to current rotation. 
	
	qTot.copy(qTmp);
	
};

function spinUp() {
  ANGLE_STEP += 25; 
}

function spinDown() {
  ANGLE_STEP -= 25; 
}

function computeNormalFromView(gl, normalMatrix, u_NormalMatrix, viewMatrix) {
	normalMatrix.setInverseOf(viewMatrix);
	normalMatrix.transpose();
	gl.uniformMatrix4fv(u_NormalMatrix, false, normalMatrix.elements);
}

function getLight(gl, u_ambientOn, u_diffuseOn, u_specularOn, u_emissiveOn, u_eyePosWorld, u_light2PosWorld, u_Lamp0Pos, u_Lamp0Amb, u_Lamp0Diff, u_Lamp0Spec,
				  u_Lamp1Pos, u_Lamp1Amb, u_Lamp1Diff, u_Lamp1Spec) {
	
	if (ambientOn) 
	{
		gl.uniform1f(u_ambientOn, 1.0);
	}
	else if (!ambientOn)
	{
		gl.uniform1f(u_ambientOn, 0.0);
	}
	
	if (diffuseOn)
	{
		gl.uniform1f(u_diffuseOn, 1.0);
	}
	else if (!diffuseOn)
	{
		gl.uniform1f(u_diffuseOn, 0.0);
	}
	
	if (specularOn)
	{
		gl.uniform1f(u_specularOn, 1.0);
	}
	else if (!specularOn)
	{
		gl.uniform1f(u_specularOn, 0.0);
	}
	
	if (emissiveOn)
	{
		gl.uniform1f(u_emissiveOn, 1.0);
	}
	else if (!emissiveOn)
	{
		gl.uniform1f(u_emissiveOn, 0.0);
	}
	
	gl.uniform4f(u_eyePosWorld, g_EyeX, g_EyeY, g_EyeZ, 1.0);
    gl.uniform4f(u_Lamp0Pos, g_EyeX, g_EyeY, g_EyeZ, 1.0);
	
	gl.uniform4f(u_light2PosWorld, lightX, lightY, lightZ, 1.0);
    gl.uniform4f(u_Lamp1Pos, lampX, lampY, lampZ, 1.0);
	
	gl.uniform3f(u_Lamp0Amb,  lamp0Amb[0], lamp0Amb[1], lamp0Amb[2]);		// ambient
    gl.uniform3f(u_Lamp0Diff, lamp0Diff[0], lamp0Diff[1], lamp0Diff[2]);		// diffuse
    gl.uniform3f(u_Lamp0Spec, lamp0Spec[0], lamp0Spec[1], lamp0Spec[2]);		// Specular
  
    gl.uniform3f(u_Lamp1Amb,  lamp1Amb[0], lamp1Amb[1], lamp1Amb[2]);		// ambient
    gl.uniform3f(u_Lamp1Diff, lamp1Diff[0], lamp1Diff[1], lamp1Diff[2]);		// diffuse
    gl.uniform3f(u_Lamp1Spec, lamp1Spec[0], lamp1Spec[1], lamp1Spec[2]);		// Specular
}

function toggleAmbient() {
	ambientOn = !ambientOn;
	if (ambientOn)
		$("#light-control .ambient").val("Turn Off Ambient");
	else
		$("#light-control .ambient").val("Turn On Ambient");
}

function toggleDiffuse() {
	diffuseOn = !diffuseOn;
	if (diffuseOn)
		$("#light-control .diffuse").val("Turn Off Diffuse");
	else
		$("#light-control .diffuse").val("Turn On Diffuse");
}

function toggleSpecular() {
	specularOn = !specularOn;
	if (specularOn)
		$("#light-control .specular").val("Turn Off Specular");
	else
		$("#light-control .specular").val("Turn On Specular");
}

function toggleEmissive() {
	emissiveOn = !emissiveOn;
	if (emissiveOn)
		$("#light-control .emissive").val("Turn Off Emissive");
	else
		$("#light-control .emissive").val("Turn On Emissive");
}

function toggleShiny() {
	shinyOn = !shinyOn;
	if (shinyOn)
		$("#light-control .shiny").val("Turn Off Shiny");
	else
		$("#light-control .shiny").val("Turn On Shiny");
}

function LampChange(which) {
	if (which == 0)
	{
		lamp0Amb[0] = $("#ambient0r").val().trim();
		lamp0Amb[1] = $("#ambient0g").val().trim();
		lamp0Amb[2] = $("#ambient0b").val().trim();
		
		lamp0Diff[0] = $("#diffuse0r").val().trim();
		lamp0Diff[1] = $("#diffuse0g").val().trim();
		lamp0Diff[2] = $("#diffuse0b").val().trim();
		
		lamp0Spec[0] = $("#specular0r").val().trim();
		lamp0Spec[1] = $("#specular0g").val().trim();
		lamp0Spec[2] = $("#specular0b").val().trim();
	}
	else
	{
		lamp1Amb[0] = $("#ambient1r").val().trim();
		lamp1Amb[1] = $("#ambient1g").val().trim();
		lamp1Amb[2] = $("#ambient1b").val().trim();
		
		lamp1Diff[0] = $("#diffuse1r").val().trim();
		lamp1Diff[1] = $("#diffuse1g").val().trim();
		lamp1Diff[2] = $("#diffuse1b").val().trim();
		
		lamp1Spec[0] = $("#specular1r").val().trim();
		lamp1Spec[1] = $("#specular1g").val().trim();
		lamp1Spec[2] = $("#specular1b").val().trim();
	}
}

function SetMaterialReflectance(gl, u_Ke, u_Ka, u_Kd, u_Ks, u_Kshiny, material) {
	var materialLightProperty = new Material(material);
  
	gl.uniform3f(u_Ke, materialLightProperty.emissive[0], 
	                   materialLightProperty.emissive[1], 
					   materialLightProperty.emissive[2]);	// Ke emissive
	gl.uniform3f(u_Ka, materialLightProperty.ambient[0], 
	                   materialLightProperty.ambient[1], 
					   materialLightProperty.ambient[2]);	// Ka ambient
	gl.uniform3f(u_Kd, materialLightProperty.diffuse[0], 
	                   materialLightProperty.diffuse[1], 
					   materialLightProperty.diffuse[2]);	// Kd diffuse
	gl.uniform3f(u_Ks, materialLightProperty.specular[0], 
	                   materialLightProperty.specular[1], 
					   materialLightProperty.specular[2]);	// Ks specular
	if (shinyOn)
	{
		gl.uniform1f(u_Kshiny, materialLightProperty.shiny);    // Kshiny shinyness
	}
	else
	{
		gl.uniform1f(u_Kshiny, 1.0);
	}
}
 
function moveLight(whichWay) {
	switch(whichWay) {
		case "up":
			lightY += 0.5;
			break;
		case "down":
			lightY -= 0.5;
			break;
		case "left":
			lightZ -= 1.5;
			break;
		case "right":
			lightZ += 1.5;
			break;
		case "forward":
			lightX += 1.5;
			break;
		case "back":
			lightX -= 1.5;
			break;
	}
}

function setLampValues() {

	$("#ambient0r").val(lamp0Amb[0]);
	$("#ambient0g").val(lamp0Amb[1]);
	$("#ambient0b").val(lamp0Amb[2]);
	$("#diffuse0r").val(lamp0Diff[0]);
	$("#diffuse0g").val(lamp0Diff[1]);
	$("#diffuse0b").val(lamp0Diff[2]);
	$("#specular0r").val(lamp0Spec[0]);
	$("#specular0g").val(lamp0Spec[1]);
	$("#specular0b").val(lamp0Spec[2]);
	
	$("#ambient1r").val(lamp1Amb[0]);
	$("#ambient1g").val(lamp1Amb[1]);
	$("#ambient1b").val(lamp1Amb[2]);
	$("#diffuse1r").val(lamp1Diff[0]);
	$("#diffuse1g").val(lamp1Diff[1]);
	$("#diffuse1b").val(lamp1Diff[2]);
	$("#specular1r").val(lamp1Spec[0]);
	$("#specular1g").val(lamp1Spec[1]);
	$("#specular1b").val(lamp1Spec[2]);
}