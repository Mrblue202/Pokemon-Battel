namespace PokemonBattel
{
    // Static class to represent the type effectiveness chart for Pokemon battles
    public static class TypChart
    {
        public static double GetEffectiveness(PokemonType attacker, PokemonType defender)
        {
            return (attacker, defender) switch
            {
                // Super effective/ not effective combinations
                (PokemonType.Fire, PokemonType.Grass) => 2.0,
                (PokemonType.Water, PokemonType.Fire) => 2.0,
                (PokemonType.Grass, PokemonType.Water) => 2.0,
                (PokemonType.Grass, PokemonType.Fire) => 0.5,
                (PokemonType.Fire, PokemonType.Water) => 0.5,
                (PokemonType.Water, PokemonType.Grass) => 0.5,
                (PokemonType.Grass, PokemonType.Grass) => 0.5,
                (PokemonType.Fire, PokemonType.Fire) => 0.5,
                (PokemonType.Water, PokemonType.Water) => 0.5,
                _ => 1.0
            };
        }
    }

    // Class to represent a move that a Pokemon can use in battle
    public class Move
    {
        public string Name { get; private set; }
        public int Power { get; private set; }
        public PokemonType Type { get; private set; }
        // Constructor to initialize a move with its name, power, and type
        public Move(string name, int power, PokemonType type)
        {
            Name = name;
            Power = power;
            Type = type;
        }
    }


    // Enum to represent different Pokemon types
    public enum PokemonType
    {
        Fire,
        Water,
        Grass,
        Normal
    }

    // Class to represent a Pokemon with its attributes and behaviors
    public class Pokemon
    {
        public string Name { get; set; }
        public PokemonType Type { get; set; }
        public int Health { get; set; }
        public int attack { get; set; }
        public int defense { get; set; }
        public int speed { get; set; }
        public List<Move> Moves { get; set; } 

        // Constructor to initialize a Pokemon with its attributes
        public Pokemon(string name, PokemonType type, int health, int attack, int defense, int speed, List<Move> moves)
        {
            
            Name = name;
            Type = type;
            Health = health;
            this.attack = attack;
            this.defense = defense;
            this.speed = speed;
            Moves = moves;
        }

        // Property to check if the Pokemon has fainted (health is 0 or less)
        public bool Isfainted => Health <= 0;

        // Method to calculate damage taken by the Pokemon based on the attacker's type and stats
        public void takeDamage(int damage)
        {
            // Reduce health by the damage taken, ensuring it doesn't go below 0
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        

        public int CalculateDamage(Pokemon attacker)
        {
            double baseDamage = (attacker.attack - this.defense) + 10; // Basic damage 

            double typeEffectiveness = TypChart.GetEffectiveness(attacker.Type, this.Type);

            return (int)(baseDamage * typeEffectiveness);
        }

    }

    // Main program class to set up the Pokemon battle scenario
    class Program
    { static void Main()
        {
            var Allpokemon = new List<Pokemon>
            { 
                // Creating instances of Pokemon with their attributes and moves

                new Pokemon("Charmander", PokemonType.Fire, 100, 52, 43, 65, new List<Move>
                {
                    new Move("Ember", 40, PokemonType.Fire),
                    new Move("Scratch", 40, PokemonType.Normal)
                }),

                new Pokemon("Squirtle", PokemonType.Water, 44, 48, 65, 43, new List<Move>
                {
                    new Move("Water Gun", 40, PokemonType.Water),
                    new Move("Tackle", 40, PokemonType.Normal)
                }),

                new Pokemon("Bulbasaur", PokemonType.Grass, 45, 49, 49, 45, new List<Move>
                {
                    new Move("Vine Whip", 45, PokemonType.Grass),
                    new Move("Tackle", 40, PokemonType.Normal)
                })
            };

        }
    }
}

