using System;
using System.Collections.Generic;

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
                        battlefield.CreatePlatoons();
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
        private static Random _random = new Random();

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
        private static Random _random = new Random();
        private List<Soldier> _soldiers;

        public Platoon()
        {
            CreateListSoldier();
        }

        public int SoldiersCount => _soldiers.Count;

        public Soldier GetSoldier()
        {
            if (SoldiersCount > 0)
            {
                return _soldiers[_random.Next(0, SoldiersCount)];
            }

            return null;
        }

        public bool TryDeleteDeadSoldier(Soldier soldier)
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
            Console.WriteLine($"В взводе ({SoldiersCount}) солдат");
        }

        private void CreateListSoldier()
        {
            _soldiers = new List<Soldier>();

            int minQuantitySoldiers = 5;
            int maxQuantitySoldiers = 25;
            int quantitySoldiers = _random.Next(minQuantitySoldiers, maxQuantitySoldiers);

            for (int i = 0; i < quantitySoldiers; i++)
            {
                _soldiers.Add(new Soldier());
            }
        }
    }

    class Battlefield
    {
        private Platoon _firstPlatoon;
        private Platoon _secondPlatoon;

        private bool _isWhetherReadyBattle = false;

        public void CreatePlatoons()
        {
            _firstPlatoon = new Platoon();
            _secondPlatoon = new Platoon();

            _firstPlatoon.ShowInfo();
            _secondPlatoon.ShowInfo();

            _isWhetherReadyBattle = true;
        }

        public void StartFight()
        {
            if (_isWhetherReadyBattle == true)
            {
                while (_firstPlatoon.SoldiersCount > 0 && _secondPlatoon.SoldiersCount > 0)
                {
                    Soldier firstSoldier = _firstPlatoon.GetSoldier();
                    Soldier secondSoldier = _secondPlatoon.GetSoldier();

                    if (firstSoldier != null && secondSoldier != null)
                    {
                        firstSoldier.Attack(secondSoldier);
                        secondSoldier.Attack(firstSoldier);

                        if (_firstPlatoon.TryDeleteDeadSoldier(firstSoldier) == true)
                        {
                            Console.WriteLine("Солдат первого взвода убит!");
                        }

                        if (_secondPlatoon.TryDeleteDeadSoldier(secondSoldier) == true)
                        {
                            Console.WriteLine("Солдат второго взвода убит!");
                        }
                    }
                }

                if (_firstPlatoon.SoldiersCount <= 0 && _secondPlatoon.SoldiersCount <= 0)
                {
                    Console.WriteLine($"\n\n К сожалению мы не смогли определить победителя поединок закончился ничей");
                }
                else if (_secondPlatoon.SoldiersCount <= 0)
                {
                    ShowWinner("Первый отряд победил");
                }
                else if (_firstPlatoon.SoldiersCount <= 0)
                {
                    ShowWinner("Второй отряд победил");
                }

                _isWhetherReadyBattle = false;
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
