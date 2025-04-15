using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorSystem.Models
{
    public class PassengerRequest
    {
        public int CurrentFloor { get; set; }
        public int DestinationFloor { get; set; }
        public int TotalPassengers { get; set; }

        public PassengerRequest(int currentFloor, int destinationFloor, int totalPassengers)
        {
            CurrentFloor = currentFloor;
            DestinationFloor = destinationFloor;
            TotalPassengers = totalPassengers;
        }
    }
}
