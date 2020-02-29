package main

import (
	"fmt"
) 


func Battery(number_of_columns int) *Battery {
	battery := new(Battery)
	battery.number_of_columns = 4
	
	return battery
}

type Commercial_Controller struct {
	Power_battery int
	number_of_columns int
	direction  string
}

type Column struct {
	column_id int
	elevators_per_column int
	elevator_list []Elevator
}


func Columns(elevators_per_column int) *Column {
	column := new(Column)
	column.elevators_per_column = 5
	for index := 0; index < column.elevators_per_column; index++ {
		elevator := Cage()
		column.elevator_list = append(column.elevator_list, *elevator)
	}
	return column
}


type Elevator struct {
	elevator_id        int
	elevator_position  int
	Floor_list        []int
	elevator_status    string
	elevator_direction string
	door_sensor        bool
	column             Column
}



func Elevator() *Elevator {
	elevator := new(Elevator)
	elevator.Floor_list = []int{}
	elevator.elevator_status = "idle"
	elevator.elevator_direction = "up"
	return elevator
}


func main() {

}