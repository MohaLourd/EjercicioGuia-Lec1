using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Socket server;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse(IP.Text);
            IPEndPoint ipep = new IPEndPoint(direc, 9003);
            

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");

            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Longitud.Checked)
            {
                string mensaje = "1/" + nombre.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split (',')[0];
                MessageBox.Show("La longitud de tu nombre es: " + mensaje);
            }


            if (Bonito.Checked ) 
            {
                string mensaje = "4/" + nombre.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split(',')[0];
              

                if (mensaje=="SI")
                    MessageBox.Show("Tu nombre ES bonito.");
                else
                    MessageBox.Show("Tu nombre NO bonito. Lo siento.");

            }

            if (Mayusculas.Checked)
            {
                string mensaje = "2/" + nombre.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Trim();

                MessageBox.Show("Nombre en mayúsculas: " + mensaje);
            }

            if (Palindromo.Checked)
            {
                string mensaje = "3/" + nombre.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Trim();

                MessageBox.Show("Nombre es palindromo: " + mensaje);
            }

            // Se terminó el servicio. 
            // Nos desconectamos
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(Celsius.Checked)
            {
                string mensaje = "5/" + numero.Text; // Enviar código 5 para conversión a Celsius
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                float fahrenheit = float.Parse(numero.Text);
                byte[] fBytes = BitConverter.GetBytes(fahrenheit);
                server.Send(fBytes);

                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string respuesta = Encoding.ASCII.GetString(msg2).Trim();

                MessageBox.Show("Respuesta del servidor: " + respuesta);
            }
            if (Fahrenheit.Checked)
            {
                string mensaje = "6/" + numero.Text; // Enviar código 6 para conversión a Fahrenheit
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                float celsius = float.Parse(numero.Text);
                byte[] cBytes = BitConverter.GetBytes(celsius);
                server.Send(cBytes);

                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string respuesta = Encoding.ASCII.GetString(msg2).Trim();

                MessageBox.Show("Respuesta del servidor: " + respuesta);
            }

            // Se terminó el servicio. 
            // Nos desconectamos
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        private void NumOpera_Click(object sender, EventArgs e)
        {
            string mensaje = "7/" + numero.Text; // Enviar código 6 para conversión a Fahrenheit
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

           

            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            string respuesta = Encoding.ASCII.GetString(msg2).Trim();

            labelCont.Text= respuesta;
        }
    }
}
