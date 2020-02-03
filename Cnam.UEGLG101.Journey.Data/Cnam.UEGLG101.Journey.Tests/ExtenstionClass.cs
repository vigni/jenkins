using Cnam.UEGLG101.Journey.Data;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cnam.UEGLG101.Journey.Tests
{
    public static class ExtenstionClass
    {
        public static bool IsValid(this Area area, out string errors)
        {
            var properties = typeof(Area).GetProperties();
            var errorsDico = new Dictionary<string, List<string>>();

            foreach (var property in properties)
            {
                var validationAttributes = property.GetCustomAttributes(false)
                    .Where(attr => attr is ValidationAttribute);
                foreach (var attribute in validationAttributes)
                {
                    var validationAttribute = attribute as ValidationAttribute;
                    if (!validationAttribute.IsValid(property.GetValue(area)))
                    {
                        if (!errorsDico.ContainsKey(property.Name))
                        {
                            errorsDico.Add(property.Name, new List<string>());
                        }

                        errorsDico[property.Name].Add(validationAttribute.ErrorMessage);
                    }
                }
            }

            var sb = new StringBuilder();
            foreach (var error in errorsDico)
            {
                sb.AppendLine(error.Key);
                foreach (var row in error.Value)
                {
                    sb.AppendLine($"  - {row}");
                }
            }
            errors = sb.ToString();
            
            return !errors.Any();
        }
    }
}
