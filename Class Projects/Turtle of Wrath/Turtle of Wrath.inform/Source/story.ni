"Turtle of Wrath" by Alex Ayerdi

Release along with cover art.

When play begins:
	say "[italic type]Nuclear war is on the horizon, two nations are in extremely high tension and anything can set the other one off to fire first.  There are rumors about talks of peace, but it seems unlikely at this point.  Each side is hoping that somehow nuclear war can be prevented to save the millions of lives that it would cause..."

The Desert is a room.  "You are a turtle, and you find yourself at the south edge of a road in the middle of a desert.  You hear a car coming from the east, and another coming from the west much farther away.  

You do not seem too interested in other things around you but you do see a cactus across the road that might offer shade and you see a little grape next to your foot. Type 'help' to see a list of possible commands."

Understand "help" as asking for help.
Asking for help is an action out of world.
Carry out asking for help: Say "You are a turtle, so you can really only move, eat, and hide in your shell.  When you want to move to another area, use cardinal directions such as 'go north' (or 'n' ,'s', 'w', 'e' as shortcuts).  

If you want to eat something type 'eat <something>' and for this game it might not always be food as you know it.  Hiding is simply done by 'hide' which could have a various number of different effects depending on the situation ('hide', 'turtle', and 'sleep' are all essentially the same action in this game).

Don't forget that you can still use 'examine <thing>' as well to see descriptions!  The shortcut 'x <thing>' works too. If you want to see the description of the current location, type 'look'"

Section 1 - Define In-Game Rules

Rule for printing the locale description of the Desert: stop.
Rule for printing the locale description of the Ventilation Shaft: stop.
Rule for printing the locale description of the Kitchen: stop.
Rule for printing the locale description of the Engineer's Workshop: stop.
Rule for printing the locale description of the Cactus Trap: stop.
Rule for printing the locale description of the Control Room: stop.

The light-counter is a number that varies.
The height-counter is a number that varies.
The hiding-counter is a number that varies.
The war-counter is a number that varies.
The engineer-counter is a number that varies.
The kitchen-eaten-counter is a number that varies.
The commander-killed-counter is a number that varies.
The closet-counter is a number that varies.

[Make sure certain things happen every round depending on particular scenarios]
Every turn:
	if the light-counter is 1:
		say "You still hear the tall animals screaming and yelling in the dark.";
		if the kitchen-eaten-counter is 1:
			say "'Someone get those lights back on!!  Find the breaker[if the commander-killed-counter is 1], and get that spy[end if]!!'";
			increment commander-killed-counter;

Section 2 - Define Turtle Actions

  Understand "hide" as hiding.
  Understand "turtle" as hiding.
  Understand "sleep" as hiding.

  Hiding is an action applying to nothing.

