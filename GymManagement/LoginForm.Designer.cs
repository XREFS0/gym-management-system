using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace GymManagement
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        private Guna2Elipse formElipse;
        private Guna2ShadowForm formShadow;
        private Guna2DragControl dragControl;
        private Panel bgWrapperPanel;
        private Guna2Panel leftCard;
        private Guna2ControlBox btnClose;
        private Guna2ControlBox btnMinimize;

        private Label lblLogoIcon;
        private Label lblBrandTitle;
        private Label lblBrandSub;
        private Label lblLoginTitle;
        private Label lblSubTitle;
        private Guna2TextBox txtEmail;
        private Guna2TextBox txtPassword;
        private Guna2CheckBox chkRemember;
        private LinkLabel lnkForgotPassword;
        private Guna2Button btnLogin;

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
            this.formShadow = new Guna2ShadowForm(this.components);
            this.dragControl = new Guna2DragControl(this.components);

            this.bgWrapperPanel = new Panel();
            this.leftCard = new Guna2Panel();
            this.btnClose = new Guna2ControlBox();
            this.btnMinimize = new Guna2ControlBox();

            this.lblLogoIcon = new Label();
            this.lblBrandTitle = new Label();
            this.lblBrandSub = new Label();
            this.lblLoginTitle = new Label();
            this.lblSubTitle = new Label();

            this.txtEmail = new Guna2TextBox();
            this.txtPassword = new Guna2TextBox();
            this.chkRemember = new Guna2CheckBox();
            this.lnkForgotPassword = new LinkLabel();
            this.btnLogin = new Guna2Button();

            this.leftCard.SuspendLayout();
            this.bgWrapperPanel.SuspendLayout();
            this.SuspendLayout();

            //
            // formElipse
            //
            this.formElipse.BorderRadius = 40;
            this.formElipse.TargetControl = this;

            //
            // formShadow
            //
            this.formShadow.BorderRadius = 40;
            this.formShadow.TargetForm = this;

            //
            // dragControl
            //
            this.dragControl.TargetControl = this.bgWrapperPanel;

            //
            // bgWrapperPanel (Main Container with full seamless background image)
            //
            this.bgWrapperPanel.BackgroundImageLayout = ImageLayout.Zoom;
            this.bgWrapperPanel.BackColor = Color.FromArgb(228, 208, 235);
            this.bgWrapperPanel.Controls.Add(this.btnClose);
            this.bgWrapperPanel.Controls.Add(this.btnMinimize);
            this.bgWrapperPanel.Controls.Add(this.leftCard);
            this.bgWrapperPanel.Dock = DockStyle.Fill;
            this.bgWrapperPanel.Location = new Point(0, 0);
            this.bgWrapperPanel.Name = "bgWrapperPanel";
            this.bgWrapperPanel.Size = new Size(1100, 680);
            this.bgWrapperPanel.TabIndex = 0;

            //
            // btnClose
            //
            this.btnClose.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.BorderRadius = 15;
            this.btnClose.FillColor = Color.FromArgb(40, 255, 255, 255);
            this.btnClose.IconColor = Color.FromArgb(50, 40, 70);
            this.btnClose.Location = new Point(1045, 18);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(36, 36);
            this.btnClose.TabIndex = 2;
            this.btnClose.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            this.btnClose.HoverState.IconColor = Color.White;

            //
            // btnMinimize
            //
            this.btnMinimize.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnMinimize.BackColor = Color.Transparent;
            this.btnMinimize.BorderRadius = 15;
            this.btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.btnMinimize.FillColor = Color.FromArgb(40, 255, 255, 255);
            this.btnMinimize.IconColor = Color.FromArgb(50, 40, 70);
            this.btnMinimize.Location = new Point(1000, 18);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new Size(36, 36);
            this.btnMinimize.TabIndex = 3;

            //
            // leftCard (Glassmorphic Translucent Curved Floating Card)
            //
            this.leftCard.BackColor = Color.Transparent;
            this.leftCard.BorderColor = Color.FromArgb(200, 255, 255, 255);
            this.leftCard.BorderRadius = 36;
            this.leftCard.BorderThickness = 1;
            this.leftCard.FillColor = Color.FromArgb(240, 255, 255, 255);
            this.leftCard.Controls.Add(this.lblLogoIcon);
            this.leftCard.Controls.Add(this.lblBrandTitle);
            this.leftCard.Controls.Add(this.lblBrandSub);
            this.leftCard.Controls.Add(this.lblLoginTitle);
            this.leftCard.Controls.Add(this.lblSubTitle);
            this.leftCard.Controls.Add(this.txtEmail);
            this.leftCard.Controls.Add(this.txtPassword);
            this.leftCard.Controls.Add(this.chkRemember);
            this.leftCard.Controls.Add(this.lnkForgotPassword);
            this.leftCard.Controls.Add(this.btnLogin);
            this.leftCard.Location = new Point(50, 60);
            this.leftCard.Name = "leftCard";
            this.leftCard.ShadowDecoration.BorderRadius = 36;
            this.leftCard.ShadowDecoration.Color = Color.FromArgb(80, 50, 110);
            this.leftCard.ShadowDecoration.Depth = 25;
            this.leftCard.ShadowDecoration.Enabled = true;
            this.leftCard.ShadowDecoration.Shadow = new Padding(0, 10, 20, 20);
            this.leftCard.Size = new Size(460, 560);
            this.leftCard.TabIndex = 0;

            //
            // lblLogoIcon
            //
            this.lblLogoIcon.AutoSize = true;
            this.lblLogoIcon.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            this.lblLogoIcon.ForeColor = Color.FromArgb(25, 20, 45);
            this.lblLogoIcon.Location = new Point(140, 42);
            this.lblLogoIcon.Name = "lblLogoIcon";
            this.lblLogoIcon.Size = new Size(41, 47);
            this.lblLogoIcon.TabIndex = 12;
            this.lblLogoIcon.Text = "e";

            //
            // lblBrandTitle
            //
            this.lblBrandTitle.AutoSize = true;
            this.lblBrandTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            this.lblBrandTitle.ForeColor = Color.FromArgb(25, 20, 45);
            this.lblBrandTitle.Location = new Point(184, 44);
            this.lblBrandTitle.Name = "lblBrandTitle";
            this.lblBrandTitle.Size = new Size(136, 28);
            this.lblBrandTitle.TabIndex = 0;
            this.lblBrandTitle.Text = "F I T N E S S";

            //
            // lblBrandSub
            //
            this.lblBrandSub.AutoSize = true;
            this.lblBrandSub.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblBrandSub.ForeColor = Color.FromArgb(120, 120, 140);
            this.lblBrandSub.Location = new Point(186, 72);
            this.lblBrandSub.Name = "lblBrandSub";
            this.lblBrandSub.Size = new Size(106, 15);
            this.lblBrandSub.TabIndex = 1;
            this.lblBrandSub.Text = "Gym management";

            //
            // lblLoginTitle
            //
            this.lblLoginTitle.AutoSize = true;
            this.lblLoginTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            this.lblLoginTitle.ForeColor = Color.FromArgb(25, 25, 35);
            this.lblLoginTitle.Location = new Point(180, 135);
            this.lblLoginTitle.Name = "lblLoginTitle";
            this.lblLoginTitle.Size = new Size(97, 37);
            this.lblLoginTitle.TabIndex = 2;
            this.lblLoginTitle.Text = "Log in";

            //
            // lblSubTitle
            //
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new Font("Segoe UI", 9F);
            this.lblSubTitle.ForeColor = Color.FromArgb(140, 140, 160);
            this.lblSubTitle.Location = new Point(135, 175);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new Size(187, 15);
            this.lblSubTitle.TabIndex = 3;
            this.lblSubTitle.Text = "Enter your credentials to continue";

            //
            // txtEmail
            //
            this.txtEmail.BorderColor = Color.FromArgb(226, 228, 238);
            this.txtEmail.BorderRadius = 14;
            this.txtEmail.Cursor = Cursors.IBeam;
            this.txtEmail.DefaultText = "admin@fitness.com";
            this.txtEmail.FillColor = Color.FromArgb(248, 249, 253);
            this.txtEmail.Font = new Font("Segoe UI", 10.5F);
            this.txtEmail.ForeColor = Color.FromArgb(40, 40, 60);
            this.txtEmail.HoverState.BorderColor = Color.FromArgb(150, 120, 230);
            this.txtEmail.FocusedState.BorderColor = Color.FromArgb(120, 90, 210);
            this.txtEmail.Location = new Point(50, 215);
            this.txtEmail.Margin = new Padding(4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PasswordChar = '\0';
            this.txtEmail.PlaceholderForeColor = Color.FromArgb(165, 165, 185);
            this.txtEmail.PlaceholderText = "Email";
            this.txtEmail.SelectedText = "";
            this.txtEmail.Size = new Size(360, 52);
            this.txtEmail.TabIndex = 4;
            this.txtEmail.TextOffset = new Point(12, 0);

            //
            // txtPassword
            //
            this.txtPassword.BorderColor = Color.FromArgb(226, 228, 238);
            this.txtPassword.BorderRadius = 14;
            this.txtPassword.Cursor = Cursors.IBeam;
            this.txtPassword.DefaultText = "admin123";
            this.txtPassword.FillColor = Color.FromArgb(248, 249, 253);
            this.txtPassword.Font = new Font("Segoe UI", 10.5F);
            this.txtPassword.ForeColor = Color.FromArgb(40, 40, 60);
            this.txtPassword.HoverState.BorderColor = Color.FromArgb(150, 120, 230);
            this.txtPassword.FocusedState.BorderColor = Color.FromArgb(120, 90, 210);
            this.txtPassword.Location = new Point(50, 282);
            this.txtPassword.Margin = new Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.PlaceholderForeColor = Color.FromArgb(165, 165, 185);
            this.txtPassword.PlaceholderText = "Password";
            this.txtPassword.SelectedText = "";
            this.txtPassword.Size = new Size(360, 52);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.TextOffset = new Point(12, 0);

            //
            // chkRemember
            //
            this.chkRemember.AutoSize = true;
            this.chkRemember.Checked = true;
            this.chkRemember.CheckedState.BorderColor = Color.FromArgb(20, 20, 30);
            this.chkRemember.CheckedState.BorderRadius = 4;
            this.chkRemember.CheckedState.BorderThickness = 0;
            this.chkRemember.CheckedState.FillColor = Color.FromArgb(20, 20, 30);
            this.chkRemember.CheckState = CheckState.Checked;
            this.chkRemember.Font = new Font("Segoe UI", 9F);
            this.chkRemember.ForeColor = Color.FromArgb(110, 110, 130);
            this.chkRemember.Location = new Point(52, 350);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new Size(124, 19);
            this.chkRemember.TabIndex = 6;
            this.chkRemember.Text = "Keep me logged in";
            this.chkRemember.UncheckedState.BorderColor = Color.FromArgb(200, 200, 215);
            this.chkRemember.UncheckedState.BorderRadius = 4;
            this.chkRemember.UncheckedState.BorderThickness = 1;
            this.chkRemember.UncheckedState.FillColor = Color.White;

            //
            // lnkForgotPassword
            //
            this.lnkForgotPassword.ActiveLinkColor = Color.FromArgb(140, 80, 240);
            this.lnkForgotPassword.AutoSize = true;
            this.lnkForgotPassword.Font = new Font("Segoe UI", 9F);
            this.lnkForgotPassword.LinkBehavior = LinkBehavior.NeverUnderline;
            this.lnkForgotPassword.LinkColor = Color.FromArgb(120, 120, 140);
            this.lnkForgotPassword.Location = new Point(310, 351);
            this.lnkForgotPassword.Name = "lnkForgotPassword";
            this.lnkForgotPassword.Size = new Size(100, 15);
            this.lnkForgotPassword.TabIndex = 7;
            this.lnkForgotPassword.TabStop = true;
            this.lnkForgotPassword.Text = "Forgot password?";
            this.lnkForgotPassword.LinkClicked += new LinkLabelLinkClickedEventHandler(this.LnkForgotPassword_LinkClicked);

            //
            // btnLogin
            //
            this.btnLogin.Animated = true;
            this.btnLogin.BorderRadius = 14;
            this.btnLogin.Cursor = Cursors.Hand;
            this.btnLogin.FillColor = Color.FromArgb(18, 16, 26);
            this.btnLogin.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.HoverState.FillColor = Color.FromArgb(45, 35, 75);
            this.btnLogin.Location = new Point(50, 400);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.ShadowDecoration.BorderRadius = 14;
            this.btnLogin.ShadowDecoration.Color = Color.FromArgb(100, 50, 120);
            this.btnLogin.ShadowDecoration.Enabled = true;
            this.btnLogin.ShadowDecoration.Depth = 12;
            this.btnLogin.Size = new Size(360, 52);
            this.btnLogin.TabIndex = 8;
            this.btnLogin.Text = "Log in";
            this.btnLogin.Click += new EventHandler(this.BtnLogin_Click);

            //
            // LoginForm
            //
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(250, 240, 248);
            this.ClientSize = new Size(1100, 680);
            this.Controls.Add(this.bgWrapperPanel);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "LoginForm";
            this.Padding = new Padding(12);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Fitness Gym - Login";
            this.Load += new EventHandler(this.LoginForm_Load);

            this.leftCard.ResumeLayout(false);
            this.leftCard.PerformLayout();
            this.bgWrapperPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
