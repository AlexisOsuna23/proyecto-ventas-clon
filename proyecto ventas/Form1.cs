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
using static reglasnegocio.sqlserver;

namespace proyecto_ventas
{
    public partial class Form1 : Form
    {
        private SQLServerClass sqlclass;
        private string NombreTab = "Productos";
        private DataTable registros;
        private DataTable Committ;

        private bool formSecundarioAbierto = false;
        public Form1()
        {
            InitializeComponent();

            this.sqlclass = new SQLServerClass();

            this.Load += new EventHandler(Form1_Load);
            this.KeyPreview = true; // Asegura que el formulario pueda capturar los eventos del teclado
            this.KeyDown += new KeyEventHandler(Form1_KeyDown); // Evento KeyDown


            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem eliminarMenuItem = new ToolStripMenuItem("Eliminar Fila");
            eliminarMenuItem.Click += dataGridViewMostrarDatos_Click;
            contextMenu.Items.Add(eliminarMenuItem);
            dataGridViewMostrarDatos.ContextMenuStrip = contextMenu;
            txtProducto.TextChanged += txtProducto_TextChanged;

        }

        private void Fecha()
        {
            var Fecha = DateTime.Now;
            txtfecha.Text = Fecha.ToShortDateString();
        }
        public string productoOriginal = string.Empty;

        private void btnAgegarDatos_Click(object sender, EventArgs e)
        {

            try
            {
                string Folio = txtfolio.Text;
                string Fecha = txtfecha.Text;
                string Total = txttotal.Text;


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

                sqlclass.Transacciones(Folio, Fecha, Total, ProductoID, Pventa, Cantidad);

                string folioSugerido = sqlclass.FolioSugerido();
                txtfolio.Text = folioSugerido;

                MessageBox.Show("Su registro fue Guardado!!!!" +
                    "Muchas Gracias Vuelva Pronto :D");

               

                totalAcumulado = 0;
                txttotal.Text = totalAcumulado.ToString();
                LimpiarDGV();



            }
            catch (Exception ex)
            {
            }

        }

        private void btnAcualizar_Click(object sender, EventArgs e)
        {
            string folioSugerido = sqlclass.FolioSugerido();
            txtfolio.Text = folioSugerido;
            LimpiarDGV();
        }

