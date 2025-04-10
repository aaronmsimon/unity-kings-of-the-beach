# Kings of the Beach
 
<img src="https://thumbnails.libretro.com/Nintendo%20-%20Nintendo%20Entertainment%20System/Named_Boxarts/Kings%20of%20the%20Beach%20-%20Professional%20Beach%20Volleyball%20%28USA%29.png" />

<table>
	<tr>
		<td><img src="https://thumbnails.libretro.com/Nintendo%20-%20Nintendo%20Entertainment%20System/Named_Titles/Kings%20of%20the%20Beach%20-%20Professional%20Beach%20Volleyball%20%28USA%29.png" /></td>
		<td><img src="https://thumbnails.libretro.com/Nintendo%20-%20Nintendo%20Entertainment%20System/Named_Snaps/Kings%20of%20the%20Beach%20-%20Professional%20Beach%20Volleyball%20%28USA%29.png" /></td>
	</tr>
	<tr>
		<td colspan="2"><img src="https://github.com/aaronmsimon/unity-kings-of-the-beach/blob/main/Resources/Spike.gif" /></td>
	</tr>
</table>

I used to love this game as a kid. Actually, I still do. The Paris Olympics have got me playing this again. I want to find a fun game like this to play, but with modern tournament rules. Basically, replay the Olympics. Maybe even have an arcade vs simulation style depending on how realistic you want to play.

I would like to actually finish this game this time. That means starting with a working MVP and then expanding upon it. In order to do so effectively, I'll need to break it down into it's basic parts. Similar to Code Monkey's [Kitchen Chaos](https://youtu.be/AmGSEH7QcDg) and [XCOM](https://www.gamedev.tv/dashboard/courses/26) courses.

[Canva attempt](https://www.canva.com/design/DAGSRB4ZBE0/guNKf3ODCAnbA20KxX11iw/edit)

### Process

#### Environment
1. ~~Create field of play with textures~~
2. ~~Create net with colliders~~
3. Stadium
4. Skybox
5. ~~Stadium Scoreboard~~

#### Player - Arcade
1. ~~Create Player Controller - Movement~~
2. ~~Create player model~~ (using [Imphenzia's Crispoly Character](https://imphenzia.com/crispoly-characters-mini))
3. ~~Create Player Controller - Bump~~
4. ~~Raycasts for collision detection to prevent movement~~
5. ~~Lock player in place to target~~
6. ~~Implement a State Machine to control movement/locking~~
7. ~~Aiming Bump~~ with Accuracy
8. ~~Don't move immediately after Bumping (from aiming)~~
9. ~~Animations~~
10. ~~Feint Spike~~
11. Block calls
12. Confidence (post point, skip option)

#### AI - Arcade
1. ~~AI Controller - Move towards target (while ball is active)~~
2. ~~AI Controller - Bump~~
3. ~~AI Controller - Aim logic~~
4. ~~AI Controller - State Machine~~
5. ~~Estimating out of bounds~~
6. ~~Defensive positioning~~
7. ~~Pass towards teammate~~
8. ~~Offensive positioning~~
9. Block calls (making & obeying)
10. AI Controller - Aim with variable opponents

#### Gameplay
1. ~~Randomness in aim~~

#### Skills
1. ~~Speed~~
2. ~~Pass Accuracy (affects aim, AI only)~~
3. Dig (more important with speed of ball)
4. ~~Serve Skill (affects serve aim)~~
5. Serve Power (affects ~~speed and~~ direction)
6. ~~Blocking (likelihood to be successful)~~
7. ~~Spike Power (affects speed)~~

#### Ball Mechanics 🏐
1. ~~Move to Target with Bezier curve~~
2. ~~Ball Fields and Properties~~
3. ~~Ball Events triggered when Property Set❓~~
4. ~~Ball State Machine (probably need to change ballstate.bump to inair/active/inplay/etc)~~
5. ~~Display target on playing surface (to help player)~~
6. ~~Ball into net based on Raycast - cleanup needed: checking twice~~
7. ~~Ball detection outside of legal range at net~~

#### Assistants
1. Coach - Bump
2. Coach - Set
3. Coach - Spike
4. Coach - Serve
5. Coach - Block

#### Rules
1. ~~Same player can't hit ball twice in-a-row~~
2. One side can only hit ball three times on side
3. ~~Point when ball hits ground (in/out depending on who last touched)~~
4. Switch sides after 7 points
5. ~~Must pass over net (between sides)~~

#### Presentation
1. ~~Cinemachine~~
2. ~~Post Processing - Bloom~~
3. Scoreboard
4. Instant Replay for in/out calls

#### Menus
1. ~~Game setup: choose players~~
2. ~~Game setup: choose outfits~~
3. Game setup: choose points/set, sets/match, assign side
4. Game setup: players assigned to teams
5. Pass setup info to Match scene

#### UI
1. Reference: [Game UI Database](https://www.gameuidatabase.com/)

### Concepts/Ideas
- use right stick to reach for ball on dig
- amount of power, direction, depends on angle of ball, how quickly can get under it, etc
- is amount of power based on how long button is held?
- block calls
- confidence, high five/hug after every point and hold hands when changing sides
- NEW interaction concept!!  hold for dig, press & release for directed pass/shot
- Replays
- Argue calls
- Pass will be the gameplay mechanic for both Dig and Set, but Set will be more accurate. Will add Set to improve accuracy of Spike on next hit
- adjustments for men vs women (spike speed, net dimensions, etc)

### Cleanup
- bump/set vs pass/across
- review all state machines for proper state handling
- don't actually need all these variables (events, too?) as Scriptable Objects
- change control names to be more universal

### Kanban Board
To help with tracking the to-do items and their status, I've created a [Kanban board in Trello](https://trello.com/b/JTSdHvtL/kings-of-the-beach). This should have been done from the beginning of the project or very long ago, but better late than never, right? 
I started, finally, because I wanted to keep tabs on the bugs as I attempt to finalize the gameplay. All other enhancements/to-dos in the readme have been moved there. The above "Process" items will be added to the board as I get to them.

### Considerations if Bugs
- In case there are issues with passing/spiking ball, consider the LockedOn feature
- In case there are issues with match management, consider the MatchManager StateMachine now initilized in Start

Replay System Notes
- Can i save every frame since not a long time. frame rate could be as low as physics (60)
- Automatic replays will happen when good plays happen (ideas below), and can be ran from pause menu
- Ability to save replays

Automatic Replay Reasons
- Game winning point
- Long rally (4+ hits)
- Power spike or block
