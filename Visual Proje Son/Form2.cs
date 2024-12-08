using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Visual_Proje_Son
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private string connectionString = "Data Source=\\grafik.db;Version=3;"; //KONUMU AYARLAMANIZ GEREKİR

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadData(); 

        }
        private void button1_Click(object sender, EventArgs e)  //Bitki Ekleme Butonu
        {
            
            string bitkiAdi = textBox1.Text;
            string tohumSayisi = textBox2.Text;

            
            if (string.IsNullOrWhiteSpace(bitkiAdi) || string.IsNullOrWhiteSpace(tohumSayisi))
            {
                label12.Text = "Lütfen tüm alanları doldurunuz.";
                return;
            }

            
            if (!int.TryParse(tohumSayisi, out int tohumSayisiInt))
            {
                label12.Text = "Lütfen 'Tohum Sayısı' alanına geçerli bir sayı giriniz.";
                return;
            }

            
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    
                    string checkQuery = "SELECT [Id], [Tohum Sayısı] FROM grafik WHERE [Bitki Adı] = @BitkiAdi";
                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@BitkiAdi", bitkiAdi);
                        using (SQLiteDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                int id = Convert.ToInt32(reader["Id"]);
                                int mevcutTohumSayisi = Convert.ToInt32(reader["Tohum Sayısı"]);
                                int yeniTohumSayisi = mevcutTohumSayisi + tohumSayisiInt;

                                string updateQuery = "UPDATE grafik SET [Tohum Sayısı] = @YeniTohumSayisi WHERE [Id] = @Id";
                                using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@YeniTohumSayisi", yeniTohumSayisi);
                                    updateCmd.Parameters.AddWithValue("@Id", id);
                                    updateCmd.ExecuteNonQuery();
                                }

                                label12.Text = "Tohum sayısı arttırıldı.";
                            }
                            else
                            {
                                
                                string insertQuery = "INSERT INTO grafik ([Bitki Adı], [Tohum Sayısı]) VALUES (@BitkiAdi, @TohumSayisi)";
                                using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn))
                                {
                                    insertCmd.Parameters.AddWithValue("@BitkiAdi", bitkiAdi);
                                    insertCmd.Parameters.AddWithValue("@TohumSayisi", tohumSayisiInt);
                                    insertCmd.ExecuteNonQuery();
                                }

                                label12.Text = "Yeni bitki eklendi.";
                            }
                        }
                    }

                    
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                }
                finally
                {
                    conn.Close(); 
                }
            }

            
            textBox1.Clear();
            textBox2.Clear();
        }



        private void LoadData() 
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM grafik";
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dataGridView1.DataSource = dt; 

                        
                        if (dataGridView1.Columns["Id"] != null)
                        {
                            dataGridView1.Columns["Id"].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                }
            }
        }


        private void inc_Click(object sender, EventArgs e)  //Tohum Sayısını 1 arttırma
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                
                int selectedId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                int currentTohumSayisi = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Tohum Sayısı"].Value);

                
                int newTohumSayisi = currentTohumSayisi + 1;

                
                UpdateTohumSayisi(selectedId, newTohumSayisi);
        
                
                LoadData();
            }
            else
            {
                label14.Text = "Lütfen bir satır seçiniz.";
            }
        }

        private void dec_Click(object sender, EventArgs e)  //Tohum sayısını 1 azaltma
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                
                int selectedId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                int currentTohumSayisi = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Tohum Sayısı"].Value);

                
                int newTohumSayisi = currentTohumSayisi - 1;

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        if (newTohumSayisi > 0)
                        {
                            
                            string updateQuery = "UPDATE grafik SET [Tohum Sayısı] = @YeniTohumSayisi WHERE [Id] = @Id";
                            using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@YeniTohumSayisi", newTohumSayisi);
                                updateCmd.Parameters.AddWithValue("@Id", selectedId);
                                updateCmd.ExecuteNonQuery();
                            }
                            label14.Text = "Tohum sayısı güncellendi.";
                        }
                        else
                        {
                            
                            string deleteQuery = "DELETE FROM grafik WHERE [Id] = @Id";
                            using (SQLiteCommand deleteCmd = new SQLiteCommand(deleteQuery, conn))
                            {
                                deleteCmd.Parameters.AddWithValue("@Id", selectedId);
                                deleteCmd.ExecuteNonQuery();
                            }
                            label14.Text = "Tohum sayısı 0'a düştü ve satır silindi.";
                        }

                        
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                    }
                }
            }
            else
            {
                label14.Text = "Lütfen bir satır seçiniz.";
            }
        }


        private void UpdateTohumSayisi(int id, int newTohumSayisi)
        {
            string query = "UPDATE grafik SET [Tohum Sayısı] = @TohumSayisi WHERE [Id] = @Id";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open(); 
                using (SQLiteTransaction transaction = conn.BeginTransaction()) 
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@TohumSayisi", newTohumSayisi);
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit(); 
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); 
                        throw; 
                    }
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(); 
            form3.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)  //Tohum Sayısını 10 arttırma
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                
                int selectedId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                int currentTohumSayisi = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Tohum Sayısı"].Value);

                
                int newTohumSayisi = currentTohumSayisi + 10;

                UpdateTohumSayisi(selectedId, newTohumSayisi);
                LoadData(); 
            }
            else
            {
                label14.Text = "Lütfen bir satır seçiniz.";
            }
        }

        private void button4_Click(object sender, EventArgs e)   //Tohum Sayısını 10 azaltma
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
               
                int selectedId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                int currentTohumSayisi = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Tohum Sayısı"].Value);

               
                int newTohumSayisi = currentTohumSayisi - 10;

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        if (newTohumSayisi > 0)
                        {
                            
                            string updateQuery = "UPDATE grafik SET [Tohum Sayısı] = @YeniTohumSayisi WHERE [Id] = @Id";
                            using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@YeniTohumSayisi", newTohumSayisi);
                                updateCmd.Parameters.AddWithValue("@Id", selectedId);
                                updateCmd.ExecuteNonQuery();
                            }
                            label14.Text = "Tohum sayısı güncellendi.";
                        }
                        else
                        {
                            
                            string deleteQuery = "DELETE FROM grafik WHERE [Id] = @Id";
                            using (SQLiteCommand deleteCmd = new SQLiteCommand(deleteQuery, conn))
                            {
                                deleteCmd.Parameters.AddWithValue("@Id", selectedId);
                                deleteCmd.ExecuteNonQuery();
                            }
                            label14.Text = "Tohum sayısı 0'a düştü ve satır silindi.";
                        }

                        
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                    }
                }
            }
            else
            {
                label14.Text = "Lütfen bir satır seçiniz.";
            }
        }



        private void button5_Click(object sender, EventArgs e)  //Bitki Çıkartma
        {
            string bitkiAdi = textBox3.Text;
            string tohumSayisi = textBox4.Text;

            if (string.IsNullOrWhiteSpace(bitkiAdi) || string.IsNullOrWhiteSpace(tohumSayisi))
            {
                label13.Text = "Lütfen tüm alanları doldurunuz.";
                return;
            }

            if (!int.TryParse(tohumSayisi, out int tohumSayisiInt))
            {
                label13.Text = "Lütfen 'Tohum Sayısı' alanına geçerli bir sayı giriniz.";
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open(); 
                using (SQLiteTransaction transaction = conn.BeginTransaction()) 
                {
                    try
                    {
                        string checkQuery = "SELECT [Id], [Tohum Sayısı] FROM grafik WHERE [Bitki Adı] = @BitkiAdi";
                        using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn, transaction))
                        {
                            checkCmd.Parameters.AddWithValue("@BitkiAdi", bitkiAdi);
                            using (SQLiteDataReader reader = checkCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    int id = Convert.ToInt32(reader["Id"]);
                                    int mevcutTohumSayisi = Convert.ToInt32(reader["Tohum Sayısı"]);

                                    if (mevcutTohumSayisi > tohumSayisiInt)
                                    {
                                        int yeniTohumSayisi = mevcutTohumSayisi - tohumSayisiInt;

                                        string updateQuery = "UPDATE grafik SET [Tohum Sayısı] = @TohumSayisi WHERE [Id] = @Id";
                                        using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn, transaction))
                                        {
                                            updateCmd.Parameters.AddWithValue("@TohumSayisi", yeniTohumSayisi);
                                            updateCmd.Parameters.AddWithValue("@Id", id);
                                            updateCmd.ExecuteNonQuery();
                                        }
                                        label13.Text = $"'{bitkiAdi}' güncellendi. Yeni tohum sayısı: {yeniTohumSayisi}";
                                    }
                                    else
                                    {
                                        string deleteQuery = "DELETE FROM grafik WHERE [Id] = @Id";
                                        using (SQLiteCommand deleteCmd = new SQLiteCommand(deleteQuery, conn, transaction))
                                        {
                                            deleteCmd.Parameters.AddWithValue("@Id", id);
                                            deleteCmd.ExecuteNonQuery();
                                        }
                                        label13.Text = $"'{bitkiAdi}' silindi.";
                                    }
                                }
                                else
                                {
                                    label13.Text = "Bu bitki mevcut değil.";
                                    return;
                                }
                            }
                        }
                        transaction.Commit(); 
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); 
                        MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                    }
                }
            }

            textBox3.Clear();
            textBox4.Clear();
            LoadData();
        }

        private void Form2_Load_1(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button6_Click(object sender, EventArgs e)    //Grafik ekranına geçme
        {
            Form1 form1 = new Form1(); 
            form1.Show();
            this.Hide();
        }
    }
}
