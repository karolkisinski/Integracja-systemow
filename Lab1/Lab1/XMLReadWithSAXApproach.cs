using DocumentFormat.OpenXml.Wordprocessing;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lab1_kk
{
    internal class XMLReadWithSAXApproach
    {
        internal static void Read(string filepath)
        {
            // konfiguracja początkowa dla XmlReadera
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;
            // odczyt zawartości dokumentu
            XmlReader reader = XmlReader.Create(filepath, settings);
            // zmienne pomocnicze
            int count = 0;
            string postac = "";
            string sc = "";
            reader.MoveToContent();
            // analiza każdego z węzłów dokumentu
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name
                == "produktLeczniczy")
                {
                    postac = reader.GetAttribute("postac");
                    sc =
                    reader.GetAttribute("nazwaPowszechnieStosowana");
                    if (postac == "Krem" && sc == "Mometasoni furoas")
                        count++;
                }
            }
            Console.WriteLine("Liczba produktów leczniczych w postaci kremu, których jedyną substancją czynną jest Mometasoni furoas {0}", count);
        }

        internal static void LiczeniePostaci(string filepath)
        {
            // odczyt zawartości dokumentu
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            int temp_licznik = 0;
            string temp, sc, postac;
            List<string> uzyte = new List<string>();
            List<string> uzyte2 = new List<string>();
            var drugs = doc.GetElementsByTagName("produktLeczniczy");
            foreach (XmlNode d in drugs)
            {
                sc = d.Attributes.GetNamedItem("nazwaPowszechnieStosowana").Value;
                postac = d.Attributes.GetNamedItem("postac").Value;
                temp = sc + " %% " + postac;
                uzyte.Add(temp);
            }
            uzyte.Sort();
            uzyte = uzyte.Distinct().ToList();
            List<string> liczenie = new List<string>();
            foreach (string name in uzyte)
            {
                int index = name.IndexOf("%%");
                liczenie.Add(name.Substring(0, index));
            }
            var q = liczenie.GroupBy(x => x)
                        .Select(g => new { Value = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count);

            foreach (var x in q)
            {
                if (x.Count > 1)
                {
                    //Console.WriteLine("Nazwa powszechna: " + x.Value + " Ilosc postaci: " + x.Count);
                    temp_licznik++;
                }
            }
            Console.WriteLine("Ilosc preparatow majacych wiecej niz 1 postac o tej samej nazwie powszechnej: {0}", temp_licznik);
        }
    }
}