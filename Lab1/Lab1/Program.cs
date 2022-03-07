﻿using System;
using System.IO;


namespace lab1_kk
{
    class Program
    {
        static void Main(string[] args)
        {
            string xmlpath = Path.Combine("Assets", "data.xml");
            Console.WriteLine("XML loaded by DOM Approach");
            XMLReadWithDOMApproach.LiczeniePostaci(xmlpath);
            /*
            // odczyt danych z wykorzystaniem SAX
            Console.WriteLine("XML loaded by SAX Approach");
            XMLReadWithSAXApproach.Read(xmlpath);
            */
            /*
            // odczyt danych z wykorzystaniem XPath i DOM
            Console.WriteLine("XML loaded with XPath");
            XMLReadWithXLSTDOM.Read(xmlpath);
            */
            Console.ReadLine();
        }
    }
}
