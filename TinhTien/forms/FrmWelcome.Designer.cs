namespace TinhTien
{
    partial class FrmWelcome
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTableList = new System.Windows.Forms.Button();
            this.btnReview = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTableList
            // 
            this.btnTableList.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTableList.Location = new System.Drawing.Point(121, 38);
            this.btnTableList.Name = "btnTableList";
            this.btnTableList.Size = new System.Drawing.Size(138, 49);
            this.btnTableList.TabIndex = 0;
            this.btnTableList.Text = "Tính Tiền";
            this.btnTableList.UseVisualStyleBackColor = true;
            this.btnTableList.Click += new System.EventHandler(this.btnTableList_Click);
            // 
            // btnReview
            // 
            this.btnReview.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReview.Location = new System.Drawing.Point(121, 129);
            this.btnReview.Name = "btnReview";
            this.btnReview.Size = new System.Drawing.Size(138, 65);
            this.btnReview.TabIndex = 2;
            this.btnReview.Text = "Xem Lại Dữ Liệu";
            this.btnReview.UseVisualStyleBackColor = true;
            this.btnReview.Click += new System.EventHandler(this.btnReview_Click);
            // 
            // FrmWelcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 246);
            this.Controls.Add(this.btnReview);
            this.Controls.Add(this.btnTableList);
            this.Name = "FrmWelcome";
            this.Text = "Bạch Tuộc  Nướng 193";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTableList;
        private System.Windows.Forms.Button btnReview;
    }
}