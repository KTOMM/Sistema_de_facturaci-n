using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;
namespace Presentacion
{
    public partial class Registro : Form
    {
        public Registro()
        {
            InitializeComponent();

        }

        //FUNCION PARA MOVER FORM
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        //VERIFICA QUE EL USUARIO NO HA ESCRITO EN EL TEXTBOX FUNCION TIPO PLACEHOLDER
        private void txtuser_Enter(object sender, EventArgs e)
        {
            if (txtuser.Text == "Usuario")
            {
                txtuser.Text = "";
                txtuser.ForeColor = Color.LightGray;
            }
        }
        //VERIFICA QUE EL USUARIO NO HA ESCRITO EN EL TEXTBOX FUNCION TIPO PLACEHOLDER
        private void txtuser_Leave(object sender, EventArgs e)
        {
            if (txtuser.Text == "")
            {
                txtuser.Text = "Usuario";
                txtuser.ForeColor = Color.Silver;
            }
        }
        //VERIFICA QUE EL USUARIO NO HA ESCRITO EN EL TEXTBOX FUNCION TIPO PLACEHOLDER
        private void txtpass_Enter(object sender, EventArgs e)
        {
            if (txtpass.Text == "Contraseña")
            {
                txtpass.Text = "";
                txtpass.ForeColor = Color.LightGray;
                txtpass.UseSystemPasswordChar = true;
            }
        }
        //VERIFICA QUE EL USUARIO NO HA ESCRITO EN EL TEXTBOX FUNCION TIPO PLACEHOLDER
        private void txtpass_Leave(object sender, EventArgs e)
        {
            if (txtpass.Text == "")
            {
                txtpass.Text = "Contraseña";
                txtpass.ForeColor = Color.Silver;
                txtpass.UseSystemPasswordChar = false;
            }
        }

        //CIERRA LA APLICACION
        private void btncerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //MINIMIZA EL LOGIN
        private void btnminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }



       
        //FUNCION QUE PERMITE EL INGRESO DE USUARIOS
        private void btnlogin_Click(object sender, EventArgs e)
        {
            String user = txtuser.Text;
            String Pass = txtpass.Text;
            //VERIFICA QUE EL USUARIO NO DEJE LOS CAMPOS VACIOS DE SER EL CASO LANZA UN ERROR
            if (user != "Usuario")
            {
                if (Pass != "Contraseña")
                {
                    lblErrorMessagge.Visible = false;
                    //VERIFICA QUE EL USUARIO EXISTA Y ESTE ACTIVO PARA PODER INICIAR SESION
                    if (ExisteyActivo(user))
                    {
                        //LANZA EL FORMULAIO PRINCIPAL PASANDOLE COMO PARAMETRO EL USUARIO Y OCULTA ESTE
                        lblErrorMessagge.Visible = false;
                        RegistradoyDesactivado(user, Pass);
                        this.Close();
                    }
                    else
                    {
                        msgError("Revise sus credenciales.");
                        txtpass.Clear();
                        txtuser.Focus();
                    }
                }
                else msgError("Ingrese su contraseña.");
            }
            else msgError("Ingrese su nombre de usuario.");


        }
        //FUNCION QUE VERIFICA SI EL USUARIO EXISTE Y ESTA ACTIVO DENTRO DEL REGISTRO DE ARCHIVO
        private bool ExisteyActivo(string us)
        {
            Users obj1 = new Users();
            TextReader sr = new StreamReader(Path.GetTempPath() + "Users.txt");
            obj1.User = sr.ReadLine();

            while (obj1.User != null)
            {
                obj1.Pass = sr.ReadLine();
                obj1.Rol = sr.ReadLine();
                obj1.Estado = sr.ReadLine();
                if (obj1.User.Equals(us))
                {
                    sr.Close();
                    return false;
                }
                obj1.User = sr.ReadLine();
            }
            sr.Close();
            return true;
        }
        private void RegistradoyDesactivado(string us, string pass)
        {
            Users obj1 = new Users();
            TextWriter sr = new StreamWriter(Path.GetTempPath() + "Users.txt",true);
            sr.WriteLine(us);


            sr.WriteLine(pass);
                sr.WriteLine("Empleado");
                sr.WriteLine("Inactivo");
              
                
          
            sr.Close();
            MessageBox.Show("Por seguridad su usuario permanecera inactivo hasta que otro administrador lo active");
        }
        //MUESTRA LOS MENSAJES DE ERROR
        private void msgError(string msg)
        {
            lblErrorMessagge.Text = "      " + msg;
            lblErrorMessagge.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //CUANDO SE CIERRE SESION EL FORMULARIO PRINCIPAL SE CIERRA Y SE MUESTRA ESTE DE NUEVO




    }
}
