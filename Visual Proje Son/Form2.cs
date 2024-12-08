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

        private string connectionString = "Data Source=C:\\Users\\mfaru\\source\\repos\\Visual Proje Son\\grafik.db;Version=3;";

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadData(); // Form yüklendiğinde mevcut verileri göster

        }
        private void button1_Click(object sender, EventArgs e)
        {
            // TextBox'lardan veri al
            string bitkiAdi = textBox1.Text;
            string tohumSayisi = textBox2.Text;

            // Boş kontrolü
            if (string.IsNullOrWhiteSpace(bitkiAdi) || string.IsNullOrWhiteSpace(tohumSayisi))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Hata");
                return;
            }

            // Tohum sayısının sayısal bir değer olduğundan emin olun
            if (!int.TryParse(tohumSayisi, out int tohumSayisiInt))
            {
                MessageBox.Show("Lütfen 'Tohum Sayısı' alanına geçerli bir sayı giriniz.", "Hata");
                return;
            }

            // Veritabanı bağlantısı
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Veriyi ekleme sorgusu
                    string query = "INSERT INTO grafik ([Bitki Adı], [Tohum Sayısı]) VALUES (@BitkiAdi, @TohumSayisi)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@BitkiAdi", bitkiAdi);
                        cmd.Parameters.AddWithValue("@TohumSayisi", tohumSayisiInt);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Kayıt başarılı!", "Başarılı");
                            LoadData(); // DataGridView'i güncelle
                        }
                        else
                        {
                            MessageBox.Show("Kayıt eklenemedi. Lütfen tekrar deneyin.", "Hata");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                }
            }

            // TextBox'ları temizle
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

                        dataGridView1.DataSource = dt; // Veriyi DataGridView'e bağla
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                }
            }
        }

        private void inc_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Seçili satırın "Id" ve "Tohum Sayısı" değerlerini al
                int selectedId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                int currentTohumSayisi = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Tohum Sayısı"].Value);

                // "Tohum Sayısı"nı bir artır
                int newTohumSayisi = currentTohumSayisi + 1;

                UpdateTohumSayisi(selectedId, newTohumSayisi);
                LoadData(); // DataGridView'i güncelle
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçiniz.", "Uyarı");
            }
        }

        private void dec_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Seçili satırın "Id" ve "Tohum Sayısı" değerlerini al
                int selectedId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                int currentTohumSayisi = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Tohum Sayısı"].Value);

                // "Tohum Sayısı"nı bir azalt, 0'ın altına düşmesini engelle
                int newTohumSayisi = Math.Max(0, currentTohumSayisi - 1);

                UpdateTohumSayisi(selectedId, newTohumSayisi);
                LoadData(); // DataGridView'i güncelle
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçiniz.", "Uyarı");
            }
        }

        private void UpdateTohumSayisi(int id, int newTohumSayisi)
        {
            string query = "UPDATE grafik SET [Tohum Sayısı] = @TohumSayisi WHERE [Id] = @Id";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TohumSayisi", newTohumSayisi);
                        cmd.Parameters.AddWithValue("@Id", id);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadData(); // DataGridView'i güncelle
        }
    }
}
