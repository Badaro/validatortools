using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ValidatorTools
{
    public class FormatData
    {
        [JsonProperty("sets")]
        public string[]? Sets { get; set; }
        [JsonProperty("cards")]
        public string[]? Cards { get; set; }
        [JsonProperty("banned")]
        public string[] Banned { get; set; }
    }
}