Section 3 - Define Rooms and Descriptions

  North of the Desert is the Road.  
  The description of the Road is "A long, narrow stretch of a smoothed surface.  The air above the road wobbles from the heat.  It appears to be the only thing between you and the cactus.

   You can still hear the second car coming from the west, it is louder than before.  You are scared, and your instinct is to hide but you are also very hot and desperatley need that shade."

  North of the Road is the Cactus. 
  The description of the Cactus is "A tall and spikey plant that really means nothing to you other than that it offers your sought after shade for this hot day in the desert."

  East of the Desert is Void. 
  West of the Desert is Void.
  South of the Desert is Void.

  Below the Cactus is the Cactus Trap.
  The description of the Cactus Trap is "A trap used to kill spies getting too close to the base.  The cactus was clearly fake and is actually a power unit for the trap itself.  The inside here is actually quite dull and uninteresting other than the ventilation shaft to the north.  The shaft is cut off by a grate, but the grate has since loosened, and you can see the bolts are practically hanging off.   
  
  The bolts look sort of like little lizards to you, and that makes you hungry."

  East of the Cactus Trap is the Wall.
  West of the Cactus Trap is the Wall.
  South of the Cactus Trap is the Wall.

  North of the Cactus Trap is the Ventilation Shaft.
  The description of the Ventilation Shaft is "A ventilation shaft used to cool down the trap because it can heat up easily from its power usage and being covered under sand.  It's pretty hot in here and you feel like sleeping, but you also want to move on and possibly find food.  There only seems to be one direction to go for now.
  
  Although the bottom of the ventilation shaft seems very weak and worn out, it probably won't break if you only spend time on it for a few seconds."
  
  North of the Ventilation Shaft is the T-Junction.
  The description of the T-Junction is "The end of the main ventilation shaft, in front of you is a massive fan, likely dangerous, and to the left and right of you are further shafts but they drop downwards into unknown rooms.  The west side of the T-Junction smells like food, but the east side of the T-Junction has a bright light. 
  
  You think lights are pretty cool, because you're a turtle.  But then again, you're getting hungry and the food seems instinctively good."

  West of the Kitchen is the Engineer's Workshop.
  The description of the Engineer's Workshop is "A fantastic workshop filled with gadgets and doohickies.  [if the light-counter is 0] You can see various sorts of power supplies, light sources, and wires come out of every which way.  However, you process none of this as important because you are a turtle.  Also, you remember that you're getting hungry again...  The tall animal has gone away, so you are confused.  Although... those wires look like little worms... [end if] [if the light-counter is 1] Although, you cannot really see anything so you don't care much. [end if]
  
  [if the kitchen-eaten-counter is 0]You also smell something nice coming from the east. [end if]"
  
  West of the Engineer's Workshop is the Wall.
  South of the Engineer's Workshop is the Wall.
  North of the Engineer's Workshop is the Wall.
  
  The Kitchen is a Room.
  The description of the Kitchen is "[if the kitchen-eaten-counter is 0]A heaven to you because you are enveloped by the smells[end if][if the light-counter is 0] and sights[end if][if the kitchen-eaten-counter is 0] of various different types of foods.  You are extremely curious as to what each of the foods are, and if you can eat them while avoiding those other tall and loud animals.[otherwise] You can't smell the food from before anymore.  You still see that interesting light coming from the east though.[end if]
  
  [if the light-counter is 0 and the kitchen-eaten-counter is 0] Upon first glance you notice a nice piece of bread near the foot of a tall and loud animal who seems to be the center of attention. You also see a carrot behind the counter where a white looking tall animal is preparing food. The only other turtle-able food you see is a piece of lettuce.  [otherwise if the kitchen-eaten-counter is 0] You can smell only a piece of bread, a carrot, and some lettuce nearby.  Your little turtle-nose is strong enough to find it if you want to eat it.  
  
  There is a large silhouette near the carrot. [end if] "

The Closet is a Room.
The description of the Closet is "The closet is a dark place and it has mops, buckets, various cleaning equipment, and tools.  These are of no interest to you because you are a turtle.  You are however interested in getting out, the door seems to be cracked open enough for you to escape.  On the other hand, you are still pretty sleepy from not eating in a while and from the hot air."

West of the Closet is the Control Room.
The Description of the Control Room is "You can see a President Switch, an Enemy Switch, and a Missle Button on the north-wall console[if the height-counter is 1] above you[otherwise] in front of you[end if].  To the east is the Closet."

West of the Control Room is the Kitchen.
North of the Control Room is the Wall.
South of the Control Room is the Wall.

The Fan is a Room.

West of the T-Junction is the West Downward Junction. 
Below the West Downward Junction is the Kitchen.
East of the T-Junction is the East Downward Junction.
Below the East Downward Junction is the Closet.
North of the T-Junction is the Fan.

The War Room is a Room.
The description of the War Room is "This is where all the main decisions take place.  The Commander is in the room along with his whole advising staff.  They are about to make an important decision: war or not war."

Section 4 - Desert Scene Actions and Objects

The grape is an edible thing.  The grape can be eaten.
The description of the grape is "A little red grape that looks like it was recently picked, perhaps arriving here after falling off of a truck."

The Grape is in the Desert. 

Instead of eating the grape:
	say "You reach down with your turtle mouth and eat the grape.  Yum!  You watch innocently as a Jalopy packed full of people and random stuff on its hood drives by at turtle-crunching speed.
	
	The second car is still quite far away, you might be able to make a break for it now across the road.";
	now the grape is eaten;
	
