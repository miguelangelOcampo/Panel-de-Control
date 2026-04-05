using Panel_de_Control.Data;
using Panel_de_Control.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Panel_de_Control.Views
{
    public partial class AgregarPeticion : Window
    {
        private int _equipoId;


        public AgregarPeticion(int equipoId)
        {
            InitializeComponent();
            _equipoId = equipoId;
        }

        private void Button_Cancelar(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Guardar(object sender, RoutedEventArgs e)
        {
        }
    }
}