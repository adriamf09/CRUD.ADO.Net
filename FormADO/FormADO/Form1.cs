using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FormADO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ConsultasSQL consulta = new ConsultasSQL();

        public void CargarPersonas()
        {
            var lista = consulta.PersonasSelectAll();
            dgvPersonas.DataSource = null;
            dgvPersonas.DataSource = lista;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dgvPersonas.AutoGenerateColumns = false;
            CargarPersonas();

            DataTable dt = new DataTable();
            dt.Columns.Add("Sexo");
            dt.Rows.Add("Masculino");
            dt.Rows.Add("Femenino");

            cboxSexo.DataSource = dt;
            cboxSexo.DisplayMember = "Sexo";

            cboxPais.DataSource = consulta.PaisSelectAll();
            cboxPais.DisplayMember = "Nombre";
            cboxPais.ValueMember = "IdPais";
        }

        private void btnFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "image files|*.jpg;*.png;*.gif";

            DialogResult dr = opf.ShowDialog();

            string pic = opf.FileName;
            picPerson.ImageLocation = pic;
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                MemoryStream ms = new MemoryStream();
                picPerson.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                if (consulta.Insertar(txtNombre.Text, dtpFecha.Value, int.Parse(cboxPais.SelectedValue.ToString()), cboxSexo.Text, maskedTxtTelefono.Text, txtEmail.Text, ms.GetBuffer()))
                {
                    MessageBox.Show("Guardado correctamente.");
                    CargarPersonas();
                }
                else
                    MessageBox.Show("Error.");
            }
           
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            foreach (var i in this.Controls)
            {
                if (i is TextBox)
                {
                    ((TextBox)i).Text = "";
                }
            }

            foreach (var i in this.Controls)
            {
                if (i is MaskedTextBox)
                {
                    ((MaskedTextBox)i).Text = "";
                }
            }

            dtpFecha.Value = DateTime.Now;
            picPerson.Image = null;
        }

        private void dgvPersonas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPersonas.Rows.Count > 0)
            {
                txtNombre.Text = dgvPersonas[1, dgvPersonas.CurrentRow.Index].Value.ToString();
                dtpFecha.Value = Convert.ToDateTime(dgvPersonas[2, dgvPersonas.CurrentRow.Index].Value.ToString());
                cboxPais.Text = dgvPersonas[3, dgvPersonas.CurrentRow.Index].Value.ToString();
                cboxSexo.Text = dgvPersonas[4, dgvPersonas.CurrentRow.Index].Value.ToString();
                maskedTxtTelefono.Text = dgvPersonas[5, dgvPersonas.CurrentRow.Index].Value.ToString();
                txtEmail.Text = dgvPersonas[6, dgvPersonas.CurrentRow.Index].Value.ToString();

                byte[] img = (byte[])dgvPersonas[7, dgvPersonas.CurrentRow.Index].Value;
                MemoryStream ms = new MemoryStream(img);
                picPerson.Image = Image.FromStream(ms);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPersonas.Rows.Count > 0)
            {
                DialogResult dr = MessageBox.Show("¿Esta seguro de realizar esta accion?", "Confirmar", MessageBoxButtons.YesNo);
                if(dr == DialogResult.Yes)
                {
                    if (consulta.Eliminar(int.Parse(dgvPersonas[0, dgvPersonas.CurrentRow.Index].Value.ToString())))
                    {
                        CargarPersonas();
                        MessageBox.Show("Eliminado correctamente.");
                    }   
                    else
                        MessageBox.Show("Error.");
                }
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                MemoryStream ms = new MemoryStream();
                picPerson.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                if (consulta.Actualizar(int.Parse(dgvPersonas[0, dgvPersonas.CurrentRow.Index].Value.ToString()), txtNombre.Text, dtpFecha.Value,
                    int.Parse(cboxPais.SelectedValue.ToString()), cboxSexo.Text, maskedTxtTelefono.Text, txtEmail.Text, ms.GetBuffer()))
                {
                    MessageBox.Show("Actualizado correctamente.");
                    CargarPersonas();
                }
                else
                    MessageBox.Show("Error.");
            }
        }

        public bool ValidarCampos()
        {
            foreach (Control i in this.Controls)
            {
                if (i is TextBox)
                {
                    if (i.Text == "")
                    {
                        MessageBox.Show("Hay campos vacios.");
                        return false;
                    }
                }
                else if (i is MaskedTextBox)
                {
                    if (i.Text == "")
                    {
                        MessageBox.Show("Hay campos vacios.");
                        return false;
                    }
                }
            }

            if (picPerson.Image ==null)
            {
                MessageBox.Show("Debe insertar una foto.");
                return false;
            }
            return true;
        }
    }
}
