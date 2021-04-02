using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlRoot("dialogue")]
public class Dialogue
{
    [XmlElement("text")]
    public string text;

    [XmlElement("node")]
    public Node[] nodes;

    public static Dialogue Load(TextAsset _xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Dialogue));
        StringReader reader = new StringReader(_xml.text);
        Dialogue dial = serializer.Deserialize(reader) as Dialogue;
        return dial;
    }
}

[Serializable]
public class Node
{
    [XmlElement("face")]
    public int face;
    [XmlElement("participant")]
    public int participant;
    [XmlElement("name")]
    public string name;
    [XmlElement("text")]
    public string text;
    [XmlArray("answers")]
    [XmlArrayItem("answer")]
    public Answer[] answers= { };
    [XmlElement("dialend")]
    public string end;
    [XmlElement("nextNode")]
    public string nextNode;
}

public class Answer
{
    [XmlAttribute("tonode")]
    public int nextNode;
    [XmlElement]
    public string text;
}

