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
    public partial class Servis : Form
    {
        public SQLiteConnection cn2 = new SQLiteConnection("Data Source=ServisTakip.s3db;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public SQLiteConnection cn = new SQLiteConnection("Data Source=ServisTakip.s3db;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public SQLiteDataAdapter da;
        public DataTable dt = new DataTable(); public int i, id;
        public string combo,combo2;public Form1 frm11;
        public Servis()
        {
            InitializeComponent();
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
                AutoCompleteStringCollection collection2 = new AutoCompleteStringCollection();
                cn.Close();
                comboBox3.Items.Clear();
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select Plaka from Arac order by Plaka asc ", cn);
                SQLiteDataReader dr = kmt.ExecuteReader();
                while (dr.Read())
                {
                    collection2.Add(dr["Plaka"].ToString());
                    comboBox3.Items.Add(dr["Plaka"].ToString());
                }
                comboBox3.AutoCompleteCustomSource = collection2;
                comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox3.AutoCompleteSource = AutoCompleteSource.CustomSource; 
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
                comboBox3.Items.Clear();
                cn.Close();
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select Plaka from Arac where kisiid=(Select id from Kullanici where Ad='" + comboBox1.Text + "') ", cn);
                SQLiteDataReader dr = kmt.ExecuteReader();
                while (dr.Read())
                {
                    comboBox3.Text = dr["Plaka"].ToString();
                    comboBox3.Items.Add(dr["Plaka"]);
                }
                cn.Close();
            }
            catch (Exception)
            {

            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Close();
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select Ad from Kullanici where id=(Select kisiid from Arac where Plaka='" + comboBox3.Text + "') ", cn);
                SQLiteDataReader dr = kmt.ExecuteReader();
                while (dr.Read())
                {
                    comboBox1.Text = dr["Ad"].ToString();
                }
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
                cn.Clone();
                cn.Open();
                if (comboBox1.Text != "" && comboBox3.Text != "")
                {
                    SQLiteCommand kmt = new SQLiteCommand("insert into Servis (Tarih,Km,Bakim,ServisNot,Kisiid,Aracid,Yapilacaklar) Values ('" + dateTimePicker1.Text + "','" + textBox2.Text + "','" + textBox1.Text + "','" + textBox3.Text + "',(Select id from Kullanici where Ad='" + comboBox1.Text + "'),(Select id from Arac where Plaka='" + comboBox3.Text + "'),'"+textBox4.Text+"')", cn);
                    kmt.ExecuteNonQuery();
                    MessageBox.Show("Kayıt Başarılı!");
                    comboBox1.Text = "";
                    comboBox3.Text = "";
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    frm11.button7.PerformClick();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lütfen Ad veya Plaka Seçiniz!");
                }
                cn.Clone();
            }
            catch (Exception)
            {
                this.Close();
            }
           
        }

        private void Servis_Load(object sender, EventArgs e)
        {
            Plakalistele(); Adlistele(); 
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd.MM.yyyy";
        }
    }
}
