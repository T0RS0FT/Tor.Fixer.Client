using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tor.Fixer.Client.Json
{
    public class SafeBoolConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                return reader.GetBoolean();
            }
            catch
            {
                return false;
            }
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}
