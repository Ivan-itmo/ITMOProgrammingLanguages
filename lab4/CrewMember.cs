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
    public record CrewMember
    {
        [property: JsonPropertyName("credit_id")] public string CreditId = "";
        [property: JsonPropertyName("department")] public string Department = "";
        [property: JsonPropertyName("gender")] public int Gender;
        [property: JsonPropertyName("id")] public int Id;
        [property: JsonPropertyName("job")] public string Job = "";
        [property: JsonPropertyName("name")] public string Name = "";
    }
}
