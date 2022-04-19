using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace LibraryManagementSystem {
    public partial class LibrarianForm : Form {

        DataTable booksTable;

        //Access Data Connection
        readonly string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\repos\LibraryManagementSystem\LibraryManagementDatabase.accdb; Jet OLEDB:Database Password=libraryApp22!";
        public LibrarianForm() {
            InitializeComponent();
            this.Text = "Welcome " + StartForm.LoggedUserName;
        }

        private void LibrarianForm_Load(object sender, EventArgs e) {
            // TODO: This line of code loads data into the 'libraryManagementDatabaseDataSet.Users' table. You can move, or remove it, as needed.
            this.usersTableAdapter.Fill(this.libraryManagementDatabaseDataSet.Users);
            // Uncomment the following code if you want the data field to load all the books
            //this.booksTableAdapter.Fill(this.libraryManagementDatabaseDataSet.Books);
            DataGridViewColumn titleColumn = BooksDataGrid.Columns[1];
            DataGridViewColumn authorColumn = BooksDataGrid.Columns[2];
            DataGridViewColumn checkedoutColumn = BooksDataGrid.Columns[4];
            titleColumn.Width = 250;
            authorColumn.Width = 125;
            checkedoutColumn.Width = 100;
        }

        private void SearchButton_Click(object sender, EventArgs e) {
            OleDbConnection conn = new OleDbConnection(connectionString);
            try {
                string query = "SELECT * from Books WHERE [Checked Out By]=@fullName;";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@fullName", CustomerComboBox.SelectedValue);

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

        private void ExitButton_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void LogOutButton_Click(object sender, EventArgs e) {
            var startForm = new StartForm();
            startForm.Show();
            this.Close();

        }

        private void CheckInButton_Click(object sender, EventArgs e) {
            OleDbConnection conn = new OleDbConnection(connectionString);
            try {
                string id = Convert.ToString(BooksDataGrid.Rows[BooksDataGrid.CurrentRow.Index].Cells[0].Value);
                string query = "UPDATE Books SET [Checked Out By]=null, [Check Out Date]=null, [Return Date]=null WHERE ID=@id";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                int a = cmd.ExecuteNonQuery();
                if(a == 0)
                    _ = MessageBox.Show("Unable to check book in.", "Check-In Error", MessageBoxButtons.OK);
                else {
                    _ = MessageBox.Show("Book checked in!", "Check-In Success", MessageBoxButtons.OK);
                    SearchButton_Click(sender, e);
                }     
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            finally {
                conn.Close();
            }
        }
    }
}
