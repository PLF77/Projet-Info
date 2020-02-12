using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Projet_Info
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestConversion_Byte_En_Int()
        {
            byte[] tab = new byte[] { 240, 4, 0, 0 };
            uint valueInt = MyImage.Convertir_Endian_To_Int(tab);
            Assert.AreEqual((uint)1264, valueInt);
        }
        [TestMethod]
        public void TestConversion_Int_En_Byte()
        {
            uint valeur = 1264;
            byte[] tabAttendu = new byte[] { 240, 4, 0, 0 };
            byte[] tab = MyImage.Convertir_UInt_To_LowEndian(valeur);
            CollectionAssert.AreEqual(tabAttendu, tab);
        }
        [TestMethod]
        public void TestConstructeur()
        {
            MyImage ImageTest = new MyImage("Damier.bmp");
            Assert.AreEqual((uint)20, ImageTest.Largeur);
            Assert.AreEqual((uint)20, ImageTest.Hauteur);
            Assert.AreEqual((uint)24, ImageTest.NmbrBitsCouleur);
            Assert.AreEqual((uint)1254, ImageTest.TailleFichier);
            Assert.AreEqual((uint)54, ImageTest.TailleOffset);
            Assert.AreEqual("bm", ImageTest.TypeImage);
        }
        public void TestConstructeur2()//Test du remplissage de la matrice avec une image 2x2
        {
            
        }
    }



}
