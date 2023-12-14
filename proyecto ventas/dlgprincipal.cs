using reglasnegocio;
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
using static reglasnegocio.SQLServerClass;

namespace proyecto_ventas
{
    public partial class dlgprincipal : Form
    {
        
        private SQLServerClass sqlclass;
        
        private bool formSecundarioAbierto = false;


        public dlgprincipal()
        {
            InitializeComponent();
            this.sqlclass = new SQLServerClass();
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
            try
            {
                SQLServerClass sqlclass = new SQLServerClass();

                List<string> productoIDs = new List<string>();
                List<string> cantidades = new List<string>();

                DataTable ventasDetalleData = sqlclass.ObtenerDatosVentasDetalle();

                foreach (DataRow row in ventasDetalleData.Rows)
                {
                    productoIDs.Add(row["ProductoID"].ToString());
                    cantidades.Add(row["Cantidad"].ToString());
                }

                //sqlclass.DeleteProducto(productoIDs, cantidades);
                sqlclass.DeshacerTransaccion(productoIDs, cantidades);
                MessageBox.Show("Transacción deshecha exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string folioSugerido = sqlclass.FolioSugerido();
                txtFolio.Text = folioSugerido;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewMostrarDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtFecha_TextChanged(object sender, EventArgs e)
        {

        }

        private void dlgprincipal_Load(object sender, EventArgs e)
        {
            Fecha();

            try
            {
                Fecha();

                string folioSugerido = sqlclass.FolioSugerido();
                txtFolio.Text = folioSugerido;
                txtProducto.TextChanged += txtProducto_TextChanged;



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el folio sugerido: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void Fecha()
        {
            var Fecha = DateTime.Now;
            txtFecha.Text = Fecha.ToShortDateString();
        }

        private void txtProducto_TextChanged(object sender, EventArgs e)
        {

        }

        private void dlgprincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 && !formSecundarioAbierto) // Verifica si la tecla presionada es F1 y el formulario secundario no está abierto
            {
                formSecundarioAbierto = true; // Marcar que el formulario secundario está abierto
                productos formSecundario = new productos();
                formSecundario.FormClosed += (s, args) => formSecundarioAbierto = false; // Evento para cuando se cierre el formulario secundario
                formSecundario.Show(); // Muestra el formulario secundario
            }
            if (e.KeyCode == Keys.F2 && !formSecundarioAbierto)
            {
                formSecundarioAbierto = true; // Marcar que el formulario secundario está abierto
                ventasdetalle formSecundario = new ventasdetalle();
                formSecundario.FormClosed += (s, args) => formSecundarioAbierto = false; // Evento para cuando se cierre el formulario secundario
                formSecundario.Show(); // Muestra el formulario secundario
            }
        }

        private void txtProducto_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
