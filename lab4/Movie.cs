using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = "";
        public IReadOnlyList<CrewMember> Crew { get; set; } = ImmutableList<CrewMember>.Empty;
        
        public IReadOnlyList<CastMember>  Cast { get; set; } = ImmutableList<CastMember>.Empty;
    }
}
