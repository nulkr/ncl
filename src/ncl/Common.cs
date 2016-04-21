using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ncl
{
    public class StringEventArgs : EventArgs
    {
        public string Str {get; set; }

        public StringEventArgs(string s)
        {
            this.Str = s;
        }
    }

    public class DataInfo
    {
        public readonly int VersionNo;

        public readonly string Description;

        public DataInfo(int verNo = 1, string desc = "ncl")
        {
            VersionNo = 1;
            Description = desc;
        }

        public override string ToString()
        {
            return Description + " Ver " + VersionNo.ToString();
        }
    }

    /// Serializable Dictionary
    /// http://stackoverflow.com/questions/12554186/how-to-serialize-deserialize-to-dictionaryint-string-from-custom-xml-not-us
    /// write:
    /// using (var writer = XmlWriter.Create("test.xml"))
    ///     (new XmlSerializer(typeof(Recipe))).Serialize(writer, this);
    /// read:
    /// using (var reader = XmlReader.Create("test.xml"))
    ///     Program.recipe = (Recipe)((new XmlSerializer(typeof(Recipe))).Deserialize(reader));
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
