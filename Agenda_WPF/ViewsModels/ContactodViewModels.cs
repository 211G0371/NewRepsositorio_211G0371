using Agenda_WPF.Models;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Agenda_WPF.ViewsModels
{
    public class ContactodViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        //Propiedades
        public string? Error { get; set; }
        public ObservableCollection<Contactos>? Contactos { get; set; } = new ObservableCollection<Contactos>();
        public Contactos Contacto { get; set; } = new Contactos();
        public string Vista { get; set; } = "ver";
        public ICommand? AgregarCommand { get; set; }
        public ICommand? EditarCommand { get; set; }
        public ICommand? EliminarCommand { get; set; }
        public ICommand? CambiarVistaCommand { get; set; }
        public ICommand? CancelarCommand { get; set; }
        public int Indice { get; set; }

        //Metodos

        //CREATE
        public void Agregar()
        {
            Error = "";
            if (string.IsNullOrEmpty(Contacto.Nombre))
            {
                Error += "Escriba el nombre del contacto\n";
            }
            if (string.IsNullOrEmpty(Contacto.Telefono))
            {
                Error += "Escriba el telefono del contacto\n";
            }
            if (string.IsNullOrEmpty(Contacto.Direccion))
            {
                Error += "Escriba la direccion del contacto\n";
            }
            if (string.IsNullOrEmpty(Contacto.Email))
            {
                Error += "Escriba el email del contacto\n";
            }
            if (Contacto.FechaNacimiento.Date > DateTime.Now.Date)
            {
                Error += "Fecha erronea\n";
            }
            if (Error == "" && Contactos != null)
            {
                Contactos.Add(Contacto);
                Guardar();
                Vista = "ver";
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        //READ


        //UPDATE
        public void Editar()
        {
            if (Contactos!=null)
            {
                Contactos[Indice] = Contacto;
                Guardar();
                CambiarVista("ver");
            }
           
        }

        //DELETE
        public void Eliminar()
        {
            if (Contacto != null && Contactos != null)
            {
                Contactos.Remove(Contacto);
                CambiarVista("ver");
                Guardar();
            }
        }

        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(Contactos); //Serializamos 
                                                               //la observableCollection
            File.WriteAllText("Datos.json", json);
        }
        public void Cargar()
        {
            if (File.Exists("Datos.json"))
            {
                var json = File.ReadAllText("Datos.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<Contactos>>(json);
                if (datos != null)
                {
                    Contactos = (ObservableCollection<Contactos>)datos;
                }
                else
                {
                    Contactos = new ObservableCollection<Contactos>();
                }
            }
        }
        public void CambiarVista(string vistaACambiar)
        {
            Vista = vistaACambiar;
            if (vistaACambiar == "agregar")
            {
                //instanciar una clase de la clase contactos
                Contacto = new Contactos();
            }
            if (vistaACambiar == "editar")
            {
                if (Contactos != null)
                {
                    int indice = Contactos.IndexOf(Contacto);
                }
                Contactos copiaContacto = new Contactos()
                {
                    Nombre = Contacto.Nombre,
                    Direccion = Contacto.Direccion,
                    Email = Contacto.Email,
                    Telefono = Contacto.Telefono,
                    FechaNacimiento = Contacto.FechaNacimiento

                };
                Contacto = copiaContacto;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("vista"));
        }
        public void Cancelar()
        {
            Vista = "ver";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));
        }
        //Constructor
        public ContactodViewModels()
        {
            Cargar();

            //Propiedades apuntan a los metodos 
            AgregarCommand = new RelayCommand(Agregar);

            EditarCommand = new RelayCommand(Editar);

            EliminarCommand = new RelayCommand(Eliminar);

            CancelarCommand = new RelayCommand(Cancelar);

            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
        }
    }
}
