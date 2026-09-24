using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using GymManagement;

namespace GymManagement
{
    public static class ScreenshotCaptureUtility
    {
        public static void RunCapture(string outputDir)
        {
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                DatabaseHelper.InitializeDatabase();

                // 1. Capture Login Form
                using (var loginForm = new LoginForm())
                {
                    loginForm.StartPosition = FormStartPosition.Manual;
                    loginForm.Location = new Point(100, 100);
                    loginForm.Show();
                    Application.DoEvents();
                    Thread.Sleep(500);
                    Application.DoEvents();

                    CaptureControl(loginForm, Path.Combine(outputDir, "00_Login_Screen.png"));
                    loginForm.Close();
                }

                // 2. Capture Dashboard and All 16 Internal Screens
                using (var dashboard = new MainDashboardForm())
                {
                    dashboard.StartPosition = FormStartPosition.Manual;
                    dashboard.Location = new Point(50, 50);
                    dashboard.Size = new Size(1366, 768);
                    dashboard.Show();
                    Application.DoEvents();
                    Thread.Sleep(500);
                    Application.DoEvents();

                    string[] screenNames = new string[]
                    {
                        "01_Dashboard_Overview",
                        "02_Members_Management",
                        "03_Member_Profile_Details",
                        "04_Membership_Plans",
                        "05_Subscriptions_Renewals",
                        "06_Payments_and_Receipts",
                        "07_Attendance_CheckIn",
                        "08_Trainers_Coaches",
                        "09_Workout_Plans",
                        "10_Expenses_Ledger",
                        "11_Financial_Reports",
                        "12_Staff_and_Roles",
                        "13_Equipment_Management",
                        "14_Expiry_Notifications",
                        "15_System_Settings",
                        "16_Backup_and_Restore"
                    };

                    for (int i = 0; i < screenNames.Length; i++)
                    {
                        dashboard.NavigateTo(i);
                        Application.DoEvents();
                        Thread.Sleep(600);
                        Application.DoEvents();

                        string outPath = Path.Combine(outputDir, $"{screenNames[i]}.png");
                        CaptureControl(dashboard, outPath);
                    }

                    dashboard.Close();
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(outputDir, "capture_error.log"), ex.ToString());
            }
        }

        private static void CaptureControl(Form form, string filePath)
        {
            using (Bitmap bmp = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
                bmp.Save(filePath, ImageFormat.Png);
            }
        }
    }
}
