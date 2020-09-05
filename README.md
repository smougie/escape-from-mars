# Escape from Mars
Another project created while processing through "Complete C# Unity Developer 3D: Learn to Code Making Games" course. Once again I decided to expand course project with some of my ideas that came to my mind while project concept was presented by course teachers.

## Game Description
The player takes on the role of a rocket pilot and his job is to move from launch pad (point A) to landing pad (point B) meanwhile avoiding all of dangerous obstacles placed on levels. In addition, the player must pay attention to fuel level, if it fall down to zero, the player will have big problem. Traveling across space levels will go hand in hand with collecting alien coins. After level completion, level score is being calculated - the main component of the score value is percentage of collected alien coins and player lifes left.

## Project Description
The main assumption is to expand already known unity and C# mechanics. Expanding course project with my own ideas aims at providing player fully finished product.

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

### Level Obstacles
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

### Core Mechanics
-`Continue` read current player progress from PlayerPrefs and move player into level section where has possibility to restart already passed level to improve total score
-`New Game` clear all current game progress and start new game
-`Options` each section - Master Volume, Background Volume, SFX Volume - has own slider to adjust and save sound levels in PlayerPrefs
-`Highscores` read top ten scores from Highscore key stored in PlayerPrefs
-`Refuel and Landing Pad` after colliding with them, rocket is going into auto landing state which places rocket object in the center of pad in vertical position
