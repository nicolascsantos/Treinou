using System.Text.Json;
using Treinou.API.Extensions.String;

namespace Treinou.API.Configurations.Policies
{
    public class JsonSnakeCasePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
            => name.ToSnakeCase();
    }
}
