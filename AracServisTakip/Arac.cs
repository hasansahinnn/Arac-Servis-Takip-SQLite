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
    public partial class Arac : Form
    {
        public SQLiteConnection cn2 = new SQLiteConnection("Data Source=ServisTakip.s3db;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public SQLiteConnection cn = new SQLiteConnection("Data Source=ServisTakip.s3db;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public SQLiteDataAdapter da;
        public DataTable dt = new DataTable(); public int i, id;
        public string combo; public Form1 frm1;
        public Arac()
        {
            InitializeComponent();
        }
        void Gridlistele()
        {
            try
            {
                dt.Clear(); cn.Close();
                cn.Open();
                using (SQLiteCommand kmt = new SQLiteCommand("Select A.id,k.Ad,A.Plaka,k.Tel,A.Marka,A.Model,A.Renk from Kullanici k join Arac A on k.id=A.Kisiid LIMIT 24", cn))
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(kmt))
                    {
                        da.Fill(dt);
                        dataGridView2.DataSource = dt;
                    }
                }
                cn.Close();
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].ReadOnly = true;
                dataGridView2.Columns[2].ReadOnly = true;
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
                using (SQLiteCommand kmt = new SQLiteCommand("Select distinct(Ad) from Kullanici order by Ad asc ", cn))
                {
                    using (SQLiteDataReader dr = kmt.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            comboBox1.Items.Add(dr["Ad"]);
                            collection.Add(dr["Ad"].ToString());
                            comboBox2.Items.Add(dr["Ad"]);
                        }
                    }               
                }
                comboBox2.AutoCompleteCustomSource = collection;
                comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                comboBox1.AutoCompleteCustomSource = collection;
                comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                cn.Close();

            }
            catch (Exception)
            {

            }
          
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Gridlistele();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Close();
                dt.Clear();
                cn.Open();
                using (SQLiteCommand kmt = new SQLiteCommand("Select A.id,k.Ad,A.Plaka,k.Tel,A.Marka,A.Model,A.Renk from Kullanici k join Arac A on k.id=A.Kisiid where k.Ad like'" + comboBox1.Text + "%' order by k.Ad asc", cn))
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(kmt))
                    {
                        da.Fill(dt);
                        dataGridView2.DataSource = dt;
                    }
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
                using (SQLiteCommand kmt = new SQLiteCommand("Select A.id,k.Ad,A.Plaka,k.Tel,A.Marka,A.Model,A.Renk from Kullanici k join Arac A on k.id=A.Kisiid where A.Plaka like'" + textBox2.Text.ToUpper() + "%' order by k.Ad asc", cn))
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(kmt))
                    {
                        da.Fill(dt);
                        dataGridView2.DataSource = dt;
                    }
                }
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
                using (SQLiteCommand kmt = new SQLiteCommand("Select A.id,k.Ad,A.Plaka,k.Tel,A.Marka,A.Model,A.Renk from Kullanici k join Arac A on k.id=A.Kisiid where k.Tel like '" + textBox3.Text + "%' order by k.Ad asc", cn))
                {
                    da = new SQLiteDataAdapter(kmt);
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                cn.Close();
            }
            catch (Exception)
            {

            }
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox6.Text=="")
                {
                    MessageBox.Show("Plaka Seçin!");
                }
                else
                {
                    if (comboBox2.Text == "")
                    {
                      
                            cn2.Close();
                            cn2.Open();
                            using (SQLiteCommand kmtq2 = new SQLiteCommand("insert into Kullanici (Ad) Values('"+textBox6.Text+"')", cn2))
                            {
                                kmtq2.ExecuteNonQuery();
                            }
                            cn2.Close();
                            cn.Close();
                            cn.Open();
                            using (SQLiteCommand kmtq = new SQLiteCommand("insert into Arac (Marka,Model,Renk,Plaka,Kisiid) Values ('" + textBox7.Text + "','" + textBox1.Text + "','" + textBox4.Text + "','" + textBox6.Text + "',(Select id from Kullanici where Ad='" + textBox6.Text + "'))", cn))
                            {
                                kmtq.ExecuteNonQuery();
                            }
                            Gridlistele();


                            this.Close();
                            frm1.button7.PerformClick();
                            textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = "";

                    }
                    else
                    {
                        cn.Close();
                        cn.Open();
                        SQLiteCommand kmt = new SQLiteCommand("select id from Arac where Plaka='" + textBox6.Text.ToUpper() + "' and kisiid=(Select id from Kullanici where Ad='"+comboBox2.Text+"')", cn);
                        SQLiteDataReader dr = kmt.ExecuteReader();
                        if (dr.Read())
                        {
                            MessageBox.Show("Bu Kişiye Ait Araç Mevcut!");
                        }
                        else
                        {
                            cn.Close();
                            cn.Open();
                            using (SQLiteCommand kmtq = new SQLiteCommand("insert into Arac (Marka,Model,Renk,Plaka,Kisiid) Values ('" + textBox7.Text + "','" + textBox1.Text + "','" + textBox4.Text + "','" + textBox6.Text + "',(Select id from Kullanici where Ad='" + comboBox2.Text + "'))", cn))
                            {
                                kmtq.ExecuteNonQuery();
                            }
                            Gridlistele();


                            this.Close();
                            frm1.button7.PerformClick();
                            textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = "";
                        }

                        cn.Close();
                    }
                    
                    
                }
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
                string marka = " ", Model = " ", Renk = " ",  Plaka = " "; int id = 0;
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    id = Convert.ToInt16(row.Cells[0].Value.ToString());
                    marka = row.Cells[4].Value.ToString();
                    Model = row.Cells[5].Value.ToString();
                    Renk = row.Cells[6].Value.ToString();
                    
                    Plaka = row.Cells[2].Value.ToString();

                }
                cn.Open();
                DialogResult dialogResult = MessageBox.Show("Güncelleme Yapmak İstediğinize Eminmisiniz?", "Güncelleme!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SQLiteCommand kmt = new SQLiteCommand("update Arac set Marka='" + marka + "',Model='" + Model + "',Renk='" + Renk + "',Plaka='" + Plaka + "' where id=" + id + " ", cn))
                    {
                        kmt.ExecuteNonQuery();
                        MessageBox.Show("Güncelleme Başarılı!");
                    }
                        
                   Gridlistele();
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

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    id = Convert.ToInt16(row.Cells[0].Value.ToString());
                    break;
                }
                DialogResult dialogResult = MessageBox.Show("Araç Silmek İstediğinize Eminmisiniz?", "Araç Sil?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    cn.Close();
                    cn.Open();
                    using (SQLiteCommand kmt = new SQLiteCommand("delete from Arac where id=" + id + " ", cn))
                    {
                        kmt.ExecuteNonQuery(); MessageBox.Show("Silindi");Gridlistele();
                    }
                    frm1.button7.PerformClick();
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

        private void comboBox2_TextUpdate(object sender, EventArgs e)
        {
           
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                id = Convert.ToInt16(row.Cells[0].Value.ToString());
                break;
            }
            aracdetay a = new aracdetay();
            a.id = id;
            a.a2 = this;
            a.ShowDialog();
        }

        private void Arac_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            dataGridView2.DefaultCellStyle = dataGridViewCellStyle1;
            Gridlistele(); Adlistele();comboBox2.Text = combo;button9.Visible = false;button8.Visible = false;
        }
    }
}
