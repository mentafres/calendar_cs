namespace WinFormsApp1
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.LabelCurrentDateTime = new System.Windows.Forms.Label();
            this.TodayHolidayListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TodayNotificationListBox = new System.Windows.Forms.ListBox();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.HolidayListBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CreateHolidayButton = new System.Windows.Forms.Button();
            this.DeleteHolidayButton = new System.Windows.Forms.Button();
            this.EditHoliayButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.NotificationListBox = new System.Windows.Forms.ListBox();
            this.EditNotificationButton = new System.Windows.Forms.Button();
            this.DeleteNotificationButton = new System.Windows.Forms.Button();
            this.CreateNotificationButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Праздники сегодня:";
            // 
            // LabelCurrentDateTime
            // 
            this.LabelCurrentDateTime.AutoSize = true;
            this.LabelCurrentDateTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LabelCurrentDateTime.Location = new System.Drawing.Point(336, 12);
            this.LabelCurrentDateTime.Name = "LabelCurrentDateTime";
            this.LabelCurrentDateTime.Size = new System.Drawing.Size(178, 15);
            this.LabelCurrentDateTime.TabIndex = 0;
            this.LabelCurrentDateTime.Text = "Текущее время: Current Time";
            this.LabelCurrentDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TodayHolidayListBox
            // 
            this.TodayHolidayListBox.FormattingEnabled = true;
            this.TodayHolidayListBox.ItemHeight = 15;
            this.TodayHolidayListBox.Location = new System.Drawing.Point(16, 36);
            this.TodayHolidayListBox.Name = "TodayHolidayListBox";
            this.TodayHolidayListBox.Size = new System.Drawing.Size(409, 94);
            this.TodayHolidayListBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(16, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ближайшие уведомления:";
            // 
            // TodayNotificationListBox
            // 
            this.TodayNotificationListBox.FormattingEnabled = true;
            this.TodayNotificationListBox.ItemHeight = 15;
            this.TodayNotificationListBox.Location = new System.Drawing.Point(16, 151);
            this.TodayNotificationListBox.Name = "TodayNotificationListBox";
            this.TodayNotificationListBox.Size = new System.Drawing.Size(409, 94);
            this.TodayNotificationListBox.TabIndex = 4;
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(438, 36);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 5;
            // 
            // HolidayListBox
            // 
            this.HolidayListBox.FormattingEnabled = true;
            this.HolidayListBox.ItemHeight = 15;
            this.HolidayListBox.Location = new System.Drawing.Point(16, 299);
            this.HolidayListBox.Name = "HolidayListBox";
            this.HolidayListBox.Size = new System.Drawing.Size(409, 79);
            this.HolidayListBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 281);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Редактирование праздников:";
            // 
            // CreateHolidayButton
            // 
            this.CreateHolidayButton.Location = new System.Drawing.Point(431, 357);
            this.CreateHolidayButton.Name = "CreateHolidayButton";
            this.CreateHolidayButton.Size = new System.Drawing.Size(171, 23);
            this.CreateHolidayButton.TabIndex = 9;
            this.CreateHolidayButton.Text = "Создать";
            this.CreateHolidayButton.UseVisualStyleBackColor = true;
            this.CreateHolidayButton.Click += new System.EventHandler(this.CreateHolidayButton_Click);
            // 
            // DeleteHolidayButton
            // 
            this.DeleteHolidayButton.Location = new System.Drawing.Point(431, 299);
            this.DeleteHolidayButton.Name = "DeleteHolidayButton";
            this.DeleteHolidayButton.Size = new System.Drawing.Size(171, 23);
            this.DeleteHolidayButton.TabIndex = 10;
            this.DeleteHolidayButton.Text = "Удалить";
            this.DeleteHolidayButton.UseVisualStyleBackColor = true;
            this.DeleteHolidayButton.Click += new System.EventHandler(this.DeleteHolidayButton_Click);
            // 
            // EditHoliayButton
            // 
            this.EditHoliayButton.Location = new System.Drawing.Point(431, 328);
            this.EditHoliayButton.Name = "EditHoliayButton";
            this.EditHoliayButton.Size = new System.Drawing.Size(171, 23);
            this.EditHoliayButton.TabIndex = 11;
            this.EditHoliayButton.Text = "Редактировать";
            this.EditHoliayButton.UseVisualStyleBackColor = true;
            this.EditHoliayButton.Click += new System.EventHandler(this.EditHoliayButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 407);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "Редактирование уведомлений:";
            // 
            // NotificationListBox
            // 
            this.NotificationListBox.FormattingEnabled = true;
            this.NotificationListBox.ItemHeight = 15;
            this.NotificationListBox.Location = new System.Drawing.Point(16, 425);
            this.NotificationListBox.Name = "NotificationListBox";
            this.NotificationListBox.Size = new System.Drawing.Size(409, 79);
            this.NotificationListBox.TabIndex = 13;
            // 
            // EditNotificationButton
            // 
            this.EditNotificationButton.Location = new System.Drawing.Point(431, 454);
            this.EditNotificationButton.Name = "EditNotificationButton";
            this.EditNotificationButton.Size = new System.Drawing.Size(171, 23);
            this.EditNotificationButton.TabIndex = 16;
            this.EditNotificationButton.Text = "Редактировать";
            this.EditNotificationButton.UseVisualStyleBackColor = true;
            this.EditNotificationButton.Click += new System.EventHandler(this.EditNotificationButton_Click);
            // 
            // DeleteNotificationButton
            // 
            this.DeleteNotificationButton.Location = new System.Drawing.Point(431, 425);
            this.DeleteNotificationButton.Name = "DeleteNotificationButton";
            this.DeleteNotificationButton.Size = new System.Drawing.Size(171, 23);
            this.DeleteNotificationButton.TabIndex = 15;
            this.DeleteNotificationButton.Text = "Удалить";
            this.DeleteNotificationButton.UseVisualStyleBackColor = true;
            this.DeleteNotificationButton.Click += new System.EventHandler(this.DeleteNotificationButton_Click);
            // 
            // CreateNotificationButton
            // 
            this.CreateNotificationButton.Location = new System.Drawing.Point(431, 483);
            this.CreateNotificationButton.Name = "CreateNotificationButton";
            this.CreateNotificationButton.Size = new System.Drawing.Size(171, 23);
            this.CreateNotificationButton.TabIndex = 14;
            this.CreateNotificationButton.Text = "Создать";
            this.CreateNotificationButton.UseVisualStyleBackColor = true;
            this.CreateNotificationButton.Click += new System.EventHandler(this.CreateNotificationButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 515);
            this.Controls.Add(this.EditNotificationButton);
            this.Controls.Add(this.DeleteNotificationButton);
            this.Controls.Add(this.CreateNotificationButton);
            this.Controls.Add(this.NotificationListBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.EditHoliayButton);
            this.Controls.Add(this.DeleteHolidayButton);
            this.Controls.Add(this.CreateHolidayButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.HolidayListBox);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.TodayNotificationListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TodayHolidayListBox);
            this.Controls.Add(this.LabelCurrentDateTime);
            this.Name = "MainForm";
            this.Text = "Календарь";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label LabelCurrentDateTime;
        private ListBox TodayHolidayListBox;
        private Label label2;
        private ListBox TodayNotificationListBox;
        private MonthCalendar monthCalendar1;
        private ListBox HolidayListBox;
        private Label label3;
        private Button CreateHolidayButton;
        private Button DeleteHolidayButton;
        private Button EditHoliayButton;
        private Label label4;
        private ListBox NotificationListBox;
        private Button EditNotificationButton;
        private Button DeleteNotificationButton;
        private Button CreateNotificationButton;
        private Label label1;
    }
}