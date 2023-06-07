using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private string connectionString;

        public Form1()
        {
            InitializeComponent();

            // Отримати рядок підключення з конфігураційного файлу
            connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Students", connection);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset, "Students");

                    listBox1.DataSource = dataset.Tables["Students"];
                    listBox1.DisplayMember = "Name";
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Помилка під час завантаження даних: " + ex.Message;
                MessageBox.Show(errorMessage);

                string filePath = "errorLog.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(errorMessage);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)listBox1.SelectedItem;
                int studentId = (int)selectedRow["id"];
                MessageBox.Show("Код студента: " + studentId);
            }
        }
    }
}
