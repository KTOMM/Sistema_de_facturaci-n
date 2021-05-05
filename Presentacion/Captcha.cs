using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
namespace Presentacion
{
    public partial class Captcha : Form
    {
        
        
        string palabra_inicial="",palabra="",palabraConfirmada="",palabraIngresadaConfirmada="",NombreIMG="";
        //DECLARACION DE LOS 3 INTENTOS DEL SISTEMA 
        int intentos=3,aleatorio=0,FragmentosMostrados=0;
        System.Collections.Hashtable palabras_confirmadas = new System.Collections.Hashtable();
        //COLAS PARA FRAGMENTOS, DONDE SE DEVUELVE UN BITMAP
        Queue<Bitmap> Fragmentos2 = new Queue<Bitmap>();
        ArrayList FragmentosConfirmados = new ArrayList();
        Queue<String> direcciones = new Queue<String>();
        Queue<Queue<Bitmap>> Listado = new Queue<Queue<Bitmap>>();
        //COLA QUE RECIBE TODOS LOS FRAGMENTOS
        public Captcha(Queue<Queue<Bitmap>> Fragmentos, Queue<string> NombreIMGs)
        {

            InitializeComponent();

            //CARGA LOS DATOS
            CargarDatos();


            //GENERA IMAGENES DEL ARRAY DE PALABRAS CONFIRMADAS
            GenerarImagenes();
            //GLOBAL PARA USARLO EN EVENTOS
            Listado = Fragmentos;
            Fragmentos2 = Listado.Dequeue() ;
            //SE ESTABLECE EL PRIMER FRAGMENTO DE LA COLA EN EL PB_NOCONFIRMADO
            pb_noconfirmado.Image = Fragmentos2.Dequeue();
            aleatorio = numerosAleatorios();
            pb_confirmado.Image = (Image)FragmentosConfirmados[aleatorio];
            FragmentosMostrados++;
            this.txt_noconfirmado.Focus();
            //ESCRIBE EL NOMBRE DE LA IMAGEN COMO NOMBRE DE LA FACTURA
            direcciones = NombreIMGs;
            NombreIMG = direcciones.Dequeue();
            EscribirNombreDeFactura(NombreIMG);


            //SE DA TAMAÑO A LA BARRA DE PROGRESO
            //progressBar1.Maximum = (Fragmentos.Count)*3;
            //progressBar1.Value = 0;

        }

        //CARGA LOS DATOS DEL ARCHIVO PALABRAS CONFIRMADAS PARA AGREGARLO AL HASHTABLE
        private void CargarDatos()
        {
           
            TextReader sr = new StreamReader(Path.GetTempPath() + "PalabrasConfirmadas.txt");
            string Keys = "",Values="";
            Keys = sr.ReadLine();
            Values = sr.ReadLine();
            while (Keys!=null)
            {
                palabras_confirmadas.Add(int.Parse(Keys), Values);
                Keys = sr.ReadLine();
                Values = sr.ReadLine();
            }
            sr.Close();
        }

