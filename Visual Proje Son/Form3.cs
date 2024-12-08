using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Visual_Proje_Son
{
    public partial class Form3 : Form
    {
        
        private string connectionString = "Data Source=\\Visual Proje Son\\grafik.db;Version=3;";  //KONUMU AYARLAMANIZ GEREKİR

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadChartData();
        }

        private void LoadChartData()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    
                    string query = "SELECT [Bitki Adı], [Tohum Sayısı] FROM grafik";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        
                        chart1.Series.Clear();

                        
                        Series series = new Series("Tohum Sayısı")
                        {
                            ChartType = SeriesChartType.Column, 
                            XValueType = ChartValueType.String
                        };

                        
                        while (reader.Read())
                        {
                            string bitkiAdi = reader["Bitki Adı"].ToString();
                            int tohumSayisi = Convert.ToInt32(reader["Tohum Sayısı"]);
                            series.Points.AddXY(bitkiAdi, tohumSayisi);
                        }

                        
                        chart1.Series.Add(series);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)  //Geri dönüş Butonu
        {
            Form2 form2 = new Form2(); 
            form2.Show();
            this.Hide();
        }
    }
}

