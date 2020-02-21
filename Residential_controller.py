class Column():
    def __init__(self, floors, cages):
        self.cages = cages
        self.floors = floors
        self.cagesList = []
        self.requestButtonsList = []
        for i in range(cages):
            self.cagesList.append(Cage(0, floors))
        for i in range(floors):
            if i == 0:
                self.requestButtonsList.append(RequestButton(i, "up"))
            else:
                self.requestButtonsList.append(RequestButton(i, "up"))
                self.requestButtonsList.append(RequestButton(i, "down"))

    def findCage(self, requestedFloor, direction):

        bestGap = self.floors
        chosenCage = None

        for cage in self.cagesList:
            if cage.direction == "up" and direction == "up" and requestedFloor < cage.currentFloor:
                chosenCage = cage
            elif cage.direction == "down" and direction == "down" and requestedFloor > cage.currentFloor:
                chosenCage = cage
            elif cage.status == "idle":
                chosenCage = cage
            else:
                for cage in self.cagesList:
                    gap = abs(cage.currentFloor - requestedFloor)
                if gap < bestGap:
                        chosenCage = cage
                        bestGap = gap
        print("Optimal cage found on floor " + str(chosenCage.currentFloor))
        return chosenCage

    def requestFloor(self, cage, requestedFloor):
        print("Sending cage from floor" + str(cage.currentFloor) + " to floor " + str(requestedFloor))
        cage.addToQueue(requestedFloor)
        cage.closeDoors()
        cage.move()

    def requestCage(self, requestedFloor, direction):
        print("Called cage to floor " + str(requestedFloor))

        cage = self.findCage(requestedFloor, direction)

        cage.addToQueue(requestedFloor)
        cage.move()
        return cage

class Cage():
    def __init__(self, currentFloor, floors):
        self.direction =  None
        self.floors = floors
        self.currentFloor = currentFloor
        self.status = "idle"
        self.queue = []
        self.cageButtonsList = []
        self.door = "closed"

        for i in range(self.floors):
            self.cageButtonsList.append(CageButton(i))

    def addToQueue(self, requestedFloor):
        self.queue.append(requestedFloor)

        if self.direction == "down":
            self.queue.sort(reverse=True)
        if self.direction == "up":
            self.queue.sort(reverse=True)


        print("Added floor " + str(requestedFloor) + " to the cage's queue. Current queue: " + ', '.join(str(x) for x in self.queue))

    def move(self):
        print ("Moving cage")
        while len(self.queue) > 0:

            firstElement = self.queue[0]

            if self.door == "open":
                print("Waiting for doorway to be cleared")
                self.closeDoors()
            if firstElement == self.currentFloor:
                del self.queue[0]
                self.openDoors()
            elif firstElement > self.currentFloor:
                self.status = "moving"
                self.direction = "up"
                self.moveUp()
            elif firstElement < self.currentFloor:
                self.status = "moving"
                self.direction = "down"
                self.moveDown()
        print("Waiting for door to be cleared")
        self.closeDoors()
        print("Cage is now idle")
        self.status = "idle"

    def moveDown(self):
        self.currentFloor -= 1
        print("Descending Cage currently on floor " + str(self.currentFloor))
	
    def moveUp(self):
        self.currentFloor += 1
        print("Ascending Cage currently on floor " + str(self.currentFloor))

    def closeDoors(self):
        self.door="closed"
        print("Closing doors")

    def openDoors(self):
        self.door = "open"
        print("Opening doors")



class RequestButton():
    def __init__(self, requestFloor, direction):
        self.requestFloor = requestFloor
        self.direction = direction

class CageButton():
    def __init__(self, floor):
        self.floor = floor


print("->Test<-")

def Test1():
    column1 = Column(10, 2)

    column1.cagesList[0].currentFloor = 2
    column1.cagesList[0].direction  =  ""
    column1.cagesList[0].status =  "idle"
    column1.cagesList[0].queue = []

    column1.cagesList[1].currentFloor = 6
    column1.cagesList[1].direction  =  ""
    column1.cagesList[1].status =  "idle"
    column1.cagesList[1].queue = []

    column1.requestCage(3, "up")

Test1()
