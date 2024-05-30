
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ToolVehicleTaxManagement
{
    // Base class ToolVehicle
    public abstract class ToolVehicle
    {
        // Properties
        public int VehicleID { get; set; }
        public string RegNo { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public decimal BasePrice { get; set; }
        public string VehicleType { get; set; }

        // Static properties
        public static int TotalVehicles { get; private set; }
        public static int TotalTaxPayingVehicles { get; private set; }
        public static int TotalNonTaxPayingVehicles { get; private set; }
        public static decimal TotalTaxCollected { get; private set; }

        // Constructor
        public ToolVehicle(int vehicleID, string regNo, string model, string brand, decimal basePrice, string vehicleType)
        {
            VehicleID = vehicleID;
            RegNo = regNo;
            Model = model;
            Brand = brand;
            BasePrice = basePrice;
            VehicleType = vehicleType;
            TotalVehicles++;
        }

        // Abstract method for paying tax
        public abstract void PayTax();

        // Method to handle passing without paying tax
        public void PassWithoutPaying()
        {
            TotalNonTaxPayingVehicles++;
        }

        // Methods to update static properties
        protected static void AddToTotalTaxCollected(decimal amount)
        {
            TotalTaxCollected += amount;
        }

        protected static void IncrementTotalTaxPayingVehicles()
        {
            TotalTaxPayingVehicles++;
        }
    }

    // Derived class Car
    public class Car : ToolVehicle
    {
        public Car(int vehicleID, string regNo, string model, string brand, decimal basePrice)
            : base(vehicleID, regNo, model, brand, basePrice, "Car") { }

        public override void PayTax()
        {
            AddToTotalTaxCollected(2); // Tax amount for car is $2
            IncrementTotalTaxPayingVehicles();
        }
    }

    // Derived class Bike
    public class Bike : ToolVehicle
    {
        public Bike(int vehicleID, string regNo, string model, string brand, decimal basePrice)
            : base(vehicleID, regNo, model, brand, basePrice, "Bike") { }

        public override void PayTax()
        {
            AddToTotalTaxCollected(1); // Tax amount for bike is $1
            IncrementTotalTaxPayingVehicles();
        }
    }

    // Derived class HeavyVehicle
    public class HeavyVehicle : ToolVehicle
    {
        public HeavyVehicle(int vehicleID, string regNo, string model, string brand, decimal basePrice)
            : base(vehicleID, regNo, model, brand, basePrice, "HeavyVehicle") { }

        public override void PayTax()
        {
            AddToTotalTaxCollected(4); // Tax amount for heavy vehicle is $4
            IncrementTotalTaxPayingVehicles();
        }
    }

    class Program
    {
        static List<ToolVehicle> vehicles = new List<ToolVehicle>();

        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Welcome to the Tool Vehicle Tax Management System!");
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Add a New Vehicle");
                Console.WriteLine("2. View Total Vehicles");
                Console.WriteLine("3. View Total Tax Paying Vehicles");
                Console.WriteLine("4. View Total Non-Tax Paying Vehicles");
                Console.WriteLine("5. View Total Tax Collected");

                string choice = Console.ReadLine();
                if (!Regex.IsMatch(choice, @"^[1-6]$"))
                {
                    Console.WriteLine("Invalid option. Please choose a number between 1 and 6.");
                    continue;
                }

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Let's add a new vehicle!");
                        AddNewVehicle();
                        break;
                    case "2":
                        Console.WriteLine($"Total Vehicles: {ToolVehicle.TotalVehicles}");
                        break;
                    case "3":
                        Console.WriteLine($"Total Tax Paying Vehicles: {ToolVehicle.TotalTaxPayingVehicles}");
                        break;
                    case "4":
                        Console.WriteLine($"Total Non-Tax Paying Vehicles: {ToolVehicle.TotalNonTaxPayingVehicles}");
                        break;
                    case "5":
                        Console.WriteLine($"Total Tax Collected: ${ToolVehicle.TotalTaxCollected}");
                        break;
                    
                }

                if (!exit)
                {
                    Console.WriteLine("Do you want to perform another operation? (Y/N): ");
                    string continueChoice = Console.ReadLine().ToUpper();
                    while (continueChoice != "Y" && continueChoice != "N")
                    {
                        Console.WriteLine("Invalid input. Please press 'Y' to continue or 'N' to exit.");
                        continueChoice = Console.ReadLine().ToUpper();
                    }
                    if (continueChoice == "N")
                    {
                        exit = true;
                        Console.WriteLine("Thank you for using the Tool Vehicle Tax Management System!");
                    }
                }
            }
        }

        static void AddNewVehicle()
        {
            Console.WriteLine("Choose the type of vehicle to add:");
            Console.WriteLine("1. Car");
            Console.WriteLine("2. Bike");
            Console.WriteLine("3. Heavy Vehicle");

            string typeChoice = Console.ReadLine();
            if (!Regex.IsMatch(typeChoice, @"^[1-3]$"))
            {
                Console.WriteLine("Invalid vehicle type. Returning to main menu.");
                return;
            }

            int vehicleID = ToolVehicle.TotalVehicles + 1;

            Console.Write("Enter Registration Number: ");
            string regNo = ReadAlphanumeric("registration number");

            Console.Write("Enter Vehicle ID: ");
            decimal vehicle = ReadDecimal();

            Console.Write("Enter Model: ");
            string model = ReadLetters("model");

            Console.Write("Enter Brand: ");
            string brand = ReadLetters("brand");

            Console.Write("Enter Base Price: ");
            decimal basePrice = ReadDecimal();

            ToolVehicle newVehicle = null;

            switch (typeChoice)
            {
                case "1":
                    newVehicle = new Car(vehicleID, regNo, model, brand, basePrice);
                    break;
                case "2":
                    newVehicle = new Bike(vehicleID, regNo, model, brand, basePrice);
                    break;
                case "3":
                    newVehicle = new HeavyVehicle(vehicleID, regNo, model, brand, basePrice);
                    break;
            }

            vehicles.Add(newVehicle);

            Console.WriteLine("Do you want to pay tax for this vehicle? (yes/no)");
            string payTaxChoice = Console.ReadLine().ToLower();
            while (payTaxChoice != "yes" && payTaxChoice != "no")
            {
                Console.WriteLine("Invalid input. Please press 'yes' to pay tax or 'no' to skip tax.");
                payTaxChoice = Console.ReadLine().ToLower();
            }
            if (payTaxChoice == "yes")
            {
                newVehicle.PayTax();
                Console.WriteLine("Tax paid successfully.");
            }
            else
            {
                newVehicle.PassWithoutPaying();
                Console.WriteLine("Vehicle passed without paying tax.");
            }
        }

        static string ReadLetters(string fieldName)
        {
            string input = "";
            while (true)
            {
                input = Console.ReadLine();
                if (Regex.IsMatch(input, @"^[a-zA-Z\s]+$"))
                {
                    break;
                }
                else
                {
                    Console.Write($"Invalid {fieldName}. Please enter only letters: ");
                }
            }
            return input;
        }

        static string ReadAlphanumeric(string fieldName)
        {
            string input = "";
            while (true)
            {
                input = Console.ReadLine();
                if (Regex.IsMatch(input, @"^[a-zA-Z0-9]+$"))
                {
                    break;
                }
                else
                {
                    Console.Write($"Invalid {fieldName}. Please enter only alphanumeric characters: ");
                }
            }
            return input;
        }

        static decimal ReadDecimal()
        {
            decimal value;
            while (true)
            {
                if (decimal.TryParse(Console.ReadLine(), out value))
                {
                    break;
                }
                else
                {
                    Console.Write("Invalid input. Please enter a valid Number: ");
                }
            }
            return value;
        }
    }
}
