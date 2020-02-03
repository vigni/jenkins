using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cnam.UEGLG101.Journey.Data
{
    public class IsInTypeReferentialAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var valid = true;
            var objectAsString = value?.ToString();
            var types = new[] { "Gare", "Arrêt de Bus", "Parking", "Station de Métro", "Supermarket" };

            if (!types.Any(type => type.Equals(objectAsString, System.StringComparison.InvariantCultureIgnoreCase)))
            {
                valid = false;
            }

            return valid;
        }
    }
}