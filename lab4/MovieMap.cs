using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using ProgLang4;

namespace Lab4
{
    public class MovieMap : ClassMap<Movie>
    {
        public MovieMap()
        {
            Map(m => m.MovieId).Name("movie_id");
            Map(m => m.Title).Name("title");
            Map(m => m.Cast).Name("cast").Convert(row =>
                {
                    var field = row.Row.GetField("cast");
                    if (string.IsNullOrEmpty(field))
                        return ImmutableList<CastMember>.Empty;
                    var castMembers = JsonConvert.DeserializeObject<List<CastMember>>(field);
                    return castMembers?.ToImmutableList() ?? ImmutableList<CastMember>.Empty;
                    // castMembers != null ? castMembers.ToImmutableList() : ImmutableList<CastMember>.Empty;
                }
                );
            
            
            Map(m => m.Crew).Name("crew").Convert(row =>
            {
                var field = row.Row.GetField("crew");
                if (string.IsNullOrEmpty(field))
                    return ImmutableList<CrewMember>.Empty;
                var crewMembers = JsonConvert.DeserializeObject<List<CrewMember>>(field);
                return crewMembers?.ToImmutableList() ?? ImmutableList<CrewMember>.Empty;
                // crewMembers != null ? crewMembers.ToImmutableList() : ImmutableList<CrewMember>.Empty;
            }
            );
        }
    }
}
