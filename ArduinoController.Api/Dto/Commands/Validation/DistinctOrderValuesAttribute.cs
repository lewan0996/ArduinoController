using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ArduinoController.Api.Dto.Commands.Validation
{
    public class DistinctOrderValuesAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (!(value is IEnumerable<CommandDto> commands))
            {
                throw new ArgumentException(
                    "DistinctOrderValuesAttribute attribute can be applied only to IEnumerable<CommandDto>");
            }

            var commandOrderNumbers = commands.Select(c => c.Order).ToArray();

            return commandOrderNumbers.Distinct().Count() == commandOrderNumbers.Length;
        }
    }
}