        //OBTENER UNA IMAGEN ALEATORIA SEGUN LA POSICION EN EL ARRAYLIST
        private int numerosAleatorios()
        {
            var guid = Guid.NewGuid();
            var justNumbers = new String(guid.ToString().Where(Char.IsDigit).ToArray());
            var seed = int.Parse(justNumbers.Substring(0, 6));

            var random = new Random(seed);
            var value = random.Next(1,cantidadPalabrasConfirmadas());
            return value;
        }
        //SABER CUANTAS PALABRAS HAY EN EL HASHTABLE
        private int cantidadPalabrasConfirmadas()
        {
            return palabras_confirmadas.Count;
        }
        //GENERA IMAGENES APARTIR DE LAS PALABRAS CONFIRMADAS
        private void GenerarImagenes()
        {
            FragmentosConfirmados.Clear();
            //ITERACION DE CADA ELEMENTO DEL HASHTABLE
            for (int i=1;i<= palabras_confirmadas.Count;i++)
            {
                //CREAMOS EL OBJETO IMAGEN
                Bitmap objBmp = new Bitmap(1, 1);
                int Width = 0;
                int Height = 0;
                //LE DAMOS EL FORMATO DE LA FUENTE
                Font objFont = new Font("Arial", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

                Graphics objGraphics = Graphics.FromImage(objBmp);

                Width = (int)objGraphics.MeasureString(palabras_confirmadas[i].ToString(), objFont).Width;
                Height = (int)objGraphics.MeasureString(palabras_confirmadas[i].ToString(), objFont).Height;

                objBmp = new Bitmap(objBmp, new Size(Width, Height));

                objGraphics = Graphics.FromImage(objBmp);

                objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                objGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                objGraphics.DrawString(palabras_confirmadas[i].ToString(), objFont, new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0);
                objGraphics.Flush();
                FragmentosConfirmados.Add(objBmp);
            }
        }

        //GENERA LOS DATOS DE LAS PALABRAS CONFIRMADAS SI EL ARCHIVO NO EXISTE
        private bool validacion()
        {
            if (txt_confirmado.Text.Equals("") || txt_noconfirmado.Text.Equals(""))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //BOTO COMPROBAR
        private void btn_comprobar_Click(object sender, EventArgs e)
        {
            this.lblConfirmada.Visible = false;
            if (validacion())
            {
                //COMPROBACION
                DialogResult dialogResult;
                string confirmadaPalabra = "";
                confirmadaPalabra = palabras_confirmadas[aleatorio + 1].ToString();
                dialogResult = MessageBox.Show("Es esta la palabra que desea confirmar?..." + txt_noconfirmado.Text + " Y " + txt_confirmado.Text, "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dialogResult == DialogResult.Yes)
                {
                    if (intentos == 3)//PRIMER INTENTO
                    {
                        palabra_inicial = "";
                        palabra_inicial = txt_noconfirmado.Text;
                        txt_noconfirmado.Text = "";

                        if (confirmadaPalabra.Equals(txt_confirmado.Text))
                        {
                            txt_confirmado.Text = "";
                            intentos--;
                            this.txt_noconfirmado.Focus();
                            aleatorio = numerosAleatorios();
                            this.pb_confirmado.Image = (Image)FragmentosConfirmados[aleatorio];
                            this.Invoke(new Action(() =>
                            {
                                pb_confirmado.Refresh();
                            }));
                            lblErrorMessagge.Visible = false;
                        }
                        else
                        {
                            aleatorio = numerosAleatorios();
                            this.pb_confirmado.Image = (Image)FragmentosConfirmados[aleatorio];
                            this.Invoke(new Action(() =>
                            {
                                pb_confirmado.Refresh();
                            }));
                            txt_confirmado.Text = "";
                            msgError("Error... las palabras no coinciden, Inicie nuevamente");
                            this.txt_noconfirmado.Focus();


                            intentos = 3;
                        }


                    }
                    else


                    //CUANDO YA NO SEA LA PALABRA CONFIRMADA
                    if (intentos >= 0 && intentos < 3)//SI LA PALABRA SE INGRESA, SE REALIZA EL SEGUNDO Y TERCER INTENTO
                    {
                        palabra = "";
                        palabra = txt_noconfirmado.Text;
                        txt_noconfirmado.Text = "";

                        this.txt_noconfirmado.Focus();
                        aleatorio = numerosAleatorios();
                        this.pb_confirmado.Image = (Image)FragmentosConfirmados[aleatorio];
                        this.Invoke(new Action(() =>
                        {
                            pb_confirmado.Refresh();
                        }));
                        if (palabra_inicial.Equals(palabra) && confirmadaPalabra.Equals(txt_confirmado.Text))
                        {
                            lblErrorMessagge.Visible = false;
                            txt_confirmado.Text = "";
                            intentos--;
                            this.txt_noconfirmado.Focus();
                        }
                        else
                        {
                            txt_confirmado.Text = "";
                            msgError("Error... las palabras no coinciden, Inicie nuevamente ");
                            this.txt_noconfirmado.Focus();
                            intentos = 3;
                        }

                    }
                }//SI NO DESEA CONFIRMAR ENTONCES
                else
                {
                    aleatorio = numerosAleatorios();
                    this.pb_confirmado.Image = (Image)FragmentosConfirmados[aleatorio];
                    this.Invoke(new Action(() =>
                    {
                        pb_confirmado.Refresh();
                    }));
                    txt_noconfirmado.Text = "";
                    txt_confirmado.Text = "";
                    this.txt_noconfirmado.Focus();
                }

                if (intentos == 0)//TERMINA DE CONFRIRMAR
                {
                    lblErrorMessagge.Visible = false;
                    this.txt_noconfirmado.Focus();
                    msgconfirmada("Palabra confirmada");
                    int KeyNueva = 0;
                    KeyNueva = Keys();
                    palabras_confirmadas.Add(KeyNueva, palabra_inicial);
                    Actualizar(KeyNueva, palabra_inicial);
                    EscribirFactura(FragmentosMostrados, palabra_inicial);
                    FragmentosMostrados++;
                    GenerarImagenes();
                    if (Fragmentos2.Count != 0)
                    {
                        this.pb_noconfirmado.Image = Fragmentos2.Dequeue();
                        aleatorio = numerosAleatorios();
                        this.pb_confirmado.Image = (Image)FragmentosConfirmados[aleatorio];
                        this.Invoke(new Action(() =>
                        {
                            pb_noconfirmado.Refresh();
                            pb_confirmado.Refresh();
                        }));
                        this.txt_noconfirmado.Focus();
                        //SE REGRESAN LOS INTENTOS POR CADA PALABRA QUE SE QUIERA CONFIRMAR
                        intentos = 3;
                    }
                    else
                    {
                        if (Fragmentos2.Count == 0 && Listado.Count != 0)
                        {
                            EscribirFacturaSeparador();
                            NombreIMG = "";
                            NombreIMG = direcciones.Dequeue();
                            EscribirNombreDeFactura(NombreIMG);
                            Fragmentos2.Clear();
                            Fragmentos2 = Listado.Dequeue();
                            aleatorio = numerosAleatorios();
                            this.pb_noconfirmado.Image = Fragmentos2.Dequeue();
                            this.pb_confirmado.Image = (Image)FragmentosConfirmados[aleatorio];
                            FragmentosMostrados=0;
                            FragmentosMostrados++;
                            this.Invoke(new Action(() => { pb_noconfirmado.Refresh(); pb_confirmado.Refresh(); }));
                            intentos = 3;
                        }
                        else
                        {
                            EscribirFacturaSeparador();
                            //CERRAR FORMULARIO ACTUAL
                            this.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Coloque las palabras porfavor");
                this.txt_noconfirmado.Focus();
            }
            
           
            
        }
        //ESCRIBE LA FACTURA SEGUN LAS PALABRAS QUE SE VAN CONFIRMANDO
        private void EscribirFactura(int fragmentosMostrados, string palabra_inicial)
        {
            TextWriter sw = new StreamWriter(Path.GetTempPath() + "Facturas.txt",true);
            if (fragmentosMostrados == 1)
            {
                sw.Write("NIT: "+palabra_inicial + '\t' + '\t' + '\t');
            }
            else if(fragmentosMostrados == 2)
            {
                sw.WriteLine("Fecha: " + palabra_inicial);
            }
            else if (fragmentosMostrados%2==0)
            {
                sw.WriteLine("Cantidad: " + palabra_inicial);
            }
            else
            {
                sw.Write("Producto: " + palabra_inicial+'\t'+'\t'+'\t');
            }
            sw.Close();
        }


        //MINIMIZA EL FORM
        private void btnminimizar_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        //ESCRIBE UN SEPARADOR SENCILLO PARA MEJORAR LA LECTURA DE CADA FACTURA EN EL MONITOR
        private void EscribirFacturaSeparador()
        {
            TextWriter sw = new StreamWriter(Path.GetTempPath() + "Facturas.txt", true);
            
            
                sw.WriteLine("=============================================");
           
            sw.Close();
        }
        //QUITA LA EXTENSION DE LA IMAGEN Y ESCRIBE SOLO EL NOMBRE COMO NOMBRE DE FACTURA
        private void EscribirNombreDeFactura(string Nombre)
        {
            char[] delimiterChars = { '.', '\\' };
            string[] nombre = Nombre.Split(delimiterChars);
            int cantidad = 0;
            cantidad = nombre.Length;
            TextWriter sw = new StreamWriter(Path.GetTempPath() + "Facturas.txt", true);


            sw.WriteLine("Factura: "+nombre[cantidad-2]);

            sw.Close();
        }
        //GENERADOR DE LLAVES PARA LAS PALABRAS CONFIRMADAS
        private int Keys()
        {
            return palabras_confirmadas.Count + 1;
            
        }
        //ACTUALIZA EL ARCHIVO DE PALABRAS CONFIRMADAS
        private void Actualizar(int keyConfirmada,string valueConfirmado)
        {
            TextWriter sw = new StreamWriter(Path.GetTempPath() + "PalabrasConfirmadas.txt", true);
            sw.WriteLine(keyConfirmada);
            sw.WriteLine(valueConfirmado);
            sw.Close();
        }
        //MUESTRA UN ERROR 
        private void msgError(string msg)
        {
            lblErrorMessagge.Text = "      " + msg;
            lblErrorMessagge.Visible = true;
            
        }
        private void msgconfirmada(string msg)
        {
            lblConfirmada.Text = "      " + msg;
            lblConfirmada.Visible = true;

        }
    }
}
