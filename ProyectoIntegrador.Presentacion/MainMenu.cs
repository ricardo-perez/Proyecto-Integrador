using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIntegrador.Presentacion
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmVentas ventana = new FrmVentas();
            ventana.FormClosed += (s, args) => this.Show();

            this.Hide();
            ventana.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmProductos ventana = new FrmProductos();
            ventana.FormClosed += (s, args) => this.Show();

            this.Hide();
            ventana.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
