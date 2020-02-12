using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

namespace Projet_Info
{
    public class Pixel
    {
        // Attributs
        public uint r; // red
        public uint g; // green 
        public uint b; // blue

        // Constructeur
        public Pixel(uint R, uint G, uint B)
        {
            this.r = R;
            this.g = G;
            this.b = B;
        }

        // Propriétés
        public uint R
        { get { return r; } }

        public uint G
        { get { return g; } }

        public uint B
        { get { return b; } }

        // Méthodes
        // Convertit un tableau de bytes en tableau de pixels
        public void Conversion(string filename)
        {
            byte[] myfile = File.ReadAllBytes(filename);
            Bitmap fichierBmp = new Bitmap(filename);

            Pixel[,] image = new Pixel[fichierBmp.Width, fichierBmp.Height];

            for (int i = 54; i < myfile.Length; i = i + 60)
            {
                for (int j = i; j < i + 60; j = j + 3)
                {
                    r = Convert.ToUInt32(myfile[j]);
                }
                for (int j = i + 1; j < i + 60; j = j + 3)
                {
                    g = Convert.ToUInt32(myfile[j]);
                }
                for (int j = i + 2; j < i + 60; j = j + 3)
                {
                    b = Convert.ToUInt32(myfile[j]);
                }
                Pixel pixelCourant = new Pixel(R, G, B);  //instanciation du pixel en question

                // remplissage de la matrice de pixels
                for (int k = 0; k < image.GetLength(0); k++)
                {
                    for (int l = 0; l < image.GetLength(1); l++)
                    {
                        
                        image[k, l] = pixelCourant;
                        Console.Write(image[k, l]);
                    }
                    Console.WriteLine();
                }
            }
        }


    }
}
