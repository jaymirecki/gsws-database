using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

public class Serializer<T> {
    public Serializer() {

    }

    public void Serialize(string filename, T obj) {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        TextWriter writer = new StreamWriter(filename);
        serializer.Serialize(writer, obj);
        writer.Close();
    }
    public void SerializeDictionary(string filename, Dictionary<string, T> obj) {
        XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
        TextWriter writer = new StreamWriter(filename);
        serializer.Serialize(writer, new List<T>(obj.Values));
        writer.Close();
    }
    public T Deserialize(string filename) {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        serializer.UnknownNode += 
            new XmlNodeEventHandler(serializer_UnknownNode);
        serializer.UnknownAttribute += 
            new XmlAttributeEventHandler(serializer_UnknownAttribute);
        FileStream fs = new FileStream(filename, FileMode.Open);
        T result = (T)serializer.Deserialize(fs);
        fs.Close();
        return result;
    }
    private void serializer_UnknownNode(object sender, XmlNodeEventArgs e) {
        // Console.WriteLine("Unknown Node:" +   e.Name + "\t" + e.Text);
    }

    private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e) {
        // System.Xml.XmlAttribute attr = e.Attr;
        // Console.WriteLine("Unknown attribute " + 
        // attr.Name + "='" + attr.Value + "'");
    }
}