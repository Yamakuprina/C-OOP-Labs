using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Backups;

namespace BackupsExtra
{
    public class Serialization
    {
        public static void SaveViaDataContractSerialization<T>(T serializable, string dataPath)
        {
            if (serializable == null) throw new BackupException("Cant save program state");
            if (dataPath == null) throw new BackupException("Cant save program state");
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = true,
                NewLineOnAttributes = true,
                ConformanceLevel = ConformanceLevel.Document,
            };
            XmlWriter writer = XmlWriter.Create(dataPath, settings);
            serializer.WriteObject(writer, serializable);
            writer.Close();
        }

        public static T LoadViaDataContractSerialization<T>(string dataPath)
        {
            if (dataPath == null) throw new BackupException("Cant save program state");
            FileStream stream = new FileStream(dataPath, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            T serializable = (T)serializer.ReadObject(reader, true);
            reader.Close();
            stream.Close();
            return serializable;
        }
    }
}