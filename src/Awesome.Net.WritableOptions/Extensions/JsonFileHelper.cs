﻿using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Awesome.Net.WritableOptions.Extensions
{
    public class JsonFileHelper
    {
        public static void AddOrUpdateSection<T>(string jsonFilePath, string sectionName, Action<T> updateAction = null)
            where T : class, new()
        {
            CreateJsonFile(jsonFilePath);

            var jsonContent = File.ReadAllText(jsonFilePath);

            var jObject = JsonConvert.DeserializeObject<JObject>(jsonContent);

            var sectionObject = jObject.TryGetValue(sectionName, out var sectionValue)
                ? JsonConvert.DeserializeObject<T>(sectionValue.ToString())
                : (new T());

            updateAction?.Invoke(sectionObject);

            jObject[sectionName] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));

            File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }

        public static void AddOrUpdateSection<T>(string jsonFilePath, string sectionName, T value)
        {
            CreateJsonFile(jsonFilePath);

            var jsonContent = File.ReadAllText(jsonFilePath);

            var jObject = JsonConvert.DeserializeObject<JObject>(jsonContent);

            if(typeof(T) == typeof(string) || typeof(T).IsValueType)
            {
                jObject[sectionName] = new JValue(value);
            }
            else
            {
                jObject[sectionName] = JObject.Parse(JsonConvert.SerializeObject(value));
            }

            File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }


        private static void CreateJsonFile(string jsonFilePath)
        {
            if(!File.Exists(jsonFilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(jsonFilePath) ?? throw new InvalidOperationException());

                File.WriteAllText(jsonFilePath, "{}");
            }
        }
    }
}