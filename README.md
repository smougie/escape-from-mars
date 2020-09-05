# Escape from Mars
## Project Description
Another project created while processing through "Complete C# Unity Developer 3D: Learn to Code Making Games" course. Once again I decided to expand course project with some of my ideas that came to my mind while project concept was presented by course teachers. Expanding course project with my own ideas aims at providing player fully finished product.

## Game Description
The player takes on the role of a rocket pilot and his task is to move from launch pad (point A) to landing pad (point B) meanwhile avoiding all of dangerous obstacles placed on levels. In addition, the player must pay attention to fuel level, if it fall down to zero, the player will have a big problem. Traveling across space levels will go hand in hand with collecting alien coins. After level completion, level score is being calculated - the main component of the score value is percentage of collected alien coins and player lifes left.

## My own vision
My own ideas which I extended the project with are:
- Alien collectible - player can collect alien coins (created using blender), those coins are one of  main components of level score.
- Rocket collectible - player can collect rocket collectible (created in blender) and restore his life if he already lost some. If current life is equal to maximum life, `Maximum rocket count` prompt will be displayed for a short time. Player lifes are displayed on life bar (three rockets).
- Life Bar - if player lose life, one of rockets will fade out (code created using C# script), if player picks up rocket collectible one of rockets will be filled once again, scaling rocket image to 120% scale and then drops to 100% (code created using C# script) to create simply animation.
- Auto Landing - when player collides with landing pad or refueling pad auto pilot system is turning on moving player into the center of landing/refuel pad and rotated to starting rotation. Meanwhile player cannot control the rocket.
- Fuel System - using space button to apply thrust uses rocket fuel which player can track on fuel bar.
- Refueling System - when collinding with refueling pad, rocket fuel is getting refueled. The smaller fuel level, the faster speed of refueling. Also refueling pad can be used as a checkpoint. If used as a checkpoint green flag(created in blender) will be raised behind refueling pad.
- Meteorites (obstacle) - created meteorites obstacle objects (created in blender) to simulate large group of meteorites falling from point A to point B. Same meteorites objects were used to create flying (single) meteorite from point A to point B. And one more obstacle was created using meteorites objects - I created hole in ground and surrounded it with crystals, than I simulated meteorite flying up from the hole.
- Metal Gates/Floodgates (obstacle)- constantly opening and closing doors, which can be adjusted  individually by setting some properties:
`delay time` we can delay gate start
`opened time` we can set how long gate stays opened
`locked time` we can set how long gate stay closed
`closing speed` we can set gate closing speed
`opening speed` we can set gate opening speed
- Fire from nozzle (obstacle)- simple fire created using particle system it can destroy player rocket or `metal boxes` spawned by `BoxMachine`. Fire Nozzle is also customizable:
`delay time` we can delay Fire Nozzle start
`prewarm time` period of time where prewarm fire is shown to warn player that soon Fire will be active
`active time` period of time where fire is active
`pause time` period of time where Fire Nozzle is not active
- Rotating Fan (obstacle) - simple fan created using unity primitives, placed on the wall and rotating, we can adjust rotation speed and direction
- Anomaly (obstacle) - simple anomaly created using unity primitives and particle system. We can adjust:
`delay time` delay anomaly start
`active time` how long obstacle will be dangerous
`pause time` how long obstacle will not be dangerous
- Lift (obstacle) - simple lift moving up and down created using unity primitives. Player has to move inside the lift, carefully move rocket with lift and travel further into level
- Metal Box (obstacle) - simple metal box created using unity primitive with five different materials. Box can collide with:
`Rocket` destroying it
`Squeezer Machine` getting moved by Squeezer face
`Fire Nozzle` when collides with nozzle fire, metling effect is getting spawned from metal box, after `2 - 4` seconds box is getting destroyed and explosion animation is getting spawned.
- Box Machine (obstacle) - machine is spawning metal boxes. Machine can work in `4` modes:
`static` machine stays in place and spawn metal boxes after specified time (can be adjusted in inspector same as delay start time)
`normal` machine travels from first point to last point, than moves to first point and repeat, spawning boxes when reach point 
`reverse` machine travels from first point to last point and from last point to first point,  spawning boxes when reach point
`random` machine pick random point and travel to them,  spawning boxes when reach point
- Camera Move - when level is to big, we set up camera trigger, when rocket trigger it, timeScale is set to 0f camera is moving to next point, previous stage entrance is getting blocked to avoid players trying go back and when camera is in right place timeScale is set to 1f. Camera triggers also enable next stage obstacles.

## Game Mechanics:
### Main Menu:
- Continue
If player already pass through first level Continue button will be shown and after pressing this button, player will be moved to Level Selection screen where he can track his progress, check level scores, restart and overwrite old level score to get the highest score.
- New Game
Starting new game, removing old progression
- Highscores
Player can check all Highscores. Highscore can be submited after completing all levels. Only ten best scores will be displayed.
- Options
Player can adjust master, background and SFX volume here.
- Quit
Quit the application

### Level Obstacles:
Rocket can only collide with alien collectible, rocket collectible, refueling pad and landing pad.
- Moving and Rotating rocks, crystals
- Narrow slits on the road to landing pad
- Flying with high speed meteorites
- Flying crystal kernel from ground holes
- Constantly opening and closing metal gates
- Rotating Fan
- Deadly fire from nozzles
- Mysterious space anomaly
- Moving lift
- Metal chests
- Metal chest machine `this machine is creating metal chests, they are mostly placed at the top of level which causes metal box to fall down until hit the floor`
- Squeezer `this machine is pushing metal boxes into fire`

### Core Mechanics:
- `Continue` read current player progress from PlayerPrefs and move player into level section where has possibility to restart already passed level to improve total score
- `New Game` clear all current game progress and start new game
- `Options` each section - Master Volume, Background Volume, SFX Volume - has own slider to adjust and save sound levels in PlayerPrefs
- `Highscores` read top ten scores from Highscore key stored in PlayerPrefs
- `Refuel and Landing Pad` after colliding with them, rocket is going into auto landing state which places rocket object in the center of pad in vertical position

## How to try?
- `A`/`Left Arrow` - rotate rocket left
- `D`/`Right Arrow` - rotate rocket left
- `SPACE` - apply thrust
Start on blue launch pad, finish on neon green landing pad, avoid obstacles and use refueling pad to reach checkpoint or refuel rocket.
