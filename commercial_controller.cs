//C# Commercial Elevator controller 

using System;
using System.Collections.Generic;
using System.Threading;

namespace CommercialController
{
    /// introducing the floors, elevators, columns, direction, battery, and lists
    public class ElevatorController
    {
        public int nbrFloors;
        public int nbrElevators;
        public int nbrColumns;
        public string userDirection;
        public Battery battery;
        public List<int> shortList;

        public ElevatorController(int nbrFloors, int nbrColumns, int nbrElevators, string userDirection)
        {
            this.nbrFloors = nbrFloors;
            this.nbrColumns = nbrColumns;
            this.nbrElevators = nbrElevators;
            this.userDirection = userDirection;
            this.battery = new Battery(this.nbrColumns);
        }


        /// <summary>
        /// This section is dedicated to the people desiering to go down.
        /// </summary>
        public Elevator requestElevator(int floorNbr, int requestedFloor)
        {
            Thread.Sleep(00);
            Console.WriteLine("REQUEST AN ELEVATOR TO FLOOR : " + floorNbr);
            Console.WriteLine("___");
            var column = battery.Find_best_column(floorNbr);
            userDirection = "DOWN";
            var elevator = column.Find_Requested_elevator(floorNbr, userDirection);
            if (elevator.elevatorFloor > floorNbr)
            {
                elevator.sendRequest(floorNbr, column.colNbr);
                elevator.sendRequest(requestedFloor, column.colNbr);
            }

            else if (elevator.elevatorFloor < floorNbr)
            {
                elevator.moveDown(requestedFloor, column.colNbr);
                elevator.sendRequest(floorNbr, column.colNbr);
                elevator.sendRequest(requestedFloor, column.colNbr);
            }
           

            return elevator;
        }

        //AssignElevator method, finds the best column, assigns the appropriate elevator
        
        public Elevator AssignElevator(int requestedFloor)
        {
            Thread.Sleep(500);
            Console.WriteLine("Request coming from floor: " + requestedFloor);

            Column column = battery.Find_best_column(requestedFloor);
            userDirection = "UP";
            var floorNbr = 1;
            Elevator elevator = column.Find_Assign_elevator(requestedFloor, floorNbr, userDirection);

            elevator.sendRequest(floorNbr, column.colNbr);
            elevator.sendRequest(requestedFloor, column.colNbr);

            return elevator;
        }
    }


    //  introduces elevator floors, direction and their variables
    public class Elevator
    {
        public int elevatorNb;
        public string status;
        public int elevatorFloor;
        public string elevator_direction;
        public bool Sensor;
        public int FloorDisplay;
        public List<int> floor_list;

        public Elevator(int elevatorNb, string status, int elevatorFloor, string elevator_direction)
        {
            this.elevatorNb = elevatorNb;
            this.status = status;
            this.elevatorFloor = elevatorFloor;
            this.elevator_direction = elevator_direction;
            this.FloorDisplay = elevatorFloor;
            this.Sensor = true;
            this.floor_list = new List<int>();
        }

        // sorts the request in ascending or descending order, based off of the direction
         public void sendRequest(int requestedFloor, char colNbr)
        {
            floor_list.Add(requestedFloor);
            if (requestedFloor > elevatorFloor)
            {
                floor_list.Sort((a, b) => a.CompareTo(b));
            }
            else if (requestedFloor < elevatorFloor)
            {
                floor_list.Sort((a, b) => -1 * a.CompareTo(b));

            }

            operElevator(requestedFloor, colNbr);
        }

