using ElevatorSystem.Implementation;
using ElevatorSystem.Models;
using System;
using System.Threading;

namespace ElevatorSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var elevatorSystem = new ElevatorController(10, 3, 12);

            Console.WriteLine("Elevator system started!!!\n");
            Thread.Sleep(1000);

            elevatorSystem.Request(new PassengerRequest(2, 3, 5));
            Thread.Sleep(5000);
            elevatorSystem.Request(new PassengerRequest(5, 10, 7));
            elevatorSystem.Request(new PassengerRequest(4, 8, 3));
            elevatorSystem.Request(new PassengerRequest(5, 1, 6));
        }
    }
}
