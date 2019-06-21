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
    public partial class Musteri : Form
    {
        public SQLiteConnection cn = new SQLiteConnection("Data Source=ServisTakip.s3db;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public SQLiteConnection cn2 = new SQLiteConnection("Data Source=ServisTakip.s3db;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public SQLiteDataAdapter da;
        public DataTable dt = new DataTable(); public int i, id,gecici=2;
        public string combo,deneme; public Form1 frm1;
        public Musteri()
        {
            InitializeComponent();
        }
        void Gridlistele()
        {
            try
            {
                cn.Close();
                dt.Clear(); cn.Close();
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select * from Kullanici LIMIT 15", cn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(kmt);
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                cn.Close();
                dataGridView2.Columns[0].Visible = false;
            }
            catch (Exception)
            {

            }
        
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Gridlistele();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Close();
                dt.Clear();
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select * from Kullanici where Tel like '" + textBox4.Text + "%' order by Ad desc", cn);
                da = new SQLiteDataAdapter(kmt);
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                cn.Close();
            }
            catch (Exception)
            {

            }
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Close();
                string ad = "ee", tel = "w", not = "w"; int id = 0;
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    id = Convert.ToInt16(row.Cells[0].Value.ToString());
                    ad = row.Cells[1].Value.ToString();
                    tel = row.Cells[2].Value.ToString();
                    not = row.Cells[3].Value.ToString();
                }
                DialogResult dialogResult = MessageBox.Show("Güncelleme Yapmak İstediğinize Eminmisiniz?", "Güncelleme!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    
                    cn.Open();
                    SQLiteCommand kmt = new SQLiteCommand("update Kullanici set Ad='" + ad + "',Tel='" + tel + "',KullaniciNot='" + not + "' where id=" + id + " ", cn);
                    kmt.ExecuteNonQuery();
                    MessageBox.Show("Güncelleme Başarılı!"); Gridlistele();
                    cn.Close();

                }
                else if (dialogResult == DialogResult.No)
                {

                }
            }
            catch (Exception)
            {
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Close();
                dt.Clear();
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select * from Kullanici where Ad like '" + comboBox1.Text + "%' order by Ad asc", cn);
                da = new SQLiteDataAdapter(kmt);
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                cn.Close();
            }
            catch (Exception)
            {

            }
           
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                id = Convert.ToInt16(row.Cells[0].Value.ToString());
                break;
            }
            Musteridetay m = new Musteridetay();
            m.id = id;
            m.m2 = this;
            m.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    id = Convert.ToInt16(row.Cells[0].Value.ToString());
                    break;
                }
                DialogResult dialogResult = MessageBox.Show("Kullanıcıyı Silmek İstediğinize Eminmisiniz?", "Servis Sil?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    cn.Close();
                    cn.Open();
                    SQLiteCommand kmt = new SQLiteCommand("delete from Kullanici where id=" + id + " ", cn);
                    kmt.ExecuteNonQuery();
                    MessageBox.Show("Silme Başarılı!"); Gridlistele();
                    cn.Close(); frm1.button7.PerformClick();

                }
                else if (dialogResult == DialogResult.No)
                {

                }
            }
            catch (Exception)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                    cn2.Open();
                    SQLiteCommand kmt2 = new SQLiteCommand("insert into Kullanici (Ad,Tel,KullaniciNot) Values ('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "')", cn2);
                    kmt2.ExecuteNonQuery();
                    cn.Close(); 
                    Gridlistele(); MessageBox.Show("Kullanıcı Eklendi");
                    cn2.Close();string kayit = textBox1.Text;
                    frm1.Adlistele(); this.Close();
                Arac ar = new Arac(); 
                ar.combo = kayit; ar.ShowDialog();
                textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = ""; frm1.button7.PerformClick();
            }
            catch (Exception)
            {
                if (gecici == 2)
                {
                    deneme = textBox1.Text;
                }
                MessageBox.Show("Kullanıcı Adı Mevcut.Bir daha Deneyin."); 
                gecici++;
                cn2.Close();
            }
        }
        public void Adlistele()
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
                cn.Close();
                comboBox1.AutoCompleteCustomSource = collection;
                comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
            catch (Exception)
            {

            }

        }
        private void Musteri_Load(object sender, EventArgs e)
        {

            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            dataGridView2.DefaultCellStyle = dataGridViewCellStyle1;
            Gridlistele();Adlistele(); button9.Visible = false;button8.Visible = false;
        }
    }
}
