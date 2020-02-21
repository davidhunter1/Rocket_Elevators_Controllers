# Rocket_Elevators_Controllers
Navigating the Javascript and Python File: <br>
In order to use and see the results of the Javascript code and its testing, the JS file must be copied into an IDE.
I have shown the first test, and hidden the second two. That can be manipulated by adding or removing the 
forward slashes which were placed in front of "Test2();" and "Test1();"
As for Python, copying the code to Repl.it or another desired location will have the same effect. Removing the pound (#) placed before the 
function will have the same effect as removing the forward slashes.


Additionally, new tests can be created by manipulating the position, orientation, status, and queue of the two
elevators by simply editing them within the code. Of course, the Request queue can be manipulated to change the
floor at which it was called at (X,_), and the desired orientation (_,"up/down") as shown below:

function Test1() {
	
	columntest = new Column(10, 2);

	columntest.cagesList[0].currentFloor = 9;   <--- Manipulate the elevators current floor (0-9)
    columntest.cagesList[0].direction = "";   <--- Manipulate the elevators direction (Up, down)
    columntest.cagesList[0].status = "idle";  <--- Manipulate the elevators status (Idle, Moving, Up, Down)
	columntest.cagesList[0].queue = [];         <--- Manipulate the elevators queue ( 1-9, multiple numbers can be placed within)

	columntest.requestCage(3, "up");    <--- Manipulate the requested floor, and desired direction




