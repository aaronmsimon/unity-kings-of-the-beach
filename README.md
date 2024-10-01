# Kings of the Beach
 
<img src="https://thumbnails.libretro.com/Nintendo%20-%20Nintendo%20Entertainment%20System/Named_Boxarts/Kings%20of%20the%20Beach%20-%20Professional%20Beach%20Volleyball%20%28USA%29.png" />

<table>
	<tr>
		<td><img src="https://thumbnails.libretro.com/Nintendo%20-%20Nintendo%20Entertainment%20System/Named_Titles/Kings%20of%20the%20Beach%20-%20Professional%20Beach%20Volleyball%20%28USA%29.png" /></td>
		<td><img src="https://thumbnails.libretro.com/Nintendo%20-%20Nintendo%20Entertainment%20System/Named_Snaps/Kings%20of%20the%20Beach%20-%20Professional%20Beach%20Volleyball%20%28USA%29.png" /></td>
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

#### Player - Arcade
1. ~~Create Player Controller - Movement~~
2. Create player model
3. ~~Create Player Controller - Bump~~
4. ~~Raycasts for collision detection to prevent movement~~
5. ~~Lock player in place to target~~
6. ~~Implement a State Machine to control movement/locking~~
7. ~~Aiming Bump~~
8. ~~Don't move immediately after Bumping (from aiming)~~

#### AI - Arcade
1. ~~AI Controller - Move towards target (while ball is active)~~
2. ~~AI Controller - Bump~~
3. AI Controller - Aim logic
4. AI Controller - State Machine
5. Estimating out of bounds
6. Defensive positioning
7. ~~Pass towards teammate~~
8. Offensive positioning

#### Gameplay
1. Randomness in aim

#### Skills
1. ~~Speed~~
2. Pass Accuracy (affects aim)
3. Dig (more important with speed of ball)

#### Ball Mechanics
1. ~~Move to Target with Bezier curve~~
2. ~~Ball Fields and Properties~~
3. Ball Events triggered when Property Set
4. ~~Ball State Machine (probably need to change ballstate.bump to inair/active/inplay/etc)~~

#### Assistants
1. Coach - Bump

#### Rules
1. ~~Same player can't hit ball twice in-a-row~~
2. One side can only hit ball three times on side
3. Point when ball hits ground (in/out depending on who last touched)
4. Switch sides after 7 points

#### Presentation
1. ~~Cinemachine~~
2. Post Processing - Bloom
3. Scoreboard
4. Instant Replay for in/out calls

#### Menus

### Notes/Next Steps:
- get side based on team
- switch sides on 7 points
- start positioning
- athlete #2
- AI: see first 4 in AI
- AI: if both of us contact same ball (first hit), last contact will override, but count increases so will hit across

### Basic Updates to do (dynamic list)
#### Athlete
- disable movement to opposite court side
- also need to implement other locations like offensive and defensive (which are placeholders for now in the AI state machine, though this should also have a default for the player so consider adding to the Athlete script)
- (future) Add Serve Power to SkillsSO, use value in duration

#### AI
- don't chase ball when won't be hit over net (test for when this occurs)
- check all movement from each state [when ai hits to opposing side, keeps moving in last direction; needs to change state when possession change] - still some movement that needs to be zeroed out on state change
- figure out why both AIs chase serve sometimes
- figure out why one AI is jumping too soon
- AI Serve to location with random speed based on skill, chance to also go out
- Offense and DigReady states are moving the same way, practically -> DRY
- spike dir based on tactic

#### Ball
- Ball net detection

#### Player
- feint for player spike
- Between points, the players can high-five, hug, etc (see Concepts/Ideas bullet 5), then player controlled skip to service
- Implement the power meter into the actual service power strike on the ball: accurace for aiming & duration for speed

#### Match
- Complete all states
- Serve start
- When starting match, setup side for Serve
- Post Point: allow for celebration/skip
- review transitions that are not in a state
- set teams will need to be a function from game setup


### Needed for MVP
6. UI for scoring
7. Game State Machine
8. Blocking

### Concepts/Ideas
- use right stick to reach for ball on dig
- amount of power, direction, depends on angle of ball, how quickly can get under it, etc
- is amount of power based on how long button is held?
- block calls
- confidence, high five/hug after every point and hold hands when changing sides
- NEW interaction concept!!  hold for dig, press & release for directed pass/shot
- Replays
- Argue calls
- Pass will be the gameplay mechanic for both Dig and Set, but Set will be more accurate

### Cleanup
- is it better to create a static TagHashes class instead of hard-coding tag strings?
- magic numbers
- debug: writing to console, gizmos like raycasts or boxes drawn
- bump/set vs pass/across
- ball reset is temp