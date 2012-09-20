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

namespace MyMediaStore
{
    /// <summary>
    /// Interaction logic for frmDashBoard.xaml
    /// </summary>
    public partial class frmDashBoard : Window
    {
        public frmDashBoard()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                lstMedias.DataContext = MovieRepository.LoadAll();
            }
            catch (Exception ex)
            {
                ErrorHandling.GenerateLog(ex.Message);
            }
           
        }

        private void lstMedias_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Movie movie = (Movie)lstMedias.SelectedItem;

            frmMovieData frmMovieData = new frmMovieData();
            frmMovieData.MovieInstance = movie;
            frmMovieData.ShowDialog();
           
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
            Button btn = (Button)sender;
            string id = btn.CommandParameter.ToString();
            frmMovie frmMovie = new frmMovie();
            frmMovie.MovieInstance = MovieRepository.GetMovie(id);
            frmMovie.ShowDialog();
            Initialize();
        }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string id = btn.CommandParameter.ToString();

            frmMovieData newform = new frmMovieData();
            newform.MovieInstance = MovieRepository.GetMovie(id);
            newform.ShowDialog();
            Initialize();
           
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            frmMovie frmMovie = new frmMovie();
            frmMovie.ShowDialog();
            Initialize();
        }
    }
}
