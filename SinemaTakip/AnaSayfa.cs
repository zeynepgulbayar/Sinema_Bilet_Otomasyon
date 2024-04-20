using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SinemaTakip
{
    public partial class AnaSayfa : Form
    {
        public AnaSayfa()
        {
            InitializeComponent();
        }
        SqlConnection baglanti=new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Sinema_bileti;Integrated Security=True");
        private void btnSalonEkle_Click(object sender, EventArgs e)
        {
           SalonEkle salonEkleform = new SalonEkle();
           
            salonEkleform.Show();
            this.Hide();
        }

        private void btnFilmEkle_Click(object sender, EventArgs e)
        {
            FilmEkle filmEkleForm = new FilmEkle();
            filmEkleForm.Show();           
            this.Hide();

        }

        private void btnSeansListele_Click(object sender, EventArgs e)
        {
            SeansListe SeansListeform = new SeansListe();
           
            SeansListeform.Show();  
            this.Hide();
        }
        private void btnSeansEkle_Click(object sender, EventArgs e)
        {
            SeansEkle FormSeansEkle = new SeansEkle();
            FormSeansEkle.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FilmveSalonGetir(ComboBox combo, string sql1, string sql2)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand(sql1, baglanti);
            SqlDataReader read=komut.ExecuteReader();
            while (read.Read()) { 
                combo.Items.Add(read[sql2].ToString());
            }
            baglanti.Close();
        }
        private void FilmAfisiGoster()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from Film_Bilgileri where FilmAdi='"+comboFilmAdi.SelectedItem+"'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read()) {

                pictureBox1.ImageLocation = read["resim"].ToString();
            }
            baglanti.Close();
        }
        int sayac = 0;
        private void Combo_Dolu_Koltuklar()
        {
            comboKoltukIptal.Items.Clear();
            comboKoltukIptal.Text = "";
            foreach (Control item in panel1.Controls)
            {
                if (item is Button)
                {
                    if (item.BackColor==Color.Red)
                    {
                        comboKoltukIptal.Items.Add(item.Text);
                    }
                }
            }
        }
        private void YenidenRenklendir()
        {
            foreach (Control item in panel1.Controls)
            {
                if (item is Button)
                {
                    item.BackColor = Color.White;
                }
            }
        }
        private void VeriTabani_Dolu_Koltuklar()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from Satis_Bilgileri where FilmAdi='"+comboFilmAdi.SelectedItem+"' and SalonAdi='"+comboSalonAdi.Text+"' and tarih='"+comboFilmTarihi.Text+"' and saat='"+comboFilmSeansı.SelectedItem+"'",baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read()) {

                foreach (Control item in panel1.Controls)
                {
                    if (item is Button)
                    {
                        if (read["koltukno"].ToString()==item.Text)
                        {
                        item.BackColor = Color.Red;

                        }
                    }
                }
            } baglanti.Close();
        }
        private void AnaSayfa_Load(object sender, EventArgs e)
        {
            BosKoltuklar();
            FilmveSalonGetir(comboFilmAdi,"select *from Film_Bilgileri","FilmAdi" );
            FilmveSalonGetir(comboSalonAdi, "select *from Salon_Bilgileri", "SalonAdi");

        }

        private void BosKoltuklar()
        {
            sayac = 1;
            for (int i = 0; i < 8; i++)
            {
                for (global::System.Int32 j = 0; j < 9; j++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(30, 30);
                    btn.BackColor = Color.White;
                    btn.Location = new Point(j * 30 + 20, i * 30 + 30);
                    btn.Name = sayac.ToString();
                    btn.Text = sayac.ToString();
                    if (j == 4)
                    {
                        continue;
                    }
                    sayac++;
                    this.panel1.Controls.Add(btn);
                    btn.Click += Btn_Click;
                }

            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button b=(Button)sender;
            if (b.BackColor == Color.White)
            {
                txtKoltukNo.Text = b.Text;
            }
        }

        private void btnSatis_Click(object sender, EventArgs e)
        {
            SatisListe SatisListeForm= new SatisListe();
            SatisListeForm.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void AnaSayfa_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void comboFilmAdi_SelectedIndexChanged(object sender, EventArgs e)
        {

            FilmAfisiGoster();
            YenidenRenklendir();
            Combo_Dolu_Koltuklar();
        }
        SinemaTableAdapters.SatisBilgileriTableAdapter satis=new SinemaTableAdapters.SatisBilgileriTableAdapter();
        private void btnBiletIptal_Click(object sender, EventArgs e)
        {

        }

        private void btnBiletSat_Click(object sender, EventArgs e)
        {
            if (txtKoltukNo.Text!="")
            try
            {
                satis.Satis_Yap(txtKoltukNo.Text, comboSalonAdi.Text, comboFilmAdi.Text, comboFilmTarihi.Text, comboFilmSeansı.Text, txtAd.Text, txtSoyad.Text, comboUcret.Text, DateTime.Now.ToShortDateString());
            }
            catch (Exception hata)
            {
                    MessageBox.Show("Hata Oluştu!!!" + hata.Message, "Uyarı");
            }
            else
            {
                MessageBox.Show("Koltuk Seçimi Yapmadınız.", "Uyarı");

            }
        }
        private void Film_Tarihi_Getir()
        {
            comboFilmTarihi.Text = "";
            comboFilmSeansı.Text = "";
            comboFilmTarihi.Items.Clear();
            comboFilmSeansı.Items.Clear();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from Seans_Bilgileri where FilmAdi='" + comboFilmAdi.SelectedItem + "' and SalonAdi='" + comboSalonAdi.SelectedItem + "'", baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                comboFilmTarihi.Items.Add(reader["tarih"].ToString());

            } baglanti.Close();
        }
        private void comboSalonAdi_SelectedIndexChanged(object sender, EventArgs e)
        {
            Film_Tarihi_Getir();
        }
    }
}
