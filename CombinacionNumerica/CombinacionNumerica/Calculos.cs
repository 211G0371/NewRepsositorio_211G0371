using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CombinacionNumerica
{
    internal class Calculos : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int[] Valores = new int[4];

        private int oportunidades;

        public int Oportunidades
        {
            get { return oportunidades; }
            set { oportunidades = value; }
        }

        private int aciertos;


        public int Aciertos
        {
            get { return aciertos; }
            set { aciertos = value; }
        }

        public bool JuegoIniciado { get; set; } = false;

        public string Resultados { get; set; }

        public void Iniciar()
        {
            Random r = new Random();
            for (int i = 0; i < 4; i++)
            {
                Valores[i] = r.Next(0, 10);
            }
            oportunidades = 10;
            aciertos = 0;
            JuegoIniciado = true;
            Resultados = "";

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public void Verificar(int[] numeros)
        {
            for (int i = 0; i < Valores.Length; i++)
            {
                if (numeros[i] == Valores[i])
                {
                    aciertos++;
                }
            }
            if (aciertos != 4)
            {
                oportunidades--;
            }
            if (aciertos == 4)
            {
                Resultados = "felicidades crack";
                JuegoIniciado = false;
            }
            if (aciertos == 0)
            {
                JuegoIniciado = false;
                Resultados = "pedists xd";
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public ICommand IniciarComando { get; set; }
        public ICommand VerificarComando { get; set; }





        public Calculos()
        {
            IniciarComando = new RelayCommand(Iniciar);
            VerificarComando = new RelayCommand<int[]>(Verificar);
        }

    }
}
