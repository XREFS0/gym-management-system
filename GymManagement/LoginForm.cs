using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace GymManagement
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                DatabaseHelper.InitializeDatabase();

                var ico = AppResources.GetIcon("app.ico");
                if (ico != null)
                {
                    this.Icon = ico;
                }

                var bgImg = AppResources.GetImage("gym_male_bg.jpg");
                if (bgImg != null)
                {
                    bgWrapperPanel.BackgroundImage = bgImg;
                    bgWrapperPanel.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("يرجى إدخال البريد الإلكتروني وكلمة المرور.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (DatabaseHelper.ValidateUser(email, password, out string fullName, out string role))
                {
                    this.Hide();
                    MainDashboardForm dashboard = new MainDashboardForm(this);
                    dashboard.Show();
                }
                else
                {
                    MessageBox.Show("بيانات الدخول غير صحيحة! يرجى التأكد من البريد الإلكتروني وكلمة المرور.", 
                                    "خطأ في تسجيل الدخول", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء الاتصال بقاعدة البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("تم إرسال تعليمات استعادة كلمة المرور إلى البريد الإلكتروني (الحساب الافتراضي: admin@fitness.com / admin123)", 
                            "استعادة الحساب", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
