using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace PracticeAdo
{
    public partial class CrearRegistros : Form
    {
        public CrearRegistros()
        {
            InitializeComponent();
        }

        private void btnagregar_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
           
            

            string foto = openFileDialog1.FileName;
            pictureBox1.ImageLocation = foto;

        }
        private void cargar()
        {
            dgvconsultas.DataSource = SellecALLpersona();
            dgvconsultas.Columns["eliminado"].Visible = false;
        }
        
        private void btnguardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtnombre.Text) | string.IsNullOrWhiteSpace(txtapellido.Text) |
                string.IsNullOrWhiteSpace(cmbp.Text) | string.IsNullOrWhiteSpace(cmbsexo.Text) |
                string.IsNullOrWhiteSpace(txtemai.Text) |
                mtbtel.MaskCompleted == false)
            {
                MessageBox.Show("Complete los campos ");

            }
            else if (string.IsNullOrWhiteSpace(txtid.Text)==true)
            {
                SqlCommand command = null;
                SqlConnection con = new SqlConnection(Properties.Settings.Default.Connection);

                

                command = con.CreateCommand();

                string name = txtnombre.Text;
                string apellido = txtapellido.Text;
                DateTime fdn = dtpa.Value;
                string  ft = pictureBox1.ImageLocation;        
                int p = int.Parse(cmbp.SelectedValue.ToString());
                string sex = cmbsexo.SelectedValue.ToString();
                string c = txtemai.Text;
                string tel = mtbtel.Text;
                Boolean deleted = false;


                command.CommandText = @"INSERT INTO [dbo].[Personas]
           ([nombre]
           ,[apellido]
           ,[fecha_nac]
           ,[foto]
           ,[pais]
           ,[sexo]
           ,[telefono]
           ,[email]
           ,[eliminado])
     VALUES
           (@nombre, 
           @apellido, 
           @fecha_nac, 
           @foto, 
           @pais, 
           @sexo, 
           @telefono,
           @email,
           @eliminado)";
                command.Parameters.AddWithValue("@nombre", name);
                command.Parameters.AddWithValue("@apellido", apellido);
                command.Parameters.AddWithValue("@fecha_nac", fdn);
                command.Parameters.AddWithValue("@foto", ft);
                command.Parameters.AddWithValue("@pais", p);
                command.Parameters.AddWithValue("@sexo", sex);
                command.Parameters.AddWithValue("@telefono", tel);
                command.Parameters.AddWithValue("@email", c);
                command.Parameters.AddWithValue("@eliminado", deleted);

                con.Open();

                command.ExecuteNonQuery();
                MessageBox.Show("Persona Guardada");

                con.Close();



                limpiar();
                cargar();

                

            }
            else if (string.IsNullOrWhiteSpace(txtid.Text) == false)
            {
                SqlCommand command = null;
                SqlConnection con = new SqlConnection(Properties.Settings.Default.Connection);



                command = con.CreateCommand();

                int id = int.Parse(txtid.Text);
                string name = txtnombre.Text;
                string apellido = txtapellido.Text;
                DateTime fdn = dtpa.Value;
                string ft = pictureBox1.ImageLocation;
                int p = int.Parse(cmbp.SelectedValue.ToString());
                string sex = cmbsexo.SelectedValue.ToString();
                string c = txtemai.Text;
                string tel = mtbtel.Text;
                Boolean deleted = false;

                command.CommandText = @" UPDATE [dbo].[Personas]
   SET [nombre] = @nombre
      ,[apellido] = @apellido
      ,[fecha_nac] = @fecha_nac
      ,[foto] = @foto
      ,[pais] = @pais
      ,[sexo] = @sexo
      ,[telefono] =@telefono
      ,[email] = @email
      ,[eliminado] = @eliminado
       WHERE [id]= @id" ;

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@nombre", name);
                command.Parameters.AddWithValue("@apellido", apellido);
                command.Parameters.AddWithValue("@fecha_nac", fdn);
                command.Parameters.AddWithValue("@foto", ft);
                command.Parameters.AddWithValue("@pais", p);
                command.Parameters.AddWithValue("@sexo", sex);
                command.Parameters.AddWithValue("@telefono", tel);
                command.Parameters.AddWithValue("@email", c);
                command.Parameters.AddWithValue("@eliminado", deleted);
                

                con.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Persona actualizada");

                con.Close();

                limpiar();
                cargar();



            }


        }

        private void CrearRegistros_Load(object sender, EventArgs e)
        {
            


            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("sexo");
            dt.Rows.Add("M", "Masculino");
            dt.Rows.Add("F", "Femenino");

            cmbsexo.DataSource = dt;
            cmbsexo.DisplayMember = "sexo";
            cmbsexo.ValueMember = "id";

            cmbp.DataSource = seleccion();
            cmbp.DisplayMember = "nombre";
            cmbp.ValueMember = "idpais";

            cargar();

            

            
           
         

        }

        private void limpiar()
        {
            foreach (var i in this.Controls)
            {
                if (i is TextBox)
                {
                    ((TextBox)i).Text = "";
                }
                if (i is MaskedTextBox)
                {
                    ((MaskedTextBox)i).Text = "";
                }
                if (i is PictureBox)
                {
                    ((PictureBox)i).Image = null;
                }
            }
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
            cargar();
        }

        private static List<Pais> seleccion()
        {

            SqlCommand command = null;
            SqlDataReader datareader = null;
            List<Pais> lista = new List<Pais>();
            SqlConnection con = new SqlConnection(Properties.Settings.Default.Connection);
            con.Open();

            command = con.CreateCommand();

            command.CommandText = "SELECT * FROM pais";
            datareader = command.ExecuteReader();
            while (datareader.Read())
            {
                Pais p = new Pais();
                p.idpais = (int)datareader["idpais"];
                p.nombre = (string)datareader["nombre"];
                lista.Add(p);
            }
            datareader.Close();
            con.Close();
            return lista;
            


        }
        private static List<Persona> SellecALLpersona()
        {
            SqlCommand command = null;
            SqlDataReader datareader = null;
            List<Persona> listap = new List<Persona>();
            SqlConnection con = new SqlConnection(Properties.Settings.Default.Connection);
            con.Open();
            command = con.CreateCommand();
            command.CommandText = "SELECT * FROM Personas where eliminado=0";
            datareader = command.ExecuteReader();
            while (datareader.Read())
            {
                Persona person = new Persona();
                person.id = (int)datareader["id"];
                person.nombre = (string)datareader["nombre"];
                person.apellido = (string)datareader["apellido"];
                person.fecha_nac = (DateTime)datareader["fecha_nac"];
                person.foto = (string)datareader["foto"];
                person.pais = (int)datareader["pais"];
                person.sexo = (string)datareader["sexo"];
                person.telefono = (string)datareader["telefono"];
                person.email = (string)datareader["email"];
                person.eliminado = (Boolean)datareader["eliminado"];
                listap.Add(person);
            }
            datareader.Close();
            con.Close();
            return listap;
        }

        private void dgvconsultas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtid.Text = dgvconsultas.CurrentRow.Cells[0].Value.ToString();
                txtnombre.Text = dgvconsultas.CurrentRow.Cells[1].Value.ToString();
                txtapellido.Text = dgvconsultas.CurrentRow.Cells[2].Value.ToString();
                dtpa.Value = Convert.ToDateTime(dgvconsultas.CurrentRow.Cells[3].Value.ToString());
                pictureBox1.ImageLocation = dgvconsultas.CurrentRow.Cells[4].Value.ToString();
                cmbp.Text = dgvconsultas.CurrentRow.Cells[5].Value.ToString();
                cmbsexo.Text = dgvconsultas.CurrentRow.Cells[6].Value.ToString();
                txtemai.Text = dgvconsultas.CurrentRow.Cells[7].Value.ToString();
                mtbtel.Text = dgvconsultas.CurrentRow.Cells[8].Value.ToString();

            }
            catch
            {

            }

        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (dgvconsultas.RowCount > 0)
            {
                try
                {
                    if (MessageBox.Show(@"Esta seguro que desea eliminar ?", @"Atención",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        eliminar();
                        cargar();
                    }

                }
                catch
                {
                    MessageBox.Show("Seleccione una celda ");
                }
            }
            else
            {
                MessageBox.Show("No hay campos para eliminar");
            }

            
        }
        private void eliminar()
        {
            SqlCommand command = null;
            SqlConnection con = new SqlConnection(Properties.Settings.Default.Connection);



            command = con.CreateCommand();

            int id = int.Parse(txtid.Text);
            Boolean deleted = true;

            command.CommandText = @" UPDATE [dbo].[Personas]
   SET [eliminado] = @eliminado
       WHERE [id]= @id";

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@eliminado", deleted);


            con.Open();
            command.ExecuteNonQuery();
            MessageBox.Show("Persona eliminada");

            con.Close();

            limpiar();
            cargar();


        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            dgvconsultas.DataSource= buscar();

        }
        private List<Persona> buscar()
        {
            SqlCommand command = null;
            SqlDataReader datareader = null;
            List<Persona> listap = new List<Persona>();
            SqlConnection con = new SqlConnection(Properties.Settings.Default.Connection);
            con.Open();
            command = con.CreateCommand();
            string name =txtbuscar.Text;
            command.CommandText = @"SELECT* FROM personas where eliminado = 0  and nombre like @nombre";

            command.Parameters.AddWithValue("@nombre", name);
            datareader = command.ExecuteReader();
            while (datareader.Read())
            {
                Persona person = new Persona();
                person.id = (int)datareader["id"];
                person.nombre = (string)datareader["nombre"];
                person.apellido = (string)datareader["apellido"];
                person.fecha_nac = (DateTime)datareader["fecha_nac"];
                person.foto = (string)datareader["foto"];
                person.pais = (int)datareader["pais"];
                person.sexo = (string)datareader["sexo"];
                person.telefono = (string)datareader["telefono"];
                person.email = (string)datareader["email"];
                person.eliminado = (Boolean)datareader["eliminado"];
                listap.Add(person);
            }
            datareader.Close();
            con.Close();
            return listap;
        }

        private void btnbuscar_TextChanged(object sender, EventArgs e)
        {
            dgvconsultas.DataSource = buscar();
        }
    }
}
