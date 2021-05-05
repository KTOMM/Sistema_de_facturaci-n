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
using System.Collections;
namespace Presentacion
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            //LEE EL ARCHIVO PALABRAS CONFIRMADAS Y ALMACENA LOS DATOS EN UN OBJETO
            TextReader sr = new StreamReader(Path.GetTempPath() + "PalabrasConfirmadas.txt");
            PalabrasConfirmadas PC = new PalabrasConfirmadas();
            PC.palabrasConfirmadas = new Hashtable();
            string Keys = "", Values = "";
            Keys = sr.ReadLine();
            Values = sr.ReadLine();
            while (Keys != null)
            {
                PC.palabrasConfirmadas.Add(int.Parse(Keys), Values);
                Keys = sr.ReadLine();
                Values = sr.ReadLine();
            }
            sr.Close();
            string mensaje = "";
            textBox1.Text = "";
            //SE MUESTRAN LOS DATOS DEL OBJETO EN UN TEXTBOX QUE ES EL MONITOR, ITERACION DE PALABRAS
            for (int i = 1; i <= PC.palabrasConfirmadas.Count; i++)
            {
                //SE CONCATENAN VALORES
                mensaje += PC.palabrasConfirmadas[i].ToString() + Environment.NewLine;
            }

            textBox1.Text = mensaje;
            textBox1.ReadOnly = true;
        }
        //CIERRA ESTE FORM
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //ESCUCHA SIEMPRE SI HAY ALGUN CAMBIO EN EL ARCHIVO PALABRASCONFIRMADAS Y SE REALMACENA EN UN OBJETO
        //POSTERIORMENTE ACTUALIZA EL TEXTBOX Y ASI SE CONSIGUE MOSTRARLO EN TIEMPO REAL
        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            TextReader sr = new StreamReader(Path.GetTempPath() + "PalabrasConfirmadas.txt");
             PalabrasConfirmadas PC = new PalabrasConfirmadas();
             PC.palabrasConfirmadas = new Hashtable();
             string Keys = "", Values = "";
             Keys = sr.ReadLine();
             Values = sr.ReadLine();
             while (Keys != null)
             {
                 PC.palabrasConfirmadas.Add(int.Parse(Keys), Values);
                 Keys = sr.ReadLine();
                 Values = sr.ReadLine();
             }
             sr.Close();
             string mensaje = "";
             textBox1.Text = "";
             //ITERACIÓN DE PALABRAS
             for (int i=1;i<=PC.palabrasConfirmadas.Count;i++)
             {
                 mensaje +=PC.palabrasConfirmadas[i].ToString() +Environment.NewLine;
             }
             textBox1.Text = mensaje;
            
        }
        //INDICAMOS DONDE ESTA ALMACENADO EL ARCHIVO QUE ESCUCHARA EL FILESYSTEMWATCHER
        private void Form2_Load(object sender, EventArgs e)
        {
            fileSystemWatcher1.Path = Path.GetTempPath() ;
        }
    }
}