        // once the elevator is moving, remove the current floor from queue. Changes and displays in console the floors progression 
        public void operElevator(int requestedFloor, char colNbr)
        {
            if (requestedFloor == elevatorFloor)
            {
                openDoors();
                this.status = "MOVING";

                this.floor_list.Remove(0);
            }
            //For descending. Moves and Displays the descending floors
            else if (requestedFloor < this.elevatorFloor)
            {
                status = "MOVING";
                Console.WriteLine("___");
                Console.WriteLine("Column : " + colNbr + " Elevator : " + this.elevatorNb + " " + status);
                Console.WriteLine("___");
                this.elevator_direction = "DOWN";
                moveDown(requestedFloor, colNbr);
                this.status = "STOP";
                Console.WriteLine("Column : " + colNbr + " Elevator : " + this.elevatorNb + " " + status);

                this.openDoors();
                this.floor_list.Remove(0);
            }
            //For ascending. Moves and Displays the ascending floors
            else if (requestedFloor > this.elevatorFloor)
            {
                Thread.Sleep(500);
                this.status = "MOVING";
                Console.WriteLine("___");
                Console.WriteLine("Column : " + colNbr + " Elevator : " + this.elevatorNb + " status:" + status);
                Console.WriteLine("___");
                this.elevator_direction = "up";
                this.Move_up(requestedFloor, colNbr);
                this.status = "STOP";
                Console.WriteLine("___");
                Console.WriteLine("Column : " + colNbr + " Elevator : " + this.elevatorNb + " " + status);


                this.openDoors();

                this.floor_list.Remove(0);
            }

        }
        //Door Methods. Open/Close
        public void openDoors()
        {
            Console.WriteLine("DOOR OPENED");
            Thread.Sleep(200);

            this.closeDoor();
        }
        public void closeDoor()
        {
            if (Sensor == true)
            {
                Console.WriteLine("DOOR CLOSED");
                Thread.Sleep(200);


                Console.WriteLine("___");
            }
            else if (Sensor == false)
            {
                openDoors();
            }
        }

        //Previous method moving and displaying the movement calls upon this method to move up
        public void Move_up(int requestedFloor, char colNbr)
        {
            Console.WriteLine("Column : " + colNbr + " Elevator : #" + elevatorNb + "  Current Floor : " + this.elevatorFloor);
            Thread.Sleep(200);
            Console.WriteLine("___");
            while (this.elevatorFloor != requestedFloor)
            {
                this.elevatorFloor += 1;
                Console.WriteLine("Column : " + colNbr + " Elevator : #" + elevatorNb + "  Floor : " + this.elevatorFloor);

                Thread.Sleep(400);
            }
        }


        //Previous method moving and displaying the movement calls upon this method to move down
        public void moveDown(int requestedFloor, char colNbr)
        {
            Console.WriteLine("Column : " + colNbr + " Elevator : #" + elevatorNb + "  Current Floor : " + this.elevatorFloor);
            Thread.Sleep(400);
            Console.WriteLine("___");

            while (this.elevatorFloor != requestedFloor)
            {
                this.elevatorFloor -= 1;
                Console.WriteLine("Column : " + colNbr + " Elevator : #" + elevatorNb + "  Floor : " + this.elevatorFloor);

                Thread.Sleep(400);
            }
            Console.WriteLine("___");

        }

    }

    // Column class, and its list.

    public class Column
    {
        public char colNbr;
        public int nbrFloors;
        public int nbrElevators;
        public List<Elevator> elevator_list;
        public List<int> call_button_list;


        public Column(char colNbr, int nbrFloors, int nbrElevators)
        {
            this.colNbr = colNbr;
            this.nbrFloors = nbrFloors;
            this.nbrElevators = nbrElevators;
            elevator_list = new List<Elevator>();
            call_button_list = new List<int>();
            for (int i = 0; i < this.nbrElevators; i++)
            {
                Elevator elevator = new Elevator(i, "IDLE", 1, "UP");
                elevator_list.Add(elevator);
            }
        }

        // method which assigns the appropriate elevator based off of the absolute distance of floors. If idle, simply return the elevator on that floor.

        public Elevator Find_Assign_elevator(int requestedFloor, int floorNbr, string userDirection)
        {

            foreach (var elevator in elevator_list)
                if (elevator.status == "IDLE")
                {
                    return elevator;
                }

            var bestElevator = 0;
            var shortest_distance = 1000;
            for (var i = 0; i < this.elevator_list.Count; i++)
            {
                var ref_distance = Math.Abs(elevator_list[i].elevatorFloor - elevator_list[i].floor_list[0]) + Math.Abs(elevator_list[i].floor_list[0] - 1);
                if (shortest_distance >= ref_distance)
                {
                    shortest_distance = ref_distance;
                    bestElevator = i;
                }
            }
            return elevator_list[bestElevator];
        }

        // finds the shortest distance, returns the closest elevator 

        public Elevator Find_Requested_elevator(int requestedFloor, string userDirection)
        {
            var shortest_distance = 999;
            var bestElevator = 0;

            for (var i = 0; i < this.elevator_list.Count; i++)
            {
                var ref_distance = elevator_list[i].elevatorFloor - requestedFloor;

                if (ref_distance > 0 && ref_distance < shortest_distance)
                {
                    shortest_distance = ref_distance;
                    bestElevator = i;
                }
            }
            return elevator_list[bestElevator];
        }

    }
    // turns the battery on, list for columns and numbering them alphabetically
    public class Battery
    {
        public string battery_status;
        public int nbrColumns;
        public List<Column> column_list;


