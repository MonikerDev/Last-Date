<!-->
Develop an Internet facing Senior Project Portfolio web site that can be accessed by potential employers. 
The web site can be custom web site hosted on any Internet Hosting Company or can be Markdown pages developed and employed on a GIT repository, such as Github or Bitbucket. 
The web site at a minimum should contain:
   an overview the project, DONE
   appropriate design diagrams, 
   code snippets, 
   other desired supporting artifacts, 
   and details of the background of the project, 
   approach to implementation, 
   and how the user needs to do to run/access the project. DONE 
This portfolio should fully summarize and demonstrate the work completed during the Senior Project in CST-451 and CST-452
<-->


# __LAST DATE__

[![](https://img.youtube.com/vi/4AC5_KpQ0t0/maxresdefault.jpg)](https://youtu.be/4AC5_KpQ0t0)

_Last Date_ is a survival-horror, role-playing game with a dating-simulation set up. The game follows the exploits of seven girls and their desperate attempts to survive the spreading horrors within their town. Delve into the darkest corners of Coolbrook and discover the truths that lie within. Those who seek answers may not like all that they find, however, as sometimes the truth is best left buried. Coolbrook does not see many visitors outside of students, so most of its businesses and people have been there as long as they can remember. It is never too late to prepare for the worst. Whether or not the decision is to face that which lurks beneath the surface, one must stay prepared in this world. Gather supplies, plans, and allies to face the truth. Even then, time is precious.

## __Gameplay Pillars__

### __Decisions__

The narrative of Last Date is relatively player driven and founded on core choices made by the player in times of need. The characters they ally with, supplies they obtain, and even secrets they learn will all be up to what they decide to do with their time and whether they can use it wisely. These decisions will mainly show up in a few places, the most blatant of which being dialog choices. A key piece of the gameplay is interacting with people around town including the other protagonists, building careful alliances, and gathering information. It is hard to gauge a world ending situation alone, so the player will need all the help they can get. On top of this the players’ exploratory decisions will be key to survival as well. Something of this horrific nature does not simply walk out amongst daylight. The truth to the secret of the town and the Brood Keeper itself lie in wait to be found, the question is just where. The player only has so much time to investigate, but if they use it wisely, they might just make it through this all yet. Lastly the items gathered along the way open new options and actions, meaning the player will need to be resourceful to navigate the woods surrounding Coolbrook. Any loose equipment could be life and death later, but they can only carry so much with them.

### __Limited Time__

One of the key elements of keeping the player feeling cornered is the imposed time limit. Right from the get-go, whether they know the game has a hard limit yet or not, they know that everything they do is being counted as the story progresses. The intent of this is to make the player be careful when making their decisions and try to tiptoe over danger as they progress. From a design perspective just the right amount of punishment for this way of thinking will cause the player to hesitate to move forwards, and almost feel unsure of any decision they come to. This is the intended audience feeling of Last Date.

### __Combat__

![A screenshot showing the battle system layout. There are four player characters on the left, and enemies on the right. To the right side of the screen are four portraits of the player characters with their health and energy. At the bottom is the dialog box with the options for the player turn.](https://imgur.com/PuGk3J8)



Combat in games can be clunky or intrusive to the flow of things, but in this case, they are to subvert the standard horror formula of the monster instantly killing you if it finds you. The player is given a chance to struggle away or hold their ground but at the cost of their wellbeing. Getting caught is not final but it is in no way rewarding by design. Damage is relatively permanent and means to heal oneself are far and few between. Overall, it is intended to best be avoided.

### __Exploration__

Among every overworld gimmick, exploring takes time. Moving from one section of town to the other costs time, so the player will need to be careful with how often they move between subsections of the map. While the sections are big enough to allow a handful of things to do per section, the player will often need to go between them for questlines and following story threads. The design of the map needs to make it feel like there is something to find everywhere in the map, that exploring holds valuable secrets. There needs to be a careful balance between it and other activities for surviving. Each location holds different valuable information or needs for progression. Some doors require keys, dark places require a light source, and some places are restricted from public access. 

## __Background__

<!-->Snippet talking about inspirations and how the idea came to be<-->

## __Implementation__

### __Battle System__
<!-->Show Battle System Design<-->

### __Movement System__
<!-->Movement System?<-->

### __Enemy AI__

### __Database__
<!-->Diagrams for SQLite DB<-->
All of the game's triggers and flags are handled in a single shared database.


## __Project Goals__

As of right now the project is simply a prototype. The idea was novel on paper but turned out to be less fun to play than anticipated. This means that the overall design needs a few reworks, and while there are ideas recorded efforts will not be made to implement them until further notice. However, for the goal of building the prototype itself, all requirements were met and the prototype was satisfactory despite its short comings.

## __Technologies Used__

### __Unity__

Unity was the engine of choice for this game project. We used __2022.1.7f1__ since it was the most stable build available at the time of starting.

> [!NOTE]
> Our Unity version is not the most recent, so some engine features may not be present.

### __Yarnspinner__

Yarnspinner is a dialog engine built by _Yarn Spinner ltd._ originally for _Secret Lab's "Night in the Woods"_. As it is opensource for use, it made for a great way to implement dialog without worrying about the overall control of it. 

> [!TIP]
> You can find more info as well as their details [here](https://www.yarnspinner.dev).

## __Notes__

* The game needs a complete redesign, but theoretically the only issue is the combat system
* There were some systems imagined that were out of scope for the time frame
  * A fleshed out item system for use in combat
  * Overworld decay
  * Character-specific stats
  * A full overworld map and host of locations

## __HOW TO PLAY__

As of right now there is no direct way to play as this repository is the uncompiled code. However official builds and releases may be posted, and links to the downloads and or instructions can be found here upon the change of that fact.

> [!WARNING]
> As this is a proof of concept, the project is not guaranteed to be finished and all design decisions are subject to change.