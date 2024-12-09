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
                var materiels = Connexion.GetAllMateriels();
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
            if (!string.IsNullOrWhiteSpace(TypeTextBox.Text) &&
                decimal.TryParse(PrixLocTextBox.Text, out var prixLoc) &&
                int.TryParse(QteDispTextBox.Text, out var qteDisp))
            {
                try
                {
                    var materiel = new Materiel
                    {
                        Type = TypeTextBox.Text,
                        PrixLoc = prixLoc,
                        QteDisp = qteDisp
                    };

                    Connexion.InsertMateriel(materiel.Type, materiel.PrixLoc, materiel.QteDisp);
                    materielList.Add(materiel);
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'ajout du matériel : {ex.Message}");
                }
            }
            else
            {
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

        private void MaterielListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class Materiel
    {
        public int IdMat { get; set; }
        public string Type { get; set; }
        public decimal PrixLoc { get; set; }
        public int QteDisp { get; set; }
    }
}