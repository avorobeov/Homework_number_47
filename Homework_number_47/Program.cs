using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
                Console.WriteLine($"\n\nДля того что бы сгенерировать отряды нажмите: {CommandCreatePlatoon}\n" +
                                  $"Для того что бы начать бой нажмите:{CommandStartFight}\n" +
                                  $"Для того что бы выйти нажмите: {CommandExit}\n");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandCreatePlatoon:
                        battlefield.CreatePlatoon();
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
        private int _armour;
        
        public Soldier(int damage, int armour, int health)
        {
            Damage = damage;
            _armour = armour;
            Health = health;
        }

        public int Damage { get; private set; }
        public  int Health { get; private set; }

        public void Attack(Soldier soldier)
        {
            soldier.TakeDamage(Damage);
        }

        public void TakeDamage(int damage)
        {
            if (damage > 0 && damage > _armour)
            {
                Health -= damage - _armour;
            }
        }
    }

    class Platoon
    {
        private Random _random = new Random();
        private List<Soldier> _soldiers;

        public Platoon(List<Soldier> soldiers)
        {
            _soldiers = soldiers;
        }

        public int QuantitySoldier => _soldiers.Count;

        public void Attack(Platoon platoon)
        {
            if (platoon.QuantitySoldier > 0 && _soldiers.Count > 0)
            {
                platoon.TakeDamage(_random.Next(0, platoon.QuantitySoldier), _soldiers[_random.Next(0, _soldiers.Count)].Damage);
            }
        }

        public void TakeDamage(int quantitySoldier, int damage)
        {
            if (quantitySoldier <= _soldiers.Count)
            {
                Soldier soldiers = _soldiers[quantitySoldier];
                soldiers.TakeDamage(damage);

                if (soldiers.Health <= 0)
                {
                    DeleteSoldier(soldiers);

                    Console.WriteLine("Солдат был убит");
                }
                else
                {
                    Console.WriteLine($"Солдат получил урон: -{damage} XP");
                }
            }
        }

        private void DeleteSoldier(Soldier soldier)
        {
            _soldiers.Remove(soldier);
        }
    }

    class Battlefield
    {
        private Random _random = new Random();
        private Platoon _firstPlatoon;
        private Platoon _secondPlatoon;

        private bool isWhetherReadyBattle = false;

        public void CreatePlatoon()
        {
            _firstPlatoon = new Platoon(CreateSoldiers());
            _secondPlatoon = new Platoon(CreateSoldiers());

            ShowInfoPlatoon(_firstPlatoon);
            ShowInfoPlatoon(_secondPlatoon);

            isWhetherReadyBattle = true;
        }

        public void StartFight()
        {
            if (isWhetherReadyBattle == true)
            {
                while (_firstPlatoon.QuantitySoldier > 0 && _secondPlatoon.QuantitySoldier > 0)
                {
                    _firstPlatoon.Attack(_secondPlatoon);
                    _secondPlatoon.Attack(_firstPlatoon);
                }

                if (_firstPlatoon.QuantitySoldier <= 0 && _secondPlatoon.QuantitySoldier <= 0)
                {
                    Console.WriteLine($"\n\n К сожалению мы не смогли определить победителя поединок закончился ничей");
                }
                else if (_secondPlatoon.QuantitySoldier <= 0)
                {
                    ShowWinner("Первый отряд победил");
                }
                else if (_firstPlatoon.QuantitySoldier <= 0)
                {
                    ShowWinner("Второй отряд победил");
                }
            }
            else
            {
                Console.WriteLine("Вы не готовы к бою");
            }
        }

        private List<Soldier> CreateSoldiers()
        {
            List<Soldier> soldiers = new List<Soldier>();

            int ninQuantitySoldiers = 5;
            int maxQuantitySoldiers = 25;

            int minQuantityDamage = 10;
            int maxQuantityDamage = 20;

            int minQuantityArmour = 7;
            int maxQuantityArmour = 10;

            int minQuantityHealth = 50;
            int maxQuantityHealth = 100;

            for (int i = 0; i < _random.Next(ninQuantitySoldiers, maxQuantitySoldiers); i++)
            {
                soldiers.Add(new Soldier(_random.Next(minQuantityDamage, maxQuantityDamage), _random.Next(minQuantityArmour, maxQuantityArmour), _random.Next(minQuantityHealth, maxQuantityHealth)));
            }

            return soldiers;
        }

        private void ShowInfoPlatoon(Platoon platoon)
        {
            Console.WriteLine($"В первом взводе ({platoon.QuantitySoldier}) солдат");
        }

        private void ShowWinner(string winner)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(winner);
            Console.ResetColor();
        }
    }
}
