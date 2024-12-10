using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
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
using MySql.Data.MySqlClient;  // Assurez-vous que vous avez ajouté cette référence

namespace NemoApp
{
    public partial class MaterielWindow : Window
    {
        private ObservableCollection<Materiel> materielList = new ObservableCollection<Materiel>();
        private Materiel selectedMateriel;

        public MaterielWindow()
        {
            InitializeComponent();
            MaterielListBox.ItemsSource = materielList;
            LoadMateriels();

            // Événement de sélection
            MaterielListBox.SelectionChanged += MaterielListBox_SelectionChanged;
        }

        private void LoadMateriels()
        {
            try
            {
                var materiels = Connexion.SelectMateriel();
                materielList.Clear();
                foreach (var materiel in materiels)
                {
                    materielList.Add(materiel);
                }

                if (materielList.Count == 0)
                {
                    MessageBox.Show("Aucun matériel trouvé dans la base de données.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des matériels : {ex.Message}");
            }
        }

        private void ClearFields()
        {
            TypeTextBox.Text = string.Empty;
            PrixLocTextBox.Text = string.Empty;
            QteDispTextBox.Text = string.Empty;
            selectedMateriel = null;
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier que les champs de texte ne sont pas vides et que la conversion réussit
            if (!string.IsNullOrWhiteSpace(TypeTextBox.Text) &&
                decimal.TryParse(PrixLocTextBox.Text, out var prixLoc) &&
                int.TryParse(QteDispTextBox.Text, out var qteDisp))
            {
                try
                {
                    // Créer un objet Materiel à partir des données saisies
                    var materiel = new Materiel
                    {
                        Type = TypeTextBox.Text,
                        PrixLoc = prixLoc,
                        QteDisp = qteDisp
                    };

                    // Appeler la méthode pour insérer dans la base de données
                    Connexion.InsertMateriel(Convert.ToInt32(materiel.Type), Convert.ToDouble(materiel.PrixLoc), materiel.QteDisp);

                    // Ajouter le matériel à la liste et vider les champs
                    materielList.Add(materiel);
                    ClearFields();
                }
                catch (Exception ex)
                {
                    // Afficher une erreur si l'insertion échoue
                    MessageBox.Show($"Erreur lors de l'ajout du matériel : {ex.Message}");
                }
            }
            else
            {
                // Afficher un message d'erreur si les champs sont incorrects
                MessageBox.Show("Veuillez remplir tous les champs correctement.");
            }
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMateriel != null)
            {
                try
                {
                    Connexion.DeleteMateriel(selectedMateriel.IdMat);
                    materielList.Remove(selectedMateriel);
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression du matériel : {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un matériel à supprimer.");
            }
        }

        private void MaterielListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MaterielListBox.SelectedItem is Materiel materiel)
            {
                selectedMateriel = materiel;

                // Remplir les champs avec les données sélectionnées
                TypeTextBox.Text = selectedMateriel.Type;
                PrixLocTextBox.Text = selectedMateriel.PrixLoc.ToString();
                QteDispTextBox.Text = selectedMateriel.QteDisp.ToString();
            }
            else
            {
                ClearFields();
            }
        }
    }

}