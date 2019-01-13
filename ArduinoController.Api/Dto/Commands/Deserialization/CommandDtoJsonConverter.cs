using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArduinoController.Api.Dto.Commands
{
    public class CommandDtoJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, 
            object existingValue, JsonSerializer serializer)
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
