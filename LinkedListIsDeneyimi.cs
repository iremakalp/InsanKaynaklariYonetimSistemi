using System;

namespace yazgel
{
    public class LinkedListIsDeneyimi
    {
        // Referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-3-listeler 
        public Node Head;
        public int boyut = 0;
        // Referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-3-listeler 
        public void isDeneyimiEkle(object value) 
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
        public void isDeneyimiSil(object Position) 
        {
            if (Head != null)
            {
                Node pointer = Head;

                Node posPreNode = new Node();
                posPreNode = Head;

                if (((IsDeneyimi)pointer.Veri).YerAdi == ((IsDeneyimi)Position).YerAdi) //Silinecek düğüm head ise head'i bir sonraki düğüm yap
                {
                    Head = pointer.adres;
                }
                while (pointer != null) //silinecek değer bulunana kadar (iş adı ile kontrol edilecek) listede ilerle
                {
                    if (((IsDeneyimi)pointer.Veri).YerAdi == ((IsDeneyimi)Position).YerAdi) //silinecek değer bulunduğunda silinecek değerin next'ini bi önceki değerin next'i yap böylece listede artık temp'i gösteren eleman kalmadı ve silme işlemi gerçekleşti
                        posPreNode.adres = pointer.adres;
                    else
                        posPreNode = pointer;

                    pointer = pointer.adres;
                }
                boyut--;
            }
        } 
        public  string isDeneyimiYazdir()
        {
            string pointer = "";
            Node i = Head;
            while (i != null) //Liste null olana kadar listedeki iş bilgilerini pointer'a ekle ve ilerle
            {
                pointer += ((IsDeneyimi)i.Veri).YerAdi.ToString() + "," + ((IsDeneyimi)i.Veri).IsAdresi.ToString() + "," + ((IsDeneyimi)i.Veri).Pozisyon.ToString()  + "," + ((IsDeneyimi)i.Veri).TecrubeSuresi.ToString()+"/";
                i = i.adres;
            }
            return pointer;
        }
        public string isDeneyimiGoster()
        {
            string pointer = "";
            Node i = Head;
            while (i != null) //Liste null olana kadar listedeki okul bilgileri pointera eklenir
            {
                pointer += "İş yeri Adı: " + ((IsDeneyimi)i.Veri).YerAdi.ToString() + " / İş Adresi: " + ((IsDeneyimi)i.Veri).IsAdresi.ToString() + " / Pozisyon/Görev: " + ((IsDeneyimi)i.Veri).Pozisyon.ToString() + " / Tecrübe Suresi: " + ((IsDeneyimi)i.Veri).TecrubeSuresi.ToString() + Environment.NewLine;
                i = i.adres;
            }
            return pointer;
        }

        public bool EnAzTecrubeli(int sure)
        {
            bool tecrube = false;
            Node pointer = Head;
            if (pointer == null)
                tecrube = false;
            else if (((IsDeneyimi)pointer.Veri).TecrubeSuresi >= sure)
                tecrube = true;
            else
                pointer = pointer.adres;
            return tecrube;
        }
 
    }
}