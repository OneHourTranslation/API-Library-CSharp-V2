using System.Collections.Generic;
using Newtonsoft.Json;

namespace oht.Entities
{
    public class Phrase
    {
        [JsonProperty(PropertyName = "source_text")]
        public string SourceText { get; set; }

        [JsonProperty(PropertyName = "uuid")]
        public string Uuid { get; set; }

        [JsonProperty(PropertyName = "context")]
        public string ContextUuid { get; set; }

        [JsonProperty(PropertyName = "remarks")]
        public string Remarks { get; set; }

        [JsonProperty(PropertyName = "flags")]
        public string Flags { get; set; }

        [JsonProperty(PropertyName = "source_language")]
        public string SourceLanguage { get; set; }

        [JsonProperty(PropertyName = "phrase_key")]
        public string PhraseKey { get; set; }

        [JsonProperty(PropertyName = "target")]
        public string TargetsRaw { get; set; }

        public List<PhraseTarget> Targets { get; set; }
    }

    public class PhraseTarget
    {
        public string TargetLanguage { get; set; }
        public List<string> TargetPhrases { get; set; }
    }
}
