using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
namespace Presentacion
{
    public partial class FormPrincipal : Form
    {
        /**
         * Hashtable que es el encargado de Generar y almacenar temporalmente las palabras confirmadas de un principio
         * cuando el programa esta recien abierto y no se cuentan con registros
         */
        System.Collections.Hashtable palabras_confirmadasTemporal = new System.Collections.Hashtable();
        /*Nombre: almacena el nombre de la factura seleccionada para poder escribirlo en los Registros*/
        string Nombre = "",Rol="";
        /**
         * Funcion Principal en donde se recibe un parametro: Nombre DE Usuario para mostrarlo en este form
         * ademas se comprueba si los archivos PalabrasConfirmadas.txt y Facturas existen , en dado caso
         * que no se generan (PalabrasConfirmadas genera sus datos iniciales mientras que facturas solo se crea el archivo)
         */
        public FormPrincipal(string NombreUser, string rol)
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            
            Nombre = "";
            Rol = "";
            Rol = rol;
            Nombre = NombreUser;
            /**
             * Condicion que comprueba si el archivo existe
             * sino lo crea e inserta datos iniciales
             */
            if (!File.Exists(Path.GetTempPath() + "PalabrasConfirmadas.txt"))
            {
                GeneraDatos();
            }
            /**
             * Condicion que comprueba si el archivo existe
             * sino lo crea
             */
            if (!File.Exists(Path.GetTempPath() + "Facturas.txt"))
            {
                FactGen();
            }
            /*Se le saluda al usuario en el titulo del form*/
            label1.Text = "Bienvenid@ " + Nombre;
            /*Se coloca su nombre en el panel izquierdo donde se muestra mas info */
            iconButton4.Text = Nombre;
            //Si es empleado el boton de usuarios se le oculta
            if (rol.Equals("Empleado"))
            {
                iconButton5.Visible = false;
            }
        }
        /*Funcion Encargada de Crear archivo PalabrasConfirmadas y llenarlo de datos iniciales
         si es la primera vez que se ejecuta el programa*/
        private void GeneraDatos()
        {
            /*se insertan datos a un arraylist llamado palabras_confirmadasTemporal */
            palabras_confirmadasTemporal.Add(1, "Hola");
        palabras_confirmadasTemporal.Add(2, "Mundo");
        palabras_confirmadasTemporal.Add(3, "Pan");
        palabras_confirmadasTemporal.Add(4, "Dulce");
            /*Se escriben las palabras iniciales en el archivo*/
            TextWriter sw = new StreamWriter(Path.GetTempPath() + "PalabrasConfirmadas.txt");
            for (int i = 1; i <= palabras_confirmadasTemporal.Count; i++)
            {
                sw.WriteLine(i);
                sw.WriteLine(palabras_confirmadasTemporal[i].ToString());
            }
            sw.Close();
        }
        /*Funcion que Genera el archivo Facturas*/
        private void FactGen()
        {
           
            TextWriter sw2 = new StreamWriter(Path.GetTempPath() + "Facturas.txt");

            sw2.Close();
        }
        /*Cuando cargue el form se settea esto en el titulo*/
        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            label1.Text = "Bienvenid@ " + Nombre;
        }
        #region Funcionalidades del formulario
        //RESIZE METODO PARA REDIMENCIONAR/CAMBIAR TAMAÑO A FORMULARIO EN TIEMPO DE EJECUCION ----------------------------------------------------------
        private int tolerance = 12;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        //----------------DIBUJAR RECTANGULO / EXCLUIR ESQUINA PANEL 
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));

            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);

            region.Exclude(sizeGripRectangle);
            this.panelContenedor.Region = region;
            this.Invalidate();
        }
        //----------------COLOR Y GRIP DE RECTANGULO INFERIOR
        protected override void OnPaint(PaintEventArgs e)
        {
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(244, 244, 244));
            e.Graphics.FillRectangle(blueBrush, sizeGripRectangle);

            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //Capturar posicion y tamaño antes de maximizar para restaurar
        int lx, ly;
        int sw, sh;
        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            lx = this.Location.X;
            ly = this.Location.Y;
            sw = this.Size.Width;
            sh = this.Size.Height;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
        }
        //BOTON PARA RESTARURAR LA VENTANA
        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            btnMaximizar.Visible = true;
            btnRestaurar.Visible = false;
            this.Size = new Size(sw, sh);
            this.Location = new Point(lx, ly);
        }

        private void panelBarraTitulo_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //METODO PARA ARRASTRAR EL FORMULARIO---------------------------------------------------------------------
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        
       



        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        //EVENTO QUE DESPLIEGA EL FORMULARIO DE INGRESO DE FACTURAS

        private void iconButton1_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Diccionario>();
            label1.Text = "Subir Factura";
            button1.BackColor = Color.FromArgb(12, 61, 92);
        }
        //EVENTO QUE DESPLIEGA EL FORMULARIO DE MONITOREO DE PALABRAS CONFIRMADAS
        private void iconButton1_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario<Form2>();
            label1.Text = "Monitor De Palabras Confirmadas en tiempo Real";
            iconButton1.BackColor = Color.FromArgb(12, 61, 92);
        }
        //EVENTO QUE DESPLIEGA EL FORMULARIO DE LAS FACTURAS EN TIEMPO REAL
        private void iconButton2_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Form3>();
            label1.Text = "Monitor de Facturas en tiempo Real";
            iconButton2.BackColor = Color.FromArgb(12, 61, 92);
        }
        //EVENTO QUE DESPLIEGA EL FORMULARIO DE ADMINISTRACION DE ACCESO A LOS USUARIOS
        private void iconButton5_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Form1>();
            label1.Text = "Administrar Acceso a Usuarios";
            iconButton5.BackColor = Color.FromArgb(12, 61, 92);
        }
       
        private void iconButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
           
                MessageBox.Show("Proyecto Programacion 3\n" 
                                + "Hecho Por:\n"
                                +"Kimberly Jemima Tomas Montes De Oca\n"
                                + "Carnet:1290-19-11531", "Acerca del Programa");
            
            
        }

   
        #endregion
        //METODO PARA ABRIR FORMULARIOS DENTRO DEL PANEL
        private void AbrirFormulario<MiForm>() where MiForm : Form, new()
        {
            Form formulario;
            formulario = panelformularios.Controls.OfType<MiForm>().FirstOrDefault();//Busca en la colecion el formulario
            //si el formulario/instancia no existe
            if (formulario == null)
            {
                formulario = new MiForm();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                panelformularios.Controls.Add(formulario);
                panelformularios.Tag = formulario;
                formulario.Show();
                formulario.BringToFront();
                formulario.FormClosed += new FormClosedEventHandler(CloseForms);
            }
            //si el formulario/instancia existe
            else
            {
                formulario.BringToFront();
            }
        }
        //PARA CERRAR LOS FORMS(INSTANCIAS)
        private void CloseForms(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms["Diccionario"] == null)
            {
                button1.BackColor = Color.FromArgb(4, 41, 68);
                label1.Text = "Bienvenid@ " + Nombre;
            }
            if (Application.OpenForms["Form2"] == null)
            {
                iconButton1.BackColor = Color.FromArgb(4, 41, 68);
                label1.Text = "Bienvenid@ " + Nombre;
            }
            if (Application.OpenForms["Form3"] == null)
            {
                iconButton2.BackColor = Color.FromArgb(4, 41, 68);
                label1.Text = "Bienvenid@ " + Nombre;

            }
            if (Application.OpenForms["Form1"] == null)
            {
                iconButton5.BackColor = Color.FromArgb(4, 41, 68);
                label1.Text = "Bienvenid@ " + Nombre;

            }
        }
    }
}
