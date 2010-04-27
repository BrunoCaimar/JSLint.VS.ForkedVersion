using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System;

/// <summary>
/// Deserializes/Serializes IDictionary from/to hard disk.
/// Parts snached from http://blogs.msdn.com/psheill/archive/2005/04/09/406823.aspx
/// </summary>
public class DictionarySerializer
{
    public static string Serialize(IDictionary dictionary)
    {
        List<Entry> entries = new List<Entry>(dictionary.Count);
        foreach (object key in dictionary.Keys)
        {
            entries.Add(new Entry(key, dictionary[key]));
        }

        XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
        using (StringWriter sw = new StringWriter())
        {
            serializer.Serialize(sw, entries);
            return sw.ToString();
        }
    }

    public static void Deserialize(string toRead, IDictionary dictionary)
    {
        if (string.IsNullOrEmpty(toRead))
        {
            return;
        }

        dictionary.Clear();
        XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
        List<Entry> list = null;

        using (StringReader sr = new StringReader(toRead))
        {
            list = (List<Entry>)serializer.Deserialize(sr);
        }

        foreach (Entry entry in list)
        {
            dictionary[entry.Key] = entry.Value;
        }
    }

    public class Entry
    {
        public object Key;
        public object Value;

        public Entry()
        {
        }

        public Entry(object key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}