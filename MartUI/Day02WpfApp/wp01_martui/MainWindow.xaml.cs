using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wp01_martui.Logics;
using MessageBox = System.Windows.Forms.MessageBox;

namespace wp01_martui
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 로그인 버튼
        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {

            string strtxtId = "";
            string strtxtPassword = "";

            if (string.IsNullOrEmpty(txtId.Text))
            {
                await this.ShowMessageAsync("오류", "아이디를 입력하세요", MessageDialogStyle.Affirmative, null);
                return;
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                await this.ShowMessageAsync("오류", "패스워드를 입력하세요", MessageDialogStyle.Affirmative, null);
                return;
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    conn.Open();
                    string query = @"SELECT Id,
                                         PassWord
                                    FROM userdb
                                    WHERE Id = @Id
                                      AND Password = @Password;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlParameter prmId = new MySqlParameter("@Id", txtId.Text);
                    MySqlParameter prmPassword = new MySqlParameter("@PassWord", txtPassword.Text);

                    cmd.Parameters.Add(prmId);
                    cmd.Parameters.Add(prmPassword);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        strtxtId = reader["Id"] != null ? reader["Id"].ToString() : "-";
                        strtxtPassword = reader["Password"] != null ? reader["Password"].ToString() : "-";
                        Commons.isManager = true;
                        await this.ShowMessageAsync("로그인 성공!", "상품을 담아주세요", MessageDialogStyle.Affirmative, null);
                        this.Close();
                    }
                    else
                    {
                        txtId.Focus();
                        await this.ShowMessageAsync("로그인 실패", "회원 정보가 맞지 않습니다", MessageDialogStyle.Affirmative, null);
                        txtId.Text = "";
                        txtPassword.Text = "";
                    }
                }
            }
            catch (Exception ex) 
            {
                await this.ShowMessageAsync("로그인 오류", $"{ex.Message}", MessageDialogStyle.Affirmative, null);
            }
        }

        private void txtId_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnOk_Click(sender, e);
            }
        }

        private void txtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnOk_Click(sender, e);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtId.Focus();
        }
    }
}

               
