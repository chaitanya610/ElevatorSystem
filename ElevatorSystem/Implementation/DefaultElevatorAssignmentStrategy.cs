using ElevatorSystem.Interfaces;
using ElevatorSystem.Models;
using System.Collections.Generic;

namespace ElevatorSystem.Implementation
{
    public class DefaultElevatorAssignmentStrategy : IElevatorAssignmentStrategy
    {
        public Elevator AssignElevator(List<Elevator> elevators, PassengerRequest passengerRequest, int totalFloors)
        {
            var suitableElevators = GetAllSuitableElevators(elevators, passengerRequest);

            foreach (var elevator in suitableElevators)
            {
                if (IsWithinCapacity(elevator, passengerRequest, totalFloors)) // Check for capacity
                {
                    return elevator;
                }
            }

            return null;
        }

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



        // Get all elevators going via requested floor to destination floor and elevators in idle state
        private static List<Elevator> GetAllSuitableElevators(List<Elevator> elevators, PassengerRequest request)
        {
            var requestedDirection = RequestManagementUtility.GetDirection(request.CurrentFloor, request.DestinationFloor);
            return elevators.FindAll(elevator => elevator.IsIdle() || (elevator.Direction == requestedDirection && elevator.IsApproaching(request.CurrentFloor)));
        }
    }
}
