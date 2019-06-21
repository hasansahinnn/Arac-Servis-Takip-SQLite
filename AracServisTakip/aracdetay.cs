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
    public partial class aracdetay : Form
    {
        public SQLiteConnection cn2 = new SQLiteConnection("Data Source=ServisTakip.s3db;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public int id,id2=0;public Arac a2;public Form1 frm1;public int kisiid;
        public aracdetay()
        {
            InitializeComponent();
        }
        void listele()
        {
            try
            {
                cn2.Close();
                cn2.Open();
                using (SQLiteCommand kmt = new SQLiteCommand("Select Marka,Model,Renk,Plaka,kisiid from Arac where id="+id+"", cn2))
                {
                    using (SQLiteDataReader dr = kmt.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            textBox7.Text = dr["Marka"].ToString();
                            textBox1.Text = dr["Model"].ToString();
                            textBox4.Text = dr["Renk"].ToString();
                            textBox6.Text = dr["Plaka"].ToString();
                            kisiid = Convert.ToInt16(dr["kisiid"]);
                        }
                    }
                }

                cn2.Close();
            }
            catch (Exception)
            {
            }
        }
        private void aracdetay_Load(object sender, EventArgs e)
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
                    using (SQLiteCommand kmt2 = new SQLiteCommand("update Arac set Marka='" + textBox7.Text + "',Model='" + textBox1.Text + "',Renk='" + textBox4.Text + "',Plaka='" + textBox6.Text + "' where id=" + id + " ", cn2))
                    {
                        kmt2.ExecuteNonQuery();
                        MessageBox.Show("Güncelleme Başarılı!");
                          a2.button7.PerformClick();
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cn2.Close();
                cn2.Open();
                DialogResult dialogResult = MessageBox.Show("Araç Silmek İstediğinize Eminmisiniz?", "Servis Sil?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SQLiteCommand kmt = new SQLiteCommand("delete from Arac where id=" + id + " ", cn2);
                    kmt.ExecuteNonQuery();
                    DialogResult dialogResult2 = MessageBox.Show("Kişiyide Silmek İstermisiniz Eminmisiniz????", "Kişi Sil????", MessageBoxButtons.YesNo);
                    if (dialogResult2 == DialogResult.Yes)
                    {
                        cn2.Close();
                        cn2.Open();
                        SQLiteCommand kmt2 = new SQLiteCommand("delete from Kullanici where id=" + kisiid + " ", cn2);
                        kmt2.ExecuteNonQuery(); cn2.Close();
                        cn2.Open();
                        SQLiteCommand kmt3 = new SQLiteCommand("delete from Servis where Kisiid=" + kisiid + " ", cn2);
                        kmt3.ExecuteNonQuery(); cn2.Close();
                    }
                    else if (dialogResult2 == DialogResult.No)
                    {

                    }
                    cn2.Close();
                    cn2.Open();
                    SQLiteCommand kmt23 = new SQLiteCommand("update Servis set Aracid=0 where Aracid=" + id + " ", cn2);
                    kmt23.ExecuteNonQuery(); cn2.Close();
                    MessageBox.Show("Silme Başarılı!");
                    if (id2 == 1)
                    {
                        frm1.button7.PerformClick();
                    }
                    else
                    {
                        a2.button7.PerformClick();
                    }
                    this.Close();
                    cn2.Close();
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
