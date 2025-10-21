using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;

namespace AROMADICOFFE.Pages;

public partial class Miperfil : ContentPage
{
    int rating = 0;
    int puntuacion = 0;

    List<ImageButton> estrellas;

    public Miperfil()
	{
		InitializeComponent();

        estrellas = new List<ImageButton> { Star1, Star2, Star3, Star4, Star5 };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await AnimateElement(ProfileImage, -50);
    }

    private async Task AnimateElement(VisualElement element, double translationY)
    {
        element.Opacity = 0;
        element.TranslationY = translationY;
        await Task.WhenAll(
            element.FadeTo(1, 400, Easing.CubicOut),
            element.TranslateTo(0, 0, 400, Easing.CubicOut)
        );
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            await DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
            return;
        }

        await AnimateButton(SaveButton);


        ResetForm();
        await DisplayAlert("Éxito", "Datos guardados y formulario reiniciado", "OK");
    }

    private void ResetForm()
    {
        UsernameEntry.Text = "";
        EmailEntry.Text = "";
        PasswordEntry.Text = "";
        CommentEditor.Text = "";
        ConfirmPasswordEntry.Text = "";
        rating = 0;
        UpdateStars();
        ProfileImage.Source = "placeholder.png";
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await AnimateButton(CancelButton);
        ResetForm();
    }
    private async Task AnimateButton(Button btn)
    {
        await btn.ScaleTo(0.95, 100);
        await btn.ScaleTo(1, 100);
    }

    private async void ConfirmButton_Clicked(object sender, EventArgs e)
    {
        string comentario = CommentEditor.Text?.Trim();

        if (string.IsNullOrEmpty(comentario))
        {
            await DisplayAlert("Aviso", "Escribe un comentario antes de confirmar", "OK");
            return;
        }

        await DisplayAlert(
          "Gracias",
          $"Has calificado con {puntuacion} estrellas.\n {CommentEditor.Text}",
          "OK");

    }

    private void Star1_Clicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        puntuacion = int.Parse(button.StyleId);

        for (int i = 0; i < estrellas.Count; i++)
        {
            if (i < puntuacion)
                estrellas[i].Source = "fressallena.png";
            else
                estrellas[i].Source = "vacia.png";
        }
    }
    private void UpdateStars()
    {
        Star1.Source = rating >= 1 ? "fressallena.png" : "vacia.png";
        Star2.Source = rating >= 2 ? "fressallena.png" : "vacia.png";
        Star3.Source = rating >= 3 ? "fressallena.png" : "vacia.png";
        Star4.Source = rating >= 4 ? "fressallena.png" : "vacia.png";
        Star5.Source = rating >= 5 ? "fressallena.png" : "vacia.png";
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Selecciona una opción", "Cancelar", null, "Galería", "Cámara");
        FileResult photo = null;

        try
        {
            if (action == "Galería")
                photo = await MediaPicker.PickPhotoAsync();

            else if (action == "Cámara")
                photo = await MediaPicker.CapturePhotoAsync();

            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                ProfileImage.Source = ImageSource.FromStream(() => stream);

                // Animación de toque
                await ProfileImage.ScaleTo(1.1, 100);
                await ProfileImage.ScaleTo(1, 100);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo seleccionar la foto: " + ex.Message, "OK");
        }
    }

}