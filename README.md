# ElevatorSystem
Designing and implementing an elevator system using a .NET console application

# Requirements
1. An elevator system which handles multiple elevators and multiple floors.
2. Each elevator has a maximum capacity.
3. Passengers can request the elevator at any floor with the destination floor number and number of passengers entering together.
4. An elevator keeps moving in same direction until all passengers are dropped at destinations.
5. All elevators should work efficiently by not making any passenger wait as long as there is enough capacity. If no capacity then the elevator will complete the passenger requests after dropping all existing passengers

# Classes and Interfaces

**ElevatorController** - Main entry class which handles the entire elevator ecosystem.
**Elevator** - Elevator instance.
**PassengerRequest** - A request to elevator by a passenger.
**Direction** - Enum with values Up, Down.

**RequestManagementUtility** - Utility methods used to perform calculations related to passenger requests.

**IElevatorAssignmentStrategy** - Implementation to decide which elevator takes care of which passenger requests.
