using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace GymManagement
{
    public partial class MainDashboardForm : Form
    {
        private readonly Form callerLoginForm;
        private Guna2Button[] navButtons;
        private UserControl currentScreenControl;

        public MainDashboardForm(Form loginForm = null)
        {
            this.callerLoginForm = loginForm;
            InitializeComponent();
        }

        private void MainDashboardForm_Load(object sender, EventArgs e)
        {
            navButtons = new Guna2Button[] {
                btnNavDashboard, btnNavMembers, btnNavMemberDetails, btnNavMemberships,
                btnNavSubscriptions, btnNavPayments, btnNavAttendance, btnNavTrainers,
                btnNavWorkoutPlans, btnNavExpenses, btnNavReports, btnNavStaff,
                btnNavEquipment, btnNavNotifications, btnNavSettings, btnNavBackup
            };

            // Set application window and taskbar icon
            try
            {
                var ico = AppResources.GetIcon("app.ico");
                if (ico != null)
                {
                    this.Icon = ico;
                }
            }
            catch { }

            // Set user initial avatar
            pbUserAvatar.Image = CreateInitialsAvatar("EM", Color.FromArgb(255, 95, 21));

            // Load initial view: Dashboard
            NavigateTo(0);
        }

        private Image CreateInitialsAvatar(string initials, Color bg)
        {
            Bitmap bmp = new Bitmap(44, 44);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Brush b = new SolidBrush(bg))
                {
                    g.FillEllipse(b, 0, 0, 43, 43);
                }
                using (Font f = new Font("Segoe UI", 12F, FontStyle.Bold))
                using (Brush textBrush = new SolidBrush(Color.White))
                {
                    SizeF size = g.MeasureString(initials, f);
                    g.DrawString(initials, f, textBrush, (44 - size.Width) / 2, (44 - size.Height) / 2);
                }
            }
            return bmp;
        }

        public void NavigateTo(int index)
        {
            string[] iconTypes = new string[] {
                "dashboard", "members", "member_details", "memberships",
                "subscriptions", "payments", "attendance", "trainers",
                "workout", "expenses", "reports", "staff",
                "equipment", "notifications", "settings", "backup"
            };

            for (int i = 0; i < navButtons.Length; i++)
            {
                if (navButtons[i] == null) continue;
                if (i == index)
                {
                    navButtons[i].FillColor = Color.FromArgb(34, 38, 50);
                    navButtons[i].ForeColor = Color.FromArgb(255, 120, 40);
                    if (i < iconTypes.Length)
                        navButtons[i].Image = IconHelper.CreateVectorIcon(iconTypes[i], Color.FromArgb(255, 120, 40), 18);

                    if (pnlNavIndicator != null)
                    {
                        pnlNavIndicator.Location = new Point(2, navButtons[i].Top + 4);
                        pnlNavIndicator.BringToFront();
                    }
                }
                else
                {
                    navButtons[i].FillColor = Color.Transparent;
                    navButtons[i].ForeColor = Color.FromArgb(145, 150, 165);
                    if (i < iconTypes.Length)
                        navButtons[i].Image = IconHelper.CreateVectorIcon(iconTypes[i], Color.FromArgb(145, 150, 165), 18);
                }
            }

            contentPanel.SuspendLayout();
            contentPanel.Controls.Clear();
            if (currentScreenControl != null)
            {
                currentScreenControl.Dispose();
                currentScreenControl = null;
            }

            UserControl screen = ScreenViewsFactory.CreateScreen(index, this);
            if (screen != null)
            {
                screen.Dock = DockStyle.Fill;
                contentPanel.Controls.Add(screen);
                currentScreenControl = screen;
            }
            contentPanel.ResumeLayout(true);
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (currentScreenControl is ISearchable searchable)
            {
                searchable.ApplySearch(txtSearch.Text);
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("هل أنت متأكد من رغبتك في تسجيل الخروج؟", "تسجيل الخروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                this.Hide();
                if (callerLoginForm != null)
                {
                    callerLoginForm.Show();
                }
                else
                {
                    new LoginForm().Show();
                }
                this.Close();
            }
        }
    }

    public interface ISearchable
    {
        void ApplySearch(string term);
    }
}
