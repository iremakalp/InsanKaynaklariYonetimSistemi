using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yazgel
{
    public class Kisi
    {
        //Sınıflar için referans https://www.youtube.com/watch?v=VAuli64QnxI
        public Kisi()
        {
            this.IsDeneyimleri = new LinkedListIsDeneyimi();
            this.EgitimBilgileri = new LinkedListEgitim();

        }
        public string Ad { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string Eposta { get; set; }
        public int DogumTarihi { get; set; } 
        public string YabancıDil { get; set; }
        public string Ehliyet { get; set; }
        public LinkedListEgitim EgitimBilgileri;
        public LinkedListIsDeneyimi IsDeneyimleri;
    }
  
    public class EgitimBilgileri
    {
        public string OkulAdi { get; set; }
        public string Turu { get; set; }
        public string Bolumu { get; set; }
        public string BaslangicTarihi { get; set; }
        public string BitisTarihi { get; set; }
        public string NotOrtalamasi { get; set; }

    }
    public class IsDeneyimi
    {
        public string YerAdi { get; set; }
        public string IsAdresi { get; set; }
        public string Pozisyon { get; set; }
        public double TecrubeSuresi { get; set; }
    }

}
