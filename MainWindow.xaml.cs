using NemoApp;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjetNemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Personnel> lesP = new List<Personnel>();
        Dictionary<int, string> roles;
        public MainWindow()
        {
            InitializeComponent();
            Connexion.Initialize();

            // Récupérer les rôles à partir de la base de données
            Dictionary<int, string> roles = Connexion.SelectedRole();

            // Créer une liste anonyme pour lier à la ComboBox (affichage des noms, stockage des IDs)
            var roleList = roles.Select(r => new { RoleID = r.Key, RoleName = r.Value }).ToList();
            
            // Lier la ComboBox aux rôles
            comboRolePersonnel.ItemsSource = roleList;

            // Afficher les noms des rôles et utiliser l'ID comme valeur sélectionnée
            comboRolePersonnel.DisplayMemberPath = "RoleName";
            comboRolePersonnel.SelectedValuePath = "RoleID";

            // Récupérer le personnel et l'afficher dans le DataGrid
            lesP = Connexion.SelectedPersonnel();
            dataGridPersonnel.ItemsSource = lesP;
        }

        private void dataGridPersonnel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Personnel selectedPersonnel = dataGridPersonnel.SelectedItem as Personnel;
            if (selectedPersonnel != null)
            {
                txtNomPersonnel.Text = selectedPersonnel.NomPers;
                txtPrenomPersonnel.Text = selectedPersonnel.PrePres;

                // Vérifier si le rôle existe dans le dictionnaire avant de récupérer l'ID
                //int myKey = roles.FirstOrDefault(x => x.Value == selectedPersonnel.NomRole).Key;

                //comboRolePersonnel.SelectedItem = myKey;

                txtCertificationPersonnel.Text = selectedPersonnel.CertifPers;
            }
        }

        private void btnAddPerso_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si un rôle a été sélectionné et si la valeur sélectionnée n'est pas null
            if (comboRolePersonnel.SelectedValue != null)
            {
                try
                {
                    // Récupérer l'ID du rôle sélectionné
                    int selectedRoleId = (int)comboRolePersonnel.SelectedValue;

                    // Insérer les données dans la table Personnel
                    Connexion.InsertPersonnel(txtNomPersonnel.Text, txtPrenomPersonnel.Text, selectedRoleId, txtCertificationPersonnel.Text);
                }
                catch (Exception ex)
                {
                    // En cas d'exception, afficher un message d'erreur
                    MessageBox.Show("Erreur lors de l'insertion : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un rôle.");
            }
            lesP = Connexion.SelectedPersonnel();
            dataGridPersonnel.ItemsSource = lesP;
            dataGridPersonnel.Items.Refresh();
        }

    }
}