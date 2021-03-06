﻿using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArduinoController.Xamarin.Core.Dto.Commands.Deserialization
{
    public class CommandDtoJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jo = new JObject();
            var type = value.GetType();
            jo.Add("type", type.Name.Replace("CommandDto", ""));

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead) continue;
                var propVal = prop.GetValue(value, null);
                if (propVal != null)
                {
                    jo.Add(prop.Name, JToken.FromObject(propVal, serializer));
                }
            }
            jo.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var commandDtoTypeName = jObject["type"] + "CommandDto";
            var commandDtoType = GetCommandDtoType(commandDtoTypeName);

            if (commandDtoType == null)
            {
                throw new Exception($"{commandDtoTypeName} does not exist");
            }

            var result = Activator.CreateInstance(commandDtoType);
            
            serializer.Populate(jObject.CreateReader(), result);

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(CommandDto));
        }

        private static Type GetCommandDtoType(string typeName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
        }
    }
}
