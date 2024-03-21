using Diacritics.Extensions;
using Sqids;
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

        public static string GenerateEntityCode(string entityPrefix, int entityNumber)
        {
            var sqids = new SqidsEncoder<long>(new()
            {
                MinLength = 6,
            });
            var id = sqids.Encode(entityNumber).ToUpper();
            return $"{entityPrefix}{DateTime.Now.ToString("yyyyMMdd").Substring(2)}{id}";
        }
    }
}
