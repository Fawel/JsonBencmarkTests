using System;
using System.Linq;
using System.Reflection.Emit;

namespace JsonBenchmarkTest.Models
{
    public class Item
    {
        public Item()
        {
        }

        private Item(string type, string name, string manufactor)
        {
            SetName(type).SetName(name).SetManufactor(manufactor);
        }

        public string Type { get; set; }
        public string Name { get; set; }
        public string Manufactor { get; set; }

        public Item SetType(string newType)
        {
            if (string.IsNullOrEmpty(newType))
                throw new ArgumentException("Тип предмета не указан", nameof(newType));

            Type = newType;
            return this;
        }

        public Item SetName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                throw new ArgumentException("Имя предмета не указано", nameof(newName));

            Name = newName;
            return this;
        }

        public Item SetManufactor(string newManufactor)
        {
            if (string.IsNullOrEmpty(newManufactor))
                throw new ArgumentException("Производитель не указан", nameof(newManufactor));

            Manufactor = newManufactor;
            return this;
        }

        public static class Factory
        {
            private static string[] _typePool = new[] {"Стол", "Стул", "Лампа", "Стойка", 
                "Напиток", "Монитор", "Пылесос", "Телевизор"};
            private static string[] _namePool = new[] { "Бесславный разоритель", "Удручающая безделушка", 
                "Дорогая штуковина", "Бесполезный хлам", "Разваливающееся нечто", "Весёлая приблуда" };
            private static string[] _manufactorPool = new[] {"Polgaref", "Muap Co.", "Qwerty inc", 
                "Supreme't", "Tolyato Factory", "Roga and Koputo"};

            private static Random _random = new Random();

            public static Item GenerateRandomItem()
            {
                // рандомизируем тип вещи
                var typeIndex = _random.Next(_typePool.Length - 1);
                var chosenType = _typePool[typeIndex];

                // рандомизируем название вещи

                var nameIndex = _random.Next(_namePool.Length - 1);
                var chosenName = _namePool[nameIndex];

                // рандомизируем производителя

                var manufactorIndex = _random.Next(_manufactorPool.Length - 1);
                var chosenManufactor = _manufactorPool[manufactorIndex];

                // генерим вещь

                var newItem = new Item(chosenType, chosenName, chosenManufactor);
                return newItem;
            }

            public static Item[] GenerateRandomItem(int count)
            {
                if (count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count),
                        "Кол-во генерируемых вещей должно 0 или больше");

                var result = Enumerable.Range(0, count)
                                .Select(x => GenerateRandomItem())
                                .ToArray();
                return result;
            }
        }
    }
}