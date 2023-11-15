namespace proyecto_ventas
{
    partial class productos
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
            this.dataGridViewMostrarDatos = new System.Windows.Forms.DataGridView();
            this.btnAgegarDatos = new System.Windows.Forms.Button();
            this.txtSaldo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPVentas = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProductoID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMostrarDatos)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewMostrarDatos
            // 
            this.dataGridViewMostrarDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMostrarDatos.Location = new System.Drawing.Point(241, 12);
            this.dataGridViewMostrarDatos.Name = "dataGridViewMostrarDatos";
            this.dataGridViewMostrarDatos.Size = new System.Drawing.Size(501, 396);
            this.dataGridViewMostrarDatos.TabIndex = 30;
            this.dataGridViewMostrarDatos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMostrarDatos_CellContentClick);
            // 
            // btnAgegarDatos
            // 
            this.btnAgegarDatos.Location = new System.Drawing.Point(26, 308);
            this.btnAgegarDatos.Name = "btnAgegarDatos";
            this.btnAgegarDatos.Size = new System.Drawing.Size(75, 23);
            this.btnAgegarDatos.TabIndex = 28;
            this.btnAgegarDatos.Text = "Agregar";
            this.btnAgegarDatos.UseVisualStyleBackColor = true;
            this.btnAgegarDatos.Click += new System.EventHandler(this.btnAgegarDatos_Click);
            // 
            // txtSaldo
            // 
            this.txtSaldo.Location = new System.Drawing.Point(26, 260);
            this.txtSaldo.Name = "txtSaldo";
            this.txtSaldo.Size = new System.Drawing.Size(142, 20);
            this.txtSaldo.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 244);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Saldo";
            // 
            // txtPVentas
            // 
            this.txtPVentas.Location = new System.Drawing.Point(26, 194);
            this.txtPVentas.Name = "txtPVentas";
            this.txtPVentas.Size = new System.Drawing.Size(142, 20);
            this.txtPVentas.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 178);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Precio Venta";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(26, 130);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(142, 20);
            this.txtDescripcion.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Descripcion";
            // 
            // txtProductoID
            // 
            this.txtProductoID.Location = new System.Drawing.Point(26, 64);
            this.txtProductoID.Name = "txtProductoID";
            this.txtProductoID.Size = new System.Drawing.Size(142, 20);
            this.txtProductoID.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "ProductoID";
            // 
            // productos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewMostrarDatos);
            this.Controls.Add(this.btnAgegarDatos);
            this.Controls.Add(this.txtSaldo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPVentas);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtProductoID);
            this.Controls.Add(this.label1);
            this.Name = "productos";
            this.Text = "productos";
            this.Load += new System.EventHandler(this.productos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMostrarDatos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewMostrarDatos;
        private System.Windows.Forms.Button btnAgegarDatos;
        private System.Windows.Forms.TextBox txtSaldo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPVentas;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProductoID;
        private System.Windows.Forms.Label label1;
    }
}