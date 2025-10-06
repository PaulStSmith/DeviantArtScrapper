namespace DeviantArtScrapper.Forms
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblClientId;
        private TextBox txtClientId;
        private Label lblClientSecret;
        private TextBox txtClientSecret;
        private Label lblTokenInfo;
        private Label lblTokenStatus;
        private Button btnTestConnection;
        private Button btnSave;
        private Button btnCancel;
        private GroupBox grpApiSettings;
        private GroupBox grpTokenInfo;

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
            lblTitle = new Label();
            grpApiSettings = new GroupBox();
            btnShowPassword = new Button();
            lblClientId = new Label();
            txtClientId = new TextBox();
            lblClientSecret = new Label();
            txtClientSecret = new TextBox();
            grpTokenInfo = new GroupBox();
            lblTokenInfo = new Label();
            lblTokenStatus = new Label();
            btnTestConnection = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            grpApiSettings.SuspendLayout();
            grpTokenInfo.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(174, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "API Configuration";
            // 
            // grpApiSettings
            // 
            grpApiSettings.Controls.Add(btnShowPassword);
            grpApiSettings.Controls.Add(lblClientId);
            grpApiSettings.Controls.Add(txtClientId);
            grpApiSettings.Controls.Add(lblClientSecret);
            grpApiSettings.Controls.Add(txtClientSecret);
            grpApiSettings.Location = new Point(20, 60);
            grpApiSettings.Name = "grpApiSettings";
            grpApiSettings.Size = new Size(440, 160);
            grpApiSettings.TabIndex = 1;
            grpApiSettings.TabStop = false;
            grpApiSettings.Text = "DeviantArt API Settings";
            // 
            // btnShowPassword
            // 
            btnShowPassword.Location = new Point(386, 118);
            btnShowPassword.Name = "btnShowPassword";
            btnShowPassword.Size = new Size(32, 31);
            btnShowPassword.TabIndex = 3;
            btnShowPassword.Text = "🔒";
            btnShowPassword.UseVisualStyleBackColor = true;
            // 
            // lblClientId
            // 
            lblClientId.AutoSize = true;
            lblClientId.Location = new Point(15, 30);
            lblClientId.Name = "lblClientId";
            lblClientId.Size = new Size(55, 15);
            lblClientId.TabIndex = 0;
            lblClientId.Text = "Client ID:";
            // 
            // txtClientId
            // 
            txtClientId.Location = new Point(15, 48);
            txtClientId.Name = "txtClientId";
            txtClientId.Size = new Size(403, 23);
            txtClientId.TabIndex = 1;
            // 
            // lblClientSecret
            // 
            lblClientSecret.AutoSize = true;
            lblClientSecret.Location = new Point(15, 101);
            lblClientSecret.Name = "lblClientSecret";
            lblClientSecret.Size = new Size(76, 15);
            lblClientSecret.TabIndex = 2;
            lblClientSecret.Text = "Client Secret:";
            lblClientSecret.Click += lblClientSecret_Click;
            // 
            // txtClientSecret
            // 
            txtClientSecret.Location = new Point(15, 122);
            txtClientSecret.Name = "txtClientSecret";
            txtClientSecret.PasswordChar = '•';
            txtClientSecret.Size = new Size(365, 23);
            txtClientSecret.TabIndex = 2;
            // 
            // grpTokenInfo
            // 
            grpTokenInfo.Controls.Add(lblTokenInfo);
            grpTokenInfo.Controls.Add(lblTokenStatus);
            grpTokenInfo.Location = new Point(20, 226);
            grpTokenInfo.Name = "grpTokenInfo";
            grpTokenInfo.Size = new Size(440, 80);
            grpTokenInfo.TabIndex = 2;
            grpTokenInfo.TabStop = false;
            grpTokenInfo.Text = "Authentication Status";
            // 
            // lblTokenInfo
            // 
            lblTokenInfo.AutoSize = true;
            lblTokenInfo.Location = new Point(15, 25);
            lblTokenInfo.Name = "lblTokenInfo";
            lblTokenInfo.Size = new Size(77, 15);
            lblTokenInfo.TabIndex = 0;
            lblTokenInfo.Text = "Token Status:";
            // 
            // lblTokenStatus
            // 
            lblTokenStatus.AutoSize = true;
            lblTokenStatus.Location = new Point(15, 45);
            lblTokenStatus.Name = "lblTokenStatus";
            lblTokenStatus.Size = new Size(105, 15);
            lblTokenStatus.TabIndex = 1;
            lblTokenStatus.Text = "No token available";
            // 
            // btnTestConnection
            // 
            btnTestConnection.Location = new Point(20, 326);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(120, 35);
            btnTestConnection.TabIndex = 4;
            btnTestConnection.Text = "Test Connection";
            btnTestConnection.UseVisualStyleBackColor = true;
            btnTestConnection.Click += btnTestConnection_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(260, 326);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 35);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(370, 326);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 35);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 385);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(btnTestConnection);
            Controls.Add(grpTokenInfo);
            Controls.Add(grpApiSettings);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings - DeviantArt Scrapper";
            grpApiSettings.ResumeLayout(false);
            grpApiSettings.PerformLayout();
            grpTokenInfo.ResumeLayout(false);
            grpTokenInfo.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        /// Applies localized text to all UI controls
        /// </summary>
        private void LocalizeControls()
        {
            // Form title
            this.Text = Localization.Localizer.SettingsWindowTitle;
            lblTitle.Text = Localization.Localizer.SettingsTitle;

            // API Settings group
            grpApiSettings.Text = Localization.Localizer.SettingsApiSettings;
            lblClientId.Text = Localization.Localizer.SettingsClientId;
            lblClientSecret.Text = Localization.Localizer.SettingsClientSecret;

            // Authentication Status group
            grpTokenInfo.Text = Localization.Localizer.SettingsAuthStatus;
            lblTokenInfo.Text = Localization.Localizer.SettingsTokenStatus;

            // Buttons
            btnTestConnection.Text = Localization.Localizer.ButtonTestConnection;
            btnSave.Text = Localization.Localizer.ButtonSave;
            btnCancel.Text = Localization.Localizer.ButtonCancel;
        }

        private Button btnShowPassword;
    }
}