using System.Windows;
using MyMediaStore.Data;
using MyMediaStore.Models;


namespace MyMediaStore
{
    /// <summary>
    /// Interaction logic for frmCategory.xaml
    /// </summary>
    public partial class frmCategory : Window
    {
        public frmCategory()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            BindGrid();
            txtCategory.Text = string.Empty;
            txtId.Text = CategoryRepository.GetCurrentID();
            btnExcluir.Visibility = System.Windows.Visibility.Hidden;
            imgExcluir.Visibility = System.Windows.Visibility.Hidden;
            lblExcluir.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (txtCategory.Text.Equals(""))
                MessageBox.Show("Favor informar a categoria");
            else
            {
                Category category = new Category()
                                        {
                                            Id = txtId.Text,
                                            Description = txtCategory.Text
                                        };

                CategoryRepository repository = new CategoryRepository();
                repository.Save(category);

                Initialize();
            }
        }

        private void BindGrid()
        {
            lstCategorias.DataContext = CategoryRepository.LoadAll();
        }

        private void lstCategorias_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Category category = (Category) lstCategorias.SelectedItem;
            txtCategory.Text = category.Description;
            txtId.Text = category.Id;
            btnExcluir.Visibility = System.Windows.Visibility.Visible;
            imgExcluir.Visibility = System.Windows.Visibility.Visible;
            lblExcluir.Visibility = System.Windows.Visibility.Visible;
        }

        private void imgSave_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Category category = new Category()
            {
                Id = txtId.Text,
                Description = txtCategory.Text
            };

            CategoryRepository repository = new CategoryRepository();
            repository.Save(category);

            Initialize();
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            Category category = (Category)lstCategorias.SelectedItem;
            if (MessageBox.Show(string.Format("{0} {1}", "Deseja realmente excluir a categoria ", category.Description),"Aviso", MessageBoxButton.YesNo, MessageBoxImage.Information)
                == MessageBoxResult.Yes)
            {
                CategoryRepository repository = new CategoryRepository();
                repository.Delete(category);
                Initialize();
            }
        }
    }
}
