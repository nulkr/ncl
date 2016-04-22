using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace ncl
{
    public static class JsonFile
    {
        public static void Load<T>(ref T o, string fileName)
        {
            using (var r = File.OpenText(fileName))
            {
                // Copy 하면서 private member 가 null 로 덮어 써지므로 public readonly 하거나 JsonProperty 로 포함시켜야 함
                o = JsonConvert.DeserializeObject<T>(r.ReadToEnd());
            }
        }

        public static void LoadBson<T>(ref T o, string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (BsonReader reader = new BsonReader(fs))
                o = (new JsonSerializer()).Deserialize<T>(reader);
        }

        public static void LoadCompressed<T>(ref T o, string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var deflate = new DeflateStream(fs, CompressionMode.Decompress))
            using (var reader = new BinaryReader(deflate))
            {
                o = JsonConvert.DeserializeObject<T>(reader.ReadString());
            }
        }

        public static void Save(object o, string fileName, Newtonsoft.Json.Formatting formatting = Formatting.None)
        {
            using (StreamWriter file = File.CreateText(fileName))
                file.Write(JsonConvert.SerializeObject(o, formatting));
        }

        public static void SaveBson(object o, string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Create))
            using (var bw = new BsonWriter(fs))
            {
                (new JsonSerializer()).Serialize(bw, o);
            }
        }

        public static void SaveCompressed(object o, string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Create))
            using (var deflate = new DeflateStream(fs, CompressionMode.Compress))
            using (var writer = new BinaryWriter(deflate))
            {
                writer.Write(JsonConvert.SerializeObject(o));
            }
        }
    }
}