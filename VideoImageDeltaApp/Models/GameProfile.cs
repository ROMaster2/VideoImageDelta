using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace VideoImageDeltaApp.Models
{
    public class GameProfile
    {
        public GameProfile(string name)
        {
            Name = name;
        }

        public GameProfile() { }

        public string Name { get; set; }
        public List<Screen> Screens { get; set; } = new List<Screen>();

        public void Serialize(string output)
        {
            Type t = GetType();
            XmlSerializer serializer = new XmlSerializer(t);
            using (TextWriter writer = new StreamWriter(output))
            {
                serializer.Serialize(writer, this);
            }
        }

        override public string ToString()
        {
            return Name;
        }

    }

}
