//C# Commercial Elevator controller 

using System;
using System.Collections.Generic;
using System.Threading;

namespace CommercialController
{
    /// introducing the floors, elevators, columns, direction, battery, and lists
    public class ElevatorController
    {
        public int numFloors;
        public int numElevators;
        public int numColumns;
        public string userDirection;
        public Battery battery;
        public List<int> shortList;

        public ElevatorController(int numFloors, int numColumns, int numElevators, string userDirection)
        {
            this.numFloors = numFloors;
            this.numColumns = numColumns;
            this.numElevators = numElevators;
            this.userDirection = userDirection;
            this.battery = new Battery(this.numColumns);
        }


        //finds the best column, finds the requested elevator, sends the request. Moves the elevator up or down accordingly
        public Elevator requestElevator(int floorNum, int requestedFloor)
        {
            Console.WriteLine("REQUEST AN ELEVATOR TO FLOOR : " + floorNum);
            Console.WriteLine("___");
            var column = battery.Find_best_column(floorNum);
            userDirection = "DOWN";
            var elevator = column.Find_Requested_elevator(floorNum, userDirection);
            if (elevator.elevatorFloor > floorNum)
            {
                elevator.Move_up(requestedFloor, column.colNum);
                elevator.sendRequest(floorNum, column.colNum);
                elevator.sendRequest(requestedFloor, column.colNum);
            }

            else if (elevator.elevatorFloor < floorNum)
            {
                elevator.moveDown(requestedFloor, column.colNum);
                elevator.sendRequest(floorNum, column.colNum);
                elevator.sendRequest(requestedFloor, column.colNum);
            }


            return elevator;
        }

        //AssignElevator method, finds the best column, assigns the appropriate elevator

        public Elevator AssignElevator(int requestedFloor)
        {
            Thread.Sleep(300);
            Console.WriteLine("Request coming from floor: " + requestedFloor);

            Column column = battery.Find_best_column(requestedFloor);
            userDirection = "UP";
            var floorNum = 1;
            Elevator elevator = column.Find_Assign_elevator(requestedFloor, floorNum, userDirection);

            elevator.sendRequest(floorNum, column.colNum);
            elevator.sendRequest(requestedFloor, column.colNum);

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
        public int FloorDisplay;
        public List<int> floor_list;

        public Elevator(int elevatorNb, string status, int elevatorFloor, string elevator_direction)
        {
            this.elevatorNb = elevatorNb;
            this.status = status;
            this.elevatorFloor = elevatorFloor;
            this.elevator_direction = elevator_direction;
            this.FloorDisplay = elevatorFloor;
            this.floor_list = new List<int>();
        }

        // sorts the request in ascending or descending order, based off of the direction
        public void sendRequest(int requestedFloor, char colNum)
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

            LstElevator(requestedFloor, colNum);
        }

        // once the elevator is moving, remove the current floor from queue. calls to move the elevator up or down, and changes and displays in console the floors progression 
        public void LstElevator(int requestedFloor, char colNum)
        {
            if (requestedFloor == elevatorFloor)
            {
               this.floor_list.Remove(0);
            }
            //For descending. Moves and Displays the descending floors
            else if (requestedFloor < this.elevatorFloor)
            {
                status = "MOVING";
                Console.WriteLine("___");
                Console.WriteLine("Column : " + colNum + " Elevator : " + this.elevatorNb + " " + status);
                Console.WriteLine("___");
                this.elevator_direction = "DOWN";
                moveDown(requestedFloor, colNum);
                this.status = "STOP";
                Console.WriteLine("Column : " + colNum + " Elevator : " + this.elevatorNb + " " + status);

             this.floor_list.Remove(0);
            }
            //For ascending. Moves and Displays the ascending floors
            else if (requestedFloor > this.elevatorFloor)
            {
                Thread.Sleep(500);
                this.status = "MOVING";
                Console.WriteLine("___");
                Console.WriteLine("Column : " + colNum + " Elevator : " + this.elevatorNb + " status:" + status);
                Console.WriteLine("___");
                this.elevator_direction = "up";
                this.Move_up(requestedFloor, colNum);
                this.status = "STOP";
                Console.WriteLine("___");
                Console.WriteLine("Column : " + colNum + " Elevator : " + this.elevatorNb + " " + status);


                this.floor_list.Remove(0);
            }

        }
      
        //Previous method moving and displaying the movement calls upon this method to move up
        public void Move_up(int requestedFloor, char colNum)
        {
            Console.WriteLine("Column : " + colNum + " Elevator : #" + elevatorNb + "  Current Floor : " + this.elevatorFloor);
            Thread.Sleep(200);
            Console.WriteLine("___");
            while (this.elevatorFloor != requestedFloor)
            {
                this.elevatorFloor += 1;
                Console.WriteLine("Column : " + colNum + " Elevator : #" + elevatorNb + "  Floor : " + this.elevatorFloor);

                Thread.Sleep(400);
            }
        }


        //Previous method moving and displaying the movement calls upon this method to move down
        public void moveDown(int requestedFloor, char colNum)
        {
            Console.WriteLine("Column : " + colNum + " Elevator : #" + elevatorNb + "  Current Floor : " + this.elevatorFloor);
            Thread.Sleep(400);
            Console.WriteLine("___");

            while (this.elevatorFloor != requestedFloor)
            {
                this.elevatorFloor -= 1;
                Console.WriteLine("Column : " + colNum + " Elevator : #" + elevatorNb + "  Floor : " + this.elevatorFloor);

                Thread.Sleep(400);
            }
            Console.WriteLine("___");

        }

    }

    // Column class, and its list.

    public class Column
    {
        public char colNum;
        public int numFloors;
        public int numElevators;
        public List<Elevator> elevator_list;
        public List<int> call_button_list;


        public Column(char colNum, int numFloors, int numElevators)
        {
            this.colNum = colNum;
            this.numFloors = numFloors;
            this.numElevators = numElevators;
            elevator_list = new List<Elevator>();
            call_button_list = new List<int>();
            for (int i = 0; i < this.numElevators; i++)
            {
                Elevator elevator = new Elevator(i, "IDLE", 1, "UP");
                elevator_list.Add(elevator);
            }
        }

        // method which assigns the appropriate elevator based off of the absolute distance of floors. If idle, simply return the elevator on that floor.

        public Elevator Find_Assign_elevator(int requestedFloor, int floorNum, string userDirection)
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
        public int numColumns;
        public List<Column> column_list;


        public Battery(int numColumns)
        {
            this.numColumns = numColumns;
            this.battery_status = "ON";
            column_list = new List<Column>();



            char cols = 'A';
            for (int i = 0; i < this.numColumns; i++, cols++)
            {
                Column column = new Column(cols, 60, 5);
                column.colNum = cols;
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
                else if (requestedFloor > 41 && requestedFloor <= 60 || requestedFloor == 1)
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