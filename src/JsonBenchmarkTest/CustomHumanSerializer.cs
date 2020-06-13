using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using JsonBenchmarkTest.Models;
using Newtonsoft.Json;

namespace JsonBenchmarkTest
{
    public static class CustomHumanSerializer
    {
        public static Human DeserializeToHuman(JsonReader jsonReader)
        {
            int age = -1;
            string name = string.Empty;
            Item[] possesions = new Item[0];
            bool isSplendid = false;

            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.StartObject)
                    continue;

                else if (jsonReader.TokenType == JsonToken.PropertyName)
                {
                    string property = (string)jsonReader.Value;
                    switch (property)
                    {
                        case "Age":
                            age = (int)jsonReader.ReadAsInt32();
                            break;
                        case "Name":
                            name = jsonReader.ReadAsString();
                            break;
                        case "IsSplendid":
                            isSplendid = (bool)jsonReader.ReadAsBoolean();
                            break;
                        case "Possesions":
                            possesions = DeserializeToItemArray(jsonReader);
                            break;
                        default:
                            throw new ArgumentException($"Неизвестное свойство: {property}");
                    }
                }

                else if (jsonReader.TokenType == JsonToken.EndObject)
                {
                    break;
                }
            }

            return Human.Factory.CreateNew(name, age, isSplendid, possesions);
        }

        public static Human[] DeserializeToHumanArray(JsonReader jsonReader)
        {
            List<Human> parsedHumans = new List<Human>();
            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.StartArray)
                    continue;

                else if (jsonReader.TokenType == JsonToken.StartObject)
                {
                    var human = DeserializeToHuman(jsonReader);
                    parsedHumans.Add(human);
                }

                else if (jsonReader.TokenType == JsonToken.EndArray)
                    break;
            }

            return parsedHumans.ToArray();
        }

        public static Item DeserializeToItem(JsonReader jsonReader)
        {
            string manufactor = string.Empty;
            string name = string.Empty;
            string type = string.Empty;

            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.StartObject)
                    continue;

                else if (jsonReader.TokenType == JsonToken.PropertyName)
                {
                    string property = (string)jsonReader.Value;
                    switch (property)
                    {
                        case "Manufactor":
                            manufactor = jsonReader.ReadAsString();
                            break;
                        case "Name":
                            name = jsonReader.ReadAsString();
                            break;
                        case "Type":
                            type = jsonReader.ReadAsString();
                            break;
                        default:
                            throw new ArgumentException($"Неизвестное свойство: {property}");
                    }
                }

                else if (jsonReader.TokenType == JsonToken.EndObject)
                {
                    break;
                }
            }

            return Item.Factory.CreateNew(name, type, manufactor);
        }

        public static Item[] DeserializeToItemArray(JsonReader jsonReader)
        {
            List<Item> parsedItems = new List<Item>();
            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.StartArray)
                    continue;

                else if (jsonReader.TokenType == JsonToken.StartObject)
                {
                    var item = DeserializeToItem(jsonReader);
                    parsedItems.Add(item);
                }

                else if (jsonReader.TokenType == JsonToken.EndArray)
                    break;
            }

            return parsedItems.ToArray();
        }
    }
}