Before going north from the Desert:
	if the grape is not eaten:
		say "CRUNCH!  You got ran over by a Jalopy carrying a ton of stuff on its hood.  Weird...  Well, you should have trusted your instincts and taken time to have a bite to eat.";
		end the story;

Instead of going east from the Desert:
	say "You think about going east, but there is nothing of interest that way, so you decided not to move."

Instead of going west from the Desert: 
	say "You think about going west, but there is nothing of interest that way, so you decided not to move."

Instead of going south from the Desert: 
	say "You think about going south, but there is nothing of interest that way, so you decided not to move."
	
Report hiding:
	if the player is in the Road and the hiding-counter is 0:
		say "You hide in your shell because the car scares you and you feel unable to proceed further.  
	
	It then approaches you, but luckily it slows down and comes to a full stop.  The tall animal inside notices you and picks you up, placing you in the passenger seat.  You are absolutely frightened.
	
	'Poor little turtle.  I wonder what you're doing out here?  Hmm... well, I'm off to work.  Do you want to join me?'
	
	... You are turtle so you do not respond ...
	
	'I'm an engineer at the military base here, you can be my company while I update the lighting in the building.'
	
	The engineer puts some snacks in front of the hole for your head and you peek out slowly.  You snatch it up as quickly as you can and then retreat back.  You like the food, so you slowly put your head back outside of your shell.  Maybe being with this tall thing that makes sounds will be beneficial.  You believe yourself to be a smart turtle.";
		move the player to the Engineer's Workshop;
		increment engineer-counter;
		increment hiding-counter;
	
Section 5 - Road Scene Actions

Instead of going west from the Road:
	say "The approaching car makes you feel scared and your little turtle legs are unable to move in that direction.  You are getting pretty hot and the cactus' shade looks nice."

Instead of going east from the Road:
	say "You are a turtle and running away from the car will be futile.  Luckily your instincts prevent you from making the bad decision.  All of this anxiety makes the desert air feel hotter, and you remember that the cactus' shade looks nice."
	
Before going south from the Road:
	say "CRUNCH!  It actually took you longer to turn completely around and then walk the same way back from whence you came, and the car ran you over ending your little turtle life.";
	end the story;
	
After going north from the Road:
	say "You get into the shade of the cactus which feels nice.  However, the ground under you starts to slip under your feet and you move closer to the edge of the cactus.";
	move the player to the Control Room;
	say "'weight trigger activated .... trap powering up!'
	
	The guard awakens to the warning from the computer and looks at the camera but sees nothing but a turtle.
	
	'dumb turtle...'
	
	The guard then resets the weight trigger and trap by pressing a button on his console.  At this point though, the turtle has already fallen through a small door where the trap was located at the cactus trunk, but the guard takes zero notice because he has already fallen back asleep.";
	move the player to the Cactus Trap;
	
Section 6 - Cactus Trap Scene Actions and Objects

The bolt is an edible thing in the Cactus Trap.  The bolt can be eaten.
Understand "bolts" as bolt.
The description of the bolt is "The little lizard-looking bolts are quite old by now, they are barely holding the grate on the Ventilation Shaft in front of you.";

The grate is a thing in the Cactus Trap.  
The description of the grate is "A uninteresting grate, but it looks pretty old and fairly loosened since the bolts are practically falling out.  The bolts look kind of tasty for some reason."

Instead of eating the bolt:
	say "You eat the bolts, they're um... not so bad.  You're not sure since you're a turtle.  However, the grate slides down and the entrance to the Ventilation Shaft is open!  You feel an urge to go through, but you're a little bit scared.  It seems to stretch on forever, dimly lit, and there is a steady flow of air rushing towards you.";
	now the bolt is eaten.

Instead of going north from the Cactus Trap:
	if the bolt is not eaten:
		say "You cannot go through the grate, but unfortunately you're unable to process that information intelligently because you're a turtle and simply run into the grate hitting your head.  You feel a small amount of pain, but you're okay.  You're also still pretty hungry; those bolts look tasty.";
		stop the action;
	otherwise:
		move the player to the Ventilation Shaft.

Instead of going west from the Cactus Trap:
	say "For some reason you try to go west inside of the Cactus Trap, but you hit your tiny little head against the wall.  Unfortunately, your dreams of being a ninja turtle have not come true at this moment and you do not scale the wall.";
	stop the action;

Instead of going east from the Cactus Trap:
	say "For some reason you try to go east inside of the Cactus Trap, but you hit your tiny little head against the wall.  Unfortunately, your dreams of being a ninja turtle have not come true at this moment and you do not scale the wall.";
	stop the action;

Instead of going south from the Cactus Trap:
	say "For some reason you try to go south inside of the Cactus Trap, but you hit your tiny little head against the wall.  Unfortunately, your dreams of being a ninja turtle have not come true at this moment and you do not scale the wall.";
	stop the action;

Section 7 - Ventilation Shaft Scene Actions  

Report hiding:
	if the player is in the Ventilation Shaft:
		say "The continuous weight of you just sitting there sleeping causes the weak under part of the ventilation shaft to screech and break underneath you!  You feel your little turtle-stomach flip over a few times before you hit the ground with your shell.
		
		You seem to have landed in some sort of dark room.";
		increment hiding-counter;
		move the player to the Closet;
		
Section 8 - Engineer's Workshop Scene Actions and Objects  

The wire is an edible thing in the Engineer's Workshop.  The wire can be eaten.
Understand "wires" as wire.

After entering the Engineer's Workshop:
	if the hiding-counter is 1:
		decrement the hiding-counter.

Instead of eating the wire:
	say "*brrrsst* 'Eeeek'
	
	You didn't like the taste of those worms, it made your mouth hurt and your body tingle.  Almost immediately following that, everything goes dark and it starts to get really loud with screaming tall animals.  
	
	You start to get a little bit scared, but you still smell something really good from the east.  So that makes you forget about the darkness.";
	now the wire is eaten;
	increment the light-counter;

Instead of going west from the Engineer's Workshop:
	say "There is only a wall there, you see a little spider climbing up the wall.  You wish you could climb like the spider."
	
Instead of going north from the Engineer's Workshop:
	say "You only see a crate and a shelf full of lighting equipment, wires, power sources, and everything else an engineer might need.  No where to go over there."
	
Instead of going south from the Engineer's Workshop:
	say "You cannot see anything of interest on that side of the workshop, just a little mouse that skitters by.  No where to go over there."

Section 9 - Closet Scene Actions

Report hiding:
	if the player is in the Closet and the hiding-counter is 0:
		say "You hear a person coming into the room...
		
		'Hmm, what was that...  I thought I heard something.  Eh, probably nothing, I have been eating way too many of those tacos...  It must be making me hallucinate now.'
		
		The guard looks down in frustration at himself and then spots you, the poor hiding turtle.
		
		'Oh!  Hi, little one.  I wonder how you snuck into here?  I'll let you be my little mascot and you can hang out on my console.  Here, have some lettuce left over from lunch.'
		
		You are finally satisfied and as you relax, the guard leaves the room reporting that he 'has to go use the restroom for a second.'";
		increment hiding-counter; [save the state of hiding]
		increment height-counter; [now we are high up]
		increment the closet-counter; [visited the closet]
		move the player to the Control Room;
		say "The Control Room is where it all happens.  There are buttons to do almost anything: control power to any room, launch missles, turn on heat and air, launch missles, call foreign nations, launch missles, and call the president.  You can also launch missles from the control room, but what do you care?  You're a turtle."

Instead of going west from the Closet:
	say "'Hmm, what was that...  I thought I heard something.  Eh, probably nothing, I have been eating way too many of those tacos...  It must be making me hallucinate now.'
	
	You sneak past the control room guard, but you don't actually realize you're doing this...  ";
	move the player to the Control Room;
	if the kitchen-eaten-counter is 0: [we haven't eaten yet]
		say "You can smell something good from the west.";
		
Section 10 - T-Junction Scene Actions 

Instead of going east from the T-Junction:
	say "You fall down the west side of the T-Junction!  Plop...  You are unhurt but a little frazzled, although you are a turtle and have no idea what just happened.  You are only concerned about the food you smell.";
	move the player to the Kitchen.
	
Instead of going west from the T-Junction:
	say "You fall down the east side of the T-Junction! Plop... You are unhurt but a little frazzled, although you are a turtle and have no idea what just happened.  You are only concerned about the pretty light.";
	move the player to the Engineer's Workshop;
	increment the engineer-counter; [we visited the engineer shop]

Instead of going north from the T-Junction:
	say "You were oddly enticed to go into the fast moving blade at the T-Junction.  As soon as you touch the blade you are swep into it and are completely ripped apart to shreds.  You live...just kidding, you died a very swift death.";
	end the story;
	
Section 11 - Kitchen Scene Actions and Objects

The Bread is an edible thing in the Kitchen.  The Bread can be eaten.
The Carrot is an edible thing in the Kitchen.  The Carrot can be eaten.
The Lettuce is an edible thing in the Kitchen.  The Lettuce can be eaten.

North of the Kitchen is the Kitchen Wall.
South of the Kitchen is the Kitchen Wall.

The description of the Bread is "A little piece of bread [if the light-counter is 0], fallen on the ground next to the Commander's foot. [otherwise] on the ground that you can smell."

The description of the Carrot is "A chopped carrot [if the light-counter is 0], fallen on the ground next to the chef's foot. [otherwise] on the ground that you can smell."

The description of the Lettuce is "A nice and watery piece of lettuce, something us turtle like very much.  It seems to have found its way into the corner of the Kitchen, away from all the commotion. [if the light-counter is 1] You can smell it in the darkness."

Instead of eating the Bread:
	if the kitchen-eaten-counter is 0: [we haven't eaten yet]
		if the light-counter is 0: [the lights are on]
			say "You walk up to the bread to eat it as just as your wrap your little turtle mouth over the crust, the Commander sees you by his foot and picks you up.
			
			'Oh... I remember owning one that looked just like you when I was a child.  I wonder if there are other innocent children elsewhere with turtles...  Yeah, we need to call this strike off.  I cannot let this happen - I must convince the president!'
			
			In his epiphany, he decides to put you back down, and you finish off eating your delicious bread.  You are simply confused, but not quite satisfied with just this little appetizer.  You see a tall animal with some lettuce leave the room to the east.  You want that lettuce!";
			now the bread is eaten;
			increment the kitchen-eaten-counter; [we have eaten now]
		otherwise: [lights are off]
			say "You sniff intently and finally find the bread in the dark.  It tastes really good, but you aren't quite satisfied.  It's still dark, but some emergency lights illuminate the Control Room to the east.  Your second most favorite thing besides food is shiny lights, so you are curious and want to take a look.";
			now the bread is eaten;
			increment the kitchen-eaten-counter; [we have eaten now]
	otherwise: [already ate the food]
		if the bread is eaten:
			say "You ate that already you dumb turtle...  You lose some self-esteem.";
		else if the light-counter is 1: [if the lights are off]
			say "You cannot find it now, it must have been kicked away in the midst of everyone running around in the dark.";
		otherwise: [we ate something else and the lights are on]
			say "The food is all swept up by now.";
	
Instead of eating the Carrot:
	if the kitchen-eaten-counter is 0: [we haven't eaten yet]
		if the light-counter is 0: [the lights are on]
			say "You walk up to the carrot and take a sniff at it, it smells pretty good and so you reach out and eat it in one bite.  Yum!
		
			**Eeek**
		
			You scream as the chef grabs you and says 'Oh, what do we have here?  Sneaking in and eating the scraps off the floor eh?  I think I'll make a nice turtle stew out of you, we have been short on food this month, so it might be nice to add a little flavor to our dinner this evening.'
		
			Unfortunately, the chef kills you and turns you into a wonderful turtle stew.  The war goes on without you, and millions die of nuclear explosions and blast radiation.";
			end the story;
		otherwise: [lights are off]
			say "You sniff intently and finally find the little carrot in the dark.  It tastes good, but you aren't quite satisfied.  As soon as you finish eating it, since it's dark the chef unknowningly trips on your shell and as he's falling his cleaver is thrown out of his hands.  The cleaver slices into the heart of someone in the dark...
		
			'Commander!!!'
		
			The Commander is instanly killed, but you don't really notice or care about any of this since you're a turtle.
		
			'We have a spy in our midst!!!  Find him immediately, our Commander is dead!!!'";
			now the carrot is eaten;
			increment the kitchen-eaten-counter; [now we've eaten]
			increment the commander-killed-counter; [now we've killed the commander]
	otherwise: [already ate the food]
		if the carrot is eaten:
			say "You ate that already you dumb turtle.  You lose a little bit of self-esteem.";
		else if the light-counter is 1:
			say "You cannot find it now, it must have been kicked away in the midst of everyone running around in the dark.";
		otherwise:
			say "The food is all swept up by now.";

Instead of eating the Lettuce:
	if the kitchen-eaten-counter is 0:
		if the light-counter is 0:
			say "You sneak by the other eating tall animals in the room and make it to the lettuce.  It's nice and crunchy, which is your favorite.  By this point the kitchen aids have already swept up the other pieces of food, but somehow you go unnoticed.  You see another tall human going east with a plate of lettuce, but another human is playing with lights and fun things to the west.
			
			You are enticed by both, but not sure which to go visit next.";
			now the lettuce is eaten;
			increment the kitchen-eaten-counter;
		otherwise: [lights are off]
			say "You sniff intently and finally find the lettuce in the dark.  It tastes really good, but you aren't quite satisfied.  It's still dark, but some emergency lights illuminate the Control Room to the east.  Your second most favorite thing besides food is shiny lights, so you are curious and want to take a look.";
			now the lettuce is eaten;
			increment the kitchen-eaten-counter;
	otherwise:  [already ate the food]
		if the lettuce is eaten:
			say "You ate that already you dumb turtle.  You are mad at yourself and lose some self-esteem.";
		else if the light-counter is 1:
			say "You cannot find it now, it must have been kicked away in the midst of everyone running around in the dark.";
		otherwise:
			say "The food is all swept up by now.";
	
Instead of going north from the Kitchen:
	say "There are a ton of tall scary animals walking around.  There doesn't seem to be anything motivating you enough to go that way."

Instead of going south from the Kitchen:
	say "You are at the edge of the kitchen so there is just a wall.  Unfortunately, your dreams to be a Ninja Turtle fail you at this point and you are unable to scale the wall."
	
Instead of going west from the Kitchen:
	if the bread is not eaten and the lettuce is not eaten:
		say "You're so hungry, you don't want to leave here yet...  You need to muster up the courage to get a little bit of food to carry on!";
	otherwise:
		continue the action;
		
Instead of going east from the Kitchen:
	if the bread is not eaten and the lettuce is not eaten:
		say "You're so hungry, you don't want to leave here yet...  You need to muster up the courage to get a little bit of food to carry on!";
	otherwise:
		continue the action;
		
Instead of going east from the Kitchen:
	move the player to the Control Room;
	if the height-counter is 0:
		say "The buttons and switches are all out of reach... but you have an urge to reach up and try to get them anyway.  You're a turtle, you don't know any better.
		
		";
	if the light-counter is 1:
		say "Someone figures out just in time where the breaker is and gets the emergency lights on, meanwhile, the engineer fixes the wires that caused the light outage. [if the lettuce is eaten]Then the control room guard sees you...[end if]	
			
		";
		decrement the light-counter;
		if the lettuce is eaten:
			say "'What, what?  What are you doing here little one?  Is that a piece of lettuce in your mouth?  Aww, well I have some extra lettuce for you up here on the console.'
			
			You are lifted onto the console with all the buttons.";
			increment the height-counter;
	if the hiding-counter is 1:
		decrement the hiding-counter;

Section 12 - Control Room Scene Actions and Objects

The President Switch is an edible thing in the Control Room.  The President Switch can be eaten.
The Enemy Switch is an edible thing in the Control Room.  The Enemy Switch can be eaten.
The Missle Button is an edible thing in the Control Room.  The Missle Button can be eaten.
The Console is a thing in the Control Room.
The description of the Console is "A beeping and booping console on the north wall that controls the missles of the base as well as communication between the president and the current enemy.  You are mesmerized by the buttons, they look tasty, but also from all the flashing lights.  The designers of this console definitely had turtles in mind.";

Report hiding:
	if the player is in the Control Room and the hiding-counter is 0:
		say "The control room guard returns.
		
		'I am about to do some very important business so you're going to have to skidaddle along now.'";
		move the player to the War Room; [move player to the end of the game]
		increment the war-counter; [we may have initiated war]

The description of the President Switch is "It is a little green button, it looks so shiny and tasty to you.  Maybe if you just took a little nibble..."

The description of the Enemy Switch is "It is a little orange button, you remember how good Oranges taste and that sort of makes you hungry again.  Maybe if you just took a little nibble..."

The description of the Missle Button is "It is a large red button that looks a lot like a cherry, but your turtle brain cannot process that since you don't remember ever having had that sort of food before.  It does look shiny and delicious though.  Maybe if you just took a little nibble..."

Before going west from the Control Room:
	if the height-counter is 1:
		say "[if the kitchen-eaten-counter is 1]You remember the Kitchen area, it was full of wonderful food. But y[otherwise]Y[end if]ou cannot since you are too high up and falling down would kill you.  Your little turtle instincts remind you to not walk off of tall places.";
		stop the action;

Before going east from the Control Room:
	if the height-counter is 1:
		say "[if the closet-counter is 1]You remember the Closet area, it was dark and scary. You never want to go back, but y[otherwise]Y[end if]ou cannot since you are too high up and falling down would kill you.  Your little turtle instincts remind you to not walk off of tall places.";
		stop the action;

Instead of going north from the Control Room:
	say "There is only a bunch of lit up screens and diodes in front of you; you cannot go anywhere...";

Instead of going south from the Control Room:
	if the height-counter is 1:
		say "You are too high up, if you go that way then you'll fall off and die.  Your little turtle instincts remind you to not walk off of tall places.";

Instead of eating the President Switch:
	if the height-counter is 0:
		say "You reach up with your tiny turtle mouth, but the console is too high for you.
		
		'What's this?  Hello little turtle!  How did you get in here?'
		
		'Here have some of my leftover lettuce from lunch!'
		
		You feel very satisfied.
		
		'I am about to do some very important business so you're going to have to skidaddle along now.'";
		move the player to the War Room;
		increment the war-counter; [we may have initiated war]
	otherwise:
		if the bread is eaten:
			say "Yum, this is a tasty one, but curiously it won't come loose and you cannot bite into it...
			
			'Oh, is that you Commander?  Stop the attack!  We are about to call on peace talk with the enemy nation.'
			
			Since no one hears this, the missles are not fired anyway.
			
			The peace talks ensue with the enemy nation and the two countries come to an agreement.  No one is killed by nuclear war and the general populous is now at ease.  You don't care one bit, you are just happy that you were able to just have fallen asleep on a full stomach and on top of a warm console.";
			end the story;
		otherwise:
			say "Yum, this is a tasty one, but curiously it won't come loose and you cannot bite into it...
			
			'Hello?  Commander, are you there?  Hmm, must have been a glitch.'
			
			Because the president spent too much time dealing with the unintended call, the enemy decided to fire missles before answering the call for peace talks, which came too late.   Millions of people die, but at least you were left with a full stomach, a safe underground bunker, and a place to sleep.";
			end the story.
		
Instead of eating the Enemy Switch:
	if the height-counter is 0:
		say "You reach up with your tiny turtle mouth, but the console is too high for you.
		
		'What's this?  Hello little turtle!  How did you get in here?'
		
		'Here have some of my leftover lettuce from lunch!'
		
		You feel very satisfied.
		
		'I am about to do some very important business so you're going to have to skidaddle along now.'";
		move the player to the War Room;
		increment the war-counter;
	otherwise:
		if the bread is eaten:
			say "The enemy gets on the line as you are eating the switch viciously.
			
			'Hello?  Anyone there?  Is this a trick!?'
			
			.... only you are in the room.
			
			'I don't trust this peace treaty anymore if all you're going to do is mock me and play dumb trickeries.  I will not have this disrespect!'
			
			The enemies start the war with you and killed millions of lives when our country was simply trying to call a treaty.  But... at least you were in the bunker safe and sound, and with food.  You fall asleep with a full stomach.";
			end the story;
		otherwise:
			say "The enemy gets on the line as you are eating the switch viciously.
			
			'Hello?  Anyone there?  Is this a trick!?'
			
			.... only you are in the room.
			
			'I was waiting for a possible peace treaty, but now this has maddened me enough!  I will not stand for this disrespect!'
			
			The enemies start the war with you and killed millions of lives when there was the chance that they were going to at least join the peace talks.  But... at least you were in the bunker safe and sound, and with food.  You fall asleep with a full stomach and no cares in the world.";
			end the story.

Instead of eating the Missle Button:
	if the height-counter is 0:
		say "You reach up with your tiny turtle mouth, but the console is too high for you.
		
		'What's this?  Hello little turtle!  How did you get in here?'
		
		'Here have some of my leftover lettuce from lunch!'
		
		You feel very satisfied.
		
		'I am about to do some very important business here in a second so you're going to have to skidaddle along now.'
		
		You are removed from the underground facility and are placed outside.";
		move the player to the War Room;
		increment the war-counter;
	otherwise:
		if the bread is eaten:
			say "'Nuclear missles armed and firing!'  You are confused as to why the button will not chew...  You continue trying to eat it.
			
			'Nuclear... Nuclear... Nuclear missles arm... Nuclear missles armed and firing!'
			
			You have inadventently started a nuclear war with a neighboring country, but since the commander already was able to convince the president to create a treaty it is a disaster.  The countries loose all faith in each other a nuclear war proceeds.  It all got started for nothing...
			
			At least you're not hungry anymore.";
			end the story;
		otherwise:
			say "'Nuclear missle armed and firing!'  You love the taste of the button, but are unsure as to why you cannot bite into it...
			
			'Nuclear... Nuclear... Nuclear missles arm... Nuclear missles armed and firing!'
			
			The odd timing of the nuclear strike causes the enemy to be caught off guard and you destroy the whole country with a barrage of missles.  The enemy is surely defeated and you saved millions of your own country's lives.
			
			You feel disatisfied and fall asleep on the console.";
			end the story.
			
Section 13 - War Room Scene (final scene) Ending Descriptions

Every turn:
	if the war-counter is 1:
		if the bread is eaten:
			say "'I have had an epiphany today while I was eating my lunch.  A little turtle came by and started eating a little bit of bread next to my foot and I wondered if there were similar little creatures in the enemy territory.'
			
			'Not likely sir!  They completely burned down all the --'
			
			'Don't interrupt me adviser!  So, as I was saying, I don't think we should start this war because it would only kill more and more living things.  We do not want to end up like the enemy.  I am going to call the President now to give him my professional recommendation.'
			
			And so the President is called and persuaded to call off the attack and start the peace talks.  You don't care one bit because you're a turtle, but at least billions of lives have been saved today and you got to leave on a full stomach.";
			end the story;
		else if the carrot is eaten:
			say "As the war room board continues conversation...
			
			'That light outage was something wasn't it?!  Just at the wrong time, when we were about to make the final decision about attacking the enemy or not.  Did anyone catch the spy?  I saw no one out of the ordinary!'
			
			'We have no idea what happened, but we do know that our Commander has died and just before making the important decision.  We have no choice but to assume this was something the enemy planned and we MUST strike now!'
			
			And so the President is called and persuaded to continue the attack.  The enemy is attacked and millions of people die, just because you wanted to have your little carrot.  No one will know what really happened, and you just simply don't care.  Face it, you're just a little turtle.";
			end the story;
		otherwise:
			say "'This enemy has been going on bullying other nations and threatening our own far too long!  They talk of peace, but we talk of defense against their treachery.  We must act now before they act!  *brrst*  Control Room!  Fire the missles!!'
			
			*brrst* 'Yes sir!' *brsst* (from the Control Room)
			
			'Now, our nation state will be safe.  It is a treacherous moment, but a necessary one. '
			
			You don't care one bit about what is happening.  Surely your own nation is now safe, but all you care about is the fact that you got to leave with a full stomach.";
			end the story;