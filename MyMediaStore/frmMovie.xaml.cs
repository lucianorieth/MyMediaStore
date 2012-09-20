using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Win32;
using MyMediaStore.Models;
using MyMediaStore.Data;

namespace MyMediaStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmMovie : Window
    {
        public Movie MovieInstance { get; set; }
        public ParentID ParentId { get; set; }

        public frmMovie()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();

            if (MovieInstance != null)
            {
                txtTitulo.Text = MovieInstance.Title;
                txtSinopse.Text = MovieInstance.Sinopse;
                txtCapa.Text = MovieInstance.Capa;
                txtUri.Text = MovieInstance.Uri;
                ckAssistido.IsChecked = (MovieInstance.Status == "Assistido" ? true : false);
                cmbType.SelectedValue = MovieTypeRepository.GetMovieTypeId(MovieInstance.Type);
                cmbFormats.SelectedValue = FormatRepository.GetFormatId(MovieInstance.Format);

                List<Category> listCategories = MovieRepository.GetMovieCategories(MovieInstance.Id);
                List<Category> listAllCategories = CategoryRepository.LoadAll();
                Category nova = new Category();
                nova.Description = "Aventura";
                nova.Id = "3";
                listAllCategories.Remove(nova);
                if (listCategories.Count > 0)
                {
                    foreach (var category in listCategories)
                    {
                        lsbCategoriesAdd.Items.Add(category);
                        listAllCategories.RemoveAll(x => x.Id == category.Id);
                    }

                    lsbCategories.Items.Clear();
                    foreach (Category item in listAllCategories)
                    {
                        lsbCategories.Items.Add(item);
                    }
                }
            }
        }

        private void Initialize()
        {
            BindCategories();
            BindTypes();
            BindFormats();
            txtCapa.Text = string.Empty;
            txtSinopse.Text = string.Empty;
            txtUri.Text = string.Empty;
            txtTitulo.Text = string.Empty;
            cmbType.SelectedIndex = 0;
            cmbFormats.SelectedIndex = 0;
        }

        private bool ValidateForm()
        {
            bool validate = true;
            if (string.IsNullOrEmpty(txtTitulo.Text))
            {
                MessageBox.Show("Favor informar um nome para o vídeo");
                validate = false;
            }
            return validate;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    Movie movie = new Movie()
                                      {
                                          Title = txtTitulo.Text,
                                          Sinopse = txtSinopse.Text,
                                          Capa = txtCapa.Text,
                                          Type = ((MovieType) cmbType.SelectedItem).Description,
                                          Uri = txtUri.Text,
                                          Status = (ckAssistido.IsChecked == true ? "Assistido" : "Não assistido"),
                                          Format = ((Format) cmbFormats.SelectedItem).Description
                                      };

                    

                    List<Category> categories = new List<Category>();
                    foreach (var item in lsbCategoriesAdd.Items)
                    {
                        categories.Add((Category) item);
                    }

                    movie.Categories = categories;

                    if (MovieInstance != null)
                        movie.Id = MovieInstance.Id;

                    MovieRepository repository = new MovieRepository();
                    repository.Save(movie);

                    if (this.ParentId == ParentID.Dashboard)
                    {
                        this.MovieInstance = null;

                        if (
                            MessageBox.Show("Filme foi salvo com sucesso. Deseja registrar outro?", "Aviso",
                                            MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                            this.Close();
                        else
                            Initialize();
                    }
                    else
                    {
                        this.MovieInstance = movie;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandling.GenerateLog(ex.Message);
                    MessageBox.Show("Erro ocorrido. Verifique o arquivo de log.", "Error", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }

        private void BindCategories()
        {
            lsbCategories.Items.Clear();
            lsbCategoriesAdd.Items.Clear();

            List<Category> list = CategoryRepository.LoadAll();
            foreach (var item in list)
            {
                lsbCategories.Items.Add(item);
            }

            lsbCategories.DisplayMemberPath = "Description";
            lsbCategories.SelectedValuePath = "Id";
            lsbCategoriesAdd.DisplayMemberPath = "Description";
            lsbCategoriesAdd.SelectedValuePath = "Id";
        }

        private void BindTypes()
        {
            cmbType.DisplayMemberPath = "Description";
            cmbType.SelectedValuePath = "Id";
            cmbType.ItemsSource = MovieTypeRepository.GetAll();
            cmbType.SelectedIndex = 0;
        }

        private void BindFormats()
        {
            cmbFormats.DisplayMemberPath = "Description";
            cmbFormats.SelectedValuePath = "Id";
            cmbFormats.ItemsSource = FormatRepository.GetAll();
            cmbFormats.SelectedIndex = 0;
        }

        private void btnSearchMovie_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDIalog = new OpenFileDialog();
            fileDIalog.Filter = "Movie files (*.rmvb, *.avi, *.mkv)|*.rmvb;*.avi;*.mkv";
            fileDIalog.AddExtension = true;
            if (fileDIalog.ShowDialog() == true)
            {
                txtUri.Text = fileDIalog.FileName;
            }
        }

        private void btnSearchImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDIalog = new OpenFileDialog();
            fileDIalog.Filter = "Image files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            fileDIalog.AddExtension = true;
            if (fileDIalog.ShowDialog() == true)
            {
                txtCapa.Text = fileDIalog.FileName;
            }
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            lsbCategoriesAdd.Items.Add((Category)lsbCategories.SelectedItem);
            lsbCategories.Items.Remove((Category)lsbCategories.SelectedItem);
        }

        private void btnRemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            lsbCategories.Items.Add((Category)lsbCategoriesAdd.SelectedItem);
            lsbCategoriesAdd.Items.Remove((Category)lsbCategoriesAdd.SelectedItem);
        }

        private void btnNewCategory_Click(object sender, RoutedEventArgs e)
        {
            frmCategory form = new frmCategory();
            form.ShowDialog();
            BindCategories();
        }
    }

    public enum ParentID
    {
        Dashboard,
        MovieData
    }
}
