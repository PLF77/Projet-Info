﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Info
{
    class Program
    {
        static void Main(string[] args)
        {
            MyImage ImageTest = new MyImage("4pix.bmp"); 
            ImageTest.From_Image_To_File("Sortie4pix.bmp");
            for (int i = 0; i < ImageTest.Hauteur; i++)
            {
                for (int j = 0; j < ImageTest.Largeur; j++)
                { 
                    Console.Write(ImageTest.image[i, j].R + " " + ImageTest.image[i, j].G + " " + ImageTest.image[i, j].B + " ");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
