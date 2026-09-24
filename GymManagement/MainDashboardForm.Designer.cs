using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace GymManagement
{
    partial class MainDashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        private Guna2Elipse formElipse;
        private Guna2DragControl dragControl;
        private Guna2ShadowForm shadowForm;

        // Main Layout
        private Panel sidebarPanel;
        private Panel topbarPanel;
        private Panel contentPanel;

        // Sidebar Elements
        private PictureBox pbLogo;
        private Label lblLogoTitle;
        private Panel menuItemsPanel;
        private Guna2Panel pnlNavIndicator;
        private Guna2Button btnLogout;

        // Navigation Buttons
        private Guna2Button btnNavDashboard;
        private Guna2Button btnNavMembers;
        private Guna2Button btnNavMemberDetails;
        private Guna2Button btnNavMemberships;
        private Guna2Button btnNavSubscriptions;
        private Guna2Button btnNavPayments;
        private Guna2Button btnNavAttendance;
        private Guna2Button btnNavTrainers;
        private Guna2Button btnNavWorkoutPlans;
        private Guna2Button btnNavExpenses;
        private Guna2Button btnNavReports;
        private Guna2Button btnNavStaff;
        private Guna2Button btnNavEquipment;
        private Guna2Button btnNavNotifications;
        private Guna2Button btnNavSettings;
        private Guna2Button btnNavBackup;

        // Topbar Elements
        private Guna2TextBox txtSearch;
        private Guna2CirclePictureBox pbUserAvatar;
        private Label lblUserName;
        private Label lblUserRole;
        private Guna2ControlBox btnClose;
        private Guna2ControlBox btnMaximize;
        private Guna2ControlBox btnMinimize;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.formElipse = new Guna2Elipse(this.components);
            this.dragControl = new Guna2DragControl(this.components);
            this.shadowForm = new Guna2ShadowForm(this.components);

            this.sidebarPanel = new Panel();
            this.topbarPanel = new Panel();
            this.contentPanel = new Panel();

            this.pbLogo = new PictureBox();
            this.lblLogoTitle = new Label();
            this.menuItemsPanel = new Panel();
            this.btnLogout = new Guna2Button();

            this.btnNavDashboard = new Guna2Button();
            this.btnNavMembers = new Guna2Button();
            this.btnNavMemberDetails = new Guna2Button();
            this.btnNavMemberships = new Guna2Button();
            this.btnNavSubscriptions = new Guna2Button();
            this.btnNavPayments = new Guna2Button();
            this.btnNavAttendance = new Guna2Button();
            this.btnNavTrainers = new Guna2Button();
            this.btnNavWorkoutPlans = new Guna2Button();
            this.btnNavExpenses = new Guna2Button();
            this.btnNavReports = new Guna2Button();
            this.btnNavStaff = new Guna2Button();
            this.btnNavEquipment = new Guna2Button();
            this.btnNavNotifications = new Guna2Button();
            this.btnNavSettings = new Guna2Button();
            this.btnNavBackup = new Guna2Button();

            this.txtSearch = new Guna2TextBox();
            this.pbUserAvatar = new Guna2CirclePictureBox();
            this.lblUserName = new Label();
            this.lblUserRole = new Label();
            this.btnClose = new Guna2ControlBox();
            this.btnMaximize = new Guna2ControlBox();
            this.btnMinimize = new Guna2ControlBox();

            this.sidebarPanel.SuspendLayout();
            this.menuItemsPanel.SuspendLayout();
            this.topbarPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUserAvatar)).BeginInit();
            this.SuspendLayout();

            //
            // formElipse
            //
            this.formElipse.BorderRadius = 30;
            this.formElipse.TargetControl = this;

            //
            // shadowForm
            //
            this.shadowForm.BorderRadius = 30;
            this.shadowForm.TargetForm = this;

            //
            // dragControl
            //
            this.dragControl.TargetControl = this.topbarPanel;

            //
            // sidebarPanel (Deep Charcoal Slate #181B24 - Exact match to original design)
            //
            this.sidebarPanel.BackColor = Color.FromArgb(24, 27, 36);
            this.sidebarPanel.Controls.Add(this.btnLogout);
            this.sidebarPanel.Controls.Add(this.menuItemsPanel);
            this.sidebarPanel.Controls.Add(this.lblLogoTitle);
            this.sidebarPanel.Controls.Add(this.pbLogo);
            this.sidebarPanel.Dock = DockStyle.Left;
            this.sidebarPanel.Location = new Point(0, 0);
            this.sidebarPanel.Name = "sidebarPanel";
            this.sidebarPanel.Size = new Size(230, 800);
            this.sidebarPanel.TabIndex = 0;

            //
            // pbLogo (dumbbell.png)
            //
            this.pbLogo.BackColor = Color.Transparent;
            this.pbLogo.Location = new Point(18, 18);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new Size(38, 38);
            this.pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 0;
            this.pbLogo.TabStop = false;
            this.pbLogo.Image = AppResources.GetImage("dumbbell.png");

            //
            // lblLogoTitle
            //
            this.lblLogoTitle.AutoSize = true;
            this.lblLogoTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblLogoTitle.ForeColor = Color.White;
            this.lblLogoTitle.Location = new Point(62, 22);
            this.lblLogoTitle.Name = "lblLogoTitle";
            this.lblLogoTitle.Size = new Size(88, 30);
            this.lblLogoTitle.TabIndex = 1;
            this.lblLogoTitle.Text = "FitFlow";

            //
            // menuItemsPanel (Navigation List - sleek, extended height, no awkward plan card)
            //
            this.menuItemsPanel.AutoScroll = true;
            this.menuItemsPanel.Controls.Add(this.btnNavBackup);
            this.menuItemsPanel.Controls.Add(this.btnNavSettings);
            this.menuItemsPanel.Controls.Add(this.btnNavNotifications);
            this.menuItemsPanel.Controls.Add(this.btnNavEquipment);
            this.menuItemsPanel.Controls.Add(this.btnNavStaff);
            this.menuItemsPanel.Controls.Add(this.btnNavReports);
            this.menuItemsPanel.Controls.Add(this.btnNavExpenses);
            this.menuItemsPanel.Controls.Add(this.btnNavWorkoutPlans);
            this.menuItemsPanel.Controls.Add(this.btnNavTrainers);
            this.menuItemsPanel.Controls.Add(this.btnNavAttendance);
            this.menuItemsPanel.Controls.Add(this.btnNavPayments);
            this.menuItemsPanel.Controls.Add(this.btnNavSubscriptions);
            this.menuItemsPanel.Controls.Add(this.btnNavMemberships);
            this.menuItemsPanel.Controls.Add(this.btnNavMemberDetails);
            this.menuItemsPanel.Controls.Add(this.btnNavMembers);
            this.menuItemsPanel.Controls.Add(this.btnNavDashboard);
            this.menuItemsPanel.Location = new Point(10, 75);
            this.menuItemsPanel.Name = "menuItemsPanel";
            this.menuItemsPanel.Size = new Size(215, 650);
            this.menuItemsPanel.TabIndex = 2;

            // Left Indicator Strip for active tab (Ultra luxury SaaS style)
            this.pnlNavIndicator = new Guna2Panel();
            this.pnlNavIndicator.BorderRadius = 3;
            this.pnlNavIndicator.FillColor = Color.FromArgb(255, 95, 21);
            this.pnlNavIndicator.Location = new Point(2, 6);
            this.pnlNavIndicator.Name = "pnlNavIndicator";
            this.pnlNavIndicator.Size = new Size(4, 32);
            this.pnlNavIndicator.TabIndex = 99;
            this.menuItemsPanel.Controls.Add(this.pnlNavIndicator);

            // Nav Button Styling Helper Function inline
            Guna2Button[] navs = new Guna2Button[] {
                btnNavDashboard, btnNavMembers, btnNavMemberDetails, btnNavMemberships,
                btnNavSubscriptions, btnNavPayments, btnNavAttendance, btnNavTrainers,
                btnNavWorkoutPlans, btnNavExpenses, btnNavReports, btnNavStaff,
                btnNavEquipment, btnNavNotifications, btnNavSettings, btnNavBackup
            };

            string[] titles = new string[] {
                "Dashboard", "Members", "Member Details", "Memberships",
                "Subscriptions", "Payments", "Attendance", "Trainers",
                "Workout Plans", "Expenses", "Reports", "Staff & Roles",
                "Equipment", "Notifications", "Settings", "Backup & Restore"
            };

            string[] iconTypes = new string[] {
                "dashboard", "members", "member_details", "memberships",
                "subscriptions", "payments", "attendance", "trainers",
                "workout", "expenses", "reports", "staff",
                "equipment", "notifications", "settings", "backup"
            };

            int topPos = 4;
            for (int i = 0; i < navs.Length; i++)
            {
                var btn = navs[i];
                btn.Animated = true;
                btn.BorderRadius = 10;
                btn.Cursor = Cursors.Hand;
                btn.FillColor = (i == 0) ? Color.FromArgb(34, 38, 50) : Color.Transparent;
                btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                btn.ForeColor = (i == 0) ? Color.FromArgb(255, 120, 40) : Color.FromArgb(145, 150, 165);
                btn.HoverState.FillColor = Color.FromArgb(29, 33, 44);
                btn.HoverState.ForeColor = Color.White;
                btn.Location = new Point(10, topPos);
                btn.Name = $"btnNav{i}";
                btn.Size = new Size(185, 40);
                btn.Text = titles[i];
                btn.TextAlign = HorizontalAlignment.Left;
                btn.TextOffset = new Point(8, 0);

                // Professional high-resolution vector icon
                Color icoColor = (i == 0) ? Color.FromArgb(255, 120, 40) : Color.FromArgb(145, 150, 165);
                btn.Image = IconHelper.CreateVectorIcon(iconTypes[i], icoColor, 18);
                btn.ImageSize = new Size(18, 18);
                btn.ImageAlign = HorizontalAlignment.Left;
                btn.ImageOffset = new Point(6, 0);

                int index = i;
                btn.Click += (s, e) => NavigateTo(index);
                topPos += 44;
            }

            //
            // btnLogout
            //
            this.btnLogout.Animated = true;
            this.btnLogout.BorderRadius = 10;
            this.btnLogout.Cursor = Cursors.Hand;
            this.btnLogout.FillColor = Color.Transparent;
            this.btnLogout.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.btnLogout.ForeColor = Color.FromArgb(239, 68, 68);
            this.btnLogout.HoverState.FillColor = Color.FromArgb(45, 30, 35);
            this.btnLogout.Location = new Point(15, 745);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new Size(200, 42);
            this.btnLogout.Text = "Log Out";
            this.btnLogout.TextAlign = HorizontalAlignment.Left;
            this.btnLogout.TextOffset = new Point(8, 0);
            this.btnLogout.Image = IconHelper.CreateVectorIcon("logout", Color.FromArgb(239, 68, 68), 18);
            this.btnLogout.ImageSize = new Size(18, 18);
            this.btnLogout.ImageAlign = HorizontalAlignment.Left;
            this.btnLogout.ImageOffset = new Point(8, 0);
            this.btnLogout.Click += new EventHandler(this.BtnLogout_Click);

            //
            // topbarPanel (Dark Slate #1E222D)
            //
            this.topbarPanel.BackColor = Color.FromArgb(30, 34, 45);
            this.topbarPanel.Controls.Add(this.btnMinimize);
            this.topbarPanel.Controls.Add(this.btnMaximize);
            this.topbarPanel.Controls.Add(this.btnClose);
            this.topbarPanel.Controls.Add(this.lblUserRole);
            this.topbarPanel.Controls.Add(this.lblUserName);
            this.topbarPanel.Controls.Add(this.pbUserAvatar);
            this.topbarPanel.Controls.Add(this.txtSearch);
            this.topbarPanel.Dock = DockStyle.Top;
            this.topbarPanel.Location = new Point(230, 0);
            this.topbarPanel.Name = "topbarPanel";
            this.topbarPanel.Size = new Size(1110, 70);
            this.topbarPanel.TabIndex = 1;

            //
            // txtSearch
            //
            this.txtSearch.BorderColor = Color.FromArgb(45, 52, 68);
            this.txtSearch.BorderRadius = 12;
            this.txtSearch.Cursor = Cursors.IBeam;
            this.txtSearch.FillColor = Color.FromArgb(24, 27, 36);
            this.txtSearch.Font = new Font("Segoe UI", 9.5F);
            this.txtSearch.ForeColor = Color.White;
            this.txtSearch.HoverState.BorderColor = Color.FromArgb(255, 95, 21);
            this.txtSearch.Location = new Point(20, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderForeColor = Color.FromArgb(120, 125, 140);
            this.txtSearch.PlaceholderText = "🔍  Search members, subscriptions, payments...";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new Size(380, 40);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextOffset = new Point(8, 0);
            this.txtSearch.TextChanged += new EventHandler(this.TxtSearch_TextChanged);

            //
            // pbUserAvatar
            //
            this.pbUserAvatar.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.pbUserAvatar.FillColor = Color.FromArgb(255, 95, 21);
            this.pbUserAvatar.ImageRotate = 0F;
            this.pbUserAvatar.Location = new Point(800, 15);
            this.pbUserAvatar.Name = "pbUserAvatar";
            this.pbUserAvatar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.pbUserAvatar.Size = new Size(40, 40);
            this.pbUserAvatar.TabIndex = 1;
            this.pbUserAvatar.TabStop = false;

            //
            // lblUserName
            //
            this.lblUserName.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.lblUserName.ForeColor = Color.White;
            this.lblUserName.Location = new Point(848, 16);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new Size(88, 17);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "Captain Emon";

            //
            // lblUserRole
            //
            this.lblUserRole.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.lblUserRole.AutoSize = true;
            this.lblUserRole.Font = new Font("Segoe UI", 8F);
            this.lblUserRole.ForeColor = Color.FromArgb(140, 145, 160);
            this.lblUserRole.Location = new Point(848, 36);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new Size(71, 13);
            this.lblUserRole.TabIndex = 3;
            this.lblUserRole.Text = "Super Admin";

            //
            // btnClose
            //
            this.btnClose.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnClose.BorderRadius = 8;
            this.btnClose.FillColor = Color.Transparent;
            this.btnClose.IconColor = Color.FromArgb(160, 165, 180);
            this.btnClose.Location = new Point(1060, 18);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(36, 32);
            this.btnClose.TabIndex = 4;
            this.btnClose.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            this.btnClose.HoverState.IconColor = Color.White;

            //
            // btnMaximize
            //
            this.btnMaximize.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnMaximize.BorderRadius = 8;
            this.btnMaximize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox;
            this.btnMaximize.FillColor = Color.Transparent;
            this.btnMaximize.IconColor = Color.FromArgb(160, 165, 180);
            this.btnMaximize.Location = new Point(1020, 18);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new Size(36, 32);
            this.btnMaximize.TabIndex = 5;

            //
            // btnMinimize
            //
            this.btnMinimize.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnMinimize.BorderRadius = 8;
            this.btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.btnMinimize.FillColor = Color.Transparent;
            this.btnMinimize.IconColor = Color.FromArgb(160, 165, 180);
            this.btnMinimize.Location = new Point(980, 18);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new Size(36, 32);
            this.btnMinimize.TabIndex = 6;

            //
            // contentPanel (Deep Background Canvas #13151C)
            //
            this.contentPanel.BackColor = Color.FromArgb(19, 21, 28);
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.Location = new Point(230, 70);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new Size(1110, 730);
            this.contentPanel.TabIndex = 2;

            //
            // MainDashboardForm
            //
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.ClientSize = new Size(1340, 800);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.topbarPanel);
            this.Controls.Add(this.sidebarPanel);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "MainDashboardForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "FitFlow - Gym Management System";
            this.Load += new EventHandler(this.MainDashboardForm_Load);

            this.sidebarPanel.ResumeLayout(false);
            this.sidebarPanel.PerformLayout();
            this.menuItemsPanel.ResumeLayout(false);
            this.topbarPanel.ResumeLayout(false);
            this.topbarPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUserAvatar)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
