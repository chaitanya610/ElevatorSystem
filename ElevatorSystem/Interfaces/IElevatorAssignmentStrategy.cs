using ElevatorSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorSystem.Interfaces
{
    public interface IElevatorAssignmentStrategy
    {
        int AssignElevator(List<Elevator> elevators, PassengerRequest passengerRequest, int totalFloors);
    }
}
