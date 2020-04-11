# Toshiko
## Abstract
AI is an underutilized aspect of game development in creating dynamic obstacles in fighting games (1). The projects goal is to use AI in an interesting way that creates a new and interesting player experience. The game is a 2D game where you face a boss that learns every time its beaten. The player needs to find new ways to beat or outsmart the boss as it learns and adapts to the way the player beats it. 
The main part of the project will involve creating a complex AI for the boss which will find a counter to the players way of beating it. The core game loop will involve the player having to beat the AI a certain number of times making it progressively harder each time. The player will die many times to the boss as they learn the enemies moves and patterns with new ones being introduced every time the boss is beaten. Meaning the AI will need to be versatile enough to counter anything the player can come up with.
The project will also have a complex animation system. Because the game takes place in real time the player character movement needs to be responsive to feel good to play. While the boss attacks need to be well telegraphed so that the player knows how to react and what the boss is doing to plan accordingly (2).
The game will be evaluated by letting many different types of people play it to gauge and adjust the difficulty and to fix any bugs and exploits that might break the game. And making them play it multiple times to see how engaging it is to replay it.

![GameLoop](https://github.com/Marcin7373/FYP-Game/blob/master/Kra/Game-loop.png?raw=true) 

## Instructions
Go right and beat the boss. Increase difficulty with instructions below if too easy.
On hard the boss gets stronger everytime its beat, so see how many times you can be it.
Controller is recommended.
Controller or lack of one is detected automatically:

### PS4
Circle   = dash  
X        = jump  
Square   = attack  
Triangle = fade (pass through attacks but lose health)  
Down     = crouch  
D-Pad up, down = volume  

### Keyboard
c        = attack  
x        = dash  
z        = fade (pass through attacks but lose health)  
up       = jump  
down     = crouch  


esc      = quit  
q        = volume down  
w        = volume up  
difficulty  
'1'        = easy  
'2'        = medium  
'3'        = hard  

## Gameplay Examples
![Prototype](https://github.com/Marcin7373/FYP-Game/blob/master/Kra/RayCasting.PNG?raw=true) 
![Prototype](https://github.com/Marcin7373/FYP-Game/blob/master/Kra/LaserAttack.PNG?raw=true) 