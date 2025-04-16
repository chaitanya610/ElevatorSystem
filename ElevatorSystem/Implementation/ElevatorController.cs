using ElevatorSystem.Interfaces;
using ElevatorSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorSystem.Implementation
{
    public class ElevatorController
    {
        public int TotalFloors { get; set; }
        public List<Elevator> Elevators { get; set; } = new List<Elevator>();
        private List<PassengerRequest> _unassignedPassengerRequests { get; set; } = new List<PassengerRequest>();

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
                elevator.ElevatorIdle += OnElevatorIdle; // Event callback
                new Thread(elevator.Move).Start();
                Elevators.Add(elevator);
            }
        }

        public Elevator Request(PassengerRequest passengerRequest)
        {
            var elevator = _elevatorAssignmentStrategy.AssignElevator(Elevators, passengerRequest, TotalFloors);
            if (elevator == null) // All elevators are going in opposite direction to the request or none of them have capacity to take this request
            {
                _unassignedPassengerRequests.Add(passengerRequest);
            }
            else
            {
                elevator.ProcessRequest(passengerRequest);
            }
            return elevator;
        }

        // Use a callback here to get notified when any elevator becomes idle
        public void OnElevatorIdle()
        {
            lock (_unassignedPassengerRequests)
            {
                if (_unassignedPassengerRequests.Any())
                {
                    // Retrigger all unassigned requests and clear the list
                    _unassignedPassengerRequests.ForEach(request => Request(request));
                    _unassignedPassengerRequests = new List<PassengerRequest>();
                }
            }
        }
    }
}
