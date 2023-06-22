using GalaSoft.MvvmLight.Command;
using librosDatos.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace librosDatos.ViewsModels
{
    public class LibrosViewsModels : INotifyPropertyChanged
    {
        public string? Error { get; set; }
        //public Libro Libro { get; set; }
        private Libro clibro;

        public Libro Libro
        {
            get { return clibro; }
            set
            {
                clibro = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        public ObservableCollection<Libro> Libros { get; set; } = new ObservableCollection<Libro>();
        public ICommand AgregarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public string Vista { get; set; } = "ver";
        public int Indice { get; set; }

        public LibrosViewsModels()
        {
            Cargar();

            AgregarCommand = new RelayCommand(Agregar);

            EditarCommand = new RelayCommand(Editar);

            EliminarCommand = new RelayCommand(Eliminar);

            CancelarCommand = new RelayCommand(Cancelar);

            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Agregar()
        {
            Error = "";
            if (string.IsNullOrEmpty(Libro.TituloOriginal))
            {
                Error = "Escriba el titulo original del libro";
            }
            if (string.IsNullOrEmpty(Libro.Genero))
            {
                Error += "Escriba el genero del libro";
            }
            if (string.IsNullOrEmpty(Libro.RutaImagen))
            {
                Error += "Escriba la ruta de la imagen";
            }
            if (Libros != null)
            {
                Libros.Add(Libro);
                Guardar();
                CambiarVista("ver");
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public void Editar()
        {
            if (Libro != null && Libros != null)
            {
                Libros[Indice] = Libro;
                CambiarVista("ver");
                Guardar();
            }
        }
        public void Eliminar()
        {
            if (Libro != null)
            {
                Libros.Remove(Libro);
                CambiarVista("ver");
                Guardar();
            }
        }
        public void Cancelar()
        {
            Vista = "ver";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));
        }
        public void Cargar()
        {
            if (File.Exists("Siuu.json"))
            {
                var json = File.ReadAllText("Siuu.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<Libro>>(json);
                if (datos != null)
                {
                    Libros = (ObservableCollection<Libro>)datos;
                }
                else
                {
                    Libros = new ObservableCollection<Libro>();
                }
            }
        }
        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(Libros);

            File.WriteAllText("Siuu.json", json);
        }
        public void CambiarVista(string VistaACambiar)
        {
            Vista = VistaACambiar;
            if (VistaACambiar == "agregar")
            {
                Libro = new Libro();
            }

            if (VistaACambiar == "editar")
            {
                if (Libro != null)
                {
                    int indice = Libros.IndexOf(Libro);

                    Libro copiaLibro = new Libro()
                    {
                        Titulo = Libro.Titulo,
                        Autor = Libro.Autor,
                        TituloOriginal = Libro.TituloOriginal,
                        Genero = Libro.Genero,
                        Reseña = Libro.Reseña,
                        RutaImagen = Libro.RutaImagen
                    };

                    Libro = copiaLibro;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("vista"));
        }
        
    }
}

