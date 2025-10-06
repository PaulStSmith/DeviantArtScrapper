namespace DeviantArtScrapper.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnSettings;
        private Label lblStatus;
        private Label lblTitle;
        private TabControl tabMain;
        private TabPage tabGallery;
        private TabPage tabFavorites;

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
            btnSettings = new Button();
            lblStatus = new Label();
            lblTitle = new Label();
            tabMain = new TabControl();
            tabGallery = new TabPage();
            lblProgress = new Label();
            lblTransferRate = new Label();
            btnCancelScraping = new Button();
            progressScraping = new ProgressBar();
            grpFileFormat = new GroupBox();
            rbXLSX = new RadioButton();
            rbHTML = new RadioButton();
            rbCSV = new RadioButton();
            btnStartScraping = new Button();
            btnBrowse = new Button();
            txtFileName = new TextBox();
            lblFileName = new Label();
            txtUsername = new TextBox();
            lblUsername = new Label();
            tabFavorites = new TabPage();
            lblProgressFav = new Label();
            lblTransferRateFav = new Label();
            btnCancelScrapingFav = new Button();
            progressScrapingFav = new ProgressBar();
            grpFileFormatFav = new GroupBox();
            rbXLSXFav = new RadioButton();
            rbHTMLFav = new RadioButton();
            rbCSVFav = new RadioButton();
            btnStartScrapingFav = new Button();
            btnBrowseFav = new Button();
            txtFileNameFav = new TextBox();
            lblFileNameFav = new Label();
            txtUsernameFav = new TextBox();
            lblUsernameFav = new Label();
            tabMain.SuspendLayout();
            tabGallery.SuspendLayout();
            grpFileFormat.SuspendLayout();
            tabFavorites.SuspendLayout();
            grpFileFormatFav.SuspendLayout();
            SuspendLayout();
            // 
            // btnSettings
            // 
            btnSettings.Location = new Point(560, 460);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(100, 30);
            btnSettings.TabIndex = 3;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(20, 60);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(97, 15);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Status: Loading...";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(226, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "DeviantArt Scrapper";
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabGallery);
            tabMain.Controls.Add(tabFavorites);
            tabMain.Location = new Point(20, 90);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(640, 350);
            tabMain.TabIndex = 2;
            // 
            // tabGallery
            // 
            tabGallery.Controls.Add(lblProgress);
            tabGallery.Controls.Add(lblTransferRate);
            tabGallery.Controls.Add(btnCancelScraping);
            tabGallery.Controls.Add(progressScraping);
            tabGallery.Controls.Add(grpFileFormat);
            tabGallery.Controls.Add(btnStartScraping);
            tabGallery.Controls.Add(btnBrowse);
            tabGallery.Controls.Add(txtFileName);
            tabGallery.Controls.Add(lblFileName);
            tabGallery.Controls.Add(txtUsername);
            tabGallery.Controls.Add(lblUsername);
            tabGallery.Location = new Point(4, 24);
            tabGallery.Name = "tabGallery";
            tabGallery.Padding = new Padding(3);
            tabGallery.Size = new Size(632, 322);
            tabGallery.TabIndex = 0;
            tabGallery.Text = "Gallery";
            tabGallery.UseVisualStyleBackColor = true;
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(160, 275);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(0, 15);
            lblProgress.TabIndex = 9;
            lblProgress.Visible = false;
            // 
            // lblTransferRate
            // 
            lblTransferRate.Location = new Point(490, 275);
            lblTransferRate.Name = "lblTransferRate";
            lblTransferRate.Size = new Size(120, 15);
            lblTransferRate.TabIndex = 10;
            lblTransferRate.Text = "";
            lblTransferRate.TextAlign = ContentAlignment.MiddleRight;
            lblTransferRate.Visible = false;
            // 
            // btnCancelScraping
            // 
            btnCancelScraping.Location = new Point(20, 272);
            btnCancelScraping.Name = "btnCancelScraping";
            btnCancelScraping.Size = new Size(120, 30);
            btnCancelScraping.TabIndex = 8;
            btnCancelScraping.Text = "Cancel";
            btnCancelScraping.UseVisualStyleBackColor = true;
            btnCancelScraping.Visible = false;
            btnCancelScraping.Click += btnCancelScraping_Click;
            // 
            // progressScraping
            // 
            progressScraping.Location = new Point(160, 231);
            progressScraping.Name = "progressScraping";
            progressScraping.Size = new Size(450, 35);
            progressScraping.Style = ProgressBarStyle.Marquee;
            progressScraping.TabIndex = 7;
            progressScraping.Visible = false;
            // 
            // grpFileFormat
            // 
            grpFileFormat.Controls.Add(rbXLSX);
            grpFileFormat.Controls.Add(rbHTML);
            grpFileFormat.Controls.Add(rbCSV);
            grpFileFormat.Location = new Point(340, 100);
            grpFileFormat.Name = "grpFileFormat";
            grpFileFormat.Size = new Size(270, 110);
            grpFileFormat.TabIndex = 6;
            grpFileFormat.TabStop = false;
            grpFileFormat.Text = "Export Format";
            // 
            // rbXLSX
            // 
            rbXLSX.AutoSize = true;
            rbXLSX.Location = new Point(15, 75);
            rbXLSX.Name = "rbXLSX";
            rbXLSX.Size = new Size(88, 19);
            rbXLSX.TabIndex = 2;
            rbXLSX.Text = "Excel (XLSX)";
            rbXLSX.UseVisualStyleBackColor = true;
            // 
            // rbHTML
            // 
            rbHTML.AutoSize = true;
            rbHTML.Location = new Point(15, 50);
            rbHTML.Name = "rbHTML";
            rbHTML.Size = new Size(58, 19);
            rbHTML.TabIndex = 1;
            rbHTML.Text = "HTML";
            rbHTML.UseVisualStyleBackColor = true;
            // 
            // rbCSV
            // 
            rbCSV.AutoSize = true;
            rbCSV.Checked = true;
            rbCSV.Location = new Point(15, 25);
            rbCSV.Name = "rbCSV";
            rbCSV.Size = new Size(46, 19);
            rbCSV.TabIndex = 0;
            rbCSV.TabStop = true;
            rbCSV.Text = "CSV";
            rbCSV.UseVisualStyleBackColor = true;
            // 
            // btnStartScraping
            // 
            btnStartScraping.Location = new Point(20, 231);
            btnStartScraping.Name = "btnStartScraping";
            btnStartScraping.Size = new Size(120, 35);
            btnStartScraping.TabIndex = 5;
            btnStartScraping.Text = "Start Scraping";
            btnStartScraping.UseVisualStyleBackColor = true;
            btnStartScraping.Click += btnStartScraping_Click;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(280, 125);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(40, 23);
            btnBrowse.TabIndex = 4;
            btnBrowse.Text = "...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // txtFileName
            // 
            txtFileName.Location = new Point(20, 125);
            txtFileName.Name = "txtFileName";
            txtFileName.PlaceholderText = "Enter filename (without extension)";
            txtFileName.Size = new Size(250, 23);
            txtFileName.TabIndex = 3;
            txtFileName.Text = "gallery_export";
            // 
            // lblFileName
            // 
            lblFileName.AutoSize = true;
            lblFileName.Location = new Point(20, 100);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(63, 15);
            lblFileName.TabIndex = 2;
            lblFileName.Text = "File Name:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(20, 55);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Enter username (e.g., hmn)";
            txtUsername.Size = new Size(300, 23);
            txtUsername.TabIndex = 1;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(20, 30);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(122, 15);
            lblUsername.TabIndex = 0;
            lblUsername.Text = "DeviantArt Username:";
            // 
            // tabFavorites
            // 
            tabFavorites.Controls.Add(lblProgressFav);
            tabFavorites.Controls.Add(lblTransferRateFav);
            tabFavorites.Controls.Add(btnCancelScrapingFav);
            tabFavorites.Controls.Add(progressScrapingFav);
            tabFavorites.Controls.Add(grpFileFormatFav);
            tabFavorites.Controls.Add(btnStartScrapingFav);
            tabFavorites.Controls.Add(btnBrowseFav);
            tabFavorites.Controls.Add(txtFileNameFav);
            tabFavorites.Controls.Add(lblFileNameFav);
            tabFavorites.Controls.Add(txtUsernameFav);
            tabFavorites.Controls.Add(lblUsernameFav);
            tabFavorites.Location = new Point(4, 24);
            tabFavorites.Name = "tabFavorites";
            tabFavorites.Padding = new Padding(3);
            tabFavorites.Size = new Size(632, 322);
            tabFavorites.TabIndex = 1;
            tabFavorites.Text = "Favorites";
            tabFavorites.UseVisualStyleBackColor = true;
            // 
            // lblUsernameFav
            // 
            lblUsernameFav.AutoSize = true;
            lblUsernameFav.Location = new Point(20, 30);
            lblUsernameFav.Name = "lblUsernameFav";
            lblUsernameFav.Size = new Size(122, 15);
            lblUsernameFav.TabIndex = 0;
            lblUsernameFav.Text = "DeviantArt Username:";
            // 
            // txtUsernameFav
            // 
            txtUsernameFav.Location = new Point(20, 55);
            txtUsernameFav.Name = "txtUsernameFav";
            txtUsernameFav.PlaceholderText = "Enter username (e.g., hmn)";
            txtUsernameFav.Size = new Size(300, 23);
            txtUsernameFav.TabIndex = 1;
            // 
            // lblFileNameFav
            // 
            lblFileNameFav.AutoSize = true;
            lblFileNameFav.Location = new Point(20, 100);
            lblFileNameFav.Name = "lblFileNameFav";
            lblFileNameFav.Size = new Size(63, 15);
            lblFileNameFav.TabIndex = 2;
            lblFileNameFav.Text = "File Name:";
            // 
            // txtFileNameFav
            // 
            txtFileNameFav.Location = new Point(20, 125);
            txtFileNameFav.Name = "txtFileNameFav";
            txtFileNameFav.PlaceholderText = "Enter filename (without extension)";
            txtFileNameFav.Size = new Size(250, 23);
            txtFileNameFav.TabIndex = 3;
            txtFileNameFav.Text = "favorites_export";
            // 
            // btnBrowseFav
            // 
            btnBrowseFav.Location = new Point(280, 125);
            btnBrowseFav.Name = "btnBrowseFav";
            btnBrowseFav.Size = new Size(40, 23);
            btnBrowseFav.TabIndex = 4;
            btnBrowseFav.Text = "...";
            btnBrowseFav.UseVisualStyleBackColor = true;
            btnBrowseFav.Click += btnBrowseFav_Click;
            // 
            // btnStartScrapingFav
            // 
            btnStartScrapingFav.Location = new Point(20, 231);
            btnStartScrapingFav.Name = "btnStartScrapingFav";
            btnStartScrapingFav.Size = new Size(120, 35);
            btnStartScrapingFav.TabIndex = 5;
            btnStartScrapingFav.Text = "Start Scraping";
            btnStartScrapingFav.UseVisualStyleBackColor = true;
            btnStartScrapingFav.Click += btnStartScrapingFav_Click;
            // 
            // grpFileFormatFav
            // 
            grpFileFormatFav.Controls.Add(rbXLSXFav);
            grpFileFormatFav.Controls.Add(rbHTMLFav);
            grpFileFormatFav.Controls.Add(rbCSVFav);
            grpFileFormatFav.Location = new Point(340, 100);
            grpFileFormatFav.Name = "grpFileFormatFav";
            grpFileFormatFav.Size = new Size(270, 110);
            grpFileFormatFav.TabIndex = 6;
            grpFileFormatFav.TabStop = false;
            grpFileFormatFav.Text = "Export Format";
            // 
            // rbCSVFav
            // 
            rbCSVFav.AutoSize = true;
            rbCSVFav.Checked = true;
            rbCSVFav.Location = new Point(15, 25);
            rbCSVFav.Name = "rbCSVFav";
            rbCSVFav.Size = new Size(46, 19);
            rbCSVFav.TabIndex = 0;
            rbCSVFav.TabStop = true;
            rbCSVFav.Text = "CSV";
            rbCSVFav.UseVisualStyleBackColor = true;
            // 
            // rbHTMLFav
            // 
            rbHTMLFav.AutoSize = true;
            rbHTMLFav.Location = new Point(15, 50);
            rbHTMLFav.Name = "rbHTMLFav";
            rbHTMLFav.Size = new Size(58, 19);
            rbHTMLFav.TabIndex = 1;
            rbHTMLFav.Text = "HTML";
            rbHTMLFav.UseVisualStyleBackColor = true;
            // 
            // rbXLSXFav
            // 
            rbXLSXFav.AutoSize = true;
            rbXLSXFav.Location = new Point(15, 75);
            rbXLSXFav.Name = "rbXLSXFav";
            rbXLSXFav.Size = new Size(88, 19);
            rbXLSXFav.TabIndex = 2;
            rbXLSXFav.Text = "Excel (XLSX)";
            rbXLSXFav.UseVisualStyleBackColor = true;
            // 
            // progressScrapingFav
            // 
            progressScrapingFav.Location = new Point(160, 231);
            progressScrapingFav.Name = "progressScrapingFav";
            progressScrapingFav.Size = new Size(450, 35);
            progressScrapingFav.Style = ProgressBarStyle.Marquee;
            progressScrapingFav.TabIndex = 7;
            progressScrapingFav.Visible = false;
            // 
            // btnCancelScrapingFav
            // 
            btnCancelScrapingFav.Location = new Point(20, 272);
            btnCancelScrapingFav.Name = "btnCancelScrapingFav";
            btnCancelScrapingFav.Size = new Size(120, 30);
            btnCancelScrapingFav.TabIndex = 8;
            btnCancelScrapingFav.Text = "Cancel";
            btnCancelScrapingFav.UseVisualStyleBackColor = true;
            btnCancelScrapingFav.Visible = false;
            btnCancelScrapingFav.Click += btnCancelScrapingFav_Click;
            // 
            // lblProgressFav
            // 
            lblProgressFav.AutoSize = true;
            lblProgressFav.Location = new Point(160, 275);
            lblProgressFav.Name = "lblProgressFav";
            lblProgressFav.Size = new Size(0, 15);
            lblProgressFav.TabIndex = 9;
            lblProgressFav.Visible = false;
            // 
            // lblTransferRateFav
            // 
            lblTransferRateFav.Location = new Point(490, 275);
            lblTransferRateFav.Name = "lblTransferRateFav";
            lblTransferRateFav.Size = new Size(120, 15);
            lblTransferRateFav.TabIndex = 10;
            lblTransferRateFav.Text = "";
            lblTransferRateFav.TextAlign = ContentAlignment.MiddleRight;
            lblTransferRateFav.Visible = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 511);
            Controls.Add(btnSettings);
            Controls.Add(tabMain);
            Controls.Add(lblStatus);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DeviantArt Scrapper";
            tabMain.ResumeLayout(false);
            tabGallery.ResumeLayout(false);
            tabGallery.PerformLayout();
            grpFileFormat.ResumeLayout(false);
            grpFileFormat.PerformLayout();
            tabFavorites.ResumeLayout(false);
            tabFavorites.PerformLayout();
            grpFileFormatFav.ResumeLayout(false);
            grpFileFormatFav.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

            // Set localized text after all components are created
            LocalizeControls();
        }

        /// <summary>
        /// Applies localized text to all UI controls
        /// </summary>
        private void LocalizeControls()
        {
            // Form title
            this.Text = Localization.Localizer.MainFormTitle;

            // Tab names
            tabGallery.Text = Localization.Localizer.TabGallery;
            tabFavorites.Text = Localization.Localizer.TabFavorites;

            // Gallery tab labels
            lblUsername.Text = Localization.Localizer.LabelUsername;
            lblFileName.Text = Localization.Localizer.LabelFileName;
            grpFileFormat.Text = Localization.Localizer.LabelFileFormat;

            // Gallery tab buttons
            btnBrowse.Text = Localization.Localizer.ButtonBrowse;
            btnStartScraping.Text = Localization.Localizer.ButtonStartScraping;
            btnCancelScraping.Text = Localization.Localizer.ButtonCancel;

            // Favorites tab labels
            lblUsernameFav.Text = Localization.Localizer.LabelUsername;
            lblFileNameFav.Text = Localization.Localizer.LabelFileName;
            grpFileFormatFav.Text = Localization.Localizer.LabelFileFormat;

            // Favorites tab buttons
            btnBrowseFav.Text = Localization.Localizer.ButtonBrowse;
            btnStartScrapingFav.Text = Localization.Localizer.ButtonStartScraping;
            btnCancelScrapingFav.Text = Localization.Localizer.ButtonCancel;

            // Settings button
            btnSettings.Text = Localization.Localizer.ButtonSettings;
        }

        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblFileName;
        private TextBox txtFileName;
        private Button btnBrowse;
        private Button btnStartScraping;
        private GroupBox grpFileFormat;
        private RadioButton rbCSV;
        private RadioButton rbHTML;
        private RadioButton rbXLSX;
        private ProgressBar progressScraping;
        private Button btnCancelScraping;
        private Label lblProgress;
        /// <summary>
        /// Label displaying the current transfer rate in KB/s for gallery scraping operations.
        /// </summary>
        private Label lblTransferRate;
        private Label lblUsernameFav;
        private TextBox txtUsernameFav;
        private Label lblFileNameFav;
        private TextBox txtFileNameFav;
        private Button btnBrowseFav;
        private Button btnStartScrapingFav;
        private GroupBox grpFileFormatFav;
        private RadioButton rbCSVFav;
        private RadioButton rbHTMLFav;
        private RadioButton rbXLSXFav;
        private ProgressBar progressScrapingFav;
        private Button btnCancelScrapingFav;
        private Label lblProgressFav;
        /// <summary>
        /// Label displaying the current transfer rate in KB/s for favorites scraping operations.
        /// </summary>
        private Label lblTransferRateFav;
    }
}