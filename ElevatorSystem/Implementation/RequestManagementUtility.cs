using ElevatorSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorSystem.Implementation
{
    public static class RequestManagementUtility
    {
        public static int GetLastDestination(this List<PassengerRequest> requests, Direction currrentDirection)
        {
            if (currrentDirection == Direction.Up)
            {
                return requests.Max(request => request.DestinationFloor);
            }
            else
            {
                return requests.Min(request => request.DestinationFloor);
            }
        }

        public static bool IsApproaching(this Elevator elevator, int floor)
        {
            if (elevator.Direction == Direction.Up)
            {
                return elevator.CurrentFloor <= floor;
            }
            else
            {
                return elevator.CurrentFloor >= floor;
            }
        }

        public static Direction GetDirection(int currentFloor, int newFloor)
        {
            return newFloor > currentFloor ? Direction.Up : Direction.Down;
        }
    }
}
