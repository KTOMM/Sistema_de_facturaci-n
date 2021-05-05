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
namespace Presentacion
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            //AL INICIALIZACE SE LEE TODO EL DOCUMENTO DE FACTURAS PARA POSTERIORMENTE MOSTRARLO EN EL MONITOR
            TextReader sr = new StreamReader(Path.GetTempPath() + "Facturas.txt");
            string Mensaje = "";
            Mensaje = sr.ReadToEnd();
            sr.Close();
            textBox1.Text = Mensaje;
            textBox1.ReadOnly = true;
            
        }
        //CIERRA ESTE FORM
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //ESCUCHA SIEMPRE SI HAY ALGUN CAMBIO EN EL ARCHIVO FACTURAS Y ASI SE CONSIGUE MOSTRARLO EN TIEMPO REAL
        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            textBox1.Text = "";
            TextReader sr = new StreamReader(Path.GetTempPath() + "Facturas.txt");
            string Mensaje = "";
            Mensaje = sr.ReadToEnd();
            sr.Close();
            textBox1.Text = Mensaje;
        }
        //INDICAMOS DONDE ESTA ALMACENADO EL ARCHIVO QUE ESCUCHARA EL FILESYSTEMWATCHER
        private void Form3_Load(object sender, EventArgs e)
        {
            fileSystemWatcher1.Path =Path.GetTempPath();
        }
    }
}
