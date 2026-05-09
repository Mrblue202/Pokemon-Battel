using System;
using System.Collections.Generic;

namespace PokemonBattel
{
    // Static class for type effectiveness
    public static class TypChart
    {
        public static double GetEffectiveness(PokemonType attacker, PokemonType defender)
        {
            return (attacker, defender) switch
            {
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

    // Move class
    public class Move
    {
        public string Name;
        public int Power { get; private set; }
        public PokemonType Type { get; private set; }

        public Move(string name, int power, PokemonType type)
        {
            Name = name;
            Power = power;
            Type = type;
        }
    }

    // Enum for Types
    public enum PokemonType { Fire, Water, Grass, Normal }

    // Pokemon class
    public class Pokemon
    {
        public string Name;
        public PokemonType Type;
        public int Health;
        public int attack;
        public int Defense;
        public int speed;
        public List<Move> Moves { get; set; }

        public Pokemon(string name, PokemonType type, int health, int newAttack, int defense, int speed, List<Move> moves)
        {
            Name = name;
            Type = type;
            Health = health;
            attack = newAttack;
            Defense = defense;
            this.speed = speed;
            Moves = moves;
        }

        public bool Isfainted => Health <= 0;

        public void takeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        public int CalculateDamage(Pokemon attacker, Move moveUsed)
        {
            // Simplified calculation: (Attacker Attack + Move Power) - Defender Defense
            double baseDamage = (attacker.attack + moveUsed.Power) - this.Defense;
            if (baseDamage < 5) baseDamage = 5; // Ensure at least some damage is done

            double typeEffectiveness = TypChart.GetEffectiveness(moveUsed.Type, this.Type);

            return (int)(baseDamage * typeEffectiveness);
        }
    }

    // Main Program
    class Program
    {
        static void Main()
        {
            var Allpokemon = new List<Pokemon>
            {
                new Pokemon("Charmander", PokemonType.Fire, 100, 52, 43, 65, new List<Move>
                {
                    new Move("Ember", 40, PokemonType.Fire),
                    new Move("Scratch", 40, PokemonType.Normal)
                }),

                new Pokemon("Squirtle", PokemonType.Water, 100, 48, 65, 43, new List<Move>
                {
                    new Move("Water Gun", 40, PokemonType.Water),
                    new Move("Tackle", 40, PokemonType.Normal)
                }),

                new Pokemon("Bulbasaur", PokemonType.Grass, 100, 49, 49, 45, new List<Move>
                {
                    new Move("Vine Whip", 45, PokemonType.Grass),
                    new Move("Tackle", 40, PokemonType.Normal)
                })
            };

            // Choose your Pokemon
            Console.WriteLine("Welcome to Battle! Choose your Pokemon (type the number):");
            for (int i = 0; i < Allpokemon.Count; i++)
            {
                Console.WriteLine($"{i}: {Allpokemon[i].Name}");
            }
            int choice = int.Parse(Console.ReadLine());
            Pokemon player = Allpokemon[choice];

            // Choose random opponent
            Random rnd = new Random();
            Pokemon enemy = Allpokemon[rnd.Next(0, Allpokemon.Count)];
            Console.WriteLine($"\nYou chose {player.Name}! You are fighting {enemy.Name}!");

            // Battle loop
            while (!player.Isfainted && !enemy.Isfainted)
            {
              
                Console.WriteLine("\n--- NEW TURN ---");
                Console.WriteLine($"{player.Name} HP: {player.Health} | {enemy.Name} HP: {enemy.Health}");

                // Player picks a move
                Console.WriteLine("Choose a move:");
                for (int i = 0; i < player.Moves.Count; i++)
                {
                    Console.WriteLine($"{i}: {player.Moves[i].Name}");
                }
                int moveChoice = int.Parse(Console.ReadLine());
                Move playerMove = player.Moves[moveChoice];

                // Player attacks enemy
                int damageToEnemy = enemy.CalculateDamage(player, playerMove);
                enemy.takeDamage(damageToEnemy);
                Console.WriteLine($"{player.Name} used {playerMove.Name} and did {damageToEnemy} damage!");

                if (enemy.Isfainted) break;

                // Enemy attacks player simple AI always picks the first move
                Move enemyMove = enemy.Moves[0];
                int damageToPlayer = player.CalculateDamage(enemy, enemyMove);
                player.takeDamage(damageToPlayer);
                Console.WriteLine($"{enemy.Name} used {enemyMove.Name} and did {damageToPlayer} damage!");
                Console.Clear();
            }

            // Win/Loss message
            if (player.Isfainted)
            {
                Console.WriteLine("\nYour Pokemon fainted! You lost.");
            }
            else
            {
                Console.WriteLine("\nThe enemy fainted! You are the winner!");
            }
        }
    }
}