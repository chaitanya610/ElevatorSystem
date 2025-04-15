using ElevatorSystem.Interfaces;
using ElevatorSystem.Models;
using System.Collections.Generic;

namespace ElevatorSystem.Implementation
{
    public class ElevatorController
    {
        public int TotalFloors { get; set; }
        public List<Elevator> Elevators { get; set; }
        private List<PassengerRequest> _unassignedPassengerRequests { get; set; } = new List<PassengerRequest>();
        private Dictionary<int, Elevator> _elevatorMap { get; set; }

        private static IElevatorAssignmentStrategy _elevatorAssignmentStrategy;

        public ElevatorController(int totalFloors, int totalElevators, int maxCapacity)
        {
            TotalFloors = totalFloors;
            _elevatorAssignmentStrategy = new DefaultElevatorAssignmentStrategy();
            Initialize(totalElevators, maxCapacity);
        }

        private void Initialize(int elevators, int maxCapactity)
        {
            for (int i = 0; i < elevators; i++)
            {
                var elevator = new Elevator(i + 1, maxCapactity);
                Elevators.Add(elevator);
                _elevatorMap.Add(i + 1, elevator);
            }
        }

        public int Request(PassengerRequest passengerRequest)
        {
            var elevatorId = _elevatorAssignmentStrategy.AssignElevator(Elevators, passengerRequest, TotalFloors);
            if (elevatorId == -1)
            {
                _unassignedPassengerRequests.Add(passengerRequest);
            }
            else
            {
                _elevatorMap[elevatorId].AssignedRequests.Add(passengerRequest);
            }
            return elevatorId;
        }
    }
}
