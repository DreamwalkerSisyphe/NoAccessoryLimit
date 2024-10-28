using System.Text.Json.Serialization;

namespace NoAccessoryLimit;

public class Config {
    [JsonInclude] public int AccessoryLimit = -1;
}
