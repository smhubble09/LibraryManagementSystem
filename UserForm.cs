using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryManagementSystem {
    public partial class UserForm : Form {
        
        DataTable booksTable;
        //Access Data Connection
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\repos\LibraryManagementSystem\LibraryManagementDatabase.accdb; Jet OLEDB:Database Password=libraryApp22!";
        public UserForm() {
            InitializeComponent();
            this.ActiveControl = TitleSearchBox;
            this.Text = "Welcome " + StartForm.LoggedUserName;
        }

        private void UserForm_Load(object sender, EventArgs e) {
            DataGridViewColumn titleColumn = BooksDataGrid.Columns[0];
            DataGridViewColumn authorColumn = BooksDataGrid.Columns[1];
            titleColumn.Width = 500;
            authorColumn.Width = 275;
        }

        private void SearchButton_Click(object sender, EventArgs e) {
            OleDbConnection conn = new OleDbConnection(connectionString);
            if (string.IsNullOrEmpty(TitleSearchBox.Text)) {
                try {
                    string query = "SELECT * from Books WHERE Available=Yes;";
                    OleDbCommand cmd = new OleDbCommand(query, conn);

                    conn.Open();
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    booksTable = new DataTable();
                    da.Fill(booksTable);

                    BooksDataGrid.DataSource = booksTable;
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
                finally { 
                    conn.Close(); 
                }
            }
            else {
                try {
                    string query = "SELECT * from Books WHERE [Book Title] LIKE @bookTitle AND Available=Yes;";
                    string wildQuery = string.Format("%{0}%", TitleSearchBox.Text);
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("@bookTitle", wildQuery);

                    conn.Open();
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    booksTable = new DataTable();
                    da.Fill(booksTable);

                    BooksDataGrid.DataSource = booksTable;
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
                finally {
                    conn.Close();
                }
            }
        }

        private void TitleSearchBox_TextChanged(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(TitleSearchBox.Text))
                SearchButton.Text = "View All";
            else
                SearchButton.Text = "Search";
        }

        private void ExitButton_Click(object sender, EventArgs e) {
            Application.Exit();
        }
    }
}
