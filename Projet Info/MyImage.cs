using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Projet_Info
{
    public class MyImage
    {
        // Attributs 

        public string typeImage; //bm
        public uint tailleFichier;  
        public uint tailleOffset;
        public uint largeur;
        public uint hauteur;
        public uint nmbrBitsCouleur;
        public Pixel[,] image; // Matrice de Pixels

        //Propriétés
        public Pixel[,] Image
        {
            get => image;
            set => image = value;
        }
        public string TypeImage
        {
            get => typeImage;
            set => typeImage = value;
        }
        public uint TailleFichier
        {
            get => tailleFichier;
            set => tailleFichier = value;
        }
        public uint TailleOffset
        {
            get => tailleOffset;
            set => tailleOffset = value;
        }
        public uint Hauteur
        {
            get => hauteur;
            set => hauteur = value;
        }
        public uint Largeur
        {
            get => largeur;
            set => largeur = value;
        }
        public uint NmbrBitsCouleur
        {
            get => nmbrBitsCouleur;
            set => nmbrBitsCouleur = value;
        }
        //Constructeurs
        public MyImage() { }

        //lit un fichier (.csv ou .bmp) et le transforme en instance de la classe MyImage 
        public MyImage(string myfile)
        {
            byte[] file = File.ReadAllBytes(myfile);
            if (file[0] == 66 && file[1] == 77) // si le fichier est un .BMP
            {
                this.typeImage = "bm";

                //Taille du fichier
                byte[] tabTaille = new byte[4];
                for (int i = 2; i < 5; i++)
                {
                    tabTaille[i - 2] = file[i];
                }
                this.tailleFichier = Convertir_Endian_To_Int(tabTaille);

                //Taille offset
                byte[] tabTailleOffSet = new byte[4];
                for (int i = 10; i < 14; i++)
                {
                    tabTailleOffSet[i - 10] = file[i];
                }
                this.tailleOffset = Convertir_Endian_To_Int(tabTailleOffSet);

                //Largeur
                byte[] tabLargeur = new byte[4];
                for (int i = 18; i < 22; i++)
                {
                    tabLargeur[i - 18] = file[i];
                }
                this.largeur = Convertir_Endian_To_Int(tabLargeur);

                //Hauteur
                byte[] tabHauteur = new byte[4];
                for (int i = 22; i < 26; i++)
                {
                    tabHauteur[i - 22] = file[i];
                }
                this.hauteur = Convertir_Endian_To_Int(tabHauteur);

                //Nombre de bits par couleur 
                byte[] tabnbBitsCouleur = new byte[4];
                for (int i = 28; i < 30; i++)
                {
                    tabnbBitsCouleur[i - 28] = file[i];
                }
                this.nmbrBitsCouleur = Convertir_Endian_To_Int(tabnbBitsCouleur);
                //Remplissage de l'attribut "image" matrice de Pixel 
                //ATTENTION dans le format BMP les bits sont codés dans l'ordre B,G,R
                Queue<Pixel> filePixels = new Queue<Pixel>();
                for (uint i = 54; i < file.Length; i = i + 3 * largeur)
                {
                    for (uint j = i; j < i + (3 * largeur); j+=3)
                    {
                        uint B = Convert.ToUInt32(file[j]);

                        uint G = Convert.ToUInt32(file[j + 1]);

                        uint R = Convert.ToUInt32(file[j + 2]);
                        Pixel pixelCourant = new Pixel(R, G, B);  //instanciation du pixel en question
                        filePixels.Enqueue(pixelCourant);
                    }

                }
                // remplissage de la matrice de pixels ATTENTION dans le format BMP les bits sont codés de bas en haut et de gauche a droite
                Pixel[,] imageRemplie = new Pixel[hauteur, largeur];
                for (int i = imageRemplie.GetLength(0)-1; i >= 0; i--)
                {
                    for (int j = 0; j < largeur; j++)
                    {
                        imageRemplie[i,j]= filePixels.Dequeue();  
                    }
                } 
                this.image = imageRemplie;
                }
            /*
            if (file[1] == ',') // si le fichier est un .CSV
            {
                this.typeImage = "csv";
                Console.WriteLine(); // "; TOTO"; // C://Clients.csv";
                Console.WriteLine();
                StreamReader fichLect = null;
                try
                {
                    fichLect = new StreamReader(filename);// ou =File.OpenText(filename)

                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                char[] sep = new char[1] { ';' };
                string ligne = "";
                string[] datas;
                int i = 0;
                try
                {
                    while (fichLect.Peek() > 0)
                    {
                        ligne = fichLect.ReadLine(); // LECTURE D’UNE LIGNE
                        Console.WriteLine("ligne lue : " + ligne);
                        datas = ligne.Split(sep);
                        ListeDeComptes[i] = new CompteBancaire(datas[0], Convert.ToInt32(datas[1]), TorF(datas[2]));
                        i++;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    if (fichLect != null) { fichLect.Close(); }
                }
            }*/
        }




        //  prend une instance de MyImage et la transforme en fichier binaire respectant 
        //la structure du fichier .bmp ou .csv 
        public void From_Image_To_File(string file)
        {
            byte[] tabImage = new byte[(Largeur * 3 * Hauteur) + 54];
            //Ecriture du type de l'image 
            tabImage[0] = 66;
            tabImage[1] = 77;
            //Ecriture de la taille du fichier en bits
            byte[] taille = new byte[4];
            taille = Convertir_UInt_To_LowEndian(tailleFichier, 4);
            for(int i=2;i<taille.Length;i++)
            {
                tabImage[i]=taille[i - 2];
            }
            //Ecriture de la taille offset 
            byte[] offset = new byte[4];
            offset = Convertir_UInt_To_LowEndian(tailleOffset, 4);
            for (int i = 0; i < offset.Length; i++)
            {
                tabImage[i+10] = offset[i];
            }
            //Ecriture de la taille du header
            byte[] headerSize = new byte[4];
            headerSize = Convertir_UInt_To_LowEndian(40, 4);
            for (int i = 0; i < headerSize.Length; i++)
            {
                tabImage[i+14] = headerSize[i];
            }
            //Ecriture de la largeur de l'image
            byte[] largeurImage = new byte[4];
            largeurImage = Convertir_UInt_To_LowEndian(largeur, 4);
            for (int i = 0; i < largeurImage.Length; i++)
            {
                tabImage[i+18] = largeurImage[i];
            }
            //Ecriture de la hauteur de l'image
            byte[] hauteurImage = new byte[4];
            hauteurImage = Convertir_UInt_To_LowEndian(hauteur, 4);
            for (int i =0; i < hauteurImage.Length; i++)
            {
                tabImage[i+22] = hauteurImage[i];
            }
            //Ecriture d'un 1 obligatoire sur le bit 26
            byte[] unObligatoire = new byte[2] ;
            unObligatoire = Convertir_UInt_To_LowEndian(1, 2);
            for (int i = 0; i < unObligatoire.Length; i++)
            {
                tabImage[i+26] = unObligatoire[i];
            }
            //Ecriture du nombre de bit par couleur
            byte[] nbBitCouleur = new byte[2];
            nbBitCouleur = Convertir_UInt_To_LowEndian(nmbrBitsCouleur, 4);
            for (int i = 0; i < nbBitCouleur.Length; i++)
            {
                tabImage[i+28] = nbBitCouleur[i];
            }
            //Ecriture de la taille l'image pure
            byte[] taillePure = new byte[4];
            taillePure = Convertir_UInt_To_LowEndian(Hauteur*Largeur*3, 4);
            for (int i = 0; i < taillePure.Length; i++)
            {
                tabImage[i+34] = taillePure[i];
            }
            //Remplissage du tableau de bits a partir de la matrice de pixels
            //ATTENTION on doit coder un image BMP donc avec des bits dans l'ordre B,G,R et avec les sens de lecture du format BMP
            Queue<byte> fileBits = new Queue<byte>();
            for (int i = Convert.ToInt32(Hauteur-1); i >= 0; i--)//On Enqueue en partant du bas gauche de limage et on remonte
            {
                for (int j = 0; j < Largeur; j++)
                {
                    fileBits.Enqueue(Convert.ToByte(image[i, j].B));
                    fileBits.Enqueue(Convert.ToByte(image[i, j].G));
                    fileBits.Enqueue(Convert.ToByte(image[i, j].R));
                }
            }
            for (uint i = 54; i < (Largeur * 3 * Hauteur) + 54 ; i++)
            {
                tabImage[i] = fileBits.Dequeue();
            }
            //Ecriture du tableau final de bits dans un fichier .bmp 
            File.WriteAllBytes(file, tabImage);
        }

        //  convertit une séquence d’octets au format little endian en entier 
        public static uint Convertir_Endian_To_Int(byte[] tab)
        {
            uint value = 0;
            for (uint i = 0; i < Convert.ToUInt32(tab.Length); i++)
            {
                value += tab[i] * Convert.ToUInt32((Math.Pow((uint)256, i)));
            }
            return value;
        }

        //  convertit un entier en séquence d’octets au format little endian 
        public static byte[] Convertir_UInt_To_LowEndian(uint val,int taille)
        {
            uint stockage = val;
            byte nbByte;
            byte[] Byte_Arr = new byte[taille];
            for (uint i = 0; i < taille; i++)
            {
                if (val > 255)
                {
                    stockage = val / 256; //division euclidienne 
                    nbByte = (byte)(val - stockage * 256); //On soustrait a notre valeur initiale la valeur décimale correspondante a la valeur du byte 
                }
                else // si la valeur est < 255
                {
                    nbByte = (byte)stockage; // alors on la garde telle-qu'elle  
                    stockage = 0;
                }
                Byte_Arr[i] = nbByte; // on remplit le tableau en commençant par le bit de poids faible 
                val = stockage;
            }
            return Byte_Arr;

        }

        // méthode
        //convertit les bytes en int 
        public static uint Conversion_Bit_To_UInt(byte bit)
        {
            uint value = Convert.ToUInt32(bit);
            return value;
        }


    }
}

