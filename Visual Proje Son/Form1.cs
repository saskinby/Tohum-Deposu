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
        
        private readonly string dbPath = @"Data Source=\girisdb.db;Version=3;";     //KONUMU AYARLAMANIZ GEREKİR

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // Giriş Yapma Butonu
        {
            
            if (string.IsNullOrWhiteSpace(textemail.Text) || string.IsNullOrWhiteSpace(textsifre.Text))
            {
                MessageBox.Show("Lütfen email ve şifre giriniz.", "Hata");
                return;
            }

            
            string dbPath = "Data Source=\\girisdb.db;Version=3;";   //KONUMU AYARLAMANIZ GEREKİR
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open(); 
                    string query = "SELECT * FROM girisdb WHERE email = @Email AND sifre = @Sifre";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@Email", textemail.Text);
                        cmd.Parameters.AddWithValue("@Sifre", textsifre.Text);

                        SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0) 
                        {
                            label4.Text = "Giriş Başarılı";
                            Form2 form2 = new Form2(); 
                            form2.Show();
                            this.Hide();
                        }
                        else
                        {
                            label4.Text  ="Email veya şifre hatalı.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                }
                finally
                {
                    conn.Close(); 
                    textemail.Clear(); 
                    textsifre.Clear();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) // Kaydolma Butonu
        {
            
            if (string.IsNullOrWhiteSpace(textemail.Text) || string.IsNullOrWhiteSpace(textsifre.Text))
            {
                label4.Text = "Lütfen email ve şifre giriniz.";
                return;
            }

           
            string dbPath = "Data Source=\\girisdb.db;Version=3;";  //KONUMU AYARLAMANIZ GEREKİR
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open(); 

                    
                    string checkQuery = "SELECT COUNT(*) FROM girisdb WHERE email = @Email";
                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", textemail.Text);

                        int emailCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (emailCount > 0)
                        {
                            label4.Text = "Bu email zaten kullanımda.";
                            return;
                        }
                    }

                    
                    string query = "INSERT INTO girisdb (email, sifre) VALUES (@Email, @Sifre)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@Email", textemail.Text);
                        cmd.Parameters.AddWithValue("@Sifre", textsifre.Text);

                        int rowsAffected = cmd.ExecuteNonQuery(); 

                        if (rowsAffected > 0)
                        {
                            label4.Text="Kayıt başarılı!";
                        }
                        else
                        {
                            label4.Text = "Kayıt başarısız oldu. Lütfen tekrar deneyin.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                }
                finally
                {
                    conn.Close();
                    textemail.Clear(); 
                    textsifre.Clear();
                }
            }
        }


    }
}
