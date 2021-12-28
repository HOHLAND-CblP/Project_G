using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace NewPlatformer
{
    [XmlRoot("dialog")]
    public class NovellaDialogXml
    {
        [XmlArray("idCharacters")]
        [XmlArrayItem("idCharacter")]
        public int[] idCharacters;
        [XmlElement("say")]
        public Say[] says;

        public static NovellaDialogXml Load(TextAsset _xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NovellaDialogXml));
            StringReader reader = new StringReader(_xml.text);
            NovellaDialogXml dial = serializer.Deserialize(reader) as NovellaDialogXml;
            return dial;
        }
        public static NovellaDialogXml Load(string _xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NovellaDialogXml));
            StreamReader sr = new StreamReader(_xml);
            NovellaDialogXml dial = serializer.Deserialize(sr) as NovellaDialogXml;
            return dial;
        }

        public static string Save(NovellaDialogXml ndXML)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NovellaDialogXml));
            StreamWriter sw = new StreamWriter(Application.dataPath+"/dialog 1.xml");
            serializer.Serialize(sw, ndXML);
            sw.Close();
            return Application.dataPath + "/dialog 1.xml";
        }
    }

    [Serializable]
    public class Say
    {
        [XmlElement("numbersay")]
        public int numberSay;
        [XmlElement("id_character")]
        public int idCharacter;
        [XmlElement("text")]
        public string text;
        [XmlArray("answers")]
        [XmlArrayItem("answer")]
        public Answer[] answers;
        [XmlElement("nextSay")]
        public int nextSay;
        [XmlElement("dialEnd")]
        public bool dialEnd;
    }

    [Serializable]
    public class Answer
    {
        [XmlAttribute("tosay")]
        public int nextSay;
        [XmlElement("text")]
        public string text;
        [XmlElement("dialEnd")]
        public bool dialEnd;
        [XmlElement("wasChosen")]
        public bool wasChosen;
    }
}