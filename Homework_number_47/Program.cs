using System;
using System.Collections.Generic;
using System.Threading;

namespace Homework_number_47
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string CommandCreatePlatoon = "1";
            const string CommandStartFight = "2";
            const string CommandExit = "3";

            Battlefield battlefield = new Battlefield();

            bool isExit = false;
            string userInput;

            while (isExit == false)
            {
                Console.WriteLine($"\n\nДля того что бы подготовить отряды нажмите: {CommandCreatePlatoon}\n" +
                                  $"Для того что бы начать бой нажмите:{CommandStartFight}\n" +
                                  $"Для того что бы выйти нажмите: {CommandExit}\n");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandCreatePlatoon:
                        battlefield.PreparePlatoon();
                        break;

                    case CommandStartFight:
                        battlefield.StartFight();
                        break;

                    case CommandExit:
                        isExit = true;
                        break;

                    default:
                        Console.WriteLine("Такой комады нет в списке команд!");
                        break;
                }

                Console.WriteLine("Для продолжения ведите любую клавишу...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    class Soldier
    {
        private Random _random = new Random();

        private int _armour;

        public Soldier()
        {
            GenerateCharacteristics();
        }

        public int Damage { get; private set; }
        public int Health { get; private set; }

        public void Attack(Soldier soldier)
        {
            soldier.TakeDamage(Damage);
        }

        public void TakeDamage(int damage)
        {
            if (damage > 0 && damage > _armour)
            {
                Health -= damage - _armour;

                Console.WriteLine($"Солдат получил урон: -{damage} HP");
            }
        }

        private void GenerateCharacteristics()
        {
            int minQuantityDamage = 10;
            int maxQuantityDamage = 20;

            int minQuantityArmour = 7;
            int maxQuantityArmour = 10;

            int minQuantityHealth = 50;
            int maxQuantityHealth = 100;

            Damage = _random.Next(minQuantityDamage, maxQuantityDamage);
            _armour = _random.Next(minQuantityArmour, maxQuantityArmour);
            Health = _random.Next(minQuantityHealth, maxQuantityHealth);
        }
    }

    class Platoon
    {
        private Random _random = new Random();
        private List<Soldier> _soldiers;

        public Platoon()
        {
            CreateListSoldier();
        }

        public int Capacity => _soldiers.Count;

        public Soldier GetSoldier()
        {
            if (Capacity > 0)
            {
                return _soldiers[_random.Next(0, Capacity)];
            }

            return null;
        }

        public bool TryDeleteSoldier(Soldier soldier)
        {
            if (soldier.Health <= 0)
            {
                _soldiers.Remove(soldier);

                return true;
            }

            return false;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"В взводе ({Capacity}) солдат");
        }

        private void CreateListSoldier()
        {
            _soldiers = new List<Soldier>();

            int ninQuantitySoldiers = 5;
            int maxQuantitySoldiers = 25;

            for (int i = 0; i < _random.Next(ninQuantitySoldiers, maxQuantitySoldiers); i++)
            {
                _soldiers.Add(new Soldier());
            }
        }
    }

    class Battlefield
    {
        private Platoon _firstPlatoon;
        private Platoon _secondPlatoon;

        private bool isWhetherReadyBattle = false;

        public void PreparePlatoon()
        {
            _firstPlatoon = new Platoon();
            Thread.Sleep(100);
            _secondPlatoon = new Platoon();

            _firstPlatoon.ShowInfo();
            _secondPlatoon.ShowInfo();

            isWhetherReadyBattle = true;
        }

        public void StartFight()
        {
            if (isWhetherReadyBattle == true)
            {
                while (_firstPlatoon.Capacity > 0 && _secondPlatoon.Capacity > 0)
                {
                    Soldier firstSoldier = _firstPlatoon.GetSoldier();
                    Soldier secondSoldier = _secondPlatoon.GetSoldier();

                    if (firstSoldier != null && secondSoldier != null)
                    {
                        firstSoldier.Attack(secondSoldier);
                        secondSoldier.Attack(firstSoldier);

                        if (_firstPlatoon.TryDeleteSoldier(firstSoldier) == true)
                        {
                            Console.WriteLine("Солдат первого взвода убит!");
                        }

                        if (_secondPlatoon.TryDeleteSoldier(secondSoldier) == true)
                        {
                            Console.WriteLine("Солдат второго взвода убит!");
                        }
                    }
                }

                if (_firstPlatoon.Capacity <= 0 && _secondPlatoon.Capacity <= 0)
                {
                    Console.WriteLine($"\n\n К сожалению мы не смогли определить победителя поединок закончился ничей");
                }
                else if (_secondPlatoon.Capacity <= 0)
                {
                    ShowWinner("Первый отряд победил");
                }
                else if (_firstPlatoon.Capacity <= 0)
                {
                    ShowWinner("Второй отряд победил");
                }
            }
            else
            {
                Console.WriteLine("Вы не готовы к бою");
            }
        }

        private void ShowWinner(string winner)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(winner);
            Console.ResetColor();
        }
    }
}
