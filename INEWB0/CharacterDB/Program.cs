using System.Text.Json;

namespace CharacterDB.Models
{
    class Program
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "characters.json");
        static void Main() {
            var characters = LoadCharacters(FilePath);
            while (true)
            {
                Console.WriteLine("Character Database Manager");
                Console.WriteLine("1. Add Character");
                Console.WriteLine("2. List Characters");
                Console.WriteLine("3. Character Statistics");
                Console.WriteLine("4. Delete Character");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCharacter(characters);
                        SaveCharacters(FilePath, characters);
                        break;
                    case "2":
                        ListCharacters(characters);
                        break;
                    case "3":
                        CharacterStatistics(characters);
                        break;
                    case "4":
                        DeleteCharacter(characters);
                        SaveCharacters(FilePath, characters);
                        break;
                    case "5":
                        Console.WriteLine("Exiting... Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void AddCharacter(List<Character> characters)
        {
            Console.Write("Enter character name: ");
            var name = Console.ReadLine();
            Console.Write("Is the character a woman (true/false): ");
            bool woman = bool.TryParse(Console.ReadLine(), out bool isWoman) && isWoman;
            Console.Write("Enter character weapon type: ");
            var weaponType = Console.ReadLine();
            Console.Write("Enter character height: ");

            if (int.TryParse(Console.ReadLine(), out int height))
            {
                Console.Write("Enter character element: ");
                var element = Console.ReadLine();
                var newCharacter = new Character(name, woman, weaponType, height, element);
                characters.Add(newCharacter);
                Console.WriteLine($"Character '{name}' added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid height. Character not added.");
            }
        }

        static void DeleteCharacter(List<Character> characters)
        {
            ListCharacters(characters);
            Console.Write("Enter the number of the character to delete: ");

            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= characters.Count)
            {
                var characterToDelete = characters[index - 1];
                characters.RemoveAt(index - 1);
                Console.WriteLine($"Character '{characterToDelete.Name}' deleted successfully.");
            }
            else
            {
                Console.WriteLine("Invalid number. No character deleted.");
            }
        }

        static void ListCharacters(List<Character> characters)
        {
            Console.WriteLine("\nListing all characters:");
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine("| {0,-5} | {1,-20} | {2,-10} | {3,-15} | {4,-8} | {5,10} |", "No.", "Name", "Woman", "Weapon Type", "Height", "Element");
            Console.WriteLine("----------------------------------------------------------------------");
            for (int i = 0; i < characters.Count; i++)
            {
                var character = characters[i];
                Console.WriteLine("| {0,-5} | {1,-20} | {2,-10} | {3,-15} | {4,-8} | {5,10} |", i + 1, character.Name, character.Woman, character.WeaponType, character.Height, character.Element);
            }
            Console.WriteLine("----------------------------------------------------------------------");
        }

        static void CharacterStatistics(List<Character> characters)
        {
            if (characters.Count == 0)
            {
                Console.WriteLine("No characters available to display statistics.");
                return;
            }

            var averageHeight = characters.Average(character => character.Height);
            var minHeight = characters.Min(character => character.Height);
            var maxHeight = characters.Max(character => character.Height);
            var totalCharacters = characters.Count;

            Console.WriteLine("Character Statistics:");
            Console.WriteLine($"Total characters: {totalCharacters}");
            Console.WriteLine($"Average height: {averageHeight}");
            Console.WriteLine($"Minimum height: {minHeight}");
            Console.WriteLine($"Maximum height: {maxHeight}");
        }

        static List<Character> LoadCharacters(string filePath)
        {
            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Character>>(jsonData) ?? new List<Character>();
            }
            else
            {
                Console.WriteLine("JSON file not found. Starting with an empty character list.");
                return new List<Character>();
            }
        }

        static void SaveCharacters(string filePath, List<Character> characters)
        {
            characters = characters.OrderBy(character => character.Name).ToList();
            var jsonData = JsonSerializer.Serialize(characters, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }
    }
}
