using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyMediaStore.Data;
using MyMediaStore.Models;
using System.Windows.Media.Imaging;

namespace MyMediaStore
{
    /// <summary>
    /// Interaction logic for frmMovieData.xaml
    /// </summary>
    public partial class frmMovieData : Window
    {
        public Movie MovieInstance { get; set; }

        public frmMovieData()
        {
            InitializeComponent();
        }
        private void Initilize()
        {
            if (MovieInstance != null)
            {
                txtTitle.Text = MovieInstance.Title;
                txtSinopse.Text = (MovieInstance.Sinopse == "" ? "Sem sinopse." : MovieInstance.Sinopse);
                txtTipo.Text = (MovieInstance.Type == "" ? "Sem tipo." : MovieInstance.Type);
                txtStatus.Text = (MovieInstance.Status == "" ? "Sem status." : MovieInstance.Status);
                btnPlay.CommandParameter = MovieInstance.Id;
                imgCapa.Source = new BitmapImage(new Uri(MovieInstance.Capa));
                List<Category> listCategories = MovieRepository.GetMovieCategories(MovieInstance.Id);
                int countCategories = 1;
                if (listCategories.Count > 0)
                {
                    txtCategorias.Text = "";
                    foreach (var category in listCategories)
                    {
                        txtCategorias.Text += category.Description;

                        if (listCategories.Count > 1 && countCategories < listCategories.Count)
                            txtCategorias.Text += ",";
                        countCategories++;
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initilize();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string id = btn.CommandParameter.ToString();
            Movie movie = MovieRepository.GetMovie(id);
            try
            {
                System.Diagnostics.Process.Start(movie.Uri);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível executar este filme." + Environment.NewLine + "Verifique se o cominho do filme está correto.");
                ErrorHandling.GenerateLog(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            //Pegar id do filme
            frmMovie frmMovie = new frmMovie();
            frmMovie.MovieInstance = this.MovieInstance;
            frmMovie.ParentId = ParentID.MovieData;
            frmMovie.ShowDialog();

            //Retorna valores da instância
            this.MovieInstance = frmMovie.MovieInstance;

            Initilize();
        }
    }
}
