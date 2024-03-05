using Diacritics.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Utils
{
    public class EntityCodeUtil
    {

        public static string GenerateNamedEntityCode(string entityPrefix, string entityName, Guid entityId)
        {
            return $"{entityPrefix}{EntityCodeConstrant.Separator}{entityName.RemoveDiacritics().ToLower().Replace(" ", "-")}{EntityCodeConstrant.Separator}{entityId}";
        }

        public static string GenerateUnnamedEntityCode(string entityPrefix, Guid entityId)
        {
            return $"{entityPrefix}{EntityCodeConstrant.Separator}{entityId}";
        }
    }
}
