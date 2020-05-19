using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace APIGateway.Helpers
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string str)
        {
            return string.Concat(
                str.Select(
                    (x, i) => i > 0 && char.IsUpper(x)
                        ? "_" + x
                        : x.ToString()
                )
            ).ToLower();
        }


    }
}
