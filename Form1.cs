using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace yazgel
{
    public partial class Form1 : Form
    {
        // https://www.youtube.com/watch?v=VAuli64QnxI
        //bağlı listelerin arayüzü için referans https://github.com/selmasaltik/Veri-yapilari-proje
        //referans https://github.com/VefaAKSOY/InsanKaynaklariBilgiSistemi

        IkiliAramaAgaci ikiliAramaAgaci = new IkiliAramaAgaci();
        Kisi kisi = new Kisi();
        IkiliAramaAgaciDugumu dugum = new IkiliAramaAgaciDugumu();
        EgitimBilgileri egitimBilgileri = new EgitimBilgileri();
        LinkedListEgitim linkedlistEgitim = new LinkedListEgitim();
        IsDeneyimi isDeneyimi = new IsDeneyimi();
        LinkedListIsDeneyimi linkedListIsDeneyimi = new LinkedListIsDeneyimi();
        public Form1()
        {
            InitializeComponent();
        } 
        private void okulEkleBtn_Click(object sender, EventArgs e)
        {
            if (okulAdiTxt.Text == "")
                MessageBox.Show("Eğitim bilgilerini girin.");
            else
            {
                //yeni egitim bilgileri dolduruldu
                egitimBilgileri.Turu = okulTuruTxt.Text;
                egitimBilgileri.OkulAdi = okulAdiTxt.Text;
                egitimBilgileri.Bolumu = bolumuTxt.Text;
                egitimBilgileri.BaslangicTarihi = baslangicTarihiTxt.Text;
                egitimBilgileri.BitisTarihi = bitisTarihiTxt.Text;
                egitimBilgileri.NotOrtalamasi = notOrtalamasiTxt.Text;

                //doldurulan eğitim bilgileri kaydedildi
                linkedlistEgitim.egitimBilgisiEkle(egitimBilgileri);
                okulAdiList.Items.Add(egitimBilgileri.OkulAdi);
                MessageBox.Show("Eğitim bilgileri eklendi.");
                okulTuruTxt.Text = okulAdiTxt.Text = bolumuTxt.Text = baslangicTarihiTxt.Text = bitisTarihiTxt.Text = notOrtalamasiTxt.Text = "";

                egitimBilgileri = new EgitimBilgileri();

            }
        }
        private void isEkleBtn_Click(object sender, EventArgs e)
        {
            if (isAdiTxt.Text == "")
                MessageBox.Show("Deneyim bilgilerini girin.");
            else
            {
                //iş bilgileri textboxlardan bağlı listeye alındı
                isDeneyimi.YerAdi = isAdiTxt.Text;
                isDeneyimi.Pozisyon = pozisyonTxt.Text;
                isDeneyimi.TecrubeSuresi = Convert.ToDouble(isTecrubesiTxt.Text);
                isDeneyimi.IsAdresi = isAdresiTxt.Text;

                // iş bilgileri kaydedildi
                linkedListIsDeneyimi.isDeneyimiEkle(isDeneyimi);
                isYeriList.Items.Add(isDeneyimi.YerAdi);
                MessageBox.Show("İşyeri bilgileri eklendi.");
                isAdiTxt.Text = pozisyonTxt.Text = isTecrubesiTxt.Text = isAdresiTxt.Text = "";
                isDeneyimi = new IsDeneyimi();
            }
        }
        private  void kaydetBtn_Click(object sender, EventArgs e)
        {


            if (adTxt.Text == "")
                MessageBox.Show("Kişi bilgilerini girin.");
            else
            {

                //Textboxdan alınan kişi bilgileri kisiye eklendi
                kisi = new Kisi();
                kisi.Ad = adTxt.Text;
                kisi.DogumTarihi = Convert.ToInt32(dTarihiTxt.Text);
                kisi.Telefon = telefonTxt.Text;
                kisi.Eposta = postaTxt.Text;
                kisi.YabancıDil = yabanciDilTxt.Text;
                kisi.Ehliyet = ehliyetTxt.Text;
                kisi.Adres = adresTxt.Text;

                //eğitim bağlı listesi kişi ile birleştirildi
                kisi.EgitimBilgileri = linkedlistEgitim;
                //iş deneyimi bağlı listesi kişi ile birleştirildi
                kisi.IsDeneyimleri = linkedListIsDeneyimi;


                ikiliAramaAgaci.KisiEkle(kisi); //doldurulan kişi bilgileri kişi ağacına eklendi
                // referans https://www.kemalkefeli.com.tr/csharp-dosya-islemleri.html
                string file = "basvurular.txt";
                StreamWriter sw = File.AppendText(file);
                sw.WriteLine("\n" + kisi.Ad + "," + kisi.DogumTarihi + "," + kisi.Telefon + "," + kisi.Eposta + "," + kisi.YabancıDil + "," + kisi.Ehliyet + "," + kisi.Adres + "/" + kisi.EgitimBilgileri.egitimBilgisiYazdir() + "/" + kisi.IsDeneyimleri.isDeneyimiYazdir());
                sw.Close();

                MessageBox.Show("Başvurunuz oluşturuldu");

                adTxt.Text = dTarihiTxt.Text = adresTxt.Text = telefonTxt.Text = postaTxt.Text = yabanciDilTxt.Text = ehliyetTxt.Text = "";
                isYeriList.Items.Clear();
                okulAdiList.Items.Clear();

                //bir sonra ki kişi için eğitim ve iş deneyimi listeleri baştan oluşturuldu
                linkedlistEgitim = new LinkedListEgitim();
                linkedListIsDeneyimi = new LinkedListIsDeneyimi();

            }
        }
        private void aramaBtn_Click(object sender, EventArgs e)
        {
            if (adSoyadTxt.Text == "")
            {
                MessageBox.Show("Arama yapmak için önce aranacak kişi ismini girin.");
            }
            else
            {
                egitimList.Items.Clear();
                isDeneyimiList.Items.Clear();
                dugum = ikiliAramaAgaci.KisiArama(adSoyadTxt.Text);
                if (dugum == null)
                {
                    MessageBox.Show("Aradığınız kişi adını tam giriniz");
                    adSoyadTxt.Text = "";
                }
                else
                {
                    kisi = ((Kisi)dugum.veri);
                    adSoyadTxt2.Text = kisi.Ad;
                    dTarihiTxt2.Text = kisi.DogumTarihi.ToString();
                    telefonTxt2.Text = kisi.Telefon;
                    postaTxt2.Text = kisi.Eposta;
                    yabanciDilTxt2.Text = kisi.YabancıDil;
                    ehliyetTxt2.Text = kisi.Ehliyet;
                    adresTxt2.Text = kisi.Adres;
                    //Kişinin kayıtlı eğitim bilgileri eğitim bilgisi listesi null olana kadar listelendi
                    Node egitimBilgileri = new Node();
                    egitimBilgileri = kisi.EgitimBilgileri.Head;
                    while (egitimBilgileri != null)
                    {
                        egitimList.Items.Add(((EgitimBilgileri)egitimBilgileri.Veri).OkulAdi.ToString());
                        egitimBilgileri = egitimBilgileri.adres;
                    }
                    //Kişinin kayıtlı deneyim bilgileri deneyim bilgisi listesi null olana kadar listelendi
                    Node deneyimBilgileri = new Node();
                    deneyimBilgileri = kisi.IsDeneyimleri.Head;
                    while (deneyimBilgileri != null)
                    {
                        isDeneyimiList.Items.Add(((IsDeneyimi)deneyimBilgileri.Veri).YerAdi.ToString());
                        deneyimBilgileri = deneyimBilgileri.adres;
                    }


                }
            }
        }
        private void guncelleBtn_Click(object sender, EventArgs e)
        {
            if (adSoyadTxt.Text == "")
                MessageBox.Show("Güncellenecek kişi ismini tam giriniz");
            else
            {
                //Güncel kişi bilgileri(Guncelleme işlemindeki textboxlardan) kişi ağacından bulunan kişiye gönderildi
                kisi = new Kisi();
                kisi = ((Kisi)dugum.veri);
                kisi.Ad = adSoyadTxt2.Text;
                kisi.DogumTarihi = Convert.ToInt32(dTarihiTxt2.Text);
                kisi.Telefon = telefonTxt2.Text;
                kisi.Eposta = postaTxt2.Text;
                kisi.YabancıDil = yabanciDilTxt2.Text;
                kisi.Ehliyet = ehliyetTxt2.Text;
                kisi.Adres = adresTxt2.Text;

                MessageBox.Show("Güncelleme işlemi başarılı.");

                //Güncelleme işlemi tamamlandıktan sonra yeni güncelleme işlemine hazırlamak için textboxlar ve listboxlar temizlendi
                adSoyadTxt.Text =adSoyadTxt2.Text = adresTxt2.Text = telefonTxt2.Text = postaTxt2.Text = yabanciDilTxt2.Text = ehliyetTxt2.Text = "";

                egitimList.Items.Clear();
                isDeneyimiList.Items.Clear();
                kisi = new Kisi(); // yeni güncelleme işlemi için kişi bilgisi yeniden oluşturuldu
            }
        }
        private void silBtn_Click(object sender, EventArgs e)
        {
            if (adSoyadTxt.Text == "")
                MessageBox.Show("Silmek istediğiniz kişinin adını giriniz.");
            else
            {
                //Kişi ağacında aranmış ve bulunmuş olan kişi, kişi ağacından silindi.
                bool sil = ikiliAramaAgaci.KisiSil(kisi.Ad);
                if (sil)
                {
                    MessageBox.Show("Silme işlemi başarılı.");
                    adSoyadTxt.Text = adSoyadTxt2.Text = dTarihiTxt2.Text = adresTxt2.Text = telefonTxt2.Text = postaTxt2.Text = yabanciDilTxt2.Text = ehliyetTxt2.Text = "";
                    egitimList.Items.Clear();
                    isDeneyimiList.Items.Clear();
                }
                else
                    MessageBox.Show("Kişi bulunamadı.");
            }
        }
        private void yeniEgitimEkleBtn_Click(object sender, EventArgs e)
        {
            //Sistemde kayıtlı kişiye yeni eğitim bilgisi eklemek
            if (adSoyadTxt.Text == "" || ikiliAramaAgaci.KisiArama(adSoyadTxt.Text) == null) //ilk olarak kişiyi bul
                MessageBox.Show("Güncelleme kısmından eğitim bilgisi eklemek için önce güncellenecek kişiyi bulun.");
            else
            {
                if (okulAdiTxt2.Text == "")
                    MessageBox.Show("Eğitim bilgilerini girin.");
                else
                {
                    //yeni eğitim bilgileri dolduruldu
                    egitimBilgileri.Turu = okulTuruTxt2.Text;
                    egitimBilgileri.OkulAdi = okulAdiTxt2.Text;
                    egitimBilgileri.Bolumu = bolumuTxt2.Text;
                    egitimBilgileri.BaslangicTarihi = baslangicTarihiTxt2.Text;
                    egitimBilgileri.BitisTarihi = bitisTarihiTxt2.Text;
                    egitimBilgileri.NotOrtalamasi = notOrtalamasiTxt2.Text;

                    //doldurulan eğitim bilgileri bulunan kişiye eklendi
                    ((Kisi)dugum.veri).EgitimBilgileri.egitimBilgisiEkle(egitimBilgileri);
                    MessageBox.Show("Eğitim bilgileri eklendi.");
                    okulTuruTxt2.Text= okulAdiTxt2.Text = bolumuTxt2.Text = baslangicTarihiTxt2.Text = bitisTarihiTxt2.Text = notOrtalamasiTxt2.Text = "";
                    egitimBilgileri = new EgitimBilgileri(); //yeni eğitim bilgisi ekleme için eğitim bilgisi yeniden oluşturuldu
                    egitimList.Items.Add(okulAdiTxt2.Text);

                }
            }
        }
        private void egitimGuncelleBtn_Click(object sender, EventArgs e)
        {

            if (egitimList.SelectedItem == null)
                MessageBox.Show("Önce güncellenecek eğitim bilgisini seçin.");
            else
            {
                //listbox'da şeçilmiş eğitim bilgisi kişinin eğitim bilgileri arasından(linked list) bulundu
                Node egitimBilgisi = new Node();
                egitimBilgisi = ((Kisi)dugum.veri).EgitimBilgileri.Head;

                while (true)
                {
                    if (((EgitimBilgileri)egitimBilgisi.Veri).OkulAdi == egitimList.SelectedItem.ToString())
                    {
                        //bulunan eğitim bilgisine ilgili textboxlardaki bilgiler gönderildi ve güncelleme gerçekleştirildi.
                        ((EgitimBilgileri)egitimBilgisi.Veri).Turu = okulTuruTxt2.Text;
                        ((EgitimBilgileri)egitimBilgisi.Veri).OkulAdi = okulAdiTxt2.Text;
                        ((EgitimBilgileri)egitimBilgisi.Veri).Bolumu = bolumuTxt2.Text;
                        ((EgitimBilgileri)egitimBilgisi.Veri).BaslangicTarihi = baslangicTarihiTxt2.Text;
                        ((EgitimBilgileri)egitimBilgisi.Veri).BitisTarihi = bitisTarihiTxt2.Text;
                        ((EgitimBilgileri)egitimBilgisi.Veri).NotOrtalamasi = notOrtalamasiTxt2.Text;
                        MessageBox.Show("Eğitim bilgisi güncellendi.");
                        break;
                    }
                    else//Eğitim bilgisi bulunamazsa diğer düğümlerde ara
                        egitimBilgisi = egitimBilgisi.adres;
                }
            }
        }
        private void yeniIsEkleBtn_Click(object sender, EventArgs e)
        {
            //Sistemde kayıtlı kişiye kayıt işleminde eklediği deneyim bilgileri dışında yeni deneyim bilgisi eklemek istersek
            if (adSoyadTxt.Text == "" || ikiliAramaAgaci.KisiArama(adSoyadTxt.Text) == null)
                MessageBox.Show("Güncelleme kısmından eğitim bilgisi eklemek için önce güncellenecek kişiyi bulun.");
            else
            {
                if (isAdiTxt2.Text == "")
                    MessageBox.Show("İşyeri bilgilerini girin.");
                else
                {
                    //yeni deneyim bilgileri dolduruldu
                    isDeneyimi.YerAdi = isAdiTxt2.Text;
                    isDeneyimi.Pozisyon = pozisyonTxt2.Text;
                    isDeneyimi.TecrubeSuresi = Convert.ToDouble(isTecrubesiTxt2.Text);
                    isDeneyimi.IsAdresi = isAdresiTxt2.Text;
                    //doldurulan deneyim bilgileri bulunan kişiye eklendi
                    ((Kisi)dugum.veri).IsDeneyimleri.isDeneyimiEkle(isDeneyimi);
                    MessageBox.Show("İşyeri bilgileri eklendi.");
                    isAdiTxt2.Text = pozisyonTxt2.Text = isTecrubesiTxt2.Text = isAdresiTxt2.Text = "";
                    isDeneyimiList.Items.Add(isDeneyimi);
                    isDeneyimi = new IsDeneyimi(); //yeni deneyim bilgisi ekleme için işDeneyimi yeniden oluşturuldu
                }
            }
        }
        private void IsGuncelleBtn_Click(object sender, EventArgs e)
        {
            if (isDeneyimiList.SelectedItem == null)
                MessageBox.Show("Önce güncellenecek geçmiş iş bilgisini seçin.");
            else
            {
                //listbox'da şeçilmiş deneyim bilgisi kişinin deneyim bilgileri arasından(linked list) bulundu
                Node isDeneyimi = new Node();
                isDeneyimi = ((Kisi)dugum.veri).IsDeneyimleri.Head;

                while (true)
                {
                    //bulunan deneyim bilgisine ilgili textboxlardaki bilgiler gönderildi ve güncelleme gerçekleştirildi.
                    if (((IsDeneyimi)isDeneyimi.Veri).YerAdi == isDeneyimiList.SelectedItem.ToString())
                    {
                        ((IsDeneyimi)isDeneyimi.Veri).YerAdi = isAdiTxt2.Text;
                        ((IsDeneyimi)isDeneyimi.Veri).Pozisyon = pozisyonTxt2.Text;
                        ((IsDeneyimi)isDeneyimi.Veri).TecrubeSuresi = Convert.ToDouble(isTecrubesiTxt2.Text);
                        ((IsDeneyimi)isDeneyimi.Veri).IsAdresi = isAdresiTxt2.Text;
                        MessageBox.Show("İş bilgisi güncellendi.");
                        break;
                    }
                    else//Deneyim bilgisi bulunamazsa diğer düğümlerde ara
                        isDeneyimi = isDeneyimi.adres;
                }
            }
        }
        private void PreorderBtn_Click(object sender, EventArgs e)
        {
            ikiliAramaAgaci.PreOrder();
            preorderList.Text = ikiliAramaAgaci.DugumleriYazdir();
        }
        private void InorderBtn_Click(object sender, EventArgs e)
        {
            ikiliAramaAgaci.InOrder();
            inorderList.Text = ikiliAramaAgaci.DugumleriYazdir();
        }
        private void PostorderBtn_Click(object sender, EventArgs e)
        {
            ikiliAramaAgaci.PostOrder();
            postorderList.Text = ikiliAramaAgaci.DugumleriYazdir();
        }
        private void derinlikBulBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ağacın derinliği : " + ikiliAramaAgaci.DerinlikBul().ToString());
        }
        private void elemanSayisiBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Eleman Sayısı : " + ikiliAramaAgaci.Eleman().ToString());
        }
        private void egitimList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (egitimList.SelectedItem == null)
                MessageBox.Show("Önce güncellenecek eğitim bilgisini seçin.");
            else
            {
                Node egitim = new Node();
                egitim = ((Kisi)dugum.veri).EgitimBilgileri.Head;

                //Listbox'da seçilmiş olan eğitim bilgisi okul adına göre bulundu
                while (((EgitimBilgileri)egitim.Veri).OkulAdi != egitimList.SelectedItem.ToString())
                    egitim = egitim.adres;
                //bulunan eğitim bilgileri listelendi
                okulTuruTxt2.Text = ((EgitimBilgileri)egitim.Veri).Turu;
                okulAdiTxt2.Text = ((EgitimBilgileri)egitim.Veri).OkulAdi;
                bolumuTxt2.Text = ((EgitimBilgileri)egitim.Veri).Bolumu;
                baslangicTarihiTxt2.Text = ((EgitimBilgileri)egitim.Veri).BaslangicTarihi.ToString();
                bitisTarihiTxt2.Text = ((EgitimBilgileri)egitim.Veri).BitisTarihi.ToString();
                notOrtalamasiTxt2.Text = ((EgitimBilgileri)egitim.Veri).NotOrtalamasi.ToString();
            }
        }
        private void isDeneyimiList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Listbox'dan seçilmiş olan iş bilgisi güncelleme ekranında textboxlarda gösterildi
            if (isDeneyimiList.SelectedItem == null)
                MessageBox.Show("Önce güncellenecek geçmiş iş bilgisini seçin.");
            else
            {
                Node isDeneyimi = new Node();
                isDeneyimi = ((Kisi)dugum.veri).IsDeneyimleri.Head;

                //Listbox'da seçilmiş olan iş bilgisi iş adına göre bulundu
                while (((IsDeneyimi)isDeneyimi.Veri).YerAdi != isDeneyimiList.SelectedItem.ToString())
                    isDeneyimi = isDeneyimi.adres;

                //bulunan iş bilgileri listelendi
                isAdiTxt2.Text = ((IsDeneyimi)isDeneyimi.Veri).YerAdi;
                pozisyonTxt2.Text = ((IsDeneyimi)isDeneyimi.Veri).Pozisyon;
                isTecrubesiTxt2.Text = ((IsDeneyimi)isDeneyimi.Veri).TecrubeSuresi.ToString();
                isAdresiTxt2.Text = ((IsDeneyimi)isDeneyimi.Veri).IsAdresi;
            }
        }
        private void basvuruAraBtn_Click(object sender, EventArgs e)
        {
            if (basvuruAraTxt.Text == "")
            {
                MessageBox.Show("Arama yapmak için önce aranacak kişi ismini girin.");
            }
            else
            {
                string isim;
                isim = basvuruAraTxt.Text;
                ikiliAramaAgaci.adaGoreKisiAra(isim);
                arananBasvuruListele.Text = ikiliAramaAgaci.DugumleriYazdir();

            }
        }
        private void tumBasvuruListele_Click(object sender, EventArgs e)
        {
            ikiliAramaAgaci.tumDugum();
            tumBasvuruList.Text = ikiliAramaAgaci.DugumleriYazdir();

        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            filtreListe.Text = "";
            if (LisansChb.Checked == true)
            {
                filtreListe.Text = "";
                ikiliAramaAgaci.enAzLisansMezunu();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true)
            {
                filtreListe.Text = "";
                ikiliAramaAgaci.ingilizceAra();
                filtreListe.Text =ikiliAramaAgaci.DugumleriYazdir();
            }
            if (FazlaDilChb.Checked == true)
            {
                filtreListe.Text = "";
                ikiliAramaAgaci.birdenFazlaDil();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (deneyimsizChb.Checked == true)
            {
                filtreListe.Text = "";
                ikiliAramaAgaci.tecrubesizAra();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (yasChb.Checked == true)
            {
                filtreListe.Text = "";
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.enAzYas(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (ehliyetChb.Checked == true)
            {
                filtreListe.Text = "";
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.EhliyetTipi(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (deneyimChb.Checked == true)
            {
                filtreListe.Text = "";
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.EnAzTecrubeli(sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true)
            {
                ikiliAramaAgaci.lisansveIngilizceFiltrele();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && FazlaDilChb.Checked == true)
            {
                filtreListe.Text = "";
                ikiliAramaAgaci.fazlaDilveLisansFiltrele();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();

            }
            if (LisansChb.Checked == true && deneyimsizChb.Checked == true)
            {
                filtreListe.Text = "";
                ikiliAramaAgaci.lisansveDeneyimsizFiltrele();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && yasChb.Checked == true)
            {
                filtreListe.Text = "";
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.lisanveYasFiltrele(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && ehliyetChb.Checked == true)
            {
                filtreListe.Text = "";
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.lisansveEhliyetFiltrele(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && deneyimChb.Checked == true)
            {
                filtreListe.Text = "";
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.lisansveDeneyimFiltrele(sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && FazlaDilChb.Checked == true)
            {
                filtreListe.Text = "";
                ikiliAramaAgaci.fazlaDilveIngilizceFiltrele();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && deneyimsizChb.Checked == true)
            {
                filtreListe.Text = "";
                ikiliAramaAgaci.ingilizceVeDeneyimsizFiltre();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && yasChb.Checked == true)
            {
                filtreListe.Text = "";
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.ingilizceveYasFiltre(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();

            }
            if (IngilzceChb.Checked == true && ehliyetChb.Checked == true)
            {
                filtreListe.Text = "";
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.inglizceVeEhliyetFiltre(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();

            }
            if (IngilzceChb.Checked == true && deneyimChb.Checked == true)
            {
                filtreListe.Text = "";
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.ingilizceveDeneyimFiltre(sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (FazlaDilChb.Checked == true && deneyimsizChb.Checked == true)
            {
                ikiliAramaAgaci.fazlaDilveDeneyimsizFiltrele();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (FazlaDilChb.Checked == true && yasChb.Checked == true)
            {
                filtreListe.Text = "";
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.fazlaDilveYasFiltrele(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (FazlaDilChb.Checked == true && ehliyetChb.Checked == true)
            {
                filtreListe.Text = "";
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.fazlaDilveEhliyetFiltrele(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (FazlaDilChb.Checked == true && deneyimChb.Checked == true)
            {
                filtreListe.Text = "";
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.fazlaDilveDeneyimFiltrele(sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (deneyimsizChb.Checked == true && yasChb.Checked == true)
            {
                filtreListe.Text = "";
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.deneyimsizveYasFiltre(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (deneyimsizChb.Checked == true && ehliyetChb.Checked == true)
            {
                filtreListe.Text = "";
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.deneyimsizveEhliyetFiltre(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                filtreListe.Text = "";
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.YasveEhliyetFiltre(dogumYili, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (yasChb.Checked == true && deneyimChb.Checked == true)
            {
                filtreListe.Text = "";
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.YasveDeneyimFiltre(dogumYili, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (ehliyetChb.Checked==true && deneyimChb.Checked == true)
            {
                filtreListe.Text = "";
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.ehliyetveDeneyimFiltre(ehliyetTipi, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && deneyimsizChb.Checked == true)
            {
                ikiliAramaAgaci.LisansIngilizceveDeneyimsiz();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && yasChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.LisansIngilizceveYas(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.LisansIngilizceveEhliyet(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && deneyimChb.Checked == true)
            {
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.LisansIngilizceveDeneyim(sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.LisansDeneyimsizveYas(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && deneyimsizChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.LisansDeneyimsizveEhliyet(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }      
            if (LisansChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.LisansYasveEhliyet(ehliyetTipi, dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && yasChb.Checked == true && deneyimChb.Checked == true)
            {
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.LisansYasveDeneyim(sure, dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.LisansEhliyetveDeneyim(sure, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.IngilizceDeneyimsizveYasFiltre(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && deneyimsizChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.IngilizceDeneyimsizveEhliyetFiltre(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.IngilizceYasveEhliyetFiltre(ehliyetTipi, dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && yasChb.Checked == true && deneyimChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.IngilizceYasveDeneyimFiltre(sure, dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.IngilizceEhliyetveDeneyimFiltre(sure, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (deneyimsizChb.Checked == true && ehliyetChb.Checked == true && yasChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.TecrübesizEhliyetveYasFiltre(ehliyetTipi, dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (deneyimChb.Checked == true && ehliyetChb.Checked == true && yasChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.TecrübeEhliyetveYas(sure, ehliyetTipi, dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.IngilizceDeneyimsizYasveEhliyet(dogumYili, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && yasChb.Checked == true && deneyimChb.Checked == true && ehliyetChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.IngilizceYasTecrübeveEhliyet(sure, dogumYili, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && FazlaDilChb.Checked == true && deneyimsizChb.Checked == true)
            {     
                ikiliAramaAgaci.lisansIngilizceDilveDeneyimsiz();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && FazlaDilChb.Checked == true && yasChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.lisansIngilizceDilveYas(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && FazlaDilChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.lisansIngilizceDilveEhliyet(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && FazlaDilChb.Checked == true && deneyimChb.Checked == true)
            {
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.lisansIngilizceDilveDeneyim(sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }

            if (LisansChb.Checked == true && IngilzceChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true)
            {
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.lisansIngilizceDilveDeneyim(sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && deneyimsizChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.lisansIngilizceDeneyimsizveEhliyet(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.lisansIngilizceYasveEhliyet(dogumYili, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && yasChb.Checked == true && deneyimChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.lisansIngilizceYasveDeneyim(sure, dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.lisansIngilizceEhliyetveDeneyim(sure, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && FazlaDilChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.lisansDilDeneyimsizveYas(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && FazlaDilChb.Checked == true && deneyimsizChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.lisansDilDeneyimsizveEhliyet(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && FazlaDilChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.lisansDilYasveEhliyet(dogumYili, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && FazlaDilChb.Checked == true && yasChb.Checked == true && deneyimChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.lisansDilYasveDeneyim(dogumYili, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && FazlaDilChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.lisansDilEhliyetveDeneyim(ehliyetTipi, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.lisansDeneyimsizYasveEhliyet(dogumYili, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.lisansYasEhliyetveDeneyim(dogumYili, ehliyetTipi,sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && FazlaDilChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.ingilizceDilDeneyimsizveYas(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && FazlaDilChb.Checked == true && deneyimsizChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.ingilizceDilDeneyimsizveEhliyet(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && FazlaDilChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.ingilizceDilYasveEhliyet(dogumYili,ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && FazlaDilChb.Checked == true && yasChb.Checked == true && deneyimChb.Checked == true)
            {
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.ingilizceDilYasveDeneyim(dogumYili,sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && FazlaDilChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.ingilizceDilEhliyetveDeneyim(ehliyetTipi, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.ingilizceDeneyimsizYasveEhliyet(ehliyetTipi, dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }      
            if (IngilzceChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.ingilizceYasveEhliyetDeneyim(ehliyetTipi,dogumYili, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (FazlaDilChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
               
                ikiliAramaAgaci.DilDeneyimsizYasveEhliyet(ehliyetTipi, dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.LisansIngilizceDeneyimsizYasveEhliyet(dogumYili, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.LisansIngilizceYasEhliyetveDeneyim(dogumYili, ehliyetTipi, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && FazlaDilChb.Checked == true)
            {
                ikiliAramaAgaci.LisansIngilizcevefazlaDil();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && FazlaDilChb.Checked == true && deneyimsizChb.Checked == true)
            {
                ikiliAramaAgaci.LisansfazlaDilveDeneyimsiz();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && FazlaDilChb.Checked == true && yasChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.LisansfazlaDilveYas(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && FazlaDilChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.LisansfazlaDilveEhliyet(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && FazlaDilChb.Checked == true && deneyimChb.Checked == true)
            {
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.LisansfazlaDilveDeneyim(sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && FazlaDilChb.Checked == true && deneyimsizChb.Checked == true)
            {
                ikiliAramaAgaci.IngilizcefazlaDilveDeneyimsiz();
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && FazlaDilChb.Checked == true && yasChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.IngilizcefazlaDilveYas(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && FazlaDilChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.IngilizcefazlaDilveEhliyet(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && FazlaDilChb.Checked == true && deneyimChb.Checked == true)
            {
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.IngilizcefazlaDilveDeneyim(sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (FazlaDilChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.fazlaDilDeneyimsizveYas(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (FazlaDilChb.Checked == true && deneyimsizChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.fazlaDilDeneyimsizveEhliyet(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }

            if (FazlaDilChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.fazlaDilYasveEhliyet(dogumYili, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (FazlaDilChb.Checked == true && yasChb.Checked == true && deneyimChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.fazlaDilYasveDeneyim(dogumYili, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (FazlaDilChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.fazlaDilEhliyetveDeneyim(ehliyetTipi, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }

            if (LisansChb.Checked == true && IngilzceChb.Checked == true && FazlaDilChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                ikiliAramaAgaci.LisansIngilizceDilDeneyimsizveYas(dogumYili);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && FazlaDilChb.Checked == true && deneyimsizChb.Checked == true && ehliyetChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.LisansIngilizceDilDeneyimsizveEhliyet(ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }

            if (LisansChb.Checked == true && IngilzceChb.Checked == true && FazlaDilChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.LisansIngilizceDilYasveEhliyet(dogumYili, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && FazlaDilChb.Checked == true && yasChb.Checked == true && deneyimChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.LisansIngilizceDilYasveDeneyim(dogumYili, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && FazlaDilChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.LisansIngilizceDilEhliyetveDeneyim(ehliyetTipi, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && FazlaDilChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.LisansDilDeneyimsizYasveEhliyet(dogumYili, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && FazlaDilChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.LisansDilYasEhliyetveDeneyim(dogumYili, ehliyetTipi, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && FazlaDilChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.IngilizceDilDeneyimsizYasveEhliyet(dogumYili, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (IngilzceChb.Checked == true && FazlaDilChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.IngilizceDilYasEhliyetveDeneyim(dogumYili, ehliyetTipi, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && FazlaDilChb.Checked == true && deneyimsizChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                ikiliAramaAgaci.LisansIngilizceDilDeneyimsizYasveEhliyet(dogumYili, ehliyetTipi);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }
         
            if (LisansChb.Checked == true && IngilzceChb.Checked == true && FazlaDilChb.Checked == true && yasChb.Checked == true && ehliyetChb.Checked == true && deneyimChb.Checked == true)
            {
                int dogumYili;
                DateTime dt = DateTime.Today;
                int yil = dt.Year;
                dogumYili = yil - Convert.ToInt32(yasTxt.Text);
                string ehliyetTipi = ehliyetTipiTxt.Text;
                int sure = Convert.ToInt32(deneyimSuresiTxt.Text);
                ikiliAramaAgaci.LisansIngilizceDilYasEhliyetveDeneyim(dogumYili, ehliyetTipi, sure);
                filtreListe.Text = ikiliAramaAgaci.DugumleriYazdir();
            }

        }
        private void yazdirBtn_Click_1(object sender, EventArgs e)
        { 
            // referans https://www.kemalkefeli.com.tr/csharp-dosya-islemleri.html
            FileStream fw;
            StreamWriter sw;
            fw = new FileStream("filtresonucu.txt", FileMode.OpenOrCreate, FileAccess.Write);
            sw = new StreamWriter(fw);
            sw.WriteLine(filtreListe.Text);
            sw.Close();
            fw.Close();
        }
      
        private void okulTuruTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
               && !char.IsSeparator(e.KeyChar);
        }
        // referans https://forum.turkishcode.com/konu-c-textbox-a-sadece-harf-sayi-ozel-karakter-girme.html
        private void adSoyadTxt2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }
        // referans https://forum.turkishcode.com/konu-c-textbox-a-sadece-harf-sayi-ozel-karakter-girme.html
        private void isTecrubesiTxt2_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        private void okulTuruTxt2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }
        private void adTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }
        // referans https://forum.turkishcode.com/konu-c-textbox-a-sadece-harf-sayi-ozel-karakter-girme.html
        private void isTecrubesiTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        private void yabanciDilTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar) && e.KeyChar != ',';
        }

        private void yabanciDilTxt2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar) && e.KeyChar != ',';
        }

        private void dTarihiTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) ;
        }
        private void dTarihiTxt2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
