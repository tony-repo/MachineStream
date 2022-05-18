using System;
using System.Text.Json.Serialization;

namespace MachineStream.Model
{
    public class MachineInfo
    {
        [JsonPropertyName("machine_id")]
        public Guid MachineId { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        // As for status we have four status.(idle, running, finished or errored)
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
