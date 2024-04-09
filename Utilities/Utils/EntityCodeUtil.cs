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
            var timeStampCode = sqids.Encode(DateTime.UtcNow.Ticks).ToUpper();
            var entityNumberCode = sqids.Encode(entityNumber).ToUpper();
            var code = entityNumberCode;
            Console.WriteLine(timeStampCode);
            Console.WriteLine(entityNumberCode);
            return $"{entityPrefix}{TimeUtil.GetCurrentVietNamTime().ToString("yyyyMMdd").Substring(2)}{code}";
        }
    }
}
