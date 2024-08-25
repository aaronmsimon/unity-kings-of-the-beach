# Kings of the Beach
 
<img src="https://thumbnails.libretro.com/Nintendo%20-%20Nintendo%20Entertainment%20System/Named_Boxarts/Kings%20of%20the%20Beach%20-%20Professional%20Beach%20Volleyball%20%28USA%29.png" />

I used to love this game as a kid. Actually, I still do. The Paris Olympics have got me playing this again. I want to find a fun game like this to play, but with modern tournament rules. Basically, replay the Olympics. Maybe even have an arcade vs simulation style depending on how realistic you want to play.

I would like to actually finish this game this time. That means starting with a working MVP and then expanding upon it. In order to do so effectively, I'll need to break it down into it's basic parts. Similar to Code Monkey's [Kitchen Chaos](https://youtu.be/AmGSEH7QcDg) and [XCOM](https://www.gamedev.tv/dashboard/courses/26) courses.

### Process

#### Environment
1. ~~Create field of play with textures~~
2. ~~Create net with colliders~~

#### Player - Arcade
1. ~~Create Player Controller - Movement~~
2. Create player model
3. Create Player Controller - Bump
4. ~~Raycasts for collision detection to prevent movement~~
5. ~~Lock player in place to target~~
6. Implement a State Machine to control movement/locking

#### AI - Arcade
1. AI Controller - Move towards target (while ball is active)
2. AI Controller - Bump
3. AI Controller - Aim logic

#### Ball Mechanics
1. ~~Move to Target with Bezier curve~~

#### Assistants
1. Coach - Bump

#### Presentation
1. Cinemachine
2. Post Processing - Bloom

#### Concepts/Ideas
- use right stick to reach for ball on dig
- amount of power, direction, depends on angle of ball, how quickly can get under it, etc
- block calls

#### Cleanup
- is it better to create a static TagHashes class instead of hard-coding tag strings?
- magic numbers
- debug: writing to console, gizmos like raycasts or boxes drawn