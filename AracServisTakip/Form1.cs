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
    public partial class Form1 : Form
    {
        public SQLiteConnection cn = new SQLiteConnection("Data Source=ServisTakip.s3db;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public SQLiteConnection cn2 = new SQLiteConnection("Data Source=ServisTakip.s3db;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public SQLiteDataAdapter da;
           public DataTable dt = new DataTable();public int i,id;
        public string combo;
        public Form1()
        {
            InitializeComponent();
        }
        public void Gridlistele()
        {
            try
            {
                cn.Close();
                dt.Clear();
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select  s.id,s.Tarih,a.Plaka,k.Ad,a.Marka,a.Model,a.Renk,s.Km,s.Yapilacaklar,s.ServisNot from Servis s join Arac a on s.Aracid=a.id join Kullanici k on k.id=s.Kisiid group by a.Plaka order by s.id desc LIMIT 24", cn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(kmt);
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                cn.Close();
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[2].ReadOnly = true;
                dataGridView2.Columns[3].ReadOnly = true;
                dataGridView2.Columns[4].ReadOnly = true;
                dataGridView2.Columns[5].ReadOnly = true;
                dataGridView2.Columns[6].ReadOnly = true;
                dataGridView2.Columns[7].ReadOnly = true;
                dataGridView2.Columns[8].ReadOnly = true;
              
            }
            catch (Exception)
            {
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
        public void sayilar()
        {
            try
            {
                cn.Close();
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select Count(*) as sayi from Servis", cn);
                SQLiteDataReader dr = kmt.ExecuteReader();dr.Read();
                label7.Text = dr["sayi"].ToString();
                cn.Close();
                cn.Open();
                SQLiteCommand kmt2 = new SQLiteCommand("Select Count(*) as sayi2 from Arac", cn);
                SQLiteDataReader dr2 = kmt2.ExecuteReader(); dr2.Read();
                label8.Text = dr2["sayi2"].ToString();
                cn.Close();
                cn.Open();
                SQLiteCommand kmt3 = new SQLiteCommand("Select Count(*) as sayi3 from Kullanici", cn);
                SQLiteDataReader dr3 = kmt3.ExecuteReader(); dr3.Read();
                label9.Text = dr3["sayi3"].ToString();
                cn.Close();
            }
            catch (Exception)
            {

            }
        }
        //void cevir()
        //{
        //    String[] dizi; string ek,ek2;
        //    foreach (DataGridViewRow row in dataGridView2.Rows)
        //    {
        //        id = Convert.ToInt16(row.Cells[0].Value.ToString());
        //        dizi = row.Cells[1].Value.ToString().Split(' ');
        //        DateTime parsedDate = DateTime.Parse(dizi[0] + dizi[1] + dizi[2]);
        //        ek = parsedDate.Day.ToString();
        //        ek2 = parsedDate.Month.ToString();
        //        if (parsedDate.Day.ToString().Length == 1)
        //        {
        //            ek = "0" + parsedDate.Day;
        //        }
        //        if (parsedDate.Month.ToString().Length == 1)
        //        {
        //            ek2 = "0" + parsedDate.Month;
        //        }
        //        string le = ek + "." + ek2 + "." + parsedDate.Year;
        //        cn2.Open();
        //        SQLiteCommand kmt2 = new SQLiteCommand("update Servis set Tarih='" + le + "' where id=" + id + " ", cn2);
        //        kmt2.ExecuteNonQuery();
        //        cn2.Close();
        //    }

        //}
        private void Form1_Load(object sender, EventArgs e)
        {
            Gridlistele(); Adlistele(); sayilar(); 
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F);
            dataGridView2.DefaultCellStyle = dataGridViewCellStyle1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Gridlistele(); sayilar();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Close();
                cn.Open();
                SQLiteCommand kmt2 = new SQLiteCommand("Select Aracid from Servis where Kisiid=(select id from Kullanici where ad='"+comboBox1.Text+"')", cn);
                SQLiteDataReader dr2 = kmt2.ExecuteReader(); dr2.Read();
                if(Convert.ToInt16(dr2["Aracid"])==0)
                {
                    dt.Clear();
                    cn2.Open();
                    SQLiteCommand kmt3 = new SQLiteCommand("Select s.id,s.Tarih,k.Ad,s.Km,s.Yapilacaklar,s.ServisNot from Servis s join Kullanici k on k.id=s.Kisiid where k.Ad like '" + comboBox1.Text + "%' order by s.id desc", cn2);
                    SQLiteDataAdapter da3 = new SQLiteDataAdapter(kmt3);
                    da3.Fill(dt);
                    dataGridView2.DataSource = dt;
                    cn2.Close();
                }
                else
                {
                    dt.Clear();
                    cn2.Open();
                    SQLiteCommand kmt = new SQLiteCommand("Select s.id,s.Tarih,a.Plaka,k.Ad,a.Marka,a.Model,a.Renk,s.Km,s.Yapilacaklar,s.ServisNot from Servis s join Arac a on s.Aracid=a.id join Kullanici k on k.id=s.Kisiid where k.Ad like '" + comboBox1.Text + "%' order by s.id desc", cn2);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(kmt);
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                    cn2.Close();
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
                cn.Close();
                dt.Clear();
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select s.id,s.Tarih,a.Plaka,k.Ad,a.Marka,a.Model,a.Renk,s.Km,s.Yapilacaklar,s.ServisNot from Servis s join Arac a on s.Aracid=a.id join Kullanici k on k.id=s.Kisiid where a.Plaka like'" + textBox2.Text.ToUpper() + "%' order by s.id desc", cn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(kmt);
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                cn.Close();
            }
            catch (Exception)
            {

            }
          
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Close();
                dt.Clear();
                cn.Open();
                SQLiteCommand kmt = new SQLiteCommand("Select s.id,s.Tarih,a.Plaka,k.Ad,a.Marka,a.Model,a.Renk,s.Km,s.Yapilacaklar,s.ServisNot from Servis s join Arac a on s.Aracid=a.id join Kullanici k on k.id=s.Kisiid where k.Tel like '" + textBox3.Text + "%' order by s.id desc", cn);
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
                cn2.Close();
                string tarih = "ee", km = "w", bakim = "w", not2 = "w"; int id = 0;
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    id = Convert.ToInt16(row.Cells[0].Value.ToString());
                    tarih = row.Cells[1].Value.ToString();
                    km = row.Cells[8].Value.ToString();
                    bakim = row.Cells[9].Value.ToString();
                    not2 = row.Cells[10].Value.ToString();
                }
                cn2.Open();
                DialogResult dialogResult = MessageBox.Show("Güncelleme Yapmak İstediğinize Eminmisiniz?", "Güncelleme!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SQLiteCommand kmt2 = new SQLiteCommand("update Servis set Tarih='" + tarih + "',Km='" + km + "',Yapilacaklar='" + bakim + "',ServisNot='" + not2 + "' where id=" + id + " ", cn2);
                    kmt2.ExecuteNonQuery();
                    MessageBox.Show("Güncelleme Başarılı!"); Gridlistele();
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
            Musteri m = new Musteri();
            m.frm1 = this;
            m.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Arac a = new Arac();
            a.frm1 = this;
            a.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Servis s = new Servis();
            s.frm11 = this;
            s.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    id = Convert.ToInt16(row.Cells[0].Value.ToString());
                    break;
                }
                detay d = new detay();
                d.frm1 = this;
                d.id = id;
                d.ShowDialog();
            }
            catch (Exception)
            {

            }
          
        }

        private void dataGridView2_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(e.ColumnIndex==3)
                {
                    string cellValue = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
                    cn.Open();
                    SQLiteCommand kmt = new SQLiteCommand("Select id from Kullanici where Ad='" + cellValue + "'", cn);
                    SQLiteDataReader dr = kmt.ExecuteReader(); dr.Read();
                    int id = Convert.ToInt16(dr["id"]);
                    cn.Close();
                    Musteridetay m = new Musteridetay();
                    m.id = id;m.id2 = 1;
                    m.frm1 = this;
                    m.ShowDialog();
                }
                else if(e.ColumnIndex==2)
                    {
                    string cellValue = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
                    cn.Open();
                    SQLiteCommand kmt = new SQLiteCommand("Select id from Arac where Plaka='" + cellValue + "'", cn);
                    SQLiteDataReader dr = kmt.ExecuteReader(); dr.Read();
                    int id = Convert.ToInt16(dr["id"]);
                    cn.Close();
                    aracdetay a = new aracdetay();
                    a.id = id;a.id2 = 1;a.frm1 = this;
                    a.ShowDialog();
                }
                
            }
            catch (Exception)
            {
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Close();
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    id = Convert.ToInt16(row.Cells[0].Value.ToString());
                    break;
                }
                cn.Open();
                DialogResult dialogResult = MessageBox.Show("Servisi Silmek İstediğinize Eminmisiniz?", "Servis Sil?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SQLiteCommand kmt = new SQLiteCommand("delete from Servis where id=" + id + " ", cn);
                    kmt.ExecuteNonQuery();
                    MessageBox.Show("Silme Başarılı!"); Gridlistele();

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
    }
}
