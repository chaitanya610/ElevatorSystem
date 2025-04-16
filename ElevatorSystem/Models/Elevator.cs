using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ElevatorSystem.Implementation;

namespace ElevatorSystem.Models
{
    public class Elevator
    {
        public int Id { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentFloor { get; set; }
        public Direction Direction { get; set; }
        public List<PassengerRequest> AssignedRequests { get; set; } = new List<PassengerRequest>();
        public event ElevatorIdleHandler ElevatorIdle;

        public Elevator(int id, int capacity)
        {
            Id = id;
            MaxCapacity = capacity;
            Direction = Direction.Up;
            CurrentFloor = 1;
        }

        public bool IsIdle()
        {
            return !AssignedRequests.Any();
        }

        public void ProcessRequest(PassengerRequest passengerRequest)
        {
            Console.WriteLine($"Request {passengerRequest.CurrentFloor} -> {passengerRequest.DestinationFloor} ({passengerRequest.TotalPassengers}) assigned to elevator {Id}");
            lock (AssignedRequests)
            {
                if (IsIdle()) // set direction and restart elevator for request after idle state
                {
                    // Drawback case: If an empty elevator is going UP to take a passenger whose destination is in direction DOWN. 
                    // For simplicity we are directly moving the elevator to the passenger's current floor.
                    // This drawback leads to missing out of handling requests that can be taken care on the way while empty elevator is going UP
                    if (Direction != RequestManagementUtility.GetDirection(passengerRequest.CurrentFloor, passengerRequest.DestinationFloor))
                    {
                        CurrentFloor = passengerRequest.CurrentFloor;
                    }
                    Direction = RequestManagementUtility.GetDirection(CurrentFloor, passengerRequest.DestinationFloor); // set direction for first request
                    Monitor.PulseAll(AssignedRequests); // Restart the elevator
                }
                AssignedRequests.Add(passengerRequest);
            }
        }

        public void Move()
        {
            while (true)
            {
                // keep checking for any requests and keeping moving till you reach the last destination
                while (AssignedRequests.Any() && CurrentFloor != AssignedRequests.GetLastDestination(Direction))
                {
                    if (Direction == Direction.Up)
                    {
                        CurrentFloor++;
                    }
                    else
                    {
                        CurrentFloor--;
                    }

                    // Drop passengers requests with destination floor same as current floor
                    var removedRequests = AssignedRequests.RemoveAll(request => request.DestinationFloor == CurrentFloor);
                    if(removedRequests > 0)
                    {
                        Console.WriteLine($"Completed {removedRequests} requests at floor {CurrentFloor} by elevator {Id}");
                    }
                    Thread.Sleep(1000); // Simulate elevator stopping for a second in every floor
                }

                lock (AssignedRequests)
                {
                    try
                    {
                        Console.WriteLine($"Elevator {Id} going into sleep mode");
                        ElevatorIdle.Invoke();
                        Monitor.Wait(AssignedRequests);
                        Console.WriteLine($"Elevator {Id} restarting");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }           
        }
    }
}
