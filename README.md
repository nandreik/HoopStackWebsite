# Hoop Stack Solver
https://hoopstacksolver.azurewebsites.net/
**IF SITE IS DOWN IT IS POSSIBLY BECUASE THE 1HR/DAY COMPUTATION TIME HAS BEEN USED BY THE FREE AZURE HOST SERVICE**
This project is a level solver for the mobile game Hoop Stack. 

It was made primarily as my own introduction to .Net Core, C#, and to get an idea of full stack development using the MVC design pattern. 

## Features 
#### 1) Simple Webpage UI using Razor pages
	The user input and output are all located on the homepage to make things more convenient.
#### 2) Manual Level Entry
	Level entry was desgined to be relatively simple with the help of input validation. 
#### 3) Level Search 
	The ability to search for previous levels that were input into the website. 
#### 4) Functional SQL Database 
	The database stores previous levels for the Level Search functionality. 
	New levels are checked with the database before they are inserted to avoid duplicates. 
#### 5) Custom Level Solver Algorithm 
	A self-implemented recursive algorithm was written to solve Hoop Stack levels. 
	The algorithm follows 4 different rules for solving a level:
	
	Until Level Solved = 
		1: If there is an empty stack, move the most common Top of Stack (TOS) hoops to that empty stack.
		2: If there is a stack with only one color, move any other TOS with the same color to that stack.
		3: If not 1 or 2, choose the shortest stack and move any TOS that match the TOS of the shortest stack to it.
		4: If the last move created a error (no possible moves to make or an infinite loop) 
		then add that previous move to a list of Wrong Moves and recursively solve the puzzle again, 
		without making that move. 