        private void EliminarFilaSeleccionada()
        {
            if (dataGridViewMostrarDatos.SelectedRows.Count > 0)
            {
                int index = dataGridViewMostrarDatos.SelectedRows[0].Index;

                dataTable.Rows.RemoveAt(index);

                dataGridViewMostrarDatos.DataSource = dataTable;

                RTA();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
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

                sqlclass.DeshacerTransaccion(productoIDs, cantidades);

                MessageBox.Show("Transacción deshecha exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string folioSugerido = sqlclass.FolioSugerido();
                txtfolio.Text = folioSugerido;
                LimpiarDGV();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string ProductoId = txtProducto.Text;
                string Cantidad = txtCantidad.Text;
                string Descripcion = txtDescripcion.Text;
                string Pventa = txtPrecioVenta.Text;

                string productoIdToCheck = ProductoId;
                decimal saldoActual = sqlclass.ObtenerSaldo(productoIdToCheck);

                if (saldoActual <= 0)
                {
                    MessageBox.Show($"No hay productos con el ProductoID = {productoIdToCheck}.", "Notifique sobre ello", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                double total = Convert.ToDouble(Cantidad) * Convert.ToDouble(Pventa);

                totalAcumulado += total;
                txttotal.Text = totalAcumulado.ToString();

                productoOriginal = ProductoId;
                txtProducto.Clear();
                txtCantidad.Clear();
                txtDescripcion.Clear();
                txtPrecioVenta.Clear();

                if (dataTable.Columns.Count == 0)
                {
                    dataTable.Columns.Add("ProductoID", typeof(string));
                    dataTable.Columns.Add("Descripcion", typeof(string));
                    dataTable.Columns.Add("PVenta", typeof(string));
                    dataTable.Columns.Add("Cantidad", typeof(string));
                    dataTable.Columns.Add("Total", typeof(double));
                }

                DataRow newRow = dataTable.NewRow();
                newRow["ProductoID"] = productoOriginal;
                newRow["Descripcion"] = Descripcion;
                newRow["PVenta"] = Pventa;
                newRow["Cantidad"] = Cantidad;
                newRow["Total"] = total;
                dataTable.Rows.Add(newRow);

                dataGridViewMostrarDatos.DataSource = dataTable;
                dataGridViewMostrarDatos.FirstDisplayedScrollingRowIndex = dataGridViewMostrarDatos.RowCount - 1;
                dataGridViewMostrarDatos.Rows[dataGridViewMostrarDatos.RowCount - 1].Selected = true;
            }
        }

        private void RTA()
        {
            totalAcumulado = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                totalAcumulado += Convert.ToDouble(row["Total"]);
            }
            txttotal.Text = totalAcumulado.ToString();
        }

        private void DGVProductos()
        {
            string ProductoId = txtProducto.Text;
            string Cantidad = txtCantidad.Text;
            string Descripcion = txtDescripcion.Text;
            string Pventa = txtPrecioVenta.Text;

            string productoIdToCheck = ProductoId;
            decimal saldoActual = sqlclass.ObtenerSaldo(productoIdToCheck);

            if (saldoActual <= 0)
            {
                MessageBox.Show($"No hay productos con el ProductoID = {productoIdToCheck}.", "Notifique sobre ello", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double total = Convert.ToDouble(Cantidad) * Convert.ToDouble(Pventa);

            totalAcumulado += total;
            txttotal.Text = totalAcumulado.ToString();

            productoOriginal = ProductoId;
            txtProducto.Clear();
            txtCantidad.Clear();
            txtDescripcion.Clear();
            txtPrecioVenta.Clear();

            if (dataTable.Columns.Count == 0)
            {
                dataTable.Columns.Add("ProductoID", typeof(string));
                dataTable.Columns.Add("Descripcion", typeof(string));
                dataTable.Columns.Add("PVenta", typeof(string));
                dataTable.Columns.Add("Cantidad", typeof(string));
                dataTable.Columns.Add("Total", typeof(double));
            }

            DataRow newRow = dataTable.NewRow();
            newRow["ProductoID"] = productoOriginal;
            newRow["Descripcion"] = Descripcion;
            newRow["PVenta"] = Pventa;
            newRow["Cantidad"] = Cantidad;
            newRow["Total"] = total;
            dataTable.Rows.Add(newRow);

            dataGridViewMostrarDatos.DataSource = dataTable;
            dataGridViewMostrarDatos.FirstDisplayedScrollingRowIndex = dataGridViewMostrarDatos.RowCount - 1;
            dataGridViewMostrarDatos.Rows[dataGridViewMostrarDatos.RowCount - 1].Selected = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Fecha();
                sqlclass.SiHayConexion(NombreTab);
                

                string folioSugerido = sqlclass.FolioSugerido();
                txtfolio.Text = folioSugerido;
                txtProducto.TextChanged += txtProducto_TextChanged;



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el folio sugerido: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarDGV()
        {
            if (dataGridViewMostrarDatos.InvokeRequired)
            {
                dataGridViewMostrarDatos.Invoke(new MethodInvoker(delegate
                {
                    dataTable.Rows.Clear();
                    dataTable.Clear();
                    dataGridViewMostrarDatos.Refresh();
                    txttotal.Text = string.Empty;

                }));
            }
            else
            {
                dataTable.Rows.Clear();
                dataTable.Clear();
                dataGridViewMostrarDatos.Refresh();
                txttotal.Text = string.Empty;

            }
        }

        private void dataGridViewMostrarDatos_Click(object sender, EventArgs e)
        {

            try
            {
                if (MessageBox.Show("¿Seguro que quieres eliminar esta fila?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    EliminarFilaSeleccionada();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar fila: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtProducto_TextChanged(object sender, EventArgs e)
        {
            string productID = txtProducto.Text;

            if (!string.IsNullOrEmpty(productID))
            {
                Tuple<string, decimal, float> valores = sqlclass.ObtenerValoresPorProductID(productID);

                if (valores != null)
                {
                    txtDescripcion.Text = valores.Item1;
                    txtCantidad.Text = "1";
                    txtPrecioVenta.Text = valores.Item2.ToString();
                }
                else
                {
                    txtDescripcion.Text = string.Empty;
                    txtCantidad.Text = string.Empty;

                    txtPrecioVenta.Text = string.Empty;

                }
            }
            else
            {
                txtDescripcion.Text = string.Empty;
                txtCantidad.Text = string.Empty;
                txtPrecioVenta.Text = string.Empty;
            }
        }

        private bool ProductoExisteEnDataTable(string productoID)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if (row["ProductoID"].ToString() == productoID)
                {
                    return true;
                }
            }
            return false;
        }

        private void dataGridViewMostrarDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void SetTextBoxValues(string ProductoID, string Cantidad, string Descripcion, string Pventa)
        {
            txtProducto.Text = ProductoID;
            txtCantidad.Text = Cantidad;
            txtDescripcion.Text = Descripcion;
            txtPrecioVenta.Text = Pventa;

        }


        private DataTable dataTable = new DataTable();
        double totalAcumulado = 0;

        private void txtCantidad_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                string ProductoId = txtProducto.Text;
                string Cantidad = txtCantidad.Text;
                string Descripcion = txtDescripcion.Text;
                string Pventa = txtPrecioVenta.Text;

                double total = Convert.ToDouble(Cantidad) * Convert.ToDouble(Pventa);

                totalAcumulado += total;

                txttotal.Text = totalAcumulado.ToString();

                txtProducto.Clear();
                txtCantidad.Clear();
                txtDescripcion.Clear();
                txtPrecioVenta.Clear();

                if (dataTable.Columns.Count == 0)
                {
                    dataTable.Columns.Add("ProductoID", typeof(string));
                    dataTable.Columns.Add("Descripcion", typeof(string));
                    dataTable.Columns.Add("PVenta", typeof(string));
                    dataTable.Columns.Add("Cantidad", typeof(string));
                    dataTable.Columns.Add("Total", typeof(double));
                }

                DataRow newRow = dataTable.NewRow();
                newRow["ProductoID"] = ProductoId;
                newRow["Descripcion"] = Descripcion;
                newRow["PVenta"] = Pventa;
                newRow["Cantidad"] = Cantidad;
                newRow["Total"] = total;
                dataTable.Rows.Add(newRow);

                dataGridViewMostrarDatos.DataSource = dataTable;
                dataGridViewMostrarDatos.FirstDisplayedScrollingRowIndex = dataGridViewMostrarDatos.RowCount - 1;
                dataGridViewMostrarDatos.Rows[dataGridViewMostrarDatos.RowCount - 1].Selected = true;
            }
        }

        private void txttotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtfolio_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 && !formSecundarioAbierto) // Verifica si la tecla presionada es F1 y el formulario secundario no está abierto
            {
                formSecundarioAbierto = true; // Marcar que el formulario secundario está abierto
                ventasdetalle formSecundario = new ventasdetalle();
                formSecundario.FormClosed += (s, args) => formSecundarioAbierto = false; // Evento para cuando se cierre el formulario secundario
                formSecundario.Show(); // Muestra el formulario secundario
            }
            if (e.KeyCode == Keys.F2 && !formSecundarioAbierto)
            {
                formSecundarioAbierto = true; // Marcar que el formulario secundario está abierto
                productos formSecundario = new productos();
                formSecundario.FormClosed += (s, args) => formSecundarioAbierto = false; // Evento para cuando se cierre el formulario secundario
                formSecundario.Show(); // Muestra el formulario secundario
            }
        }
    }
}
