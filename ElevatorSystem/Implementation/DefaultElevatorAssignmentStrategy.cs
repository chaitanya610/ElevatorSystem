using ElevatorSystem.Interfaces;
using ElevatorSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace ElevatorSystem.Implementation
{
    public class DefaultElevatorAssignmentStrategy : IElevatorAssignmentStrategy
    {
        // First, try to assign any moving elevator which is going in same director and approaching the request's current floor.
        // This will save resources, no need to restart new elevator for every request.
        public Elevator AssignElevator(List<Elevator> elevators, PassengerRequest passengerRequest, int totalFloors)
        {
            var suitableElevators = GetAllSuitableMovingElevators(elevators, passengerRequest);

            foreach (var elevator in suitableElevators)
            {
                if (IsWithinCapacity(elevator, passengerRequest, totalFloors)) // Check for capacity
                {
                    return elevator;
                }
            }

            // No moving elevator is found suitable. Return an idle elevator or null if none
            return elevators.FirstOrDefault(elevator => elevator.IsIdle());
        }

        // Using a hash table to check if at any floor if the elevator exceeds maximum capacity
        private static bool IsWithinCapacity(Elevator elevator, PassengerRequest passengerRequest, int totalFloors)
        {
            var capacityRecord = new int[totalFloors + 1];

            foreach (var record in elevator.AssignedRequests)
            {
                capacityRecord[record.CurrentFloor] += record.TotalPassengers;
                capacityRecord[record.DestinationFloor] -= record.TotalPassengers;
            }

            capacityRecord[passengerRequest.CurrentFloor] += passengerRequest.TotalPassengers;
            capacityRecord[passengerRequest.DestinationFloor] -= passengerRequest.TotalPassengers;

            var currentCapacity = 0;
            for (int i = 1; i <= totalFloors; i++)
            {
                currentCapacity += capacityRecord[i];
                if (currentCapacity > elevator.MaxCapacity)
                {
                    return false;
                }
            }

            return true;
        }



        // Get all elevators going via requested floor to destination floor
        private static List<Elevator> GetAllSuitableMovingElevators(List<Elevator> elevators, PassengerRequest request)
        {
            var requestedDirection = RequestManagementUtility.GetDirection(request.CurrentFloor, request.DestinationFloor);
            return elevators.FindAll(elevator => elevator.Direction == requestedDirection && elevator.IsApproaching(request.CurrentFloor));
        }
    }
}
