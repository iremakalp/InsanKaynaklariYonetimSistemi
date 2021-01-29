using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace yazgel
{
    public class IkiliAramaAgaci
    {
        // referans1  https://www.slideshare.net/DenizKILIN/yzm-2116-blm-7-tree-ve-binary-tree-kili-aa
        // referans2 https://www.slideshare.net/DenizKILIN/yzm-2116-blm-8-kili-arama-aac-binary-search-tree

        private IkiliAramaAgaciDugumu kok;
        private string dugumler;
        public IkiliAramaAgaci() // ikili arama agacı için kurucu metot 
        {
        }
        public IkiliAramaAgaci(IkiliAramaAgaciDugumu kok) //IkiliAramaAgaciDugumu sınfından türetilen kökü mevcut köke atar.
        {
            this.kok = kok;
        }
        public string DugumleriYazdir()
        {
            return dugumler;
        }
        //referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-8-kili-arama-aac-binary-search-tree
        #region agaca kisi ekleme metodu
        public void KisiEkle (Kisi deger) //kişi ekleme metodu
        {
            Boolean Solda = true;
            IkiliAramaAgaciDugumu parent = new IkiliAramaAgaciDugumu(); // yeni eklenecek düğüm için parent oluşturuldu.
            IkiliAramaAgaciDugumu pointer = kok; // başlangıç noktası kök olarak belirlendi ve pointer ile ilerlemesi sağlandı
            //eklenecek kişinin konumunu belirlemek için while döngüsü kullanıldı
            while (pointer!=null) 
            {
                //eğer kök doluysa işaretli kök yeni kişinin parent'ı olarak atandı
                parent = pointer; 
                if(deger.Ad==((Kisi)pointer.veri).Ad) // pointer ın bulunduğu kişinin adı ile eklenecek kişinin adı aynı ise
                {
                    return;
                }
                else if (deger.Ad[0]<((Kisi)pointer.veri).Ad[0]) // yeni eklenen kişinin adının ilk harfi ascii karşılığında pointerdan küçükse ağacın soluna git
                {
                    pointer = pointer.sol;
                }
                else if (deger.Ad[0]== ((Kisi)pointer.veri).Ad[0]) // eklenen kişi adının ilk harfi ile ascii karşılığına göre pointerın ilk harfi aynı ise
                {
                    int i = 1; 
                    while (deger.Ad[i]!=null) 
                    {
                        if(deger.Ad[i] == ((Kisi)pointer.veri).Ad[i]) //pointer ve eklenen kişinin harfleri aynı olduğu sürece hep bir sonraki harfe geçer ve devam eder
                        {
                            i++;
                            continue;
                        }
                        else if (deger.Ad[i] < ((Kisi)pointer.veri).Ad[i]) //eklenen kişi adının ascii karşılığı pointerdan küçük ise işaretçi sola kayar ve çıkar
                        {
                            pointer = pointer.sol;
                            break;
                        }
                        else //eklenen kişi adının ascii karşılığı pointerdan büyük ise işaretçi sağa kayar ve çıkar
                        {
                            Solda = false;
                            pointer = pointer.sag;
                            break;
                        }
                    }
                   
                }
                else //ilk harfin ascii karşılığı büyükse ağaçta sağa git 
                        pointer = pointer.sag;
            }
            IkiliAramaAgaciDugumu eklenecek = new IkiliAramaAgaciDugumu(deger);
            //while da bulunan konuma yeni kişiyi eklemek
            if (kok == null) //eğer kök boşsa yeni kişiyi köke ekle
            {
                kok = eklenecek;
            }
            else if (deger.Ad[0] < ((Kisi)parent.veri).Ad[0]) // eğer kişinin ilk harfi parentın ilk harfinden küçükse kişiyi sola ekle
            {
                parent.sol = eklenecek;
            }
            else if (deger.Ad[0] == ((Kisi)parent.veri).Ad[0] && Solda) // eğer kişinin ilk harfi parentın ilk harfi aynıysa ve diğer harfler kontrol edildiğinde solda true ise sola ekle
            {
                parent.sol = eklenecek;
            }
            else  // eğer kişinin ilk harfi parentın ilk harfinden büyük kişiyi sağa ekle
            {
                parent.sag = eklenecek;
            }
        }
        #endregion

        //referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-8-kili-arama-aac-binary-search-tree
        #region agactan kisi silme metotlaru
        //referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-8-kili-arama-aac-binary-search-tree
        private IkiliAramaAgaciDugumu Successor(IkiliAramaAgaciDugumu silinecekDugum) // Silinecek düğümün çocuğu ile düğümün ailesi(parent) arasında bağ kurulur
        {
            IkiliAramaAgaciDugumu successorParent = silinecekDugum;
            IkiliAramaAgaciDugumu successor = silinecekDugum;
            IkiliAramaAgaciDugumu current = silinecekDugum.sag; //silinecek düğümün sağı varsa current'a ata

            while (current != null)
            {
                //(succesorü bulmak için) Current null değilse null olana kadar sola git 
                successorParent = successor;
                successor = current;
                current = current.sol;
            }
            if (successor != silinecekDugum.sag) //successor silinecek düğümün sağındaysa yani daha küçük yoksa(sola gidilmediyse) yer değiştirme işlemi yap daha sonra successor'ü gönder
            {
                successorParent.sol = successor.sag;
                successor.sag = silinecekDugum.sag;
            }
            return successor;
        }
        public bool KisiSil(string deger) //kişi silme metodu
        {
            if (kok == null) //ağacın kökü yoksa silme işlemi gerçekleştirilmez
            {
                return false;
            }
            else
            {
                IkiliAramaAgaciDugumu current = kok; //silinecek düğüm
                IkiliAramaAgaciDugumu parent = kok; //silinecek düğümün parentı
                bool solda = true;
                //Kişi adına göre silinecek düğümün konumunu bul
                while (((Kisi)current.veri).Ad != deger)
                {
                    parent = current; 
                    if (deger[0] < ((Kisi)current.veri).Ad[0]) //silinecek kişi adının ilk harfi(ascii karşılığı) düğümdeki kişinin ilk harfinden küçükse silinecek değer ağacın solunda olduğu için silinecek düğüm sola kayar
                    {
                        solda = true;
                        current = current.sol;
                    }
                    else if (deger[0] == ((Kisi)current.veri).Ad[0]) //silinecek kişi isminin ilk harfi aktif düğümdeki kişinin ilk harfine eşitse diğer harfler kontrol edilir
                    { 
                        int i = 1;
                        while (deger[i] != null) //Farklı harfi bulana kadar döngü çalışır
                        {
                            if (deger[i] == ((Kisi)current.veri).Ad[i]) //aktif düğüm ve silinecek kişinin harfleri aynı olduğu sürece hep bir sonraki harfe geçer ve devam eder
                            {
                                i++;
                                continue;
                            }
                            else if (deger[i] < ((Kisi)current.veri).Ad[i]) //silinecek kişi adının harfinin ascii karşılığı aktif düğümdeki kişinin adının harfinden küçük ise düğüm sola kayar ve çıkar
                            {
                                current = current.sol;
                                break;
                            }
                            else //silinecek kişi adının harfinin ascii karşılığı aktif düğümden büyük ise düğüm sağa kayar ve çıkar
                            {
                                solda = false;
                                current = current.sag;
                                break;
                            }
                        }
                    }
                    else //silinecek kişi isminin ilk harfi düğümdeki kişinin ilk harfinden büyükse silinecek değer ağacın sağ tarafındadır. sağa git.
                    {
                        solda = false;
                        current = current.sag;
                    }
                    if (current == null)
                        return false;
                }
                //Current(silinecek düğüm) bulunursa ilk olarak onun eğitim ve iş bilgilerini sil.
                ((Kisi)current.veri).EgitimBilgileri = null;
                ((Kisi)current.veri).IsDeneyimleri = null;

                //Düğüm yaprak düğümse 
                if (current.sol == null && current.sag == null)
                {
                    if (current == kok)
                        kok = null;
                    else if (solda)
                        parent.sol = null;
                    else
                        parent.sag = null;
                }
                // Düğüm tek çocuklu düğüm ise
                else if (current.sag == null)
                {
                    if (current == kok)
                        kok = current.sol;
                    else if (solda)
                        parent.sol = current.sol;
                    else
                        parent.sag = current.sol;
                }
                else if (current.sol == null)
                {
                    if (current == kok)
                        kok = current.sag;
                    else if (solda)
                        parent.sol = current.sag;
                    else
                        parent.sag = current.sag;
                }
                else
                {
                    IkiliAramaAgaciDugumu successor = Successor(current);
                    if (current == kok)
                    {
                        kok = successor;
                    }
                    else if (solda)
                    {
                        parent.sol = successor;
                    }
                    else
                    {
                        parent.sag = successor;
                    }
                    successor.sol = current.sol;
                }
                return true; //silme işlemi başarıyla gerçekleştiyse true dön

            }
                

        }
        #endregion

        //referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-8-kili-arama-aac-binary-search-tree
        #region agacta Kisi arama

        public IkiliAramaAgaciDugumu KisiArama(string anahtar) //kişi arama
        {
            IkiliAramaAgaciDugumu dugum = kok;
            while (dugum != null)
            {
                if (((Kisi)dugum.veri).Ad == anahtar)
                {
                    return dugum;
                }
                else if (((Kisi)dugum.veri).Ad[0] > anahtar[0])
                {
                    dugum = dugum.sol;
                }
                else
                {
                    dugum = dugum.sag;
                }
            }
            return null;
        }
        #endregion

        #region tüm basvuru sıralama metotları
        private void tumDugumlerinBilgisi(IkiliAramaAgaciDugumu dugum) // tüm başvurular lisyeleneceğinde düğümleri buraya getirir ve bilgilerini tutar
        {
            dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }
        public void tumDugum()
        {
            dugumler = " ";
            tumDugumler(kok);

        }
        private void tumDugumler(IkiliAramaAgaciDugumu dugum) //tüm düğümleri listeler
        {
            if (dugum != null)
            {
                tumDugumlerinBilgisi(dugum);
                tumDugumler(dugum.sol);
                tumDugumler(dugum.sag);
            }
            else
            {
                return;
            }
        }
        #endregion

        #region ada göre kisi arama ve bilgilerini listeleme metotları
        private void kisiBilgisi(IkiliAramaAgaciDugumu dugum, string isim)
        {
            if (((Kisi)dugum.veri).Ad.Contains(isim))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;

            }

        }
        public void adaGoreKisiAra(string isim)
        {
            dugumler = "";
            adaGoreKisiAra(kok, isim);
        }
        private void adaGoreKisiAra(IkiliAramaAgaciDugumu dugum, string isim)
        {
            if (dugum == null)
                return;
            kisiBilgisi(dugum, isim);
            adaGoreKisiAra(dugum.sol, isim);
            adaGoreKisiAra(dugum.sag, isim);
        }

        #endregion

        #region en az lisans mezunu arama
        private void lisanMezunu(IkiliAramaAgaciDugumu dugum)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true)
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }
        public void enAzLisansMezunu()
         {
             dugumler = "";
             enAzLisansMezunuAra(kok);
         } 
        private void enAzLisansMezunuAra(IkiliAramaAgaciDugumu dugum)
         {
             if (dugum != null)
             {
                 lisanMezunu(dugum);
                 enAzLisansMezunuAra(dugum.sol);
                 enAzLisansMezunuAra(dugum.sag);
             }
             else
             {
                 return;
             }
         }
        #endregion

        #region ingilizce bilenleri filtreleme metodu
        private void DilKontrol(IkiliAramaAgaciDugumu dugum)
        {
            if (((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")||((Kisi)dugum.veri).YabancıDil.Contains("ingilizce"))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }

        }

        public void ingilizceAra()
        {
            dugumler = "";
            ingilizceArama(kok);
        }
        private void ingilizceArama(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum == null)
                return;
            DilKontrol(dugum);
            ingilizceArama(dugum.sol);
            ingilizceArama(dugum.sag); 
        }
        #endregion

        #region birden fazla dil bilenleri filtreleme metotları

        private void dilBilenler(IkiliAramaAgaciDugumu dugum)
        { 
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (sayi > 0 )
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }

        public void birdenFazlaDil()
        {
            dugumler = "";
            birdenFazlaDilArama(kok);
        }
        private void birdenFazlaDilArama(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum == null)
                return;
            dilBilenler(dugum); //dil kontrol işlemine git
            birdenFazlaDilArama(dugum.sol); //kontrol bittiyse sola git
            birdenFazlaDilArama(dugum.sag); //kontrol bittiyse sağa git
        }

        #endregion

        #region deneyimsiz kişi filtreleme metotları

        private void isDeneyimiOlmayan(IkiliAramaAgaciDugumu dugum)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.Head == null)
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        public void tecrubesizAra()
        {
            dugumler = "";
            tecrubesizKisiArama(kok);
        }
        private void tecrubesizKisiArama(IkiliAramaAgaciDugumu dugum)
        {

            if (dugum != null)
            {
                isDeneyimiOlmayan(dugum);
                tecrubesizKisiArama(dugum.sol);
                tecrubesizKisiArama(dugum.sag);
            }
        }
        #endregion

        #region girilen yasın altındakileri gösterme metotları

        private void enazYasGoster(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (((Kisi)dugum.veri).DogumTarihi> dogumYili)
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        public void enAzYas(int dogumYili)
        {
            dugumler = "";
            enAzYasArama(kok, dogumYili);
        }
        private void enAzYasArama(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum != null)
            {
                enazYasGoster(dugum, dogumYili);
                enAzYasArama(dugum.sol, dogumYili);
                enAzYasArama(dugum.sag, dogumYili);
            }
        }
        #endregion

        #region ehliyet tipine göre filtreleme metotları
        private void EhliyetTipiGoster(IkiliAramaAgaciDugumu dugum, string ehliyeTipi)
        {
           if (((Kisi)dugum.veri).Ehliyet.Contains(ehliyeTipi))
           {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
           }
        }

        public void EhliyetTipi(string ehliyeTipi)
        {
            dugumler = "";
            EhliyetTipiArama(kok, ehliyeTipi);
        }
        private void EhliyetTipiArama(IkiliAramaAgaciDugumu dugum,string ehliyeTipi)
        {
            if (dugum == null)
                return;
            EhliyetTipiGoster(dugum, ehliyeTipi);
            EhliyetTipiArama(dugum.sol, ehliyeTipi);
            EhliyetTipiArama(dugum.sag, ehliyeTipi);
        }
        #endregion

        #region girilen tecrübe süresine göre filtreleme metotları
        private void EnAzTecrubeliKisiBilgisi(IkiliAramaAgaciDugumu dugum, int sure)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true)
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        public void EnAzTecrubeli(int sure)
        {
            dugumler = "";
            EnAzTecrubeliArama(kok, sure);
        }
        private void EnAzTecrubeliArama(IkiliAramaAgaciDugumu dugum, int sure)
        {
            if (dugum != null)
            {
                EnAzTecrubeliKisiBilgisi(dugum, sure);
                EnAzTecrubeliArama(dugum.sol, sure);
                EnAzTecrubeliArama(dugum.sag, sure);
            }
        }
        #endregion

        #region ingilizce bilen ve en az lisans filtreleme metodu
        private void ingilizceveLisans(IkiliAramaAgaciDugumu dugum)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("İngilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("ingilizce")))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }

        }
        public void ingilizceveLisansFiltrele()
        {
            dugumler = "";
            ingilizceveLisansFiltre(kok);
        }
        private void ingilizceveLisansFiltre(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum == null)
                return;
            ingilizceveLisans(dugum);
            ingilizceveLisansFiltre(dugum.sol);
            ingilizceveLisansFiltre(dugum.sag); 
        }
        #endregion

        #region birden fazla dil ve en az lisan mezunu olanalrı filtreleme metotları
        private void lisansveDil(IkiliAramaAgaciDugumu dugum)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (sayi > 0 && ((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }

        public void lisansveDilFiltrele()
        {
            dugumler = "";
            lisansveDilFiltre(kok);
        }
        private void lisansveDilFiltre(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum == null)
                return;
            lisansveDil(dugum); //dil kontrol işlemine git
            lisansveDilFiltre(dugum.sol); //kontrol bittiyse sola git
            lisansveDilFiltre(dugum.sag); //kontrol bittiyse sağa git
        }

        #endregion

        #region en az lisans mezunu ve deneyimsiz filtreleme
        private void lisansveDeneyimsiz(IkiliAramaAgaciDugumu dugum)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).IsDeneyimleri.Head == null)
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }
        public void lisansveDeneyimsizFiltrele()
        {
            dugumler = "";
            lisansveDeneyimsizFiltre(kok);
        }
        private void lisansveDeneyimsizFiltre(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum != null)
            {
                lisansveDeneyimsiz(dugum);
                lisansveDeneyimsizFiltre(dugum.sol);
                lisansveDeneyimsizFiltre(dugum.sag);
            }
            else
            {
                return;
            }
        }
        #endregion

        #region en az lisans mezunu ve yas filtreleme
        private void lisanveYas(IkiliAramaAgaciDugumu dugum,int dogumYili)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).DogumTarihi> dogumYili)
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }
        public void lisanveYasFiltrele(int dogumYili)
        {
            dugumler = "";
            lisanveYasFiltre(kok, dogumYili);
        }
        private void lisanveYasFiltre(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum != null)
            {
                lisanveYas(dugum, dogumYili);
                lisanveYasFiltre(dugum.sol, dogumYili);
                lisanveYasFiltre(dugum.sag, dogumYili);
            }
            else
            {
                return;
            }
        }
        #endregion

        #region en az lisans mezunu ve ehliyet filtreleme
        private void lisansveEhliyet(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi))
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }
        public void lisansveEhliyetFiltrele(string ehliyetTipi)
        {
            dugumler = "";
            lisansveEhliyetFiltre(kok, ehliyetTipi);
        }
        private void lisansveEhliyetFiltre(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (dugum != null)
            {
                lisansveEhliyet(dugum, ehliyetTipi);
                lisansveEhliyetFiltre(dugum.sol, ehliyetTipi);
                lisansveEhliyetFiltre(dugum.sag, ehliyetTipi);
            }
            else
            {
                return;
            }
        }
        #endregion

        #region en az lisans mezunu ve deneyim filtreleme
        private void lisansveDeneyim(IkiliAramaAgaciDugumu dugum, int sure)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true)
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }
        public void lisansveDeneyimFiltrele(int sure)
        {
            dugumler = "";
            lisansveDeneyimFiltre(kok, sure);
        }
        private void lisansveDeneyimFiltre(IkiliAramaAgaciDugumu dugum, int sure)
        {
            if (dugum != null)
            {
                lisansveDeneyim(dugum, sure);
                lisansveDeneyimFiltre(dugum.sol, sure);
                lisansveDeneyimFiltre(dugum.sag, sure);
            }
            else
            {
                return;
            }
        }
        #endregion

        #region en az lisans mezunu ve ingilizce bilen filtreleme
        private void lisansveIngilizce(IkiliAramaAgaciDugumu dugum)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("İngilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("ingilizce")))
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }
        public void lisansveIngilizceFiltrele()
        {
            dugumler = "";
            lisansveIngilizceFiltre(kok);
        }
        private void lisansveIngilizceFiltre(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum != null)
            {
                lisansveIngilizce(dugum);
                lisansveIngilizceFiltre(dugum.sol);
                lisansveIngilizceFiltre(dugum.sag);
            }
            else
            {
                return;
            }
        }
        #endregion

        #region birden fazla dil bilenler ve ingilizce bilen filtreleme metotları
        private void fazlaDilveIngilizce(IkiliAramaAgaciDugumu dugum)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (sayi > 0 && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void fazlaDilveIngilizceFiltrele()
        {
            dugumler = "";
            fazlaDilveIngilizceFiltre(kok);
        }
        private void fazlaDilveIngilizceFiltre(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum == null)
                return;
            fazlaDilveIngilizce(dugum); //dil kontrol işlemine git
            fazlaDilveIngilizceFiltre(dugum.sol); //kontrol bittiyse sola git
            fazlaDilveIngilizceFiltre(dugum.sag); //kontrol bittiyse sağa git
        }

        #endregion

        #region birden fazla dil bilenler ve lisans mezunu filtreleme metotları
        private void fazlaDilveLisans(IkiliAramaAgaciDugumu dugum)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (sayi > 0 && ((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void fazlaDilveLisansFiltrele()
        {
            dugumler = "";
            fazlaDilveLisansFiltre(kok);
        }
        private void fazlaDilveLisansFiltre(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum == null)
                return;
            fazlaDilveLisans(dugum);
            fazlaDilveLisansFiltre(dugum.sol);
            fazlaDilveLisansFiltre(dugum.sag); 
        }

        #endregion

        #region birden fazla dil bilenler ve deneyimsiz filtreleme metotları
        private void fazlaDilveDeneyimsiz(IkiliAramaAgaciDugumu dugum)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (sayi > 0 && ((Kisi)dugum.veri).IsDeneyimleri.Head == null)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void fazlaDilveDeneyimsizFiltrele()
        {
            dugumler = "";
            fazlaDilveDeneyimsizFiltre(kok);
        }
        private void fazlaDilveDeneyimsizFiltre(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum == null)
                return;
            fazlaDilveDeneyimsiz(dugum);
            fazlaDilveDeneyimsizFiltre(dugum.sol);
            fazlaDilveDeneyimsizFiltre(dugum.sag); 
        }

        #endregion

        #region birden fazla dil bilenler ve yas filtreleme metotları
        private void fazlaDilveYas(IkiliAramaAgaciDugumu dugum,int dogumYili)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (sayi > 0 && ((Kisi)dugum.veri).DogumTarihi> dogumYili)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void fazlaDilveYasFiltrele(int dogumYili)
        {
            dugumler = "";
            fazlaDilveYasFiltre(kok,dogumYili);
        }
        private void fazlaDilveYasFiltre(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum == null)
                return;
            fazlaDilveYas(dugum,dogumYili);
            fazlaDilveYasFiltre(dugum.sol,dogumYili);
            fazlaDilveYasFiltre(dugum.sag,dogumYili);
        }

        #endregion

        #region birden fazla dil bilenler ve ehliyet filtreleme metotları
        private void fazlaDilveEhliyet(IkiliAramaAgaciDugumu dugum, string ehliyet)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (sayi > 0 && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void fazlaDilveEhliyetFiltrele(string ehliyet)
        {
            dugumler = "";
            fazlaDilveEhliyetFiltre(kok, ehliyet);
        }
        private void fazlaDilveEhliyetFiltre(IkiliAramaAgaciDugumu dugum, string ehliyet)
        {
            if (dugum == null)
                return;
            fazlaDilveEhliyet(dugum, ehliyet);
            fazlaDilveEhliyetFiltre(dugum.sol, ehliyet);
            fazlaDilveEhliyetFiltre(dugum.sag, ehliyet); 
        }

        #endregion

        #region birden fazla dil bilenler ve deneyim filtreleme metotları
        private void fazlaDilveDeneyim(IkiliAramaAgaciDugumu dugum, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (sayi > 0 && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void fazlaDilveDeneyimFiltrele(int sure)
        {
            dugumler = "";
            fazlaDilveDeneyimFiltre(kok, sure);
        }
        private void fazlaDilveDeneyimFiltre(IkiliAramaAgaciDugumu dugum, int sure)
        {
            if (dugum == null)
                return;
            fazlaDilveDeneyim(dugum, sure);
            fazlaDilveDeneyimFiltre(dugum.sol, sure);
            fazlaDilveDeneyimFiltre(dugum.sag, sure); 
        }

        #endregion

        #region ingilizce ve yaş filtreleme metotları
        private void ingilizceveYas(IkiliAramaAgaciDugumu dugum,int dogumYili)
        {
            if (((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).YabancıDil.Contains("ingilizce"))
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        public void ingilizceveYasFiltre(int dogumYili)
        {
            dugumler = "";
            ingilizceveYasFiltrele(kok, dogumYili);
        }
        private void ingilizceveYasFiltrele(IkiliAramaAgaciDugumu dugum,int dogumYili)
        {

            if (dugum != null)
            {
                ingilizceveYas(dugum, dogumYili);
                ingilizceveYasFiltrele(dugum.sol, dogumYili);
                ingilizceveYasFiltrele(dugum.sag, dogumYili);
            }
        }

        #endregion

        #region ingilizce bilen ve deneyimsiz 
        private void ingilizceVeDeneyimsiz(IkiliAramaAgaciDugumu dugum)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.Head == null && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")))
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        public void ingilizceVeDeneyimsizFiltre()
        {
            dugumler = "";
            ingilizceVeDeneyimsizFiltrele(kok);
        }
        private void ingilizceVeDeneyimsizFiltrele(IkiliAramaAgaciDugumu dugum)
        {

            if (dugum != null)
            {
                ingilizceVeDeneyimsiz(dugum);
                ingilizceVeDeneyimsizFiltrele(dugum.sol);
                ingilizceVeDeneyimsizFiltrele(dugum.sag);
            }
        }

        #endregion

        #region ingilizce ve ehliyet tipi filtreleme
        private void inglizceVeEhliyet(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && ((Kisi)dugum.veri).YabancıDil.Contains("ingilizce"))
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        public void inglizceVeEhliyetFiltre(string ehliyetTipi)
        {
            dugumler = "";
            inglizceVeEhliyetFiltrele(kok, ehliyetTipi);
        }
        private void inglizceVeEhliyetFiltrele(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {

            if (dugum != null)
            {
                inglizceVeEhliyet(dugum, ehliyetTipi);
                inglizceVeEhliyetFiltrele(dugum.sol, ehliyetTipi);
                inglizceVeEhliyetFiltrele(dugum.sag, ehliyetTipi);
            }
        }
        #endregion

        #region ingilizce ve deneyim filtre
        private void ingilizceveDeneyim(IkiliAramaAgaciDugumu dugum,int sure)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) && ((Kisi)dugum.veri).YabancıDil.Contains("ingilizce"))
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        public void ingilizceveDeneyimFiltre(int sure)
        {
            dugumler = "";
            ingilizceveDeneyimFiltre(kok,sure);
        }
        private void ingilizceveDeneyimFiltre(IkiliAramaAgaciDugumu dugum,int sure)
        {

            if (dugum != null)
            {
                ingilizceveDeneyim(dugum,sure);
                ingilizceveDeneyimFiltre(dugum.sol,sure);
                ingilizceveDeneyimFiltre(dugum.sag,sure);
            }
        }
        #endregion

        #region deneyimsiz ve yaş filtreleme metotları
        private void deneyimsizveYas(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.Head == null && ((Kisi)dugum.veri).DogumTarihi > dogumYili)
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        public void deneyimsizveYasFiltre(int dogumYili)
        {
            dugumler = "";
            deneyimsizveYasFiltrele(kok, dogumYili);
        }
        private void deneyimsizveYasFiltrele(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {

            if (dugum != null)
            {
                deneyimsizveYas(dugum, dogumYili);
                deneyimsizveYasFiltrele(dugum.sol, dogumYili);
                deneyimsizveYasFiltrele(dugum.sag, dogumYili);
            }
        }


        #endregion

        #region deneyimsiz ve ehliyet tipi filtre  metotları
        private void deneyimsizveEhliyet(IkiliAramaAgaciDugumu dugum, string ehliyeTipi)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.Head == null && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyeTipi))
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        public void deneyimsizveEhliyetFiltre(string ehliyeTipi)
        {
            dugumler = "";
            deneyimsizveEhliyetFiltrele(kok, ehliyeTipi);
        }
        private void deneyimsizveEhliyetFiltrele(IkiliAramaAgaciDugumu dugum, string ehliyeTipi)
        {

            if (dugum != null)
            {
                deneyimsizveEhliyet(dugum,ehliyeTipi);
                deneyimsizveEhliyetFiltrele(dugum.sol, ehliyeTipi);
                deneyimsizveEhliyetFiltrele(dugum.sag, ehliyeTipi);
            }
        }
        #endregion

        #region yaş ve ehliyet filtreleme
        private void YasveEhliyet(IkiliAramaAgaciDugumu dugum, int dogumYili,string ehliyeTipi)
        {
            if (((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyeTipi))
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        public void YasveEhliyetFiltre(int dogumYili, string ehliyeTipi)
        {
            dugumler = "";
            YasveEhliyetFiltrele(kok, dogumYili, ehliyeTipi);
        }
        private void YasveEhliyetFiltrele(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyeTipi)
        {
            if (dugum != null)
            {
                YasveEhliyet(dugum, dogumYili, ehliyeTipi);
                YasveEhliyetFiltrele(dugum.sol, dogumYili, ehliyeTipi);
                YasveEhliyetFiltrele(dugum.sag, dogumYili, ehliyeTipi);
            }
        }
        #endregion

        #region yaş ve deneyim filtreleme
        private void YasveDeneyim(IkiliAramaAgaciDugumu dugum, int dogumYili, int sure)
        {
            if (((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure))
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        public void YasveDeneyimFiltre(int dogumYili, int sure)
        {
            dugumler = "";
            YasveDeneyimFiltrele(kok, dogumYili, sure);
        }
        private void YasveDeneyimFiltrele(IkiliAramaAgaciDugumu dugum, int dogumYili, int sure)
        {
            if (dugum != null)
            {
                YasveDeneyim(dugum, dogumYili, sure);
                YasveDeneyimFiltrele(dugum.sol, dogumYili, sure);
                YasveDeneyimFiltrele(dugum.sag, dogumYili, sure);
            }
        }
        #endregion

        #region ehliyet ve deneyim filtreleme
        private void ehliyetveDeneyim(IkiliAramaAgaciDugumu dugum, string ehliyetTipi, int sure)
        {
            if (((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure))
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        public void ehliyetveDeneyimFiltre(string ehliyetTipi, int sure)
        {
            dugumler = "";
            ehliyetveDeneyimFiltrele(kok, ehliyetTipi, sure);
        }
        private void ehliyetveDeneyimFiltrele(IkiliAramaAgaciDugumu dugum, string ehliyetTipi, int sure)
        {
            if (dugum != null)
            {
                ehliyetveDeneyim(dugum, ehliyetTipi, sure);
                ehliyetveDeneyimFiltrele(dugum.sol, ehliyetTipi, sure);
                ehliyetveDeneyimFiltrele(dugum.sag, ehliyetTipi, sure);
            }
        }
        #endregion

        //3lü başlangıç
        #region ingilizce bilen, deneyimsiz ve belirli tip ehliyete göre filtreleme metotları
        private void IngilizceDeneyimsizveEhliyet(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.Head == null && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizceDeneyimsizveEhliyetFiltre(string ehliyetTipi)
        {
            dugumler = "";
            IngilizceDeneyimsizveEhliyetFiltrele(kok, ehliyetTipi);
        }
        private void IngilizceDeneyimsizveEhliyetFiltrele(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (dugum != null)
            {
                IngilizceDeneyimsizveEhliyet(dugum, ehliyetTipi);
                IngilizceDeneyimsizveEhliyetFiltrele(dugum.sol, ehliyetTipi);
                IngilizceDeneyimsizveEhliyetFiltrele(dugum.sag, ehliyetTipi);
            }
        }
        #endregion

        #region ingilizce bilen, deneyimsiz ve girilen yasın altındakileri gösterme metotları
        private void IngilizceDeneyimsizveYas(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.Head == null && ((Kisi)dugum.veri).DogumTarihi > dogumYili && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizceDeneyimsizveYasFiltre(int dogumYili)
        {
            dugumler = "";
            IngilizceDeneyimsizveYasFiltrele(kok, dogumYili);
        }
        private void IngilizceDeneyimsizveYasFiltrele(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum != null)
            {
                IngilizceDeneyimsizveYas(dugum, dogumYili);
                IngilizceDeneyimsizveYasFiltrele(dugum.sol, dogumYili);
                IngilizceDeneyimsizveYasFiltrele(dugum.sag, dogumYili);
            }
        }
        #endregion

        #region ingilizce bilen, girilen yasın altındakileri gösteren ve belirli tip ehliyete göre filtreleme metotları
        private void IngilizceYasveEhliyet(IkiliAramaAgaciDugumu dugum, string ehliyetTipi, int dogumYili)
        {
            if (((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && ((Kisi)dugum.veri).DogumTarihi > dogumYili && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizceYasveEhliyetFiltre(string ehliyetTipi, int dogumYili)
        {
            dugumler = "";
            IngilizceYasveEhliyetFiltrele(kok, ehliyetTipi, dogumYili);
        }
        private void IngilizceYasveEhliyetFiltrele(IkiliAramaAgaciDugumu dugum, string ehliyetTipi, int dogumYili)
        {
            if (dugum != null)
            {
                IngilizceYasveEhliyet(dugum, ehliyetTipi, dogumYili);
                IngilizceYasveEhliyetFiltrele(dugum.sol, ehliyetTipi, dogumYili);
                IngilizceYasveEhliyetFiltrele(dugum.sag, ehliyetTipi, dogumYili);
            }
        }
        #endregion

        #region ingilizce bilen, girilen yasın altındakileri gösteren ve girilen tecrübe süresine göre filtreleme metotları
        private void IngilizceYasveDeneyim(IkiliAramaAgaciDugumu dugum, int sure, int dogumYili)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && ((Kisi)dugum.veri).DogumTarihi > dogumYili && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizceYasveDeneyimFiltre(int sure, int dogumYili)
        {
            dugumler = "";
            IngilizceYasveDeneyimFiltrele(kok, sure, dogumYili);
        }
        private void IngilizceYasveDeneyimFiltrele(IkiliAramaAgaciDugumu dugum, int sure, int dogumYili)
        {
            if (dugum != null)
            {
                IngilizceYasveDeneyim(dugum, sure, dogumYili);
                IngilizceYasveDeneyimFiltrele(dugum.sol, sure, dogumYili);
                IngilizceYasveDeneyimFiltrele(dugum.sag, sure, dogumYili);
            }
        }
        #endregion

        #region ingilizce bilen, belirli tip ehliyet ve girilen tecrübe süresine göre filtreleme metotları
        private void IngilizceEhliyetveDeneyim(IkiliAramaAgaciDugumu dugum, int sure, string ehliyetTipi)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizceEhliyetveDeneyimFiltre(int sure, string ehliyetTipi)
        {
            dugumler = "";
            IngilizceEhliyetveDeneyimFiltrele(kok, sure, ehliyetTipi);
        }
        private void IngilizceEhliyetveDeneyimFiltrele(IkiliAramaAgaciDugumu dugum, int sure, string ehliyetTipi)
        {
            if (dugum != null)
            {
                IngilizceEhliyetveDeneyim(dugum, sure, ehliyetTipi);
                IngilizceEhliyetveDeneyimFiltrele(dugum.sol, sure, ehliyetTipi);
                IngilizceEhliyetveDeneyimFiltrele(dugum.sag, sure, ehliyetTipi);
            }
        }
        #endregion

        #region tecrübesiz, belirli tip ehliyet ve girilen yasın altındakileri gösterme metotları
        private void TecrübesizEhliyetveYas(IkiliAramaAgaciDugumu dugum, string ehliyetTipi, int dogumYili)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.Head == null && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && ((Kisi)dugum.veri).DogumTarihi > dogumYili)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void TecrübesizEhliyetveYasFiltre(string ehliyetTipi, int dogumYili)
        {
            dugumler = "";
            TecrübesizEhliyetveYasFiltrele(kok, ehliyetTipi, dogumYili);
        }
        private void TecrübesizEhliyetveYasFiltrele(IkiliAramaAgaciDugumu dugum, string ehliyetTipi, int dogumYili)
        {
            if (dugum != null)
            {
                TecrübesizEhliyetveYas(dugum, ehliyetTipi, dogumYili);
                TecrübesizEhliyetveYasFiltrele(dugum.sol, ehliyetTipi, dogumYili);
                TecrübesizEhliyetveYasFiltrele(dugum.sag, ehliyetTipi, dogumYili);
            }
        }
        #endregion

        #region girilen tecrübe süresine göre, belirli tip ehliyet ve girilen yasın altındakileri gösterme metotları
        private void TecrübeEhliyetveYasGoster(IkiliAramaAgaciDugumu dugum, int sure, string ehliyetTipi, int dogumYili)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && ((Kisi)dugum.veri).DogumTarihi > dogumYili)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void TecrübeEhliyetveYas(int sure, string ehliyetTipi, int dogumYili)
        {
            dugumler = "";
            TecrübeEhliyetveYasAra(kok, sure, ehliyetTipi, dogumYili);
        }
        private void TecrübeEhliyetveYasAra(IkiliAramaAgaciDugumu dugum, int sure, string ehliyetTipi, int dogumYili)
        {
            if (dugum != null)
            {
                TecrübeEhliyetveYasGoster(dugum, sure, ehliyetTipi, dogumYili);
                TecrübeEhliyetveYasAra(dugum.sol, sure, ehliyetTipi, dogumYili);
                TecrübeEhliyetveYasAra(dugum.sag, sure, ehliyetTipi, dogumYili);
            }
        }
        #endregion  

        #region en az lisans, ingilizce bilen ve deneyimsize göre filtreleme metotları
        private void LisansIngilizceveDeneyimsizGoster(IkiliAramaAgaciDugumu dugum)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).IsDeneyimleri.Head == null)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceveDeneyimsiz()
        {
            dugumler = "";
            LisansIngilizceveDeneyimsizAra(kok);
        }
        private void LisansIngilizceveDeneyimsizAra(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum != null)
            {
                LisansIngilizceveDeneyimsizGoster(dugum);
                LisansIngilizceveDeneyimsizAra(dugum.sol);
                LisansIngilizceveDeneyimsizAra(dugum.sag);
            }
        }

        #endregion

        #region en az lisans, ingilizce bilen ve girilen yasın altındakiler göre filtreleme metotları
        private void LisansIngilizceveYasGoster(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).DogumTarihi > dogumYili)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceveYas(int dogumYili)
        {
            dugumler = "";
            LisansIngilizceveYasAra(kok, dogumYili);
        }
        private void LisansIngilizceveYasAra(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum != null)
            {
                LisansIngilizceveYasGoster(dugum, dogumYili);
                LisansIngilizceveYasAra(dugum.sol, dogumYili);
                LisansIngilizceveYasAra(dugum.sag, dogumYili);
            }
        }

        #endregion

        #region en az lisans, ingilizce bilen ve ehliyete göre filtreleme metotları
        private void LisansIngilizceveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceveEhliyet(string ehliyetTipi)
        {
            dugumler = "";
            LisansIngilizceveEhliyetAra(kok, ehliyetTipi);
        }
        private void LisansIngilizceveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (dugum != null)
            {
                LisansIngilizceveEhliyetGoster(dugum, ehliyetTipi);
                LisansIngilizceveEhliyetAra(dugum.sol, ehliyetTipi);
                LisansIngilizceveEhliyetAra(dugum.sag, ehliyetTipi);
            }
        }

        #endregion

        #region en az lisans, ingilizce bilen ve deneyime göre filtreleme metotları
        private void LisansIngilizceveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int sure)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceveDeneyim(int sure)
        {
            dugumler = "";
            LisansIngilizceveDeneyimAra(kok, sure);
        }
        private void LisansIngilizceveDeneyimAra(IkiliAramaAgaciDugumu dugum, int sure)
        {
            if (dugum != null)
            {
                LisansIngilizceveDeneyimGoster(dugum, sure);
                LisansIngilizceveDeneyimAra(dugum.sol, sure);
                LisansIngilizceveDeneyimAra(dugum.sag, sure);
            }
        }
        #endregion

        #region en az lisans, deneyimsiz ve girilen yasın altındakileri gösterme metotları
        private void LisansDeneyimsizveYasGoster(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.Head == null && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansDeneyimsizveYas(int dogumYili)
        {
            dugumler = "";
            LisansDeneyimsizveYasAra(kok, dogumYili);
        }
        private void LisansDeneyimsizveYasAra(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum != null)
            {
                LisansDeneyimsizveYasGoster(dugum, dogumYili);
                LisansDeneyimsizveYasAra(dugum.sol, dogumYili);
                LisansDeneyimsizveYasAra(dugum.sag, dogumYili);
            }
        }
        #endregion

        #region en az lisans, deneyimsiz ve belirli tip ehliyete göre filtreleme metotları
        private void LisansDeneyimsizveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.Head == null && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && ((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansDeneyimsizveEhliyet(string ehliyetTipi)
        {
            dugumler = "";
            LisansDeneyimsizveEhliyetAra(kok, ehliyetTipi);
        }
        private void LisansDeneyimsizveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (dugum != null)
            {
                LisansDeneyimsizveEhliyetGoster(dugum, ehliyetTipi);
                LisansDeneyimsizveEhliyetAra(dugum.sol, ehliyetTipi);
                LisansDeneyimsizveEhliyetAra(dugum.sag, ehliyetTipi);
            }
        }
        #endregion

        #region en az lisans, girilen yasın altındakileri gösteren ve belirli tip ehliyete göre filtreleme metotları
        private void LisansYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyetTipi, int dogumYili)
        {
            if (((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansYasveEhliyet(string ehliyetTipi, int dogumYili)
        {
            dugumler = "";
            LisansYasveEhliyetAra(kok, ehliyetTipi, dogumYili);
        }
        private void LisansYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyetTipi, int dogumYili)
        {
            if (dugum != null)
            {
                LisansYasveEhliyetGoster(dugum, ehliyetTipi, dogumYili);
                LisansYasveEhliyetAra(dugum.sol, ehliyetTipi, dogumYili);
                LisansYasveEhliyetAra(dugum.sag, ehliyetTipi, dogumYili);
            }
        }
        #endregion

        #region en az lisans, girilen yasın altındakileri gösteren ve deneyime göre filtreleme metotları
        private void LisansYasveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int sure, int dogumYili)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansYasveDeneyim(int sure, int dogumYili)
        {
            dugumler = "";
            LisansYasveDeneyimAra(kok, sure, dogumYili);
        }
        private void LisansYasveDeneyimAra(IkiliAramaAgaciDugumu dugum, int sure, int dogumYili)
        {
            if (dugum != null)
            {
                LisansYasveDeneyimGoster(dugum, sure, dogumYili);
                LisansYasveDeneyimAra(dugum.sol, sure, dogumYili);
                LisansYasveDeneyimAra(dugum.sag, sure, dogumYili);
            }
        }
        #endregion

        #region en az lisans, belirli tip ehliyete ve deneyime göre filtreleme metotları
        private void LisansEhliyetveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int sure, string ehliyetTipi)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && ((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansEhliyetveDeneyim(int sure, string ehliyetTipi)
        {
            dugumler = "";
            LisansEhliyetveDeneyimAra(kok, sure, ehliyetTipi);
        }
        private void LisansEhliyetveDeneyimAra(IkiliAramaAgaciDugumu dugum, int sure, string ehliyetTipi)
        {
            if (dugum != null)
            {
                LisansEhliyetveDeneyimGoster(dugum, sure, ehliyetTipi);
                LisansEhliyetveDeneyimAra(dugum.sol, sure, ehliyetTipi);
                LisansEhliyetveDeneyimAra(dugum.sag, sure, ehliyetTipi);
            }
        }
        #endregion
        
        #region en az lisans, ingilizce bilen ve birden fazla dil bilene göre filtreleme metotları
        private void LisansIngilizcevefazlaDilGoster(IkiliAramaAgaciDugumu dugum)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizcevefazlaDil()
        {
            dugumler = "";
            LisansIngilizcevefazlaDilAra(kok);
        }
        private void LisansIngilizcevefazlaDilAra(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum != null)
            {
                LisansIngilizcevefazlaDilGoster(dugum);
                LisansIngilizcevefazlaDilAra(dugum.sol);
                LisansIngilizcevefazlaDilAra(dugum.sag);
            }
        }

        #endregion

        #region en az lisans, birden fazla dil bilen ve deneyimsize göre filtreleme metotları
        private void LisansfazlaDilveDeneyimsizGoster(IkiliAramaAgaciDugumu dugum)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).IsDeneyimleri.Head == null && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansfazlaDilveDeneyimsiz()
        {
            dugumler = "";
            LisansfazlaDilveDeneyimsizAra(kok);
        }
        private void LisansfazlaDilveDeneyimsizAra(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum != null)
            {
                LisansfazlaDilveDeneyimsizGoster(dugum);
                LisansfazlaDilveDeneyimsizAra(dugum.sol);
                LisansfazlaDilveDeneyimsizAra(dugum.sag);
            }
        }

        #endregion

        #region en az lisans, birden fazla dil bilen ve yaşa göre filtreleme metotları
        private void LisansfazlaDilveYasGoster(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).DogumTarihi > dogumYili && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansfazlaDilveYas(int dogumYili)
        {
            dugumler = "";
            LisansfazlaDilveYasAra(kok, dogumYili);
        }
        private void LisansfazlaDilveYasAra(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum != null)
            {
                LisansfazlaDilveYasGoster(dugum, dogumYili);
                LisansfazlaDilveYasAra(dugum.sol, dogumYili);
                LisansfazlaDilveYasAra(dugum.sag, dogumYili);
            }
        }

        #endregion

        #region en az lisans, birden fazla dil bilen ve ehliyete göre filtreleme metotları
        private void LisansfazlaDilveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansfazlaDilveEhliyet(string ehliyetTipi)
        {
            dugumler = "";
            LisansfazlaDilveEhliyetAra(kok, ehliyetTipi);
        }
        private void LisansfazlaDilveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (dugum != null)
            {
                LisansfazlaDilveEhliyetGoster(dugum, ehliyetTipi);
                LisansfazlaDilveEhliyetAra(dugum.sol, ehliyetTipi);
                LisansfazlaDilveEhliyetAra(dugum.sag, ehliyetTipi);
            }
        }
        #endregion

        #region en az lisans, birden fazla dil bilen ve deneyime göre filtreleme metotları
        private void LisansfazlaDilveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansfazlaDilveDeneyim(int sure)
        {
            dugumler = "";
            LisansfazlaDilveDeneyimAra(kok, sure);
        }
        private void LisansfazlaDilveDeneyimAra(IkiliAramaAgaciDugumu dugum, int sure)
        {
            if (dugum != null)
            {
                LisansfazlaDilveDeneyimGoster(dugum, sure);
                LisansfazlaDilveDeneyimAra(dugum.sol, sure);
                LisansfazlaDilveDeneyimAra(dugum.sag, sure);
            }
        }

        #endregion

        #region ingilizce, birden fazla dil bilen ve deneyimsize göre filtreleme metotları
        private void IngilizcefazlaDilveDeneyimsizGoster(IkiliAramaAgaciDugumu dugum)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).IsDeneyimleri.Head == null && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizcefazlaDilveDeneyimsiz()
        {
            dugumler = "";
            IngilizcefazlaDilveDeneyimsizAra(kok);
        }
        private void IngilizcefazlaDilveDeneyimsizAra(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum != null)
            {
                IngilizcefazlaDilveDeneyimsizGoster(dugum);
                IngilizcefazlaDilveDeneyimsizAra(dugum.sol);
                IngilizcefazlaDilveDeneyimsizAra(dugum.sag);
            }
        }

        #endregion

        #region ingilizce, birden fazla dil bilen ve yaşa göre filtreleme metotları
        private void IngilizcefazlaDilveYasGoster(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).DogumTarihi > dogumYili && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizcefazlaDilveYas(int dogumYili)
        {
            dugumler = "";
            IngilizcefazlaDilveYasAra(kok, dogumYili);
        }
        private void IngilizcefazlaDilveYasAra(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum != null)
            {
                IngilizcefazlaDilveYasGoster(dugum, dogumYili);
                IngilizcefazlaDilveYasAra(dugum.sol, dogumYili);
                IngilizcefazlaDilveYasAra(dugum.sag, dogumYili);
            }
        }
        #endregion

        #region ingilizce, birden fazla dil bilen ve ehliyete göre filtreleme metotları
        private void IngilizcefazlaDilveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizcefazlaDilveEhliyet(string ehliyetTipi)
        {
            dugumler = "";
            IngilizcefazlaDilveEhliyetAra(kok, ehliyetTipi);
        }
        private void IngilizcefazlaDilveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (dugum != null)
            {
                IngilizcefazlaDilveEhliyetGoster(dugum, ehliyetTipi);
                IngilizcefazlaDilveEhliyetAra(dugum.sol, ehliyetTipi);
                IngilizcefazlaDilveEhliyetAra(dugum.sag, ehliyetTipi);
            }
        }
        #endregion

        #region ingilizce, birden fazla dil bilen ve deneyime göre filtreleme metotları
        private void IngilizcefazlaDilveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizcefazlaDilveDeneyim(int sure)
        {
            dugumler = "";
            IngilizcefazlaDilveDeneyimAra(kok, sure);
        }
        private void IngilizcefazlaDilveDeneyimAra(IkiliAramaAgaciDugumu dugum, int sure)
        {
            if (dugum != null)
            {
                IngilizcefazlaDilveDeneyimGoster(dugum, sure);
                IngilizcefazlaDilveDeneyimAra(dugum.sol, sure);
                IngilizcefazlaDilveDeneyimAra(dugum.sag, sure);
            }
        }

        #endregion

        #region birden fazla dil bilen, deneyimsiz ve yaşa göre filtreleme metotları
        private void fazlaDilDeneyimsizveYasGoster(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).IsDeneyimleri.Head == null && ((Kisi)dugum.veri).DogumTarihi > dogumYili && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void fazlaDilDeneyimsizveYas(int dogumYili)
        {
            dugumler = "";
            fazlaDilDeneyimsizveYasAra(kok, dogumYili);
        }
        private void fazlaDilDeneyimsizveYasAra(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum != null)
            {
                fazlaDilDeneyimsizveYasGoster(dugum, dogumYili);
                fazlaDilDeneyimsizveYasAra(dugum.sol, dogumYili);
                fazlaDilDeneyimsizveYasAra(dugum.sag, dogumYili);
            }
        }
        #endregion

        #region birden fazla dil bilen, deneyimsiz ve ehliyete göre filtreleme metotları
        private void fazlaDilDeneyimsizveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).IsDeneyimleri.Head == null && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void fazlaDilDeneyimsizveEhliyet(string ehliyetTipi)
        {
            dugumler = "";
            fazlaDilDeneyimsizveEhliyetAra(kok, ehliyetTipi);
        }
        private void fazlaDilDeneyimsizveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (dugum != null)
            {
                fazlaDilDeneyimsizveEhliyetGoster(dugum, ehliyetTipi);
                fazlaDilDeneyimsizveEhliyetAra(dugum.sol, ehliyetTipi);
                fazlaDilDeneyimsizveEhliyetAra(dugum.sag, ehliyetTipi);
            }
        }
        #endregion  

        #region birden fazla dil bilen, yaş ve ehliyete göre filtreleme metotları
        private void fazlaDilYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyetTipi)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).DogumTarihi > dogumYili && sayi > 0 && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void fazlaDilYasveEhliyet(int dogumYili, string ehliyetTipi)
        {
            dugumler = "";
            fazlaDilYasveEhliyetAra(kok, dogumYili, ehliyetTipi);
        }
        private void fazlaDilYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyetTipi)
        {
            if (dugum != null)
            {
                fazlaDilYasveEhliyetGoster(dugum, dogumYili, ehliyetTipi);
                fazlaDilYasveEhliyetAra(dugum.sol, dogumYili, ehliyetTipi);
                fazlaDilYasveEhliyetAra(dugum.sag, dogumYili, ehliyetTipi);
            }
        }
        #endregion

        #region birden fazla dil bilen, yaş ve Deneyime göre filtreleme metotları
        private void fazlaDilYasveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).DogumTarihi > dogumYili && sayi > 0 && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void fazlaDilYasveDeneyim(int dogumYili, int sure)
        {
            dugumler = "";
            fazlaDilYasveDeneyimAra(kok, dogumYili, sure);
        }
        private void fazlaDilYasveDeneyimAra(IkiliAramaAgaciDugumu dugum, int dogumYili, int sure)
        {
            if (dugum != null)
            {
                fazlaDilYasveDeneyimGoster(dugum, dogumYili, sure);
                fazlaDilYasveDeneyimAra(dugum.sol, dogumYili, sure);
                fazlaDilYasveDeneyimAra(dugum.sag, dogumYili, sure);
            }
        }
        #endregion

        #region birden fazla dil bilen, belirli tip ehliyet ve Deneyime göre filtreleme metotları
        private void fazlaDilEhliyetveDeneyimGoster(IkiliAramaAgaciDugumu dugum, string ehliyetTipi, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && sayi > 0 && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void fazlaDilEhliyetveDeneyim(string ehliyetTipi, int sure)
        {
            dugumler = "";
            fazlaDilEhliyetveDeneyimAra(kok, ehliyetTipi, sure);
        }
        private void fazlaDilEhliyetveDeneyimAra(IkiliAramaAgaciDugumu dugum, string ehliyetTipi, int sure)
        {
            if (dugum != null)
            {
                fazlaDilEhliyetveDeneyimGoster(dugum, ehliyetTipi, sure);
                fazlaDilEhliyetveDeneyimAra(dugum.sol, ehliyetTipi, sure);
                fazlaDilEhliyetveDeneyimAra(dugum.sag, ehliyetTipi, sure);
            }
        }
        #endregion

        //3lü son
       

        //4lü
        #region ingilizce bilen, deneyimsiz, girilen yasın altındakileri gösterme ve belirli tip ehliyete göre filtreleme metotları
        private void IngilizceDeneyimsizYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyetTipi)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.Head == null && ((Kisi)dugum.veri).DogumTarihi > dogumYili && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizceDeneyimsizYasveEhliyet(int dogumYili, string ehliyetTipi)
        {
            dugumler = "";
            IngilizceDeneyimsizYasveEhliyetAra(kok, dogumYili, ehliyetTipi);
        }
        private void IngilizceDeneyimsizYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyetTipi)
        {
            if (dugum != null)
            {
                IngilizceDeneyimsizYasveEhliyetGoster(dugum, dogumYili, ehliyetTipi);
                IngilizceDeneyimsizYasveEhliyetAra(dugum.sol, dogumYili, ehliyetTipi);
                IngilizceDeneyimsizYasveEhliyetAra(dugum.sag, dogumYili, ehliyetTipi);
            }
        }
        #endregion

        #region ingilizce bilen, girilen yasın altındakileri gösteren,belirli tip ehliyet ve girilen tecrübe süresine göre filtreleme metotları
        private void IngilizceYasTecrübeveEhliyetGoster(IkiliAramaAgaciDugumu dugum, int sure, int dogumYili, string ehliyetTipi)
        {
            if (((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && ((Kisi)dugum.veri).DogumTarihi > dogumYili && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizceYasTecrübeveEhliyet(int sure, int dogumYili, string ehliyetTipi)
        {
            dugumler = "";
            IngilizceYasTecrübeveEhliyetAra(kok, sure, dogumYili, ehliyetTipi);
        }
        private void IngilizceYasTecrübeveEhliyetAra(IkiliAramaAgaciDugumu dugum, int sure, int dogumYili, string ehliyetTipi)
        {
            if (dugum != null)
            {
                IngilizceYasTecrübeveEhliyetGoster(dugum, sure, dogumYili, ehliyetTipi);
                IngilizceYasTecrübeveEhliyetAra(dugum.sol, sure, dogumYili, ehliyetTipi);
                IngilizceYasTecrübeveEhliyetAra(dugum.sag, sure, dogumYili, ehliyetTipi);
            }
        }
        #endregion

        #region en az lisans mezunu olan, ingilizce bilen, birden fazla dil bilen ve deneyimsiz kişileri filtreleme metotları
        private void lisansIngilizceDilveDeneyimsizGoster(IkiliAramaAgaciDugumu dugum)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true  && ((Kisi)dugum.veri).IsDeneyimleri.Head == null &&
                (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && sayi>0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansIngilizceDilveDeneyimsiz()
        {
            dugumler = "";
            lisansIngilizceDilveDeneyimsizAra(kok);
        }
        private void lisansIngilizceDilveDeneyimsizAra(IkiliAramaAgaciDugumu dugum)
        {
            if (dugum != null)
            {
                lisansIngilizceDilveDeneyimsizGoster(dugum);
                lisansIngilizceDilveDeneyimsizAra(dugum.sol);
                lisansIngilizceDilveDeneyimsizAra(dugum.sag);
            }
        }
        #endregion

        #region en az lisans mezunu olan, ingilizce bilen, birden fazla dil bilen ve yaşa göre filtreleme metotları
        private void lisansIngilizceDilveYasGoster(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).DogumTarihi > dogumYili &&
                (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansIngilizceDilveYas(int dogumYili)
        {
            dugumler = "";
            lisansIngilizceDilveYasAra(kok, dogumYili);
        }
        private void lisansIngilizceDilveYasAra(IkiliAramaAgaciDugumu dugum,int dogumYili)
        {
            if (dugum != null)
            {
                lisansIngilizceDilveYasGoster(dugum, dogumYili);
                lisansIngilizceDilveYasAra(dugum.sol, dogumYili);
                lisansIngilizceDilveYasAra(dugum.sag, dogumYili);
            }
        }
        #endregion

        #region en az lisans mezunu olan, ingilizce bilen, birden fazla dil bilen ve ehliyete göre filtreleme metotları
        private void lisansIngilizceDilveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) &&
                (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansIngilizceDilveEhliyet(string ehliyetTipi)
        {
            dugumler = "";
            lisansIngilizceDilveEhliyetAra(kok, ehliyetTipi);
        }
        private void lisansIngilizceDilveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyetTipi)
        {
            if (dugum != null)
            {
                lisansIngilizceDilveEhliyetGoster(dugum, ehliyetTipi);
                lisansIngilizceDilveEhliyetAra(dugum.sol, ehliyetTipi);
                lisansIngilizceDilveEhliyetAra(dugum.sag, ehliyetTipi);
            }
        }
        #endregion

        #region en az lisans mezunu olan, ingilizce bilen,birden fazla dil  ve deneyim süresi filtreleme metotları
        private void lisansIngilizceDilveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) &&
                (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansIngilizceDilveDeneyim(int sure)
        {
            dugumler = "";
            lisansIngilizceDilveDeneyimAra(kok, sure);
        }
        private void lisansIngilizceDilveDeneyimAra(IkiliAramaAgaciDugumu dugum, int sure)
        {
            if (dugum != null)
            {
                lisansIngilizceDilveDeneyimGoster(dugum, sure);
                lisansIngilizceDilveDeneyimAra(dugum.sol, sure);
                lisansIngilizceDilveDeneyimAra(dugum.sag, sure);
            }
        }
        #endregion

        #region en az lisans mezunu olan, ingilizce bilen, deneyimsiz ve yaşa göre filtreleme metotları
        private void lisansIngilizceDeneyimsizveYasAraGoster(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).DogumTarihi > dogumYili &&
                (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).IsDeneyimleri.Head == null)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansIngilizceDeneyimsizveYas(int dogumYili)
        {
            dugumler = "";
            lisansIngilizceDeneyimsizveYasAra(kok, dogumYili);
        }
        private void lisansIngilizceDeneyimsizveYasAra(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum != null)
            {
                lisansIngilizceDeneyimsizveYasAraGoster(dugum, dogumYili);
                lisansIngilizceDeneyimsizveYasAra(dugum.sol, dogumYili);
                lisansIngilizceDeneyimsizveYasAra(dugum.sag, dogumYili);
            }
        }
        #endregion

        #region en az lisans mezunu olan, ingilizce bilen,deneyimsiz  ve ehliyete göre  filtreleme metotları
        private void lisansIngilizceDeneyimsizveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyet)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) &&
                (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).IsDeneyimleri.Head == null)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansIngilizceDeneyimsizveEhliyet(string ehliyet)
        {
            dugumler = "";
            lisansIngilizceDeneyimsizveEhliyetAra(kok, ehliyet);
        }
        private void lisansIngilizceDeneyimsizveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyet)
        {
            if (dugum != null)
            {
                lisansIngilizceDeneyimsizveEhliyetGoster(dugum, ehliyet);
                lisansIngilizceDeneyimsizveEhliyetAra(dugum.sol, ehliyet);
                lisansIngilizceDeneyimsizveEhliyetAra(dugum.sag, ehliyet);
            }
        }
        #endregion

        #region en az lisans mezunu olan, ingilizce bilen, yaş ve ehliyete filtreleme metotları
        private void lisansIngilizceYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).DogumTarihi > dogumYili &&
                (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansIngilizceYasveEhliyet(int dogumYili, string ehliyet)
        {
            dugumler = "";
            lisansIngilizceYasveEhliyetAra(kok, dogumYili, ehliyet);
        }
        private void lisansIngilizceYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
            if (dugum != null)
            {
                lisansIngilizceYasveEhliyetGoster(dugum, dogumYili, ehliyet);
                lisansIngilizceYasveEhliyetAra(dugum.sol, dogumYili, ehliyet);
                lisansIngilizceYasveEhliyetAra(dugum.sag, dogumYili, ehliyet);
            }
        }
        #endregion   

        #region en az lisans mezunu olan, ingilizce bilen, yaş ve deneyim filtreleme metotları
        private void lisansIngilizceYasveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int sure, int dogumYili)
        {

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).DogumTarihi > dogumYili &&
                (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansIngilizceYasveDeneyim(int sure, int dogumYili)
        {
            dugumler = "";
            lisansIngilizceYasveDeneyimAra(kok, sure, dogumYili);
        }
        private void lisansIngilizceYasveDeneyimAra(IkiliAramaAgaciDugumu dugum, int sure, int dogumYili)
        {
            if (dugum != null)
            {
                lisansIngilizceYasveDeneyimGoster(dugum, sure, dogumYili);
                lisansIngilizceYasveDeneyimAra(dugum.sol, sure, dogumYili);
                lisansIngilizceYasveDeneyimAra(dugum.sag, sure, dogumYili);
            }
        }
        #endregion

        #region en az lisans mezunu olan, ingilizce bilen, ehliyet tipine ve deneyim süresine göre filtreleme
        private void lisansIngilizceEhliyetveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int sure, string ehliyet)
        {

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) &&
                (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansIngilizceEhliyetveDeneyim(int sure, string ehliyet)
        {
            dugumler = "";
            lisansIngilizceEhliyetveDeneyimAra(kok, sure, ehliyet);
        }
        private void lisansIngilizceEhliyetveDeneyimAra(IkiliAramaAgaciDugumu dugum, int sure, string ehliyet)
        {
            if (dugum != null)
            {
                lisansIngilizceEhliyetveDeneyimGoster(dugum, sure, ehliyet);
                lisansIngilizceEhliyetveDeneyimAra(dugum.sol, sure, ehliyet);
                lisansIngilizceEhliyetveDeneyimAra(dugum.sag, sure, ehliyet);
            }
        }

        #endregion

        #region en az lisans mezunu olan,birden fazla dil bilen,deneyimsiz ve yaşa göre  filtreleme metotları
        private void lisansDilDeneyimsizveYasGoster(IkiliAramaAgaciDugumu dugum,int dogumYili)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).IsDeneyimleri.Head == null
                && ((Kisi)dugum.veri).DogumTarihi > dogumYili && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansDilDeneyimsizveYas(int dogumYili)
        {
            dugumler = "";
            lisansDilDeneyimsizveYasAra(kok,dogumYili);
        }
        private void lisansDilDeneyimsizveYasAra(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum != null)
            {
                lisansDilDeneyimsizveYasGoster(dugum,dogumYili);
                lisansDilDeneyimsizveYasAra(dugum.sol,dogumYili);
                lisansDilDeneyimsizveYasAra(dugum.sag,dogumYili);
            }
        }
        #endregion

        #region en az lisans mezunu olan,birden fazla dil bilen,deneyimsiz ve ehliyete göre  filtreleme metotları
        private void lisansDilDeneyimsizveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyet)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).IsDeneyimleri.Head == null
                && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansDilDeneyimsizveEhliyet(string ehliyet)
        {
            dugumler = "";
            lisansDilDeneyimsizveEhliyetAra(kok, ehliyet);
        }
        private void lisansDilDeneyimsizveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyet)
        {
            if (dugum != null)
            {
                lisansDilDeneyimsizveEhliyetGoster(dugum, ehliyet);
                lisansDilDeneyimsizveEhliyetAra(dugum.sol, ehliyet);
                lisansDilDeneyimsizveEhliyetAra(dugum.sag, ehliyet);
            }
        }
        #endregion

        #region en az lisans mezunu olan,birden fazla dil bilen,yaş ve ehliyete göre  filtreleme metotları
        private void lisansDilYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, int dogumYili,string ehliyet)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet)
                && ((Kisi)dugum.veri).DogumTarihi > dogumYili && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansDilYasveEhliyet(int dogumYili,string ehliyet)
        {
            dugumler = "";
            lisansDilYasveEhliyetAra(kok, dogumYili, ehliyet);
        }
        private void lisansDilYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, int dogumYili,string ehliyet)
        {
            if (dugum != null)
            {
                lisansDilYasveEhliyetGoster(dugum, dogumYili,ehliyet);
                lisansDilYasveEhliyetAra(dugum.sol, dogumYili, ehliyet);
                lisansDilYasveEhliyetAra(dugum.sag, dogumYili, ehliyet);
            }
        }
        #endregion

        #region en az lisans mezunu olan,birden fazla dil bilen,yaş ve deneyim süresine göre  filtreleme metotları
        private void lisansDilYasveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure)==true
                && ((Kisi)dugum.veri).DogumTarihi > dogumYili && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansDilYasveDeneyim(int dogumYili, int sure)
        {
            dugumler = "";
            lisansDilYasveDeneyimAra(kok, dogumYili, sure);
        }
        private void lisansDilYasveDeneyimAra(IkiliAramaAgaciDugumu dugum, int dogumYili, int sure)
        {
            if (dugum != null)
            {
                lisansDilYasveDeneyimGoster(dugum, dogumYili, sure);
                lisansDilYasveDeneyimAra(dugum.sol, dogumYili, sure);
                lisansDilYasveDeneyimAra(dugum.sag, dogumYili, sure);
            }
        }
        #endregion

        #region en az lisans mezunu olan,birden fazla dil bilen,ehliyet ve deneyim süresine göre  filtreleme metotları
        private void lisansDilEhliyetveDeneyimGoster(IkiliAramaAgaciDugumu dugum, string ehliyet, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure)==true
                && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansDilEhliyetveDeneyim(string ehliyet, int sure)
        {
            dugumler = "";
            lisansDilEhliyetveDeneyimAra(kok, ehliyet, sure);
        }
        private void lisansDilEhliyetveDeneyimAra(IkiliAramaAgaciDugumu dugum, string ehliyet, int sure)
        {
            if (dugum != null)
            {
                lisansDilEhliyetveDeneyimGoster(dugum, ehliyet, sure);
                lisansDilEhliyetveDeneyimAra(dugum.sol, ehliyet, sure);
                lisansDilEhliyetveDeneyimAra(dugum.sag, ehliyet, sure);
            }
        }
        #endregion

        #region en az lisans mezunu olan,deneyimsiz,yaş ve ehliyete göre filtreleme metotları
        private void lisansDeneyimsizYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
   
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet)
                && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).IsDeneyimleri.Head == null)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansDeneyimsizYasveEhliyet(int dogumYili, string ehliyet)
        {
            dugumler = "";
            lisansDeneyimsizYasveEhliyetAra(kok, dogumYili, ehliyet);
        }
        private void lisansDeneyimsizYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
            if (dugum != null)
            {
                lisansDeneyimsizYasveEhliyetGoster(dugum, dogumYili, ehliyet);
                lisansDeneyimsizYasveEhliyetAra(dugum.sol, dogumYili, ehliyet);
                lisansDeneyimsizYasveEhliyetAra(dugum.sag, dogumYili, ehliyet);
            }
        }
        #endregion

        #region en az lisans mezunu olan,yaş ve ehliyete ve deneyim süresine göre filtreleme metotları
        private void lisansYasEhliyetveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet,int sure)
        {

            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet)
                && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void lisansYasEhliyetveDeneyim(int dogumYili, string ehliyet,int sure)
        {
            dugumler = "";
            lisansYasEhliyetveDeneyimAra(kok, dogumYili, ehliyet,sure);
        }
        private void lisansYasEhliyetveDeneyimAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet,int sure)
        {
            if (dugum != null)
            {
                lisansYasEhliyetveDeneyimGoster(dugum, dogumYili, ehliyet,sure);
                lisansYasEhliyetveDeneyimAra(dugum.sol, dogumYili, ehliyet,sure);
                lisansYasEhliyetveDeneyimAra(dugum.sag, dogumYili, ehliyet,sure);
            }
        }
        #endregion

        #region ingilizce,birden fazla dil,deneyimsiz ve yaşa göre filtreleme metotları
        private void ingilizceDilDeneyimsizveYasGoster(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && sayi>0
                && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).IsDeneyimleri.Head == null)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void ingilizceDilDeneyimsizveYas(int dogumYili)
        {
            dugumler = "";
            ingilizceDilDeneyimsizveYasAra(kok, dogumYili );
        }
        private void ingilizceDilDeneyimsizveYasAra(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum != null)
            {
                ingilizceDilDeneyimsizveYasGoster(dugum, dogumYili);
                ingilizceDilDeneyimsizveYasAra(dugum.sol, dogumYili);
                ingilizceDilDeneyimsizveYasAra(dugum.sag, dogumYili);
            }
        }
        #endregion

        #region ingilizce,birden fazla dil,deneyimsiz ve ehliyet göre filtreleme metotları
        private void ingilizceDilDeneyimsizveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyet)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && sayi > 0
                && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && ((Kisi)dugum.veri).IsDeneyimleri.Head == null)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void ingilizceDilDeneyimsizveEhliyet(string ehliyet)
        {
            dugumler = "";
            ingilizceDilDeneyimsizveEhliyetAra(kok, ehliyet);
        }
        private void ingilizceDilDeneyimsizveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyet)
        {
            if (dugum != null)
            {
                ingilizceDilDeneyimsizveEhliyetGoster(dugum, ehliyet);
                ingilizceDilDeneyimsizveEhliyetAra(dugum.sol, ehliyet);
                ingilizceDilDeneyimsizveEhliyetAra(dugum.sag, ehliyet);
            }
        }
        #endregion

        #region ingilizce,birden fazla dil, yaşa ve ehliyete göre filtreleme metotları
        private void ingilizceDilYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && sayi > 0
                && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet))
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void ingilizceDilYasveEhliyet(int dogumYili, string ehliyet)
        {
            dugumler = "";
            ingilizceDilYasveEhliyetAra(kok, dogumYili,ehliyet);
        }
        private void ingilizceDilYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, int dogumYili,string ehliyet)
        {
            if (dugum != null)
            {
                ingilizceDilYasveEhliyetGoster(dugum, dogumYili,ehliyet);
                ingilizceDilYasveEhliyetAra(dugum.sol, dogumYili,ehliyet);
                ingilizceDilYasveEhliyetAra(dugum.sag, dogumYili,ehliyet);
            }
        }
        #endregion

        #region ingilizce,birden fazla dil, yaşa ve deneyim süresine göre filtreleme metotları
        private void ingilizceDilYasveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && sayi > 0
                && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void ingilizceDilYasveDeneyim(int dogumYili, int sure)
        {
            dugumler = "";
            ingilizceDilYasveDeneyimAra(kok, dogumYili, sure);
        }
        private void ingilizceDilYasveDeneyimAra(IkiliAramaAgaciDugumu dugum, int dogumYili, int sure)
        {
            if (dugum != null)
            {
                ingilizceDilYasveDeneyimGoster(dugum, dogumYili, sure);
                ingilizceDilYasveDeneyimAra(dugum.sol, dogumYili, sure);
                ingilizceDilYasveDeneyimAra(dugum.sag, dogumYili, sure);
            }
        }
        #endregion

        #region ingilizce,birden fazla dil, ehliyet ve deneyim süresine göre filtreleme metotları
        private void ingilizceDilEhliyetveDeneyimGoster(IkiliAramaAgaciDugumu dugum, string ehliyet, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && sayi > 0
                && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void ingilizceDilEhliyetveDeneyim(string ehliyet, int sure)
        {
            dugumler = "";
            ingilizceDilEhliyetveDeneyimAra(kok, ehliyet, sure);
        }
        private void ingilizceDilEhliyetveDeneyimAra(IkiliAramaAgaciDugumu dugum, string ehliyet, int sure)
        {
            if (dugum != null)
            {
                ingilizceDilEhliyetveDeneyimGoster(dugum, ehliyet, sure);
                ingilizceDilEhliyetveDeneyimAra(dugum.sol, ehliyet, sure);
                ingilizceDilEhliyetveDeneyimAra(dugum.sag, ehliyet, sure);
            }
        }
        #endregion

        #region ingilizce,deneyimsiz, yaş ve ehliyet tipine göre filtreleme metotları
        private void ingilizceDeneyimsizYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyet, int dogumYili)
        {    
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).IsDeneyimleri.Head == null
                && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && ((Kisi)dugum.veri).DogumTarihi > dogumYili)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void ingilizceDeneyimsizYasveEhliyet(string ehliyet, int dogumYili)
        {
            dugumler = "";
            ingilizceDeneyimsizYasveEhliyetAra(kok, ehliyet, dogumYili);
        }
        private void ingilizceDeneyimsizYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyet, int dogumYili)
        {
            if (dugum != null)
            {
                ingilizceDeneyimsizYasveEhliyetGoster(dugum, ehliyet, dogumYili);
                ingilizceDeneyimsizYasveEhliyetAra(dugum.sol, ehliyet, dogumYili);
                ingilizceDeneyimsizYasveEhliyetAra(dugum.sag, ehliyet, dogumYili);
            }
        }
        #endregion
      
        #region ingilizce, yaş ve ehliyet ve deneyim süresine göre filtreleme metotları
        private void ingilizceYasveEhliyetDeneyimGoster(IkiliAramaAgaciDugumu dugum, string ehliyet, int dogumYili,int sure)
        {
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true
                && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && ((Kisi)dugum.veri).DogumTarihi > dogumYili)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void ingilizceYasveEhliyetDeneyim(string ehliyet, int dogumYili,int sure)
        {
            dugumler = "";
            ingilizceYasveEhliyetDeneyimAra(kok, ehliyet, dogumYili,sure);
        }
        private void ingilizceYasveEhliyetDeneyimAra(IkiliAramaAgaciDugumu dugum, string ehliyet, int dogumYili,int sure)
        {
            if (dugum != null)
            {
                ingilizceYasveEhliyetDeneyimGoster(dugum, ehliyet, dogumYili,sure);
                ingilizceYasveEhliyetDeneyimAra(dugum.sol, ehliyet, dogumYili,sure);
                ingilizceYasveEhliyetDeneyimAra(dugum.sag, ehliyet, dogumYili,sure);
            }
        }
        #endregion

        #region birden fazla dil bilen,deneyimsiz, yaş ve ehliyet tipine göre filtreleme metotları
        private void DilDeneyimsizYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyet, int dogumYili)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (sayi>0 && ((Kisi)dugum.veri).IsDeneyimleri.Head == null
                && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && ((Kisi)dugum.veri).DogumTarihi > dogumYili)
            {
               dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;            
            }
        }
        public void DilDeneyimsizYasveEhliyet(string ehliyet, int dogumYili)
        {
            dugumler = "";
            DilDeneyimsizYasveEhliyetAra(kok, ehliyet, dogumYili);
        }
        private void DilDeneyimsizYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyet, int dogumYili)
        {
            if (dugum != null)
            {
                DilDeneyimsizYasveEhliyetGoster(dugum, ehliyet, dogumYili);
                DilDeneyimsizYasveEhliyetAra(dugum.sol, ehliyet, dogumYili);
                DilDeneyimsizYasveEhliyetAra(dugum.sag, ehliyet, dogumYili);
            }
        }
        #endregion

        //4lü son

        //5li

        #region en az lisans, ingilizce bilen, birden fazla dil bilen, deneyimsiz ve girilen yasın altındakilere göre filtreleme metotları
        private void LisansIngilizceDilDeneyimsizveYasGoster(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).DogumTarihi > dogumYili && sayi > 0 && ((Kisi)dugum.veri).IsDeneyimleri.Head == null)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceDilDeneyimsizveYas(int dogumYili)
        {
            dugumler = "";
            LisansIngilizceDilDeneyimsizveYasAra(kok, dogumYili);
        }
        private void LisansIngilizceDilDeneyimsizveYasAra(IkiliAramaAgaciDugumu dugum, int dogumYili)
        {
            if (dugum != null)
            {
                LisansIngilizceDilDeneyimsizveYasGoster(dugum, dogumYili);
                LisansIngilizceDilDeneyimsizveYasAra(dugum.sol, dogumYili);
                LisansIngilizceDilDeneyimsizveYasAra(dugum.sag, dogumYili);
            }
        }

        #endregion

        #region en az lisans, ingilizce bilen, birden fazla dil bilen, deneyimsiz ve belirli tip ehliyete göre filtreleme metotları
        private void LisansIngilizceDilDeneyimsizveEhliyetGoster(IkiliAramaAgaciDugumu dugum, string ehliyet)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && sayi > 0 && ((Kisi)dugum.veri).IsDeneyimleri.Head == null)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceDilDeneyimsizveEhliyet(string ehliyet)
        {
            dugumler = "";
            LisansIngilizceDilDeneyimsizveEhliyetAra(kok, ehliyet);
        }
        private void LisansIngilizceDilDeneyimsizveEhliyetAra(IkiliAramaAgaciDugumu dugum, string ehliyet)
        {
            if (dugum != null)
            {
                LisansIngilizceDilDeneyimsizveEhliyetGoster(dugum, ehliyet);
                LisansIngilizceDilDeneyimsizveEhliyetAra(dugum.sol, ehliyet);
                LisansIngilizceDilDeneyimsizveEhliyetAra(dugum.sag, ehliyet);
            }
        }

        #endregion   

        #region en az lisans, ingilizce bilen, birden fazla dil bilen, yaş ve belirli tip ehliyete göre filtreleme metotları
        private void LisansIngilizceDilYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceDilYasveEhliyet(int dogumYili, string ehliyet)
        {
            dugumler = "";
            LisansIngilizceDilYasveEhliyetAra(kok, dogumYili, ehliyet);
        }
        private void LisansIngilizceDilYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
            if (dugum != null)
            {
                LisansIngilizceDilYasveEhliyetGoster(dugum, dogumYili, ehliyet);
                LisansIngilizceDilYasveEhliyetAra(dugum.sol, dogumYili, ehliyet);
                LisansIngilizceDilYasveEhliyetAra(dugum.sag, dogumYili, ehliyet);
            }
        }

        #endregion

        #region en az lisans, ingilizce bilen, birden fazla dil bilen, yaş ve deneyime göre filtreleme metotları
        private void LisansIngilizceDilYasveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceDilYasveDeneyim(int dogumYili, int sure)
        {
            dugumler = "";
            LisansIngilizceDilYasveDeneyimAra(kok, dogumYili, sure);
        }
        private void LisansIngilizceDilYasveDeneyimAra(IkiliAramaAgaciDugumu dugum, int dogumYili, int sure)
        {
            if (dugum != null)
            {
                LisansIngilizceDilYasveDeneyimGoster(dugum, dogumYili, sure);
                LisansIngilizceDilYasveDeneyimAra(dugum.sol, dogumYili, sure);
                LisansIngilizceDilYasveDeneyimAra(dugum.sag, dogumYili, sure);
            }
        }

        #endregion

        #region en az lisans, ingilizce bilen, birden fazla dil bilen, ehliyet ve deneyime göre filtreleme metotları
        private void LisansIngilizceDilEhliyetveDeneyimGoster(IkiliAramaAgaciDugumu dugum, string ehliyet, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceDilEhliyetveDeneyim(string ehliyet, int sure)
        {
            dugumler = "";
            LisansIngilizceDilEhliyetveDeneyimAra(kok, ehliyet, sure);
        }
        private void LisansIngilizceDilEhliyetveDeneyimAra(IkiliAramaAgaciDugumu dugum, string ehliyet, int sure)
        {
            if (dugum != null)
            {
                LisansIngilizceDilEhliyetveDeneyimGoster(dugum, ehliyet, sure);
                LisansIngilizceDilEhliyetveDeneyimAra(dugum.sol, ehliyet, sure);
                LisansIngilizceDilEhliyetveDeneyimAra(dugum.sag, ehliyet, sure);
            }
        }

        #endregion

        #region en az lisans, fazla dil bilen, deneyimsiz, girilen yasın altındakiler ve belirli tip ehliyete göre filtreleme metotları
        private void LisansDilDeneyimsizYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).IsDeneyimleri.Head == null && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansDilDeneyimsizYasveEhliyet(int dogumYili, string ehliyet)
        {
            dugumler = "";
            LisansDilDeneyimsizYasveEhliyetAra(kok, dogumYili, ehliyet);
        }
        private void LisansDilDeneyimsizYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
            if (dugum != null)
            {
                LisansDilDeneyimsizYasveEhliyetGoster(dugum, dogumYili, ehliyet);
                LisansDilDeneyimsizYasveEhliyetAra(dugum.sol, dogumYili, ehliyet);
                LisansDilDeneyimsizYasveEhliyetAra(dugum.sag, dogumYili, ehliyet);
            }
        }

        #endregion
 

        #region en az lisans, fazla dil bilen, girilen yas, ehliyet ve deneyime göre filtreleme metotları
        private void LisansDilYasEhliyetveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansDilYasEhliyetveDeneyim(int dogumYili, string ehliyet, int sure)
        {
            dugumler = "";
            LisansDilYasEhliyetveDeneyimAra(kok, dogumYili, ehliyet, sure);
        }
        private void LisansDilYasEhliyetveDeneyimAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet, int sure)
        {
            if (dugum != null)
            {
                LisansDilYasEhliyetveDeneyimGoster(dugum, dogumYili, ehliyet, sure);
                LisansDilYasEhliyetveDeneyimAra(dugum.sol, dogumYili, ehliyet, sure);
                LisansDilYasEhliyetveDeneyimAra(dugum.sag, dogumYili, ehliyet, sure);
            }
        }

        #endregion

        #region ingilize bilen, fazla dil bilen, girilen yas, ehliyet ve deneyimsize göre filtreleme metotları
        private void IngilizceDilDeneyimsizYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && ((Kisi)dugum.veri).IsDeneyimleri.Head == null && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizceDilDeneyimsizYasveEhliyet(int dogumYili, string ehliyet)
        {
            dugumler = "";
            IngilizceDilDeneyimsizYasveEhliyetAra(kok, dogumYili, ehliyet);
        }
        private void IngilizceDilDeneyimsizYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
            if (dugum != null)
            {
                IngilizceDilDeneyimsizYasveEhliyetGoster(dugum, dogumYili, ehliyet);
                IngilizceDilDeneyimsizYasveEhliyetAra(dugum.sol, dogumYili, ehliyet);
                IngilizceDilDeneyimsizYasveEhliyetAra(dugum.sag, dogumYili, ehliyet);
            }  
        }

        #endregion

        #region ingilize bilen, fazla dil bilen, girilen yas, belirli tip ehliyet ve deneyime göre filtreleme metotları
        private void IngilizceDilYasEhliyetveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && sayi > 0)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void IngilizceDilYasEhliyetveDeneyim(int dogumYili, string ehliyet, int sure)
        {
            dugumler = "";
            IngilizceDilYasEhliyetveDeneyimAra(kok, dogumYili, ehliyet, sure);
        }
        private void IngilizceDilYasEhliyetveDeneyimAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet, int sure)
        {
            if (dugum != null)
            {
                IngilizceDilYasEhliyetveDeneyimGoster(dugum, dogumYili, ehliyet, sure);
                IngilizceDilYasEhliyetveDeneyimAra(dugum.sol, dogumYili, ehliyet, sure);
                IngilizceDilYasEhliyetveDeneyimAra(dugum.sag, dogumYili, ehliyet, sure);
            }
        }

        #endregion

        #region en az lisans, ingilizce bilen, deneyimsiz, girilen yasın altındakiler ve belirli tip ehliyete göre filtreleme metotları
        private void LisansIngilizceDeneyimsizYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyetTipi)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyetTipi) && ((Kisi)dugum.veri).IsDeneyimleri.Head == null)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceDeneyimsizYasveEhliyet(int dogumYili, string ehliyetTipi)
        {
            dugumler = "";
            LisansIngilizceDeneyimsizYasveEhliyetAra(kok, dogumYili, ehliyetTipi);
        }
        private void LisansIngilizceDeneyimsizYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyetTipi)
        {
            if (dugum != null)
            {
                LisansIngilizceDeneyimsizYasveEhliyetGoster(dugum, dogumYili, ehliyetTipi);
                LisansIngilizceDeneyimsizYasveEhliyetAra(dugum.sol, dogumYili, ehliyetTipi);
                LisansIngilizceDeneyimsizYasveEhliyetAra(dugum.sag, dogumYili, ehliyetTipi);
            }
        }

        #endregion    

        #region en az lisans, ingilizce bilen, girilen yasın altındakiler, belirli tip ehliyet ve deneyime göre filtreleme metotları
        private void LisansIngilizceYasEhliyetveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet, int sure)
        {
            if (((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true && (((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "----------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceYasEhliyetveDeneyim(int dogumYili, string ehliyet, int sure)
        {
            dugumler = "";
            LisansIngilizceYasEhliyetveDeneyimAra(kok, dogumYili, ehliyet, sure);
        }
        private void LisansIngilizceYasEhliyetveDeneyimAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet, int sure)
        {
            if (dugum != null)
            {
                LisansIngilizceYasEhliyetveDeneyimGoster(dugum, dogumYili, ehliyet, sure);
                LisansIngilizceYasEhliyetveDeneyimAra(dugum.sol, dogumYili, ehliyet, sure);
                LisansIngilizceYasEhliyetveDeneyimAra(dugum.sag, dogumYili, ehliyet, sure);
            }
        }

        #endregion

        //5li son

        //6lı
        #region en az lisans, ingilizce bilen, fazla dil bilen, deneyimsiz, girilen yasın altındakiler ve belirli tip ehliyete göre filtreleme metotları
        private void LisansIngilizceDilDeneyimsizYasveEhliyetGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && sayi > 0 && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && ((Kisi)dugum.veri).IsDeneyimleri.Head == null && ((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceDilDeneyimsizYasveEhliyet(int dogumYili, string ehliyet)
        {
            dugumler = "";
            LisansIngilizceDilDeneyimsizYasveEhliyetAra(kok, dogumYili, ehliyet);
        }
        private void LisansIngilizceDilDeneyimsizYasveEhliyetAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet)
        {
            if (dugum != null)
            {
                LisansIngilizceDilDeneyimsizYasveEhliyetGoster(dugum, dogumYili, ehliyet);
                LisansIngilizceDilDeneyimsizYasveEhliyetAra(dugum.sol, dogumYili, ehliyet);
                LisansIngilizceDilDeneyimsizYasveEhliyetAra(dugum.sag, dogumYili, ehliyet);
            }
        }

        #endregion

        #region en az lisans, ingilizce bilen, fazla dil bilen, girilen yas, belirli tip ehliyet ve deneyime göre filtreleme metotları
        private void LisansIngilizceDilYasEhliyetveDeneyimGoster(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet, int sure)
        {
            int sayi = ((Kisi)dugum.veri).YabancıDil.Length - ((Kisi)dugum.veri).YabancıDil.Replace(",", "").Length;
            if ((((Kisi)dugum.veri).YabancıDil.Contains("ingilizce") || ((Kisi)dugum.veri).YabancıDil.Contains("İngilizce")) && sayi > 0 && ((Kisi)dugum.veri).Ehliyet.Contains(ehliyet) && ((Kisi)dugum.veri).IsDeneyimleri.EnAzTecrubeli(sure) == true && ((Kisi)dugum.veri).DogumTarihi > dogumYili && ((Kisi)dugum.veri).EgitimBilgileri.enAzLisansMezunu() == true)
            {
                dugumler += "Ad Soyad: " + ((Kisi)dugum.veri).Ad + Environment.NewLine + "Doğum Tarihi: " + ((Kisi)dugum.veri).DogumTarihi + Environment.NewLine + "Telefon: " + ((Kisi)dugum.veri).Telefon + Environment.NewLine + "Eposta: " + ((Kisi)dugum.veri).Eposta + Environment.NewLine + "Yabancı Dil: " + ((Kisi)dugum.veri).YabancıDil + Environment.NewLine + "Ehliyet: " + ((Kisi)dugum.veri).Ehliyet + Environment.NewLine + "Adres: " + ((Kisi)dugum.veri).Adres + Environment.NewLine + ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiGoster() + ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiGoster() + "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
            }
        }
        public void LisansIngilizceDilYasEhliyetveDeneyim(int dogumYili, string ehliyet, int sure)
        {
            dugumler = "";
            LisansIngilizceDilYasEhliyetveDeneyimAra(kok, dogumYili, ehliyet, sure);
        }
        private void LisansIngilizceDilYasEhliyetveDeneyimAra(IkiliAramaAgaciDugumu dugum, int dogumYili, string ehliyet, int sure)
        {
            if (dugum != null)
            {
                LisansIngilizceDilYasEhliyetveDeneyimGoster(dugum, dogumYili, ehliyet, sure);
                LisansIngilizceDilYasEhliyetveDeneyimAra(dugum.sol, dogumYili, ehliyet, sure);
                LisansIngilizceDilYasEhliyetveDeneyimAra(dugum.sag, dogumYili, ehliyet, sure);
            }
        }

        #endregion

        //6lı son

        // referans  https://www.slideshare.net/DenizKILIN/yzm-2116-blm-7-tree-ve-binary-tree-kili-aa
        #region preorder-inorder-postorder sıralama metotları
  
        private void Gezinti(IkiliAramaAgaciDugumu dugum) //her gezintide düğümleri buraya getirir ve bir arada tutar
        {
            dugumler += ((Kisi)dugum.veri).Ad + Environment.NewLine;
        }
        public void PreOrder()
        {
            dugumler = " ";
            PreOrderGezinme(kok);
        }
        // referans  https://www.slideshare.net/DenizKILIN/yzm-2116-blm-7-tree-ve-binary-tree-kili-aa
        private void PreOrderGezinme(IkiliAramaAgaciDugumu dugum) //kök-sol-sağ
        {
            if (dugum != null)
            {
                Gezinti(dugum);
                PreOrderGezinme(dugum.sol);
                PreOrderGezinme(dugum.sag);
            }
            else
            {
                return;
            }
        }
        // referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-7-tree-ve-binary-tree-kili-aa
        public void InOrder()
        {
            dugumler = " ";
            InOrderGezinme(kok);
        }
        // referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-7-tree-ve-binary-tree-kili-aa
        private void InOrderGezinme(IkiliAramaAgaciDugumu dugum) // sol-kök-sağ
        {
            if (dugum == null)
                return;
            InOrderGezinme(dugum.sol);
            Gezinti(dugum);
            InOrderGezinme(dugum.sag);
        }
        // referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-7-tree-ve-binary-tree-kili-aa
        public void PostOrder()
        {
            dugumler = " ";
            PostOrderGezinme(kok);
        }
        // referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-7-tree-ve-binary-tree-kili-aa
        private void PostOrderGezinme(IkiliAramaAgaciDugumu dugum) //sol-sağ-kök
        {
            if (dugum != null)
            {
                PostOrderGezinme(dugum.sol);
                PostOrderGezinme(dugum.sag);
                Gezinti(dugum);
            }
        }

        #endregion

        #region derinlik bulma

        public int DerinlikBul()
        {
            return DerinlikBulma(kok);
        }
        private int DerinlikBulma(IkiliAramaAgaciDugumu dugum) //ağacın derinliğini bulma
        {
            if (dugum == null)
                return 0;
            else
            {
                int solYukseklik = 0, sagYukseklik = 0;
                solYukseklik = DerinlikBulma(dugum.sol); //Düğümün solu null olana kadar sol yüksekliği hesaplar
                sagYukseklik = DerinlikBulma(dugum.sag); //Düğümün sağı null olana kadar sağ yüksekliği hesaplar
                if (solYukseklik > sagYukseklik)
                    return solYukseklik + 1;
                else
                    return sagYukseklik + 1;
            }
        }
        #endregion

        // referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-7-tree-ve-binary-tree-kili-aa
        #region eleman sayısı metotları

        public int Eleman()
        {
            return ElemanSayisi(kok);
        }
        public int ElemanSayisi(IkiliAramaAgaciDugumu dugum) // eleman sayısı bulma metodu
        {
            if (dugum == null)//Düğümün içi boşsa 
            {
                return 0;
            }
            else
            {
                return ElemanSayisi(dugum.sag) + ElemanSayisi(dugum.sol) + 1;
            }
        }
        #endregion
    }
}
