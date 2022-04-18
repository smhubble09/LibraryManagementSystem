using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryManagementSystem {
    public partial class StartForm : Form {
        //Used to set username to Title Case
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        public static string LoggedUserName = "";
        String[] ArrayLoggedUserName;

        //Access Data Connection
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\repos\LibraryManagementSystem\LibraryManagementDatabase.accdb; Jet OLEDB:Database Password=libraryApp22!";
        
        public StartForm() {
            InitializeComponent();
            this.ActiveControl = UsernameInput;
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
                OleDbConnection conn = new OleDbConnection(connectionString);
                string query = "SELECT * from Users WHERE Username LIKE @username and password=@password;";
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
                    ArrayLoggedUserName = UsernameInput.Text.Split('.');
                    LoggedUserName = textInfo.ToTitleCase(ArrayLoggedUserName[0]);
                    var userForm = new UserForm();
                    userForm.Show();
                    this.Hide();
                }
                else {
                    DialogResult dialog = MessageBox.Show("Invalid username/password.", "Invalid login", MessageBoxButtons.OK);
                    PasswordInput.Text = "";
                }

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
                string query = "SELECT * from Librarians WHERE Username LIKE @username and password=@password;";
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
                    ArrayLoggedUserName = UsernameInput.Text.Split('.');
                    LoggedUserName = textInfo.ToTitleCase(ArrayLoggedUserName[0]);
                    var librarianForm = new LibrarianForm();
                    librarianForm.Show();
                    this.Hide();
                }
                else {
                    DialogResult dialog = MessageBox.Show("Invalid username/password.", "Invalid login", MessageBoxButtons.OK);
                    PasswordInput.Text = "";
                }
                    
            }   
        }
    }
}
