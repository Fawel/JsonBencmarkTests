using BenchmarkDotNet.Toolchains.Results;
using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections;
using System.Linq;

namespace JsonBenchmarkTest.Models
{
    /// <summary>
    /// Класс для теста сериализации, с одним массивом с потенциально большим кол-во объектов
    /// </summary>
    public class Human
    {
        public Human() { }
        private Human(string name,
            int age,
            bool isSplendid,
            Item[] possesions)
        {
            SetName(name)
                .SetAge(age)
                .SetPossessions(possesions)
                .SetIsSplendid(isSplendid);
        }

        Human SetName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                throw new ArgumentException("Любой человек должен иметь имя, хотя бы дурацкое", nameof(newName));

            Name = newName;
            return this;
        }

        Human SetIsSplendid(bool isSplendid)
        {
            if (Name == "Сакуя")
            {
                // Сакуя всегда хороша
                IsSplendid = true;
            }
            else
            {
                IsSplendid = isSplendid;
            }
            return this;
        }

        Human SetAge(int age)
        {
            if (age < 0)
                throw new ArgumentOutOfRangeException(nameof(age), "Возраст меньше 0 - уже перебор");

            Age = age;
            return this;
        }

        public Human SetPossessions(Item[] newPossesions)
        {
            if (newPossesions is null)
                throw new ArgumentNullException(nameof(newPossesions),
                    "Лишать всей собственности надо через отдельный метод");

            Possesions = newPossesions;
            return this;
        }

        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsSplendid { get; set; }
        public Item[] Possesions { get; set; }

        /// <summary>
        /// Предоставляет методы создания или генерации новых хуманов
        /// </summary>
        public static class Factory
        {
            private static readonly string[] _namePool = new[]{"Владимир", "Сакуя", "Ангелина",
                "Томас", "Рей", "Дженерик", "Лариса", "Нарита"};
            private static readonly Random _random = new Random();

            public static Human GenerateNewRandom()
            {
                // рандомизируем имя

                var nameIndex = _random.Next(_namePool.Length - 1);
                var chosenName = _namePool[nameIndex];

                // рандомизируем возраст

                var newAge = _random.Next(16, 40);

                // рандомизируем охренительность

                var generatedIsSplendid = _random.Next(1) == 1;

                // рандомизируем имущество

                var itemsToGenerate = _random.Next(0, 156);
                var possession = Item.Factory.GenerateRandomItem(itemsToGenerate);

                var newHuman = new Human(chosenName,
                    newAge,
                    generatedIsSplendid,
                    possession
                    );
                return newHuman;
            }

            public static Human[] GenerateNewRandom(int count) =>
                Enumerable.Range(0, count)
                    .Select(x => GenerateNewRandom())
                    .ToArray();

        }
    }
}