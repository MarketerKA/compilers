using QUT.Gppg;
using System.Text.Json.Serialization;

namespace DLang.Parsing
{

    internal class Locationed
    {
        [JsonIgnore]
        public LexLocation Location { get; set; }

        public Locationed(LexLocation location)
        {
            Location = location;
        }
    }

}
