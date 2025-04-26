using System;
using System.Collections.Generic;

abstract class EmergencyUnit
{
    public string Name { get; set; }
    public int Speed { get; set; }

    public EmergencyUnit(string name, int speed)
    {
        Name = name;
        Speed = speed;
    }

    // Abstract method to check if the unit can handle a specific incident type
    public abstract bool CanHandle(string incidentType);

    // Abstract method to simulate responding to an incident
    public abstract void RespondToIncident(Incident incident);
}

class Police : EmergencyUnit
{
    public Police(string name, int speed) : base(name, speed) { }

    public override bool CanHandle(string incidentType)
    {
        return incidentType == "Crime";
    }

    public override void RespondToIncident(Incident incident)
    {
        Console.WriteLine($"{Name} is handling a crime incident at {incident.Location}.");
    }
}

class Firefighter : EmergencyUnit
{
    public Firefighter(string name, int speed) : base(name, speed) { }

    public override bool CanHandle(string incidentType)
    {
        return incidentType == "Fire";
    }

    public override void RespondToIncident(Incident incident)
    {
        Console.WriteLine($"{Name} is extinguishing a fire at {incident.Location}.");
    }
}

class Ambulance : EmergencyUnit
{
    public Ambulance(string name, int speed) : base(name, speed) { }

    public override bool CanHandle(string incidentType)
    {
        return incidentType == "Medical";
    }

    public override void RespondToIncident(Incident incident)
    {
        Console.WriteLine($"{Name} is treating patients at {incident.Location}.");
    }
}

class Incident
{
    public string Type { get; set; }
    public string Location { get; set; }
    public string Difficulty { get; set; } // Difficulty: Easy, Medium, Hard

    public Incident(string type, string location, string difficulty)
    {
        Type = type;
        Location = location;
        Difficulty = difficulty;
    }

    // Method to return the points based on difficulty
    public int GetBasePoints()
    {
        return Difficulty switch
        {
            "Easy" => 10,
            "Medium" => 20,
            "Hard" => 30,
            _ => 0
        };
    }
}

class Program
{
    static void Main()
    {
        // Initialize units
        List<EmergencyUnit> units = new List<EmergencyUnit>
        {
            new Police("Police Unit 1", 10),
            new Firefighter("Firefighter Unit 1", 8),
            new Ambulance("Ambulance Unit 1", 7)
        };

        // Initialize score
        int score = 0;

        // Game loop: 5 rounds
        for (int turn = 1; turn <= 5; turn++)
        {
            Console.WriteLine($"--- Turn {turn} ---");

            // Generate a random incident type
            Random rand = new Random();
            string[] incidentTypes = { "Crime", "Fire", "Medical" };
            string incidentType = incidentTypes[rand.Next(incidentTypes.Length)];

            // Random difficulty level for the incident
            string[] difficulties = { "Easy", "Medium", "Hard" };
            string difficulty = difficulties[rand.Next(difficulties.Length)];

            // Prompt the user to input the location of the incident
            Console.WriteLine("Enter the location of the incident:");
            string location = Console.ReadLine();

            // Create the incident
            Incident incident = new Incident(incidentType, location, difficulty);

            // Display the incident to the player
            Console.WriteLine($"Incident: {incidentType} at {location} (Difficulty: {difficulty})");

            // Allow the player to select which unit to send
            Console.WriteLine("\nSelect a unit to send:");
            for (int i = 0; i < units.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {units[i].Name} (Speed: {units[i].Speed})");
            }

            int unitChoice = 0;
            bool validChoice = false;

            while (!validChoice)
            {
                try
                {
                    unitChoice = int.Parse(Console.ReadLine()) - 1;
                    if (unitChoice >= 0 && unitChoice < units.Count)
                    {
                        validChoice = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please select a valid unit.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input. Please select a valid unit.");
                }
            }

            // Selected unit
            EmergencyUnit selectedUnit = units[unitChoice];
            bool handled = false;
            int basePoints = incident.GetBasePoints(); // Get points based on incident difficulty
            int responseTime = CalculateResponseTime(selectedUnit); // Calculate response time based on unit speed
            int totalPoints = basePoints + responseTime; // Total points for the incident

            // Check if the selected unit can handle the incident
            if (selectedUnit.CanHandle(incidentType))
            {
                selectedUnit.RespondToIncident(incident);

                // Display response time and points
                Console.WriteLine($"Response Time: {responseTime} seconds");
                Console.WriteLine($"Base Points for Incident: {basePoints}");
                Console.WriteLine($"Total Points for Response: {totalPoints}");

                score += totalPoints; // Award points for handling the incident
                handled = true;
            }
            else
            {
                Console.WriteLine($"The selected unit cannot handle {incidentType} at {location}. Incident ignored.");
                score -= 5; // Incorrect handling
            }

            // Always add 10 points at the end of the turn
            score += 10;

            // Display current score
            Console.WriteLine($"Current Score: {score}\n");
        }

        // Final score
        Console.WriteLine($"Final Score: {score}");
    }

    // Method to calculate response time based on unit speed (lower speed means slower response)
    static int CalculateResponseTime(EmergencyUnit unit)
    {
        // Assuming the base response time is 30 seconds, and units with higher speed respond faster
        int baseTime = 30;
        int responseTime = baseTime - unit.Speed;
        if (responseTime < 5) responseTime = 5; // Minimum response time (5 seconds)
        return responseTime;
    }
}
