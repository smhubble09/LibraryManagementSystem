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
    public partial class StartForm : Form {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\repos\noteTaker\NotesApp.accdb; Jet OLEDB:Database Password=libraryApp22!";
        public StartForm() {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void UserButton_Click(object sender, EventArgs e) {
            if (UsernameInput.Text == "") {
                DialogResult dialog = MessageBox.Show("Please provide a username.", "Missing Field", MessageBoxButtons.OK);
            }
            else if (PasswordInput.Text == "") {
                DialogResult dialog = MessageBox.Show("Please provide a password.", "Missing Field", MessageBoxButtons.OK);
            }
            else {
                var userForm = new UserForm();
                userForm.Show();
                this.Hide();
            }
        }

        private void LibrarianButton_Click(object sender, EventArgs e) {
            if (UsernameInput.Text == "") {
                DialogResult dialog = MessageBox.Show("Please provide a username.", "Missing Field", MessageBoxButtons.OK);
            }
            else if (PasswordInput.Text == "") {
                DialogResult dialog = MessageBox.Show("Please provide a password.", "Missing Field", MessageBoxButtons.OK);
            }
            else {
                OleDbConnection conn = new OleDbConnection(connectionString);
                string query = "select * from Users where username like @username and password = @password;";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", UsernameInput.Text);
                cmd.Parameters.AddWithValue("@password", PasswordInput.Text);

                conn.Open();
                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                conn.Close();

                bool loginSuccessful = ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0));

                if (loginSuccessful) {
                    var librarianForm = new LibrarianForm();
                    librarianForm.Show();
                    this.Hide();
                }
                else {
                    DialogResult dialog = MessageBox.Show("Invalid username/password.", "Invalid login", MessageBoxButtons.OK);
                }
                    
            }   
        }
    }
}
