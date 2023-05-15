using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_onCSV_App
{
    public class Animal
    {
        public string AnimalType { get; set; }
        public string Name { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"D:\Faculta\CRUD_onCSV_App\ZooCatalogue.csv";
            List<Animal> animals = new List<Animal>();

            if (!File.Exists(filePath))
            {
                var writer = new StreamWriter(filePath);
                var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
                csv.WriteHeader<Animal>();
                csv.NextRecord();
                writer.Flush();
                writer.Close();
            }
            else
            {
                var reader = new StreamReader(filePath);
                var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
                animals = csv.GetRecords<Animal>().ToList();
                reader.Close();
            }

            while (true)
            {
                Console.WriteLine("Choose an operation:");
                Console.WriteLine("1. Add a new animal");
                Console.WriteLine("2. Update the name of an animal");
                Console.WriteLine("3. Read the name of an animal");
                Console.WriteLine("4. Delete an animal");
                Console.WriteLine("5. Save changes");
                Console.WriteLine("6. Show all animals");
                Console.WriteLine("7. Exit");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid choice. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter the type of the animal:");
                        string animalType = Console.ReadLine();
                        Console.WriteLine("Enter the name of the animal:");
                        string animalName = Console.ReadLine();
                        animals.Add(new Animal { AnimalType = animalType, Name = animalName });
                        break;
                    case 2:
                        Console.WriteLine("Enter the type of the animal to update:");
                        string updateType = Console.ReadLine();
                        if (!animals.Exists(a => a.AnimalType == updateType))
                        {
                            Console.WriteLine("Animal not found.");
                            break;
                        }
                        Console.WriteLine("Enter the new name of the animal:");
                        string newName = Console.ReadLine();
                        Animal animalToUpdate = animals.First(a => a.AnimalType == updateType);
                        animalToUpdate.Name = newName;
                        break;
                    case 3:
                        Console.WriteLine("Enter the type of the animal to read:");
                        string readType = Console.ReadLine();
                        if (!animals.Exists(a => a.AnimalType == readType))
                        {
                            Console.WriteLine("Animal not found.");
                            break;
                        }
                        Animal animalToRead = animals.First(a => a.AnimalType == readType);
                        Console.WriteLine($"The name of the {readType} is {animalToRead.Name}");
                        break;
                    case 4:
                        Console.WriteLine("Enter the type of the animal to delete:");
                        string deleteType = Console.ReadLine();
                        if (!animals.Exists(a => a.AnimalType == deleteType))
                        {
                            Console.WriteLine("Animal not found.");
                            break;
                        }
                        Animal animalToDelete = animals.First(a => a.AnimalType == deleteType);
                        animals.Remove(animalToDelete);
                        break;
                    case 5:
                        var writerUpdate = new StreamWriter(filePath);
                        var csvUpdate = new CsvWriter(writerUpdate, System.Globalization.CultureInfo.InvariantCulture);
                        csvUpdate.WriteRecords(animals);
                        writerUpdate.Flush();
                        writerUpdate.Close();
                        Console.WriteLine("Changes saved.");
                        break;
                    case 6:
                        Console.WriteLine("All animals in the catalogue:");
                        foreach (var animal in animals)
                        {
                            Console.WriteLine($"{animal.AnimalType}: {animal.Name}");
                        }
                        break;
                    case 7:
                        Console.WriteLine("Do you want to save changes before exit? (y/n)");
                        string saveBeforeExit = Console.ReadLine();
                        if (saveBeforeExit.ToLower() == "y")
                        {
                            var writerUpdateExit = new StreamWriter(filePath);
                            var csvUpdateExit = new CsvWriter(writerUpdateExit, System.Globalization.CultureInfo.InvariantCulture);
                            csvUpdateExit.WriteRecords(animals);
                            writerUpdateExit.Flush();
                            writerUpdateExit.Close();
                            Console.WriteLine("Changes saved.");
                        }
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}
