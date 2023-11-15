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
    public partial class ventasdetalle : Form
    {
        private SQLServerClass Sqlclass;
        private string Produ = "Productos";
        private DataTable registros;
        public ventasdetalle()
        {
            InitializeComponent();
            this.Sqlclass = new SQLServerClass();
        }

        private void DGVVD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void CargarVentasDetalle()
        {
            try
            {
                using (DataTable registros = Sqlclass.ObtenerVistaVentas())
                {
                    DGVVD.DataSource = registros;
                }

                foreach (DataGridViewRow row in DGVVD.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar registros.", ex);
            }

        }
        private void ventasdetalle_Load(object sender, EventArgs e)
        {

            try
            {
                CargarVentasDetalle();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
