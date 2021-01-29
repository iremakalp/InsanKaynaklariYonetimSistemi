using System;

namespace yazgel
{
    public class LinkedListEgitim
    {
        // Referans  https://www.slideshare.net/DenizKILIN/yzm-2116-blm-3-listeler 
        public Node Head;
        public int boyut = 0;
        // Referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-3-listeler 
        public void egitimBilgisiEkle(object value) 
        {
            Node eklenecek = new Node() { Veri = value };

            if (Head == null)  //ilk düğüm null ise liste boşsa eklenecek düğümü head yap
            {
                Head = eklenecek;
            }
            else  //head'i head'in next'i yap ve yeni head'i eklenecek düğüm yap
            {
                eklenecek.adres = Head;
                Head = eklenecek;
            }
            boyut++;
        }
        // Referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-3-listeler 
        public void egitimBilgisiSil(object Position) 
        {
            if (Head != null)
            {
                Node pointer = Head;

                Node posPreNode = new Node();
                posPreNode = Head;

                if (((EgitimBilgileri)pointer.Veri).OkulAdi == ((EgitimBilgileri)Position).OkulAdi) //Silinecek düğüm head ise head'i bir sonraki düğüm yap
                {
                    Head = pointer.adres;
                }
                while (pointer != null) //silinecek değer bulunana kadar (okul adı ile kontrol edilecek) listede ilerle
                {
                    if (((EgitimBilgileri)pointer.Veri).OkulAdi == ((EgitimBilgileri)Position).OkulAdi) //silinecek değer bulunduğunda silinecek değerin next'ini bi önceki değerin next'i yap böylece listede artık temp'i gösteren eleman kalmadı ve silme işlemi gerçekleşti
                    {
                        posPreNode.adres = pointer.adres;
                    }
                    else
                    {
                        posPreNode = pointer;
                    }

                    pointer = pointer.adres;
                }
                boyut--;
            }

        }
        // Referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-3-listeler 
        public string egitimBilgisiYazdir()
        {
            string pointer = "";
            Node i = Head;
            while (i != null) //Liste null olana kadar listedeki okul bilgileri pointera eklenir
            {
                pointer += ((EgitimBilgileri)i.Veri).Turu.ToString() + "," + ((EgitimBilgileri)i.Veri).OkulAdi.ToString() + "," + ((EgitimBilgileri)i.Veri).Bolumu.ToString()+ "," +  ((EgitimBilgileri)i.Veri).BaslangicTarihi.ToString()+  "," + ((EgitimBilgileri)i.Veri).BitisTarihi.ToString()+ "," +((EgitimBilgileri)i.Veri).NotOrtalamasi.ToString();
                i = i.adres;
            }
            return pointer;
        }

        public string egitimBilgisiGoster()
        {
            string pointer = "";
            Node i = Head;
            while (i != null) //Liste null olana kadar listedeki okul bilgileri pointera eklenir
            {
                pointer +="Okul Adı: " +((EgitimBilgileri)i.Veri).OkulAdi.ToString()+ " / Türü: " + ((EgitimBilgileri)i.Veri).Turu.ToString() + " / Bölümü: " + ((EgitimBilgileri)i.Veri).Bolumu.ToString() + " / Not Ortalaması: " + ((EgitimBilgileri)i.Veri).NotOrtalamasi.ToString()+  " / Başlangıç Tarihi: " + ((EgitimBilgileri)i.Veri).BaslangicTarihi.ToString() + " / Bitiş Tarihi: " + ((EgitimBilgileri)i.Veri).BitisTarihi.ToString()  + Environment.NewLine;
                i = i.adres;
            }
            return pointer;
        }

        public bool enAzLisansMezunu()
        {
            bool lisansMezunu = false;
            Node pointer = Head;
            if (pointer == null)
                lisansMezunu = false;
            else if (((EgitimBilgileri)pointer.Veri).Turu=="Lisans" || ((EgitimBilgileri)pointer.Veri).Turu == "Yüksek Lisans" || ((EgitimBilgileri)pointer.Veri).Turu == "Doktora")
                lisansMezunu = true;
            else
                pointer = pointer.adres;
            return lisansMezunu;
        }
      
    }
}