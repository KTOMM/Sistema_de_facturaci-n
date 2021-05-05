using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class Diccionario : Form
    {
        public Diccionario()
        {
            InitializeComponent();
            //SE LE DA TITULO
            label1.Text = "Subir Factura";
        }
        //ABRE UN EXPLORADOR PARA SELECCIONAR UNA IMAGEN SOLO DE TIPO .JPG
        //POSTERIORMENTE SE INICIALIZA EL CAPTCHA Y SE ENVIA EL NOMBRE DE LA IMAGEN ASI COMO UNA COLA DE LOS
        //FRAGMENTOS DE LA IMAGEN A CONFIRMAR
        private void button1_Click(object sender, EventArgs e)
        {
            Queue<string> direcciones = new Queue<String>();
            Queue<Queue<Bitmap>> Listado = new Queue<Queue<Bitmap>>();
            OpenFileDialog getImage = new OpenFileDialog();
            getImage.InitialDirectory = "C:\\";
            getImage.Filter = "Archivos de Imagen (*.jpg)|*jpg;";
            getImage.Title = "Selecciona la Factura";
            getImage.Multiselect = true;
            if(getImage.ShowDialog() == DialogResult.OK)
            {
                foreach (string item in getImage.FileNames)
                {
                    direcciones.Enqueue(item);

                }
                foreach (string item in direcciones)
                {

                    Listado.Enqueue(imagenes(item));
                }

                Captcha form2 = new Captcha(Listado, direcciones);
                form2.Show();
            }
            else
            {
                MessageBox.Show("No se selecciono imagen", "Sin seleccion", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }
        //GENERA UN BITMAP DE IMAGENES 
        public static Bitmap CropImage(Bitmap source, Rectangle section)
        {
            Bitmap bmp = new Bitmap(section.Width, section.Height);
            Graphics g = Graphics.FromImage(bmp);

            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }
        //RECORTA LA IMAGEN EN CIERTA ALTURA Y ANCHO PARA CADA FRAGMENTO Y EL FRAGMENTO CORTADO LO GUARDA EN UNA COLA QUE SE RETORNA
        public Queue<Bitmap> imagenes(string @PATH)
        {
          
            Queue<Bitmap> Lista = new Queue<Bitmap>();
            int posXmin = 0, posYmin = 0, anchura = 200, altura = 100;
            //EL USUARIO SUBA LA IMAGEN, TOMA RUTA Y LA ALMACENA EN EL PATH
            Bitmap source = new Bitmap(@PATH);
            
            int alto = source.Height;
            int altoSteps = alto / 100;
          
            for (int i = 0; i < altoSteps; i++)
            {
                posXmin = 0;
                for (int j = 0; j < (source.Width / 200); j++)
                {

                    Rectangle rectOrig = new Rectangle(posXmin, posYmin, anchura, altura);

                    Bitmap CroppedImage = CropImage(source, rectOrig);
                    Lista.Enqueue(CroppedImage);

                    posXmin = 200;
                }
                posYmin += 100;
            }
            return Lista;
        }
        //CIERRA ESTE FORM
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
