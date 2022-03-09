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

        internal static void liczeniePostaci(string filepath)
        {
            // odczyt zawartości dokumentu
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            //zmienne pomocnicze
            int temp_licznik = 0;
            string temp, sc, postac;
            List<string> uzyte = new List<string>();
            List<string> uzyte2 = new List<string>();
            var drugs = doc.GetElementsByTagName("produktLeczniczy");
            // wczytywanie danych do listy
            foreach (XmlNode d in drugs)
            {
                sc = d.Attributes.GetNamedItem("nazwaPowszechnieStosowana").Value;
                postac = d.Attributes.GetNamedItem("postac").Value;
                temp = sc + " %% " + postac;
                uzyte.Add(temp);
            }
            uzyte.Sort(); // sortowanie listy
            uzyte = uzyte.Distinct().ToList(); // usuwanie duplikatow w liscie
            List<string> liczenie = new List<string>();
            //modyfikacja elementow listy w celu pozniejszego liczenia
            foreach (string name in uzyte)
            {
                int index = name.IndexOf("%%");
                liczenie.Add(name.Substring(0, index));
            }
            //zliczenie wystapien nazwPowszechnieStosowanych
            var q = liczenie.GroupBy(x => x)
                        .Select(g => new { Value = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count);

            //zliczanie, gdy posiada wiecej niz 1 postac
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

        internal static void liczenieProdukcji(string filepath)
        {
            // odczyt zawartości dokumentu
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            //zmienne pomocnicze
            string temp, po, postac;
            List<string> dane = new List<string>();
            var drugs = doc.GetElementsByTagName("produktLeczniczy");
            //wczytywanie danych do listy
            foreach (XmlNode d in drugs)
            {
                po = d.Attributes.GetNamedItem("podmiotOdpowiedzialny").Value;
                postac = d.Attributes.GetNamedItem("postac").Value;
                temp = po + " %% " + postac;
                dane.Add(temp);
            }
            dane.Sort(); //sortowanie listy
            List<string> produkcja = new List<string>();
            List<string> kremy = new List<string>();
            //sprawdzanie, czy postac to krem
            foreach (string name in dane)
            {
                int index = name.IndexOf("%%");
                if (name.Remove(0, index + 3).StartsWith("Krem"))
                {
                    kremy.Add(name.Substring(0, index));
                }
            }
            //znajdowanie podmiotu z najwieksza iloscia produkowanych kremow
            string maxKremy = kremy.GroupBy(s => s)
                         .OrderByDescending(s => s.Count())
                         .First().Key;

            Console.WriteLine("Najwiecej kremow produkuje {0}", maxKremy);

            //sprawdzanie, czy postac to tabletki
            List<string> tabletki = new List<string>();
            foreach (string name in dane)
            {
                int index = name.IndexOf("%%");
                if (name.Remove(0, index + 3).StartsWith("Tabletki"))
                {
                    tabletki.Add(name.Substring(0, index));
                }
            }
            // znajdowanie podmiotu z najwieksza iloscia produkowanych tabletek
            string maxTabletki = tabletki.GroupBy(s => s)
                         .OrderByDescending(s => s.Count())
                         .First().Key;

            Console.WriteLine("Najwiecej tabletek produkuje {0}", maxTabletki);
        }
    }
}