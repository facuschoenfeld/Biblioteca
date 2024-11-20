using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace DBVentana
{
    class Ventana
    {
        public int Ancho;
        public int Alto;

        public Ventana(int ancho, int alto)
        {
            Ancho = ancho;
            Alto = alto;
            Init();
        }

        private void Init()
        {
            Console.SetWindowSize(Ancho, Alto);
            Console.Title = "GESTOR DE BIBLIOTECA";
        }
    }
}
