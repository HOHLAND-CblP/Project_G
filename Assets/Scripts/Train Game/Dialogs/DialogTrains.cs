using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlRoot("dialog")]
public class DialogTrains
{
    [XmlElement("say")]
    public Say[] says;

    public static DialogTrains Load(TextAsset _xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DialogTrains));
        StringReader reader = new StringReader(_xml.text);
        DialogTrains dial = serializer.Deserialize(reader) as DialogTrains;
        return dial;
    }
}

public class Say
{
    [XmlElement("id")]
    public int id;
    [XmlElement("text")]
    public string text;
    [XmlElement("dialEnd")]
    public bool dialEnd;
    [XmlElement("nextSay")]
    public int nextSay;
}