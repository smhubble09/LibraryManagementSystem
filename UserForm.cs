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
        readonly string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\repos\LibraryManagementSystem\LibraryManagementDatabase.accdb; Jet OLEDB:Database Password=libraryApp22!";

        public UserForm() {
            InitializeComponent();
            this.Text = "Welcome " + StartForm.LoggedUserName;
        }

        private void UserForm_Load(object sender, EventArgs e) {

            DataGridViewColumn titleColumn = GuestBookDataGrid.Columns[1];
            DataGridViewColumn checkDateColumn = GuestBookDataGrid.Columns[2];
            DataGridViewColumn returnDateColumn = GuestBookDataGrid.Columns[3];
            titleColumn.Width = 300;
            checkDateColumn.Width = 115;
            returnDateColumn.Width = 115;
            OleDbConnection conn = new OleDbConnection(connectionString);
            try {
                string query = "SELECT * from Books WHERE [Checked Out By] LIKE @firstName;";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@firstName", StartForm.LoggedUserName + " %");

                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                booksTable = new DataTable();
                da.Fill(booksTable);

                GuestBookDataGrid.DataSource = booksTable;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            finally {
                conn.Close();
            }
        }

        private void LogOutButton_Click(object sender, EventArgs e) {
            var startForm = new StartForm();
            startForm.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, EventArgs e) {
            Application.Exit();
        }
    }
}
