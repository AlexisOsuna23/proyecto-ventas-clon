using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static reglasnegocio.sqlserver;

namespace proyecto_ventas
{
    public partial class dlgprincipal : Form
    {

        private SQLServerClass Sqlclass;
        private string Produ = "Productos";
        private DataTable registros;

        private SQLServerClass sqlclass;

        public dlgprincipal()
        {
            InitializeComponent();
            this.Sqlclass = new SQLServerClass();
        }

      

        private void btnGuardarProducto_Click(object sender, EventArgs e)
        {

            try
            {
                string Folio = txtFolio.Text;
                string Fecha = txtFecha.Text;
                string Total = txtTotal.Text;


                List<string> ProductoID = new List<string>();
                List<string> Pventa = new List<string>();
                List<string> Cantidad = new List<string>();

                foreach (DataGridViewRow row in dataGridViewMostrarDatos.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        if (row.Cells["ProductoID"].Value != null)
                            ProductoID.Add(row.Cells["ProductoID"].Value.ToString());
                        if (row.Cells["Pventa"].Value != null)
                            Pventa.Add(row.Cells["Pventa"].Value.ToString());
                        if (row.Cells["Cantidad"].Value != null)
                            Cantidad.Add(row.Cells["Cantidad"].Value.ToString());
                    }
                }

                sqlclass.InsertarInventario(Folio, Fecha, Total, ProductoID, Pventa, Cantidad);

                string folioSugerido = sqlclass.FolioSugerido();
                txtFolio.Text = folioSugerido;

                MessageBox.Show("Su registro fue Guardado");

            }
            catch (Exception ex)
            {

            }


        }

        private void btnEliminarProducto_Click(object sender, EventArgs e)
        {

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewMostrarDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
