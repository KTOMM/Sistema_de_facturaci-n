using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;
namespace Presentacion
{
    public partial class Form1 : Form
    {
        //ARRAYLIST DONDE SE ALMACENAN LOS DATOS DE LOS USUARIOS
        ArrayList Linea = new ArrayList();
        public Form1(ArrayList lineas)
        {
            InitializeComponent();
            Linea = lineas;
            //SE MUESTRAN LOS DATOS DEL USUARIO EN UN LISTBOX
            //ITERACION DE CADA ELEMENTO DEL ARRAYLIST
            foreach (string item in Linea)
            {
                if (item != null)
                {
                    listBox1.Items.Add(item);
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        //CIERRA ESTE FORM
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        //INDICAMOS DONDE ESTA ALMACENADO EL ARCHIVO QUE ESCUCHARA EL FILESYSTEMWATCHER
        private void Form1_Load(object sender, EventArgs e)
        {
            fileSystemWatcher1.Path = Path.GetTempPath() ;

            listBox1.Items.Clear();
            ArrayList a1 = new ArrayList();
            string l;
            TextReader sr = new StreamReader(Path.GetTempPath() + "Users.txt");
            l = sr.ReadLine();
            a1.Add(l);
            while (l != null)
            {
                l = sr.ReadLine();
                a1.Add(l);
                
            }
            sr.Close();
            Linea = a1;
            foreach (string item in a1)
            {
                if (item != null)
                {
                    listBox1.Items.Add(item);
                }
            }
        }
        //ESCUCHA SIEMPRE SI HAY ALGUN CAMBIO EN EL ARCHIVO USERS Y SE REALMACENA EN UN ARRAYLIST 
        //POSTERIORMENTE ACTUALIZA EL lISTBOX Y ASI SE CONSIGUE MOSTRARLO EN TIEMPO REAL
        private void fileSystemWatcher1_Changed_1(object sender, FileSystemEventArgs e)
        {
            listBox1.Items.Clear();
            ArrayList a1 = new ArrayList();
            string l;
            TextReader sr = new StreamReader(Path.GetTempPath() + "Users.txt");
            l = sr.ReadLine();
            a1.Add(l);
            while (l != null)
            {
                l = sr.ReadLine();
                a1.Add(l);
                l = sr.ReadLine();
                a1.Add(l);
                l = sr.ReadLine();
                a1.Add(l);

                l = sr.ReadLine();
                a1.Add(l);
            }
            sr.Close();
            Linea = a1;
            foreach (string item in a1)
            {
                if (item != null)
                {
                    listBox1.Items.Add(item);
                }
            }
        }
        //CIERRA ESTE FORM
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        
        //EVENTO DE DOBLE CLICK EN EL LIST BOX COMPRUEBA QUE SEA UN ESTADO Y SI ES ASI
        //LO MODIFICA AL CONTRARIO EN EL ARCHIVO
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            //MessageBox.Show(listBox1.SelectedItem.ToString() + "|" + listBox1.SelectedIndex.ToString());
            if (listBox1.SelectedItem.ToString().Equals("Activo") && listBox1.SelectedIndex != 3 || listBox1.SelectedItem.ToString().Equals("Inactivo"))
            {
                /*agregar condicion si es otro que no sea activo o inactivo decir que no se puede o investigar como desabilitar la seleccion
                 si esta activo que pregunte si esta seguro de desactivarlo si esta desactivado pregunte si esta seguro de activarlo*/
                if (listBox1.SelectedItem.ToString().Equals("Inactivo"))
                {
                    Linea[listBox1.SelectedIndex] = "Activo";

                    TextWriter sr = new StreamWriter(Path.GetTempPath() + "Users.txt");
                    foreach (string item in Linea)
                    {
                        if (item != null)
                        {
                            sr.WriteLine(item);
                        }
                    }
                    sr.Close();
                }
                else
                {
                    Linea[listBox1.SelectedIndex] = "Inactivo";

                    TextWriter sr = new StreamWriter(Path.GetTempPath() + "Users.txt");
                    foreach (string item in Linea)
                    {
                        if (item != null)
                        {
                            sr.WriteLine(item);
                        }
                    }
                    sr.Close();
                }
                MessageBox.Show("Actualizado");
            }
            else
            {
                MessageBox.Show("No Puede Cambiar Este Campo");
            }
        }
    }
}
