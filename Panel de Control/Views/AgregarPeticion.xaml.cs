using Panel_de_Control.Data;
using Panel_de_Control.Models;
using System;
using System.Collections.Generic;
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
            CargarUsuarios();

        }

        private void Button_Cancelar(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //GUARDA LA NUEVA PETICION EN LA BASE DE DATOS
        private void Button_Guardar(object sender, RoutedEventArgs e)
        {
            if (cbTecnico.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un técnico.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var peticion = new PeticionEquipo
            {
                EquipoId = _equipoId, //asignamos el equipo correcto acorde al constructor
                Tipo = (cbTipo.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Descripcion = txtDescripcion.Text,
                UsuarioId = (int)cbTecnico.SelectedValue,
                Fecha = DateTime.Now,
                Estado = "pendiente"
            };

            var dao = new HistorialDAO();
            dao.InsertarPeticion(peticion); // Aquí ya se guarda correctamente la petición con el equipo
            this.DialogResult = true;       // Devuelve true a la ventana DetalleEquipo
            this.Close();
        }

        //METODO PARA CARGAR LOS USUARIOS EN EL COMBOBOX DE TECNICOS
        private void CargarUsuarios()
        {
            var dao = new UsuarioDAO();
            List<Usuario> usuarios = dao.ObtenerTodos(); // Trae todos los usuarios

            cbTecnico.ItemsSource = usuarios;

            // Mostrar el nombre en el ComboBox
            cbTecnico.DisplayMemberPath = "Nombre";

            // El valor seleccionado será el Id del usuario
            cbTecnico.SelectedValuePath = "Id";
        }
    }
}
