using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace motos11
{
    public partial class Form1 : Form
    {
        acceso oDB;
        List<moto> lMotos;
        
        public Form1()
        {
            InitializeComponent();
            oDB = new acceso();
            lMotos = new List<moto>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            habilitar(false);
            limpiar();
            cargarCombo();
            cargarLista();
        }

        private void cargarLista()
        {
            lstMotos.Items.Clear();
            lMotos.Clear();
            DataTable tabla = oDB.consultarDB("SELECT * FROM MOTOS");
            foreach (DataRow fila in tabla.Rows)
            {
                moto m = new moto();
                m.pCodigo = Convert.ToInt32(fila["codigo"]);
                m.pModelo = Convert.ToString(fila["modelo"]);
                m.pMarca = Convert.ToInt32(fila["marca"]);
                m.pPrecio = Convert.ToDouble(fila["precio"]);
                m.pFecha = Convert.ToDateTime(fila["fecha"]);
                lstMotos.Items.Add(m);
                lMotos.Add(m);
            }
        }

        private void cargarCombo()
        {
            DataTable tabla = oDB.consultarDB("SELECT * FROM MARCAS");
            cboMarca.DataSource = tabla;
            cboMarca.DisplayMember = "descripcion";
            cboMarca.ValueMember = "marca";
            cboMarca.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void limpiar()
        {
            txtCodigo.Text = string.Empty;
            txtModelo.Text = string.Empty;
            cboMarca.SelectedIndex = -1;
            txtPrecio.Text = string.Empty;
            dtpFecha.Value = DateTime.Today;
        }

        private void habilitar(bool v)
        {
            txtCodigo.Enabled = v;
            txtModelo.Enabled = v;
            cboMarca.Enabled = v;
            txtPrecio.Enabled = v;
            dtpFecha.Enabled = v;
            btnCancelar.Enabled = v;
            btnEditar.Enabled = !v;
            btnEliminar.Enabled = !v;
            btnGuardar.Enabled = v;
            btnNuevo.Enabled = !v;
            btnSalir.Enabled = !v;
            lstMotos.Enabled = !v;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                moto m = new moto();
                m.pCodigo = Convert.ToInt32(txtCodigo.Text);
                m.pModelo = txtModelo.Text;
                m.pMarca = Convert.ToInt32(cboMarca.SelectedValue);
                m.pPrecio = Convert.ToDouble(txtPrecio.Text);
                m.pFecha = dtpFecha.Value;

                if (!existe(m))
                {
                    string consultaSQL = "INSERT INTO MOTOS VALUES(@codigo, @modelo, @marca, @precio, @fecha)";
                    List<parametro> lParametros = new List<parametro>();
                    lParametros.Add(new parametro("@codigo", m.pCodigo));
                    lParametros.Add(new parametro("@modelo", m.pModelo));
                    lParametros.Add(new parametro("@marca", m.pMarca));
                    lParametros.Add(new parametro("@precio", m.pPrecio));
                    lParametros.Add(new parametro("@fecha", m.pFecha));

                    if(oDB.actualizarDB(consultaSQL, lParametros)> 0)
                    {
                        MessageBox.Show("registrado correctamente");
                        cargarLista();
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show("no se pudo registrar");
                        limpiar();
                    }
                }
                else
                {
                    string consultaSQL = "UPDATE MOTOS SET modelo=@modelo, marca=@marca, precio=@precio, fecha=@fecha WHERE codigo=@codigo";
                    List<parametro> lParametros = new List<parametro>();
                    lParametros.Add(new parametro("@codigo", m.pCodigo));
                    lParametros.Add(new parametro("@modelo", m.pModelo));
                    lParametros.Add(new parametro("@marca", m.pMarca));
                    lParametros.Add(new parametro("@precio", m.pPrecio));
                    lParametros.Add(new parametro("@fecha", m.pFecha));

                    if (oDB.actualizarDB(consultaSQL, lParametros) > 0)
                    {
                        MessageBox.Show("editado correctamente");
                        cargarLista();
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show("no se pudo editar");
                        limpiar();
                    }
                }
                habilitar(false);
            }
            
        }

        private bool existe(moto nueva)
        {
            for (int i = 0; i < lMotos.Count; i++)
            {
                if(lMotos[i].pCodigo == nueva.pCodigo)
                    return true;
            }
            return false;
        }

        private bool validar()
        {
            if(txtCodigo.Text == string.Empty)
            {
                MessageBox.Show("debe ingresar un codigo");
                txtCodigo.Focus();
                return false;
            }
            else
            {
                try
                {
                    Convert.ToInt32(txtCodigo.Text);
                }
                catch (Exception)
                {

                    MessageBox.Show("debe ingresar valores numericos");
                    txtCodigo.Focus();
                    return false;
                }
            }
            if(txtModelo.Text == string.Empty)
            {
                MessageBox.Show("debe ingresar un modelo");
                txtModelo.Focus();
                return false;
            }
            if(cboMarca.SelectedIndex == -1)
            {
                MessageBox.Show("debe seleccionar una marca");
                cboMarca.Focus();
                return false;
            }
            if (txtPrecio.Text == string.Empty)
            {
                MessageBox.Show("debe ingresar un precio");
                txtPrecio.Focus();
                return false;
            }
            else
            {
                try
                {
                    Convert.ToDouble(txtPrecio.Text);
                }
                catch (Exception)
                {

                    MessageBox.Show("debe ingresar valores numericos");
                    txtPrecio.Focus();
                    return false;
                }
            }
            if(dtpFecha.Value > DateTime.Today)
            {
                MessageBox.Show("no puede ingresar fechas mayores a la actual");
                dtpFecha.Focus();
                return false;
            }
            return true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            habilitar(true);
            limpiar();
            txtCodigo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            habilitar(true);
            txtCodigo.Enabled = false;
            lstMotos.Enabled = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
            habilitar(false);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("desea salir?"
                ,"SALIR"
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Question
                , MessageBoxDefaultButton.Button2)
                == DialogResult.Yes)
                Close();
        }

        private void lstMotos_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarCampo(lstMotos.SelectedIndex);
        }

        private void cargarCampo(int posicion)
        {
            txtCodigo.Text = lMotos[posicion].pCodigo.ToString();
            txtModelo.Text = lMotos[posicion].pModelo.ToString();
            cboMarca.SelectedValue = lMotos[posicion].pMarca;
            txtPrecio.Text = lMotos[posicion].pPrecio.ToString();
            dtpFecha.Value = lMotos[posicion].pFecha;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("desea eliminar este registro?"
                , "ELIMINAR"
                ,MessageBoxButtons.YesNo
                , MessageBoxIcon.Warning
                ,MessageBoxDefaultButton.Button2)
                == DialogResult.Yes)
            {
                try
                {
                    string consultaSQL = "DELETE MOTOS WHERE codigo=" + lMotos[lstMotos.SelectedIndex].pCodigo;
                    if (oDB.actualizarDB(consultaSQL) > 0)
                    {
                        MessageBox.Show("registro eliminado correctamente");
                        cargarLista();
                        limpiar();
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("debe seleccionar un registro para eliminar");
                }
                
            }
            habilitar(false);
        }
    }
}
