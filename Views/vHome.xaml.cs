namespace djaramilloT1.Views;

public partial class vHome : ContentPage
{
	public vHome()
	{
		InitializeComponent();
		pkTipoId.SelectedIndex = 0;
	}

	// Solo permite numeros
	private void SoloNumeros_TextChanged(object? sender, TextChangedEventArgs e)
	{
		Entry entry = (Entry)sender!;
		string texto = new string((e.NewTextValue ?? "").Where(char.IsDigit).ToArray());
		if (entry.Text != texto)
			entry.Text = texto;
	}

	// Solo permite letras y espacios
	private void SoloLetras_TextChanged(object? sender, TextChangedEventArgs e)
	{
		Entry entry = (Entry)sender!;
		string texto = new string((e.NewTextValue ?? "").Where(c => char.IsLetter(c) || c == ' ').ToArray());
		if (entry.Text != texto)
			entry.Text = texto;
	}

	private async void btnGuardar_Clicked(object? sender, EventArgs e)
	{
		// Campos vacios
		if (string.IsNullOrWhiteSpace(txtNumeroId.Text) ||
			string.IsNullOrWhiteSpace(txtApellidoPaterno.Text) ||
			string.IsNullOrWhiteSpace(txtApellidoMaterno.Text) ||
			string.IsNullOrWhiteSpace(txtNombres.Text) ||
			string.IsNullOrWhiteSpace(txtTelefono.Text) ||
			string.IsNullOrWhiteSpace(txtCorreo.Text) ||
			string.IsNullOrWhiteSpace(txtConfirmarCorreo.Text))
		{
			await DisplayAlertAsync("Error", "Complete todos los campos", "OK");
			return;
		}

		// Largo cedula - RUC
		if (pkTipoId.SelectedItem.ToString() == "CÉDULA" && txtNumeroId.Text.Length != 10)
		{
			await DisplayAlertAsync("Error", "La cédula debe tener 10 dígitos", "OK");
			return;
		}

		if (pkTipoId.SelectedItem.ToString() == "RUC" && txtNumeroId.Text.Length != 13)
		{
			await DisplayAlertAsync("Error", "El RUC debe tener 13 dígitos", "OK");
			return;
		}

		// Telefono 10 digitos
		if (txtTelefono.Text.Length != 10)
		{
			await DisplayAlertAsync("Error", "El teléfono debe tener 10 dígitos", "OK");
			return;
		}

		// Formato correo
		if (!txtCorreo.Text.Contains('@') || !txtCorreo.Text.Contains('.'))
		{
			await DisplayAlertAsync("Error", "El correo no es válido", "OK");
			return;
		}

		// Correos iguales
		if (txtCorreo.Text != txtConfirmarCorreo.Text)
		{
			await DisplayAlertAsync("Error", "Los correos no coinciden", "OK");
			return;
		}

		// Guardar en TXT
		string datos =
			"Identificación: " + pkTipoId.SelectedItem + " " + txtNumeroId.Text + "\n" +
			"Apellidos: " + txtApellidoPaterno.Text + " " + txtApellidoMaterno.Text + "\n" +
			"Nombres: " + txtNombres.Text + "\n" +
			"Teléfono: " + txtTelefono.Text + "\n" +
			"Correo: " + txtCorreo.Text + "\n" +
			"Fecha: " + DateTime.Now + "\n" +
			"------------------------------\n";

		try
		{
			string ruta = Path.Combine(FileSystem.AppDataDirectory, "inscripciones.txt");
			File.AppendAllText(ruta, datos);

			await DisplayAlertAsync("Guardado", "Datos guardados en:\n" + ruta, "OK");
			Limpiar();
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("Error", "No se pudo guardar el archivo: " + ex.Message, "OK");
		}
	}

	// Dejar formulario vacio 
	private void Limpiar()
	{
		pkTipoId.SelectedIndex = 0;
		txtNumeroId.Text = "";
		txtApellidoPaterno.Text = "";
		txtApellidoMaterno.Text = "";
		txtNombres.Text = "";
		txtTelefono.Text = "";
		txtCorreo.Text = "";
		txtConfirmarCorreo.Text = "";
	}
}
