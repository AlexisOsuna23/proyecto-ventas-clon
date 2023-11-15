namespace proyecto_ventas
{
    partial class ventasdetalle
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
            this.DGVVD = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DGVVD)).BeginInit();
            this.SuspendLayout();
            // 
            // DGVVD
            // 
            this.DGVVD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVVD.Location = new System.Drawing.Point(12, 12);
            this.DGVVD.Name = "DGVVD";
            this.DGVVD.Size = new System.Drawing.Size(561, 243);
            this.DGVVD.TabIndex = 1;
            this.DGVVD.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVVD_CellContentClick);
            // 
            // ventasdetalle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 273);
            this.Controls.Add(this.DGVVD);
            this.Name = "ventasdetalle";
            this.Text = "ventasdetalle";
            this.Load += new System.EventHandler(this.ventasdetalle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVVD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGVVD;
    }
}