
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Lab4
{
    public record CastMember
    {
        [property: JsonPropertyName("cast_id")] public string CastId = "";
        [property: JsonPropertyName("character")] public string Character = "";
        [property: JsonPropertyName("credit_id")] public string CreditId = "";
        [property: JsonPropertyName("gender")] public int Gender;
        [property: JsonPropertyName("id")] public int Id;
        [property: JsonPropertyName("name")] public string Name = "";
        [property: JsonPropertyName("order")] public int Order;
        
    }
}