        public Battery(int nbrColumns)
        {
            this.nbrColumns = nbrColumns;
            this.battery_status = "ON";
            column_list = new List<Column>();



            char cols = 'A';
            for (int i = 0; i < this.nbrColumns; i++, cols++)
            {
                Column column = new Column(cols, 66, 5);
                column.colNbr = cols;
                column_list.Add(column);
            }
        }

        // where the columns are categorized by their floor parameters, for optimal use.
        // col 1 is used for basements
        public Column Find_best_column(int requestedFloor)
        {
            Column best_column = null;
            foreach (Column column in column_list)
            {
                if (requestedFloor >= -5 && requestedFloor <= 0 || requestedFloor == 1)
                {
                    best_column = column_list[0];
                }
                else if (requestedFloor > 2 && requestedFloor <= 20 || requestedFloor == 1)
                {
                    best_column = column_list[1];
                }
                else if (requestedFloor > 21 && requestedFloor <= 40 || requestedFloor == 1)
                {
                    best_column = column_list[2];
                }
                else if (requestedFloor > 4 && requestedFloor <= 66 || requestedFloor == 1)
                {
                    best_column = column_list[3];
                }

            }
            return best_column;
        }
    }
    //time to test the fuction. Simply remove the slash and star of whichever test scenario you'd like to see, and press play.

