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
    public partial class Musteridetay : Form
    {
        public SQLiteConnection cn2 = new SQLiteConnection("Data Source=ServisTakip.s3db;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public int id,id2=0;
        public Musteri m2;
        public Form1 frm1;
        public Musteridetay()
        {
            InitializeComponent();
        }
        void listele()
        {
            try
            {
                cn2.Close();
                cn2.Open();
                using (SQLiteCommand kmt = new SQLiteCommand("Select Ad,Tel,KullaniciNot from Kullanici where id=" + id + "", cn2))
                {
                    using (SQLiteDataReader dr = kmt.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            textBox1.Text = dr["Ad"].ToString();
                            textBox2.Text = dr["Tel"].ToString();
                            textBox3.Text = dr["KullaniciNot"].ToString();
                        }
                    }
                }

                cn2.Close();
            }
            catch (Exception)
            {
            }
        }
        private void Musteridetay_Load(object sender, EventArgs e)
        {
            listele();

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
                    using (SQLiteCommand kmt2 = new SQLiteCommand("update Kullanici set Ad='" + textBox1.Text + "',Tel='" + textBox2.Text + "',KullaniciNot='" + textBox3.Text + "' where id=" + id + " ", cn2))
                    {
                        kmt2.ExecuteNonQuery();
                        MessageBox.Show("Güncelleme Başarılı!");
                    }
                    this.Close();
                }
                else if (dialogResult == DialogResult.No)
                {

                }
                cn2.Close();
                m2.button7.PerformClick();
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cn2.Close();
                cn2.Open();
                DialogResult dialogResult = MessageBox.Show("Kişi ve Bütün Araçlarını Silmek İstediğinize Eminmisiniz?", "Kişi Sil?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SQLiteCommand kmt = new SQLiteCommand("delete from Kullanici where id=" + id + " ", cn2);
                    kmt.ExecuteNonQuery();cn2.Close();
                    cn2.Open();
                    SQLiteCommand kmt2 = new SQLiteCommand("delete from Arac where Kisiid=" + id + " ", cn2);
                    kmt2.ExecuteNonQuery(); cn2.Close();
                    cn2.Open();
                    SQLiteCommand kmt3 = new SQLiteCommand("delete from Servis where Kisiid=" + id + " ", cn2);
                    kmt3.ExecuteNonQuery(); cn2.Close();
                    MessageBox.Show("Silme Başarılı!");
                    if(id2==1)
                    {
                        frm1.button7.PerformClick();
                    }
                    else
                    {
                        m2.button7.PerformClick();
                    }
                    this.Close();
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
    }
}
