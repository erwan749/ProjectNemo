using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NemoApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadMateriels();
            LoadTypes();
        }

        private void LoadMateriels()
        {
            try
            {
                // Récupérer les matériels depuis la base de données
                List<Materiel> materiels = Connexion.SelectMateriel();

                // Assigner la liste directement au DataGrid
                dataGridMateriels.ItemsSource = materiels;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des matériels : " + ex.Message);
            }
        }

        private void LoadTypes()
        {
            try
            {
                // Ajouter des types fictifs dans le ComboBox pour tester
                comboNomTypeMat.Items.Clear();
                comboNomTypeMat.Items.Add(new ComboBoxItem { Content = "Bouteille", Tag = 1 });
                comboNomTypeMat.Items.Add(new ComboBoxItem { Content = "Palmes", Tag = 2 });
                comboNomTypeMat.Items.Add(new ComboBoxItem { Content = "Masque", Tag = 3 });
                comboNomTypeMat.Items.Add(new ComboBoxItem { Content = "Combinaison", Tag = 4 });
                comboNomTypeMat.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des types de matériel : " + ex.Message);
            }
        }

        private void ModifierMateriel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridMateriels.SelectedItem is Materiel selectedMateriel)
            {
                try
                {
                    // Récupérer les informations saisies
                    string nomTypeMat = comboNomTypeMat.Text;
                    if (double.TryParse(txtPrixLoc.Text, out double prixLoc) &&
                        int.TryParse(txtQteDisp.Text, out int qteDisp))
                    {
                        // Vérifier que le type sélectionné est valide
                        if (comboNomTypeMat.SelectedValue != null)
                        {
                            int idTypeMat = (int)comboNomTypeMat.SelectedValue;

                            // Appeler la méthode de mise à jour
                            Connexion.UpdateMateriel(selectedMateriel.IdMat, idTypeMat, prixLoc, qteDisp);

                            // Recharger les matériels
                            LoadMateriels();
                            MessageBox.Show("Matériel modifié avec succès !");
                        }
                        else
                        {
                            MessageBox.Show("Veuillez sélectionner un type de matériel valide.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Veuillez entrer des valeurs valides pour le prix et la quantité.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la modification : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un matériel à modifier.");
            }
        }

        private void AjoutezMateriel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (comboNomTypeMat.SelectedItem is ComboBoxItem selectedItem &&
                    double.TryParse(txtPrixLoc.Text, out double prixLoc) &&
                    int.TryParse(txtQteDisp.Text, out int qteDisp))
                {
                    int idTypeMat = (int)selectedItem.Tag;

                    // Appeler la méthode d'ajout
                    Connexion.InsertMateriel(idTypeMat, prixLoc, qteDisp);

                    // Recharger les matériels
                    LoadMateriels();

                    // Vérification après rechargement
                    if (dataGridMateriels.Items.Count > 0) // Ajustez selon votre logique
                    {
                        MessageBox.Show("Matériel ajouté avec succès !");
                    }
                    else
                    {
                        MessageBox.Show("Erreur : Le matériel n'a pas été ajouté.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez entrer des valeurs valides pour le prix et la quantité.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout : {ex.Message}");
            }
        }



        private void SupprimerMateriel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridMateriels.SelectedItem is Materiel selectedMateriel)
            {
                try
                {
                    // Demander confirmation à l'utilisateur
                    var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce matériel ?", "Confirmation", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        // Appeler la méthode de suppression
                        Connexion.DeleteMateriel(selectedMateriel.IdMat);

                        // Recharger les matériels
                        LoadMateriels();
                        MessageBox.Show("Matériel supprimé avec succès !");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un matériel à supprimer.");
            }
        }
    }
}
