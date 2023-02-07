using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace EGACaseProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hangi işlemi yapmak istiyorsunuz?");
            Console.WriteLine("1-Ekle");
            Console.WriteLine("2-Listele");
            Console.Write("Seçiminiz: ");
            var result = Convert.ToInt32(Console.ReadLine());
            switch (result)
            {
                case 1:
                    XmlElement XmlSGKKOD, XmlTCKN, XmlSoyad, XmlAd, XmlBabaAdi, XmlTutar;
                    string[] xmlFileNames = Directory.GetFiles(@"C:\Users\user\Desktop\EgaCase\", "*.xml");
                    string xmlFileName = xmlFileNames.FirstOrDefault();
                    FileStream xmlFile = System.IO.File.Open(xmlFileName, FileMode.Open, FileAccess.ReadWrite);
                    XmlDocument document = new XmlDocument();
                    document.Load(xmlFile);
                    XmlElement checkData = null; 
                    XmlElement root = document.DocumentElement;
                    for (int J = 0; J < root.GetElementsByTagName("SGKIstek").Count; J++)
                    {
                        checkData = (XmlElement)root.GetElementsByTagName("SGKIstek").Item(J);
                        XmlSGKKOD = (XmlElement)checkData.GetElementsByTagName("SGKKOD").Item(0);
                        XmlTCKN = (XmlElement)checkData.GetElementsByTagName("TCKN").Item(0);
                        XmlSoyad = (XmlElement)checkData.GetElementsByTagName("Soyad").Item(0);
                        XmlAd = (XmlElement)checkData.GetElementsByTagName("Ad").Item(0);
                        XmlBabaAdi = (XmlElement)checkData.GetElementsByTagName("BabaAdi").Item(0);
                        XmlTutar = (XmlElement)checkData.GetElementsByTagName("Tutar").Item(0);

                        Customer customer = new Customer()
                        {
                            SGKKOD = XmlSGKKOD.InnerXml.ToString(),
                            TCKN = XmlTCKN.InnerXml.ToString(),
                            Soyad = XmlSoyad.InnerXml.ToString(),
                            Ad = XmlAd.InnerXml.ToString(),
                            BabaAdi = XmlBabaAdi.InnerXml.ToString(),
                            Tutar = XmlTutar.InnerXml.ToString()
                        };
                        Ekle(customer);
                        Console.WriteLine(customer.Ad);
                        Console.WriteLine(customer.Soyad);
                        Console.WriteLine(customer.TCKN);
                        Console.WriteLine(customer.SGKKOD);
                        Console.WriteLine(customer.BabaAdi);
                        Console.WriteLine(customer.Tutar);
                        Console.WriteLine();
                        Console.WriteLine("--------------------------------------------\n");
                    }
                    Console.ReadLine();
                    break;
                case 2:
                    Listele();
                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Lütfen geçerli bir sayı giriniz\n");
                    break;
            }
        }

        static SqlConnection baglanti;  
        static SqlCommand komut;
        static SqlDataReader reader;

        //Listele metodu
        public static void Listele()
        {
            baglanti = new SqlConnection();
            baglanti.ConnectionString = @"data source=(localdb)\MSSQLLocalDB; initial catalog=EgaCaseDb; integrated security=true;";
            komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "SELECT * FROM Customers";
            baglanti.Open();
            reader = komut.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("---------------------------------------");
                    Console.WriteLine("Ad: " + reader[1]);
                    Console.WriteLine("Soyad: " + reader[2]);
                    Console.WriteLine("Baba Adi: " + reader[3]);
                    Console.WriteLine("SGKKOD: " + reader[4]);
                    Console.WriteLine("TCKN: " + reader[5]);
                    Console.WriteLine("Tutar: " + reader[6]);
                    Console.WriteLine("---------------------------------------\n");
                }
            }
            else
            {
                Console.WriteLine("Veritabanına kayıt ekleme işlemi yaptıktan sonra tekrar deneyiniz.");
            }
            baglanti.Close();

        }
        //Ekle metodu
        public static void Ekle(Customer customers)
        {
            baglanti = new SqlConnection();
            baglanti.ConnectionString = @"data source=(localdb)\MSSQLLocalDB; initial catalog=EgaCaseDb; integrated security=true;";
            komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "INSERT INTO Customers (Ad,Soyad,TCKN,SGKKOD,BabaAdi,Tutar) VALUES ('" + customers.Ad + "','" + customers.Soyad + "','" + customers.TCKN + "','" + customers.SGKKOD + "','" + customers.BabaAdi + "','" + customers.Tutar + "')";
            baglanti.Open();
            int sonuc = komut.ExecuteNonQuery();
            baglanti.Close();
            if (sonuc > 0)
            {
                Console.WriteLine("Veri tabanına ekleme işlemi başarılı!\n");
            }
            else
            {
                Console.WriteLine("Veri tabanına ekleme işlemi başarısız\n");
            }

        }

    }
}
