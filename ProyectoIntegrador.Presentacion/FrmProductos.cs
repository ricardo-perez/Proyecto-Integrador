using System;
using System.Data;
using System.Windows.Forms;
using ProyectoIntegrador.Negocio;

namespace ProyectoIntegrador.Presentacion
{
    public partial class FrmProductos : Form
    {
        private readonly ProductoNegocio productoNegocio;

        public FrmProductos()
        {
            InitializeComponent();
            productoNegocio = new ProductoNegocio();
            CargarProductos();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CargarProductos()
        {
            try
            {
                dbVisualizador.DataSource = productoNegocio.ObtenerProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBuscador_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string texto = txtBuscador.Text.Trim();
                DataTable resultados = productoNegocio.BuscarProductos(texto);
                dbVisualizador.DataSource = resultados;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Hide();
        }
    }
}
