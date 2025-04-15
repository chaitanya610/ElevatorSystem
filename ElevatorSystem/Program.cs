using ElevatorSystem.Implementation;
using ElevatorSystem.Models;
using System;

namespace ElevatorSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var elevatorSystem = new ElevatorController(10, 3, 12);

            elevatorSystem.Request(new PassengerRequest(2, 6, 5));
        }
    }
}
