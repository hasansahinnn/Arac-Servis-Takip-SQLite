using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace AracServisTakip
{
    public partial class detay : Form
    {
        public SQLiteConnection cn = new SQLiteConnection("Data Source=ServisTakip.s3db;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public SQLiteConnection cn2 = new SQLiteConnection("Data Source=ServisTakip.s3db;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public SQLiteDataAdapter da;
        public DataTable dt = new DataTable(); public int i, id; public Form1 frm1;
        public detay()
        {
            InitializeComponent();
        }
        void listele()
        {
            try
            {
                 cn.Close();
                Adlistele();
                Plakalistele();



                cn.Open();
                using (SQLiteCommand kmt = new SQLiteCommand("Select s.Tarih,a.Plaka,k.Ad,k.Tel,a.Marka,a.Model,a.Renk,s.Km,s.Yapilacaklar,s.Bakim,s.ServisNot from Servis s join Arac a on s.Aracid=a.id join Kullanici k on k.id=s.Kisiid where s.id=" + id + " ", cn))
                {
                    using (SQLiteDataReader dr = kmt.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            textBox7.Text = dr["Tarih"].ToString();
                            comboBox2.Text = dr["Plaka"].ToString();
                            comboBox1.Text = dr["Ad"].ToString();
                            textBox2.Text = dr["Km"].ToString();
                            textBox4.Text = dr["Yapilacaklar"].ToString();
                            textBox1.Text = dr["Bakim"].ToString();
                            textBox3.Text = dr["ServisNot"].ToString();
                        }
                    }
                }


              

                cn.Close();
            }
            catch (Exception)
            {
            }
        }
        void Adlistele()
        {
            try
            {
                AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
                cn.Close();
                comboBox1.Items.Clear();
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select distinct(Ad) from Kullanici order by Ad asc ", cn);
                SQLiteDataReader dr = kmt.ExecuteReader();
                while (dr.Read())
                {
                    collection.Add(dr["Ad"].ToString());
                    comboBox1.Items.Add(dr["Ad"]);
                }
                comboBox1.AutoCompleteCustomSource = collection;
                comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                cn.Close();
            }
            catch (Exception)
            {

            }

        }
        void Plakalistele()
        {
            try
            {
                AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
                cn.Close();
                comboBox2.Items.Clear();
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select distinct(Plaka) from Arac ", cn);
                SQLiteDataReader dr = kmt.ExecuteReader();
                while (dr.Read())
                {
                    collection.Add(dr["Plaka"].ToString());
                    comboBox2.Items.Add(dr["Plaka"]);
                }
                comboBox2.AutoCompleteCustomSource = collection;
                comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                cn.Close();
            }
            catch (Exception)
            {

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                cn2.Close();
                cn2.Open();
                DialogResult dialogResult = MessageBox.Show("Güncelleme Yapmak İstediğinize Eminmisiniz?", "Güncelleme!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SQLiteCommand kmt2 = new SQLiteCommand("update Servis set Tarih='" + textBox7.Text + "',Km='" + textBox2.Text + "',Yapilacaklar='" + textBox4.Text + "',Bakim='" + textBox1.Text + "',ServisNot='" + textBox3.Text + "',kisiid=(Select id from Kullanici where Ad='"+comboBox1.Text+"'),aracid=(Select id from Arac where Plaka='"+comboBox2.Text+"') where id=" + id + " ", cn2))
                    {
                        kmt2.ExecuteNonQuery();
                        MessageBox.Show("Güncelleme Başarılı!");
                    }
                    frm1.button7.PerformClick(); this.Close();
                }
                else if (dialogResult == DialogResult.No)
                {

                }
                cn2.Close();
            }
            catch (Exception)
            { 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Close();
                cn.Open();
                DialogResult dialogResult = MessageBox.Show("Servisi Silmek İstediğinize Eminmisiniz?", "Servis Sil?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SQLiteCommand kmt = new SQLiteCommand("delete from Servis where id=" + id + " ", cn);
                    kmt.ExecuteNonQuery();
                    MessageBox.Show("Silme Başarılı!");
                    frm1.button7.PerformClick(); this.Close();
                }
                else if (dialogResult == DialogResult.No)
                {

                }
                cn.Close();
            }
            catch (Exception)
            {

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBox2.Items.Clear();
                cn2.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select Plaka from Arac where kisiid=(Select id from Kullanici where Ad='" + comboBox1.Text + "') ", cn2);
                SQLiteDataReader dr = kmt.ExecuteReader();
                while (dr.Read())
                {
                    comboBox2.Items.Add(dr["Plaka"]);
                }
                cn2.Close();

            }
            catch (Exception)
            {

            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBox1.Items.Clear();
                cn2.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select Ad from Kullanici where id=(Select kisiid from Arac where Plaka='" + comboBox2.Text + "') ", cn2);
                SQLiteDataReader dr = kmt.ExecuteReader();
                while (dr.Read())
                {
                    comboBox1.Text = dr["Ad"].ToString();
                }
                cn2.Close();
            }
            catch (Exception)
            {

            }
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                comboBox2.Items.Clear();
                cn.Close();
                comboBox2.Text = "";
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select Plaka from Arac where kisiid=(Select id from Kullanici where Ad like'" + comboBox1.Text + "%') ", cn);
                SQLiteDataReader dr = kmt.ExecuteReader();
                while (dr.Read())
                {
                    comboBox2.Items.Add(dr["Plaka"]);
                }
                cn.Close();
            }
            catch (Exception)
            {

            }
        }

        private void detay_Load(object sender, EventArgs e)
        {
            listele();
        }
    }
}
