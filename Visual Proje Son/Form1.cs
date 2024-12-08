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
using System;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Security.Policy;

namespace Visual_Proje_Son
{
    public partial class Form1 : Form
    {
        // Database yolunu sabit olarak tanımlıyoruz
        private readonly string dbPath = @"Data Source=C:\Users\mfaru\source\repos\Visual Proje Son\Visual Proje Son\girisdb.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // Giriş Yap
        {
            // Eğer email veya şifre boşsa, hata mesajı göster
            if (string.IsNullOrWhiteSpace(textemail.Text) || string.IsNullOrWhiteSpace(textsifre.Text))
            {
                MessageBox.Show("Lütfen email ve şifre giriniz.", "Hata");
                return;
            }

            // Veritabanı bağlantısı
            string dbPath = "Data Source=C:\\Users\\mfaru\\source\\repos\\Visual Proje Son\\girisdb.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open(); // Bağlantıyı aç
                    string query = "SELECT * FROM girisdb WHERE email = @Email AND sifre = @Sifre";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        // Parametreleri ekle
                        cmd.Parameters.AddWithValue("@Email", textemail.Text);
                        cmd.Parameters.AddWithValue("@Sifre", textsifre.Text);

                        SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0) // Kullanıcı bulunduysa
                        {
                            MessageBox.Show("Giriş başarılı!", "Başarılı");
                            Form2 form2 = new Form2(); // Form2'ye geç
                            form2.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Email veya şifre hatalı.", "Hata");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                }
                finally
                {
                    conn.Close(); // Bağlantıyı kapat
                    textemail.Clear(); // TextBox'ları temizle
                    textsifre.Clear();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) // Kaydol
        {
            // Eğer email veya şifre boşsa, hata mesajı göster
            if (string.IsNullOrWhiteSpace(textemail.Text) || string.IsNullOrWhiteSpace(textsifre.Text))
            {
                MessageBox.Show("Lütfen email ve şifre giriniz.", "Hata");
                return;
            }

            // Veritabanı bağlantısı
            string dbPath = "Data Source=C:\\Users\\mfaru\\source\\repos\\Visual Proje Son\\girisdb.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open(); // Bağlantıyı aç
                    string query = "INSERT INTO girisdb (email, sifre) VALUES (@Email, @Sifre)";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        // Parametreleri ekle
                        cmd.Parameters.AddWithValue("@Email", textemail.Text);
                        cmd.Parameters.AddWithValue("@Sifre", textsifre.Text);

                        int rowsAffected = cmd.ExecuteNonQuery(); // Veriyi ekle ve eklenip eklenmediğini kontrol et

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Kayıt başarılı!", "Başarılı");
                        }
                        else
                        {
                            MessageBox.Show("Kayıt başarısız oldu. Lütfen tekrar deneyin.", "Hata");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                }
                finally
                {
                    conn.Close(); // Bağlantıyı kapat
                    textemail.Clear(); // TextBox'ları temizle
                    textsifre.Clear();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
