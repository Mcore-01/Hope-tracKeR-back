using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hope_tracKeR_back.Config;

public static class JsonConfig
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}