using Panel_de_Control.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Panel_de_Control.Views
{
    /// <summary>
    /// Lógica de interacción para AgregarEquipo.xaml
    /// </summary>
    public partial class AgregarEquipo : Window
    {
        public AgregarEquipo()
        {
            InitializeComponent();
        }

        private void Button_Cancelar(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
        private void Button_Guardar(object sender, RoutedEventArgs e)//Convertimos los textbox y combobox a un objeto de equipo para que se puedan usar metodos de obtencion y guardado (set y get)
        {
            var equipo = new Equipo//Creamos un objeto pasandole los atributos del equipo como los datos de los textbox y combobox
            {
                CodigoActivo = txtCodigoActivo.Text,
                Nombre = txtNombre.Text,
                NumeroSerie = txtSerie.Text,
                Modelo = txtModelo.Text,
                Marca = txtMarca.Text,
                PeriodicidadMantenimiento = txtPeriodicidad.Text,
                UltimoMantenimiento = dpUltimoMantenimiento.SelectedDate,
                Calibracion = txtCalibracion.Text,
                UltimaCalibracion = dpUltimaCalibracion.SelectedDate,
                Ubicacion = txtUbicacion.Text,
                Responsable = txtResponsable.Text,
                FechaAdquisicion = dpFechaAdquisicion.SelectedDate,

                //ComboBox
                Estado = (cbEstado.SelectedItem as ComboBoxItem)?.Content.ToString()
            };

            var dao = new EquipoDAO(); //Creamos un objeto de la clase EquipoDAO para usar el metodo de insercion
            dao.InsertarEquipo(equipo);//usamos el metodo de insercion para guardar el equipo en la base de datos

            DialogResult = true;//Indicamos que la operacion fue exitosa para que el MainWindow pueda actualizar la lista de equipos
            this.Close();
        }

    }
}
