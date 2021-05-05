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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            //DA UN TIPO DE TAMAÑO A LA IMAGEN
            if (!File.Exists(Path.GetTempPath()+"Users.txt"))
            {
                GeneraDatosAdmin();
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void GeneraDatosAdmin()
        {
            TextWriter sw = new StreamWriter(Path.GetTempPath() + "Users.txt");
            
                sw.WriteLine("Ktomasm");
                sw.WriteLine("admin");
            sw.WriteLine("Admin");
            sw.WriteLine("Activo");

            sw.Close();
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
            Application.Exit();
        }
        //MINIMIZA EL LOGIN
        private void btnminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }



        private void Form1_Load(object sender, EventArgs e)
        {

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
                    if (ExisteyActivo(user, Pass)&&!rol(user,Pass).Equals(""))
                    {
                        //LANZA EL FORMULAIO PRINCIPAL PASANDOLE COMO PARAMETRO EL USUARIO Y OCULTA ESTE
                        lblErrorMessagge.Visible = false;
                        FormPrincipal mainMenu = new FormPrincipal(user,rol(user,Pass));
                        mainMenu.Show();
                        mainMenu.FormClosed += Logout;

                        this.Hide();
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
        private bool ExisteyActivo(string us, string pass)
        {
            Users obj1 = new Users();
            TextReader sr = new StreamReader(Path.GetTempPath() + "Users.txt");
            obj1.User = sr.ReadLine();

            while (obj1.User != null)
            {
                obj1.Pass = sr.ReadLine();
                obj1.Rol = sr.ReadLine();
                obj1.Estado = sr.ReadLine();
                if (obj1.User.Equals(us) && obj1.Pass.Equals(pass) && obj1.Estado.Equals("Activo"))
                {
                    sr.Close();
                    return true;
                }
                obj1.User = sr.ReadLine();
            }
            sr.Close();
            return false;
        }
        //FUNCION QUE RETORNA EL ROL DEL EMPLEADO
        private string rol(string us, string pass)
        {
            Users obj1 = new Users();
            TextReader sr = new StreamReader(Path.GetTempPath() + "Users.txt");
            obj1.User = sr.ReadLine();

            while (obj1.User != null)
            {
                obj1.Pass = sr.ReadLine();
                obj1.Rol = sr.ReadLine();
                obj1.Estado = sr.ReadLine();
                if (obj1.User.Equals(us) && obj1.Pass.Equals(pass))
                {
                    sr.Close();
                    return obj1.Rol;
                }
                obj1.User = sr.ReadLine();
            }
            sr.Close();
            return "";
        }
        //MUESTRA LOS MENSAJES DE ERROR
        private void msgError(string msg)
        {
            lblErrorMessagge.Text = "      " + msg;
            lblErrorMessagge.Visible = true;
        }
        //CUANDO SE CIERRE SESION EL FORMULARIO PRINCIPAL SE CIERRA Y SE MUESTRA ESTE DE NUEVO
        private void Logout(object sender,FormClosedEventArgs e)
        {
            txtpass.Clear();
            txtuser.Clear();
            txtuser.Text = "Usuario";
            txtuser.ForeColor = Color.Silver;
            txtuser.Focus();
            lblErrorMessagge.Visible = false;
            this.Show();
            txtpass.Text = "Contraseña";
            txtpass.ForeColor = Color.LightGray;
            txtpass.UseSystemPasswordChar = false;
            
        }

        private void linkpass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registro vq = new Registro();
            vq.Show();
            vq.FormClosed += Logout;

            this.Hide();
        }
    }
}
