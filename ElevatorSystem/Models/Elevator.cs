using System.Collections.Generic;

namespace ElevatorSystem.Models
{
    public class Elevator
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public int CurrentFloor { get; set; }
        public Direction Direction { get; set; }
        public List<PassengerRequest> AssignedRequests { get; set; } = new List<PassengerRequest>();

        public Elevator(int id, int capacity)
        {
            Id = id;
            Capacity = capacity;
            Direction = Direction.Up;
            CurrentFloor = 1;
        }
    }
}
