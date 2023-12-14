using Microsoft.Win32;
using reglasnegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static reglasnegocio.SQLServerClass;

namespace proyecto_ventas
{
    public partial class productos : Form
    {
        private SQLServerClass Sqlclass;
        private string Produ = "Productos";
        private DataTable registros;

        private SQLServerClass sqlclass;
        public productos()
        {
            InitializeComponent();
            this.Sqlclass = new SQLServerClass();
           
        }

        private void btnAgegarDatos_Click(object sender, EventArgs e)
        {
            string ProductId = txtProductoID.Text;
            string Descripcion = txtDescripcion.Text;
            string PrecioVenta = txtPVentas.Text;
            string saldo = txtSaldo.Text;

            bool resultado = Sqlclass.InsertarProductos(ProductId, Descripcion, PrecioVenta, saldo);

            if (resultado)
            {
                MessageBox.Show("Nuevo producto");
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al insertar producto");
            }
        }

        private void productos_Load(object sender, EventArgs e)
        {
            CargarResgistros(Produ);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

        }

        private void CargarResgistros(string Produ)
        {
            try
            {
                registros = Sqlclass.ObtenerRegistrosDeTabla(Produ);

                dataGridViewMostrarDatos.DataSource = registros;

                foreach (DataGridViewRow row in dataGridViewMostrarDatos.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar registros: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewMostrarDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow fila = dataGridViewMostrarDatos.Rows[e.RowIndex];

                string Producto = fila.Cells["ProductoID"].Value.ToString();
                string Descripcion = fila.Cells["Descripcion"].Value.ToString();
                string Pventa = fila.Cells["Pventa"].Value.ToString();
                int CantidadUtilizada = 1;

                if (Application.OpenForms["Form1"] is Form1 Form1)
                {
                    Form1.SetTextBoxValues(Producto, CantidadUtilizada.ToString(), Descripcion, Pventa);
                }

                this.Close();
            }
        }
    }
}
