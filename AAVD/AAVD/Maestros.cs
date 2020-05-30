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
    public partial class Maestros : Form
    {
        Professor data = new Professor();

        Conexion connect = new Conexion();

        string buff = "";
        DialogResult dialogResult;

        DataTable view = new DataTable();

        Professor registro = new Professor();

        public Maestros()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Maestros_Load(object sender, EventArgs e)
        {

            view.Columns.Add("CURP");
            view.Columns.Add("Nombre");
            view.Columns.Add("Apellido Paterno");
            view.Columns.Add("Apellido Materno");
            view.Columns.Add("Sexo");
            view.Columns.Add("Fecha Nacimiento");
            view.Columns.Add("Email 1");
            view.Columns.Add("Email 2");
            view.Columns.Add("Email 3");

            comboBox1.Items.Add("nombre");
            comboBox1.Items.Add("apellidop");
            comboBox1.Items.Add("apellidom");
            comboBox1.Items.Add("sexo");
            comboBox1.Items.Add("fecha_nacimiento");


            dataGridView1.DataSource = view;

        }

        private void btn_registrar_Click_1(object sender, EventArgs e)
        {
            data.Emails = new List<string>();

            if (text_curp.Text == "" || text_nombre.Text == "" || text_email.Text == "" || dateTimePicker1.Text == "" || text_apellidoMat.Text == "" || text_apellidoPat.Text == "" || sexo.Text == "")
                MessageBox.Show("Completa todos los campos.", "Error", 0, MessageBoxIcon.Information);
            else
            {
                DateTime Shortdate = dateTimePicker1.Value.Date;

                data.CURP = text_curp.Text;
                data.Nombre = text_nombre.Text;
                data.Emails.Add(text_email.Text);
                data.Fecha_Nacimiento = Shortdate.GetDateTimeFormats('d')[6];
                data.ApellidoM = text_apellidoMat.Text;
                data.ApellidoP = text_apellidoPat.Text;
                data.Sexo = sexo.Text;

                connect.InsertProfessor(data);
                connect.InsertEmail(data);


                MessageBox.Show("Profesor dado de alta con exito.", "Insert", 0, MessageBoxIcon.Information);

                dialogResult = MessageBox.Show("Agregar un nuevo correo?", "Insert", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    btn_registrar.Enabled = false;
                    text_curp.Enabled = false;
                    text_email.Text = buff;
                }
                else
                {

                    text_curp.Text = buff;
                    text_nombre.Text = buff;
                    text_email.Text = buff;
                    text_apellidoMat.Text = buff;
                    text_apellidoPat.Text = buff;
                    sexo.Text = buff;
                }

            }
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            data.Emails = new List<string>();

            if (text_curp.Text == "" || text_nombre.Text == "" || text_email.Text == "" || dateTimePicker1.Text == "" || text_apellidoMat.Text == "" || text_apellidoPat.Text == "" || sexo.Text == "")
                MessageBox.Show("Completa todos los campos.", "Error", 0, MessageBoxIcon.Information);
            else
            {
                DateTime Shortdate = dateTimePicker1.Value.Date;

                data.CURP = text_curp.Text;
                data.Nombre = text_nombre.Text;
                data.Emails.Add(text_email.Text);
                data.Fecha_Nacimiento = Shortdate.GetDateTimeFormats('d')[6];
                data.ApellidoM = text_apellidoMat.Text;
                data.ApellidoP = text_apellidoPat.Text;
                data.Sexo = sexo.Text;

                connect.UpdateProfessor(data);
                connect.InsertEmail(data);

                MessageBox.Show("Datos actualizados con exito.", "Update", 0, MessageBoxIcon.Information);

                dialogResult = MessageBox.Show("Agregar un nuevo correo?", "Profesor", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    btn_registrar.Enabled = false;
                    text_curp.Enabled = false;
                    text_email.Text = buff;

                    view.Rows.Clear();
                    connect.ShowALL(connect.GetProfessors(), view);
                }
                else
                {
                    view.Rows.Clear();
                    connect.ShowALL(connect.GetProfessors(), view);

                    text_curp.Text = buff;
                    text_nombre.Text = buff;
                    text_email.Text = buff;
                    text_apellidoMat.Text = buff;
                    text_apellidoPat.Text = buff;
                    sexo.Text = buff;
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

                Professor registro = new Professor();

                data.CURP = text_buscar.Text;

                registro = connect.ShowProfessor(data);

                if (registro.Nombre != null)
                {


                    DataRow row = view.NewRow();

                    row["CURP"] = registro.CURP;
                    row["Nombre"] = registro.Nombre;

                    int d = 1;
                    for (int i = 0; i < registro.Emails.Count; i++)
                    {
                        string email = "Email ";
                        email += d;
                        d++;
                        row[email] = registro.Emails[i];
                    }

                    row["Apellido Materno"] = registro.ApellidoM;
                    row["Apellido Paterno"] = registro.ApellidoP;
                    row["Sexo"] = registro.Sexo;
                    row["Fecha Nacimiento"] = registro.Fecha;

                    view.Rows.Add(row);
                }

                else
                    MessageBox.Show("Profesor no encontrado.", "Search", 0, MessageBoxIcon.Information);
            }
        }

        private void btn_buscartodo_Click(object sender, EventArgs e)
        {
            view.Rows.Clear();
            connect.ShowALL(connect.GetProfessors(), view);
        }

        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            if (registro.CURP == "")
                MessageBox.Show("Seleccione algun registro.", "Error", 0, MessageBoxIcon.Information);
            else
            {
                btn_registrar.Enabled = false;
                text_curp.Enabled = false;

                dialogResult = MessageBox.Show("Esta seguro que eliminar este registro?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    connect.DeleteProfessor(registro);
                    MessageBox.Show("Profesor eliminado con exito.", "Update", 0, MessageBoxIcon.Information);

                    view.Rows.Clear();
                    connect.ShowALL(connect.GetProfessors(), view);

                    btn_registrar.Enabled = true;
                    text_curp.Enabled = true;
                    text_curp.Text = buff;
                    text_nombre.Text = buff;
                    text_email.Text = buff;
                    text_apellidoMat.Text = buff;
                    text_apellidoPat.Text = buff;
                    sexo.Text = buff;
                }

            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            if (text_email.Text == "" || text_curp.Text == "")
                MessageBox.Show("Escriba el CURP y el nombre del email.", "Error", 0, MessageBoxIcon.Information);

            else
            {
                registro.CURP = text_curp.Text;
                string email = text_email.Text;

                dialogResult = MessageBox.Show("Se eliminara el email escrito en el campo. Desea continuar?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    if (connect.DeleteEmail(registro, email))
                    {
                        MessageBox.Show("Email eliminado con exito.", "Update", 0, MessageBoxIcon.Information);

                        view.Rows.Clear();
                        connect.ShowALL(connect.GetProfessors(), view);

                        text_email.Text = buff;
                    }
                    else
                        MessageBox.Show("Email no encontrado.", "Update", 0, MessageBoxIcon.Information);
                }

            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            registro.CURP = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            registro = connect.ShowProfessor(registro);

            btn_registrar.Enabled = false;
            text_curp.Enabled = false;

            text_curp.Text = registro.CURP;
            text_nombre.Text = registro.Nombre;
            text_email.Text = registro.Emails[0];
            dateTimePicker1.Text = registro.Fecha_Nacimiento;
            text_apellidoMat.Text = registro.ApellidoM;
            text_apellidoPat.Text = registro.ApellidoP;
            sexo.Text = registro.Sexo;
        }

        private void Eliminar_campo_Click(object sender, EventArgs e)
        {
            if (registro.CURP == "")
                MessageBox.Show("Seleccione algun registro.", "Error", 0, MessageBoxIcon.Information);
            else
            {

                string coulumn = comboBox1.Text;

                if (coulumn == "")
                    MessageBox.Show("Seleccione alguna columna.", "Error", 0, MessageBoxIcon.Information);
                else
                {
                    connect.DeleteProfessorCoulumn(registro, coulumn);
                    MessageBox.Show("Campo eliminado con exito.", "Update", 0, MessageBoxIcon.Information);

                    view.Rows.Clear();
                    connect.ShowALL(connect.GetProfessors(), view);
                }

                int index = comboBox1.SelectedIndex;

                if (index == 0)
                {
                    text_nombre.Text = buff;
                }
                else if (index == 1)
                {
                    text_apellidoPat.Text = buff;
                }
                else if (index == 2)
                {
                    text_apellidoMat.Text = buff;
                }
                else if (index == 3)
                {
                    sexo.Text = buff;
                }



            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btn_registrar.Enabled = true;
            text_curp.Enabled = true;
            text_curp.Text = buff;
            text_nombre.Text = buff;
            text_email.Text = buff;
            text_apellidoMat.Text = buff;
            text_apellidoPat.Text = buff;
            sexo.Text = buff;
        }
    }
  
}
