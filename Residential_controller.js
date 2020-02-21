class Column {
    constructor(floors, cages) {
        this.floors = floors;
        this.cages = cages;
        this.cagesList = [];
        this.requestButtonList = [];
        for (let i = 0; i < cages; i++) {
            this.cagesList.push(new Cage(1, floors));
        }
		for (let i = 0; i < this.floors; i++) {
            if (i === 0) {
                this.requestButtonList.push(new RequestButton(i, "up", false));
                
            }else if (i === 9) {
                this.requestButtonList.push(new RequestButton(i, "down", false));
            }else {
                this.requestButtonList.push(new RequestButton(i, "up", false));
                this.requestButtonList.push(new RequestButton(i, "down", false));
            }
		}
    }

    findCage(requestedFloor, direction) {       //finding the appropriate elevator by the following circumstances://

		var bestGap = this.floors;
		var chosenCage = null;

		for (let i = 0; i < this.cagesList.length; i++) { //if elevator and request are ascending and the request is above the elevator
			if (this.cagesList[i].direction === "up" && direction === "up" && requestedFloor > this.cagesList[i].currentFloor) {
                chosenCage = this.cagesList[i];
			}else if (this.cagesList[i].direction === "down" && direction === "down" && requestedFloor < this.cagesList[i].currentFloor) {
                chosenCage = this.cagesList[i]; 
                     //if elevator and request are descending and the request is below the elevator
            }else if (this.cagesList[i].status == "idle") {
                chosenCage = this.cagesList[i];
                    //else if the status is idle
			}else {
				for (let i = 0; i < this.cagesList.length; i++) {
                    let gap = Math.abs(this.cagesList[i].currentFloor - requestedFloor); //absolute value of the elevator floor - requested floor
                    if (gap < bestGap) {
                        chosenCage = this.cagesList[i];
                        bestGap = gap;   //shortest gap becomes the chosen gap
                    }
                }
			}
		}
		console.log("Optimal cage found on floor " + chosenCage.currentFloor);
		return chosenCage; //console.log output displaying the chosen elevator
	}
	requestCage(requestedFloor, direction) {

		console.log("Called cage to floor " + requestedFloor);
		
		let cage = this.findCage(requestedFloor, direction);

		cage.addToQueue(requestedFloor);
        cage.move();
		return cage;
	}

	requestFloor(cage, requestedFloor) {
        console.log("Sending cage from floor " + cage.currentFloor + " to floor " + requestedFloor);
		cage.addToQueue(requestedFloor);
		cage.closeDoors();
		cage.move();
    }
}

class Cage { 
	constructor(currentFloor, floors) {

		this.direction = null;
		this.floors = floors;
		this.currentFloor = currentFloor;
		this.status = "idle";
		this.queue = [];
		this.cageButtonList = [];
		this.door = "closed";

		for (let i = 0; i < this.floors; i++) {
            this.cageButtonList.push(new CageButton(i, false));
        }
    }
	addToQueue(requestedFloor) {
		this.queue.push(requestedFloor)

		if (this.direction == "up") {
			this.queue.sort((a, b) => a - b)
		}
		else if (this.direction == "down") {
			this.queue.sort((a, b) => b - a)
		}

		console.log("Added floor " + requestedFloor + " to the cage's queue. Current queue: " + this.queue.join(", "));
	}
	move() {
		console.log("Moving cage");
		while (this.queue.length > 0) {

            let firstElement = this.queue[0];
            
			if (firstElement == this.currentFloor) {
				this.queue.shift();
				this.openDoors();
			}
			if (firstElement > this.currentFloor) {
				this.status = "moving";
				this.direction = "up";
				this.moveUp();
			}
			if (firstElement < this.currentFloor) {
				this.status = "moving";
				this.direction = "down";
				this.moveDown()
			}
		}
		if (this.queue.length === 0) {
			console.log("Unloading Passengers");
			this.closeDoors();
			console.log("Cage is now idle");
			this.status = "idle";
		}
	}
	moveDown() {
		this.currentFloor--;
		console.log("descending cage currently on floor " + this.currentFloor);
    }
    
    moveUp() {
		this.currentFloor++;
		console.log("ascending cage currently on floor " + this.currentFloor);
    }
    
	openDoors() {
			this.door = "open"
			console.log("<> Opened doors");
	}
	
	closeDoors() {
			this.door="closed"
			console.log(">< Closed doors");
	}

}

class RequestButton {
	constructor(requestFloor, direction, buttonPressed) {
		this.requestFloor = requestFloor;
		this.direction = direction;
		this.pressed = buttonPressed;
	}
}

class CageButton {
	constructor(floor, buttonPressed) {
		this.floor = floor;
		this.buttonPressed = buttonPressed;
	}
}

console.log("->Test<-")

function Test1() {
	
	columntest = new Column(10, 2);

	columntest.cagesList[0].currentFloor = 2;
    columntest.cagesList[0].direction = "";
    columntest.cagesList[0].status = "idle";
	columntest.cagesList[0].queue = [];

	columntest.cagesList[1].currentFloor = 6;
	columntest.cagesList[1].direction = "";
	columntest.cagesList[1].status = "idle";
	columntest.cagesList[1].queue = [];

	columntest.requestCage(3, "up");
}

Test1();