    public class CommercialCS
    {
        public static void Main(string[] args)
        {
            ElevatorController controller = new ElevatorController(66, 4, 20, "DOWN");

                                     /* Scenario One */
            /*
            controller.battery.column_list[1].elevator_list[0].elevatorFloor = 20;
            controller.battery.column_list[1].elevator_list[0].elevator_direction = "UP";
            controller.battery.column_list[1].elevator_list[0].status = "MOVING";
            controller.battery.column_list[1].elevator_list[0].floor_list.Add(5);

            controller.battery.column_list[1].elevator_list[1].elevatorFloor = 3;
            controller.battery.column_list[1].elevator_list[1].elevator_direction = "UP";
            controller.battery.column_list[1].elevator_list[1].status = "MOVING";
            controller.battery.column_list[1].elevator_list[1].floor_list.Add(15);


             controller.battery.column_list[1].elevator_list[2].elevatorFloor = 13;
             controller.battery.column_list[1].elevator_list[2].elevator_direction = "DOWN";
             controller.battery.column_list[1].elevator_list[2].status = "MOVING";
             controller.battery.column_list[1].elevator_list[2].floor_list.Add(1);


             controller.battery.column_list[1].elevator_list[3].elevatorFloor = 15;
             controller.battery.column_list[1].elevator_list[3].elevator_direction = "DOWN";
             controller.battery.column_list[1].elevator_list[3].status = "MOVING";
             controller.battery.column_list[1].elevator_list[3].floor_list.Add(2);


             controller.battery.column_list[1].elevator_list[4].elevatorFloor = 6;
             controller.battery.column_list[1].elevator_list[4].elevator_direction = "DOWN";
             controller.battery.column_list[1].elevator_list[4].status = "MOVING";
             controller.battery.column_list[1].elevator_list[4].floor_list.Add(1);
             controller.AssignElevator(20);
             Elevator elevator = controller.requestElevator(1, 36);
                        */

                                    /* Scenario Two */
            /* 
               controller.battery.column_list[2].elevator_list[0].elevatorFloor = 1;
               controller.battery.column_list[2].elevator_list[0].elevator_direction = "UP";
               controller.battery.column_list[2].elevator_list[0].status = "IDLE";
               controller.battery.column_list[2].elevator_list[0].floor_list.Add(21);

               controller.battery.column_list[2].elevator_list[1].elevatorFloor = 23;
               controller.battery.column_list[2].elevator_list[1].elevator_direction = "UP";
               controller.battery.column_list[2].elevator_list[1].status = "MOVING";
               controller.battery.column_list[2].elevator_list[1].floor_list.Add(28);


               controller.battery.column_list[2].elevator_list[2].elevatorFloor = 33;
               controller.battery.column_list[2].elevator_list[2].elevator_direction = "DOWN";
               controller.battery.column_list[2].elevator_list[2].status = "MOVING";
               controller.battery.column_list[2].elevator_list[2].floor_list.Add(1);


               controller.battery.column_list[2].elevator_list[3].elevatorFloor = 40;
               controller.battery.column_list[2].elevator_list[3].elevator_direction = "DOWN";
               controller.battery.column_list[2].elevator_list[3].status = "MOVING";
               controller.battery.column_list[2].elevator_list[3].floor_list.Add(24);


               controller.battery.column_list[2].elevator_list[4].elevatorFloor = 39;
               controller.battery.column_list[2].elevator_list[4].elevator_direction = "DOWN";
               controller.battery.column_list[2].elevator_list[4].status = "MOVING";
               controller.battery.column_list[2].elevator_list[4].floor_list.Add(1);

               controller.AssignElevator(36);
               Elevator elevator = controller.requestElevator(1, 36);
             */

                                    /* Scenario Three */
            /*
             controller.battery.column_list[3].elevator_list[0].elevatorFloor = 58;
             controller.battery.column_list[3].elevator_list[0].elevator_direction = "DOWN";
             controller.battery.column_list[3].elevator_list[0].status = "MOVING";
             controller.battery.column_list[3].elevator_list[0].floor_list.Add(1);

             controller.battery.column_list[3].elevator_list[1].elevatorFloor = 50;
             controller.battery.column_list[3].elevator_list[1].elevator_direction = "UP";
             controller.battery.column_list[3].elevator_list[1].status = "MOVING";
             controller.battery.column_list[3].elevator_list[1].floor_list.Add(60);

             controller.battery.column_list[3].elevator_list[2].elevatorFloor = 46;
             controller.battery.column_list[3].elevator_list[2].elevator_direction = "UP";
             controller.battery.column_list[3].elevator_list[2].status = "MOVING";
             controller.battery.column_list[3].elevator_list[2].floor_list.Add(58);

             controller.battery.column_list[3].elevator_list[3].elevatorFloor = 1;
             controller.battery.column_list[3].elevator_list[3].elevator_direction = "up";
             controller.battery.column_list[3].elevator_list[3].status = "MOVING";
             controller.battery.column_list[3].elevator_list[3].floor_list.Add(54);

             controller.battery.column_list[3].elevator_list[4].elevatorFloor = 60;
             controller.battery.column_list[3].elevator_list[4].elevator_direction = "DOWN";
             controller.battery.column_list[3].elevator_list[4].status = "MOVING";
             controller.battery.column_list[3].elevator_list[4].floor_list.Add(1);

             
             Elevator elevator = controller.requestElevator(54, 1);

             */

                                /* Scenario Four */

           /* controller.battery.column_list[0].elevator_list[2].elevatorFloor = -4;
            controller.battery.column_list[0].elevator_list[2].elevator_direction = "UP";
            controller.battery.column_list[0].elevator_list[2].status = "IDLE";
            controller.battery.column_list[0].elevator_list[2].floor_list.Add(-4);

            controller.battery.column_list[0].elevator_list[2].elevatorFloor = 1;
            controller.battery.column_list[0].elevator_list[2].elevator_direction = "DOWN";
            controller.battery.column_list[0].elevator_list[2].status = "IDLE";
            controller.battery.column_list[0].elevator_list[2].floor_list.Add(-4);

            controller.battery.column_list[0].elevator_list[2].elevatorFloor = -3;
            controller.battery.column_list[0].elevator_list[2].elevator_direction = "DOWN";
            controller.battery.column_list[0].elevator_list[2].status = "MOVING";
            controller.battery.column_list[0].elevator_list[2].floor_list.Add(-5);

            controller.battery.column_list[0].elevator_list[3].elevatorFloor = -5;
            controller.battery.column_list[0].elevator_list[3].elevator_direction = "UP";
            controller.battery.column_list[0].elevator_list[3].status = "MOVING";
            controller.battery.column_list[0].elevator_list[3].floor_list.Add(1);

            controller.battery.column_list[0].elevator_list[4].elevatorFloor = -1;
            controller.battery.column_list[0].elevator_list[4].elevator_direction = "DOWN";
            controller.battery.column_list[0].elevator_list[4].status = "MOVING";
            controller.battery.column_list[0].elevator_list[4].floor_list.Add(-6);

            Elevator elevator = controller.requestElevator(-3, 1); */
                        
        }
    }
}