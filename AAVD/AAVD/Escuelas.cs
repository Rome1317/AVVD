using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AAVD
{
    public partial class Escuelas : Form
    {
        string buff = "";
        DialogResult dialogResult;

        DataTable view = new DataTable();

        School data = new School();
        Conexion plug_in = new Conexion();

        School registro = new School();

        public Escuelas()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Escuelas_Load(object sender, EventArgs e)
        {
            view.Columns.Add("Clave");
            view.Columns.Add("Nombre");
            view.Columns.Add("RFC");
            view.Columns.Add("Depto. 1");
            view.Columns.Add("Depto. 2");
            view.Columns.Add("Depto. 3");
            view.Columns.Add("Fecha_Inaguracion");
            view.Columns.Add("Pais");
            view.Columns.Add("Ciudad");


            comboBox1.Items.Add("nombre");
            comboBox1.Items.Add("rfc");
            comboBox1.Items.Add("pais");
            comboBox1.Items.Add("ciudad");
            comboBox1.Items.Add("fecha_inaguracion");

            dataGridView1.DataSource = view;
        }

        private void btn_registrar_Click(object sender, EventArgs e)
        {
            data.Departamentos = new List<string>();

            if (clave.Text == "" || Nombre.Text == "" || rfc.Text == "" || Departamento.Text == "" || dateTimePicker1.Text == "" || pais.Text == "" || ciudad.Text == "")
                MessageBox.Show("Completa todos los campos.", "Error", 0, MessageBoxIcon.Information);
            else
            {
                DateTime Shortdate = dateTimePicker1.Value.Date;

                data.Clave = Convert.ToInt32(clave.Text);
                data.Nombre = Nombre.Text;
                data.RFC = rfc.Text;
                data.Departamentos.Add(Departamento.Text);
                data.Fecha_Inaguracion = Shortdate.GetDateTimeFormats('d')[6];
                data.Pais = pais.Text;
                data.Ciudad = ciudad.Text;

                plug_in.InsertSchool(data);
                plug_in.InsertSchoolDepartment(data);


                MessageBox.Show("Escuela creada con exito.", "Insert", 0, MessageBoxIcon.Information);

                dialogResult = MessageBox.Show("Agregar un nuevo departamento?", "Insert", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    btn_registrar.Enabled = false;
                    clave.Enabled = false;
                    Departamento.Text = buff;
                }
                else
                {

                    clave.Text = buff;
                    Nombre.Text = buff;
                    rfc.Text = buff;
                    Departamento.Text = buff;
                    pais.Text = buff;
                    ciudad.Text = buff;
                }

            }
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            data.Departamentos = new List<string>();

            if (clave.Text == "" || Nombre.Text == "" || rfc.Text == "" || Departamento.Text == "" || dateTimePicker1.Text == "" || pais.Text == "" || ciudad.Text == "")
                MessageBox.Show("Completa todos los campos.", "Error", 0, MessageBoxIcon.Information);
            else
            {
                DateTime Shortdate = dateTimePicker1.Value.Date;

                data.Clave = Convert.ToInt32(clave.Text);
                data.Nombre = Nombre.Text;
                data.RFC = rfc.Text;
                data.Departamentos.Add(Departamento.Text);
                data.Fecha_Inaguracion = Shortdate.GetDateTimeFormats('d')[6];
                data.Pais = pais.Text;
                data.Ciudad = ciudad.Text;

                plug_in.InsertSchoolDepartment(data);
                plug_in.UpdateSchool(data);

                MessageBox.Show("Datos actualizados con exito.", "Update", 0, MessageBoxIcon.Information);

                dialogResult = MessageBox.Show("Agregar un nuevo departamento?", "Escuela", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    btn_registrar.Enabled = false;
                    clave.Enabled = false;
                    Departamento.Text = buff;

                    view.Rows.Clear();
                    plug_in.SchoolList(plug_in.GetAll(), view);
                }
                else
                {
                    view.Rows.Clear();
                    plug_in.SchoolList(plug_in.GetAll(), view);

                    btn_registrar.Enabled = true;
                    clave.Enabled = true;
                    clave.Text = buff;
                    Nombre.Text = buff;
                    rfc.Text = buff;
                    Departamento.Text = buff;
                    pais.Text = buff;
                    ciudad.Text = buff;
                }

            }
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            if (text_buscar.Text == "")
                MessageBox.Show("No hay datos de busqueda.", "Search", 0, MessageBoxIcon.Information);

            else
            {
                view.Rows.Clear();

                School registro = new School();

                data.Clave = Convert.ToInt32(text_buscar.Text);

                registro = plug_in.ShowSchool(data);

                if (registro.Nombre != null)
                {


                    DataRow row = view.NewRow();

                    row["Clave"] = registro.Clave;
                    row["Nombre"] = registro.Nombre;
                    row["RFC"] = registro.RFC;


                    int d = 1;
                    for (int i = 0; i < registro.Departamentos.Count; i++)
                    {
                        string depts = "Depto. ";
                        depts += d;
                        d++;
                        row[depts] = registro.Departamentos[i];
                    }

                    row["Fecha_Inaguracion"] = registro.Date;
                    row["Pais"] = registro.Pais;
                    row["Ciudad"] = registro.Ciudad;

                    view.Rows.Add(row);
                }

                else
                    MessageBox.Show("Escuela no encontrada.", "Search", 0, MessageBoxIcon.Information);
            }
        }

        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            if (registro.Clave == 0)
                MessageBox.Show("Seleccione algun registro.", "Error", 0, MessageBoxIcon.Information);
            else
            {
                btn_registrar.Enabled = false;
                clave.Enabled = false;

                dialogResult = MessageBox.Show("Esta seguro que eliminar este registro?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    plug_in.DeleteSchool(registro);
                    MessageBox.Show("Escuela eliminada con exito.", "Update", 0, MessageBoxIcon.Information);

                    view.Rows.Clear();
                    plug_in.SchoolList(plug_in.GetAll(), view);

                    btn_registrar.Enabled = true;
                    clave.Enabled = true;
                    clave.Text = buff;
                    Nombre.Text = buff;
                    rfc.Text = buff;
                    Departamento.Text = buff;
                    pais.Text = buff;
                    ciudad.Text = buff;
                }
            }

        }

        private void btn_buscartodo_Click(object sender, EventArgs e)
        {
            view.Rows.Clear();
            plug_in.SchoolList(plug_in.GetAll(), view);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            registro.Clave = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());

            registro = plug_in.ShowSchool(registro);

            btn_registrar.Enabled = false;
            clave.Enabled = false;

            clave.Text = registro.Clave.ToString();
            Nombre.Text = registro.Nombre;
            rfc.Text = registro.RFC;
            if (registro.Departamentos != null)
            {
                Departamento.Text = registro.Departamentos[0];
            }
            dateTimePicker1.Text = registro.Fecha_Inaguracion;
            pais.Text = registro.Pais;
            ciudad.Text = registro.Nombre;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            if (Departamento.Text == "" || clave.Text == "")
                MessageBox.Show("Escriba la clave y el nombre del departamento.", "Error", 0, MessageBoxIcon.Information);

            else
            {
                registro.Clave = Convert.ToInt32(clave.Text);
                string dept = Departamento.Text;

                dialogResult = MessageBox.Show("Se eliminara el departamento escrito en el campo. Desea continuar?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    if (plug_in.DeleteSchoolDepartment(registro, dept))
                    {
                        MessageBox.Show("Departamento eliminado con exito.", "Update", 0, MessageBoxIcon.Information);

                        view.Rows.Clear();
                        plug_in.SchoolList(plug_in.GetAll(), view);

                        Departamento.Text = buff;
                    }
                    else
                        MessageBox.Show("Departamento no encontrado.", "Update", 0, MessageBoxIcon.Information);
                }

            }
        }

        private void Eliminar_campo_Click(object sender, EventArgs e)
        {
            if (registro.Clave == 0)
                MessageBox.Show("Seleccione algun registro.", "Error", 0, MessageBoxIcon.Information);
            else
            {

                string coulumn = comboBox1.Text;

                if(coulumn == "")
                    MessageBox.Show("Seleccione alguna columna.", "Error", 0, MessageBoxIcon.Information);
                else
                {
                    plug_in.DeleteSchoolCoulumn(registro, coulumn);
                    MessageBox.Show("campo eliminado con exito.", "Update", 0, MessageBoxIcon.Information);

                    view.Rows.Clear();
                    plug_in.SchoolList(plug_in.GetAll(), view);
                }

                int index = comboBox1.SelectedIndex;

                if (index == 0)
                {
                    Nombre.Text = buff;
                }
                else if (index == 1)
                {
                    rfc.Text = buff;
                }
                else if (index == 2)
                {
                    pais.Text = buff;
                }
                else if (index == 3)
                {
                    ciudad.Text = buff;
                }


            }

        }

        private void clave_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Solo numeros
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
              if (Char.IsControl(e.KeyChar)) 
            {
                e.Handled = false;
            }
            else
            {
                //el resto de teclas pulsadas se desactivan
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btn_registrar.Enabled = true;
            clave.Enabled = true;
            clave.Text = buff;
            Nombre.Text = buff;
            rfc.Text = buff;
            Departamento.Text = buff;
            pais.Text = buff;
            ciudad.Text = buff;
        }
    }
}
