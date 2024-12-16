using MySql.Data.MySqlClient;
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
            LoadPersonnels();
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

        private void LoadPersonnels()
        {
            // Exemple : charger les personnels dans le DataGrid
            dataGridPersonnel.ItemsSource = Connexion.SelectedPersonnel(); // Implémentez SelectPersonnels dans la classe Connexion
        }
        private void AjouterPersonnel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validation des données d'entrée
                if (!string.IsNullOrWhiteSpace(txtNomPersonnel.Text) &&
                    !string.IsNullOrWhiteSpace(txtPrenomPersonnel.Text) &&
                    !string.IsNullOrWhiteSpace(txtRolePersonnel.Text) &&
                    !string.IsNullOrWhiteSpace(txtCertificationPersonnel.Text)
                   )
                {
                    string nomPers = txtNomPersonnel.Text;
                    string prePers = txtPrenomPersonnel.Text;
                    int nomRole = Convert.ToInt32(txtRolePersonnel.Text);
                    string certifRole = txtCertificationPersonnel.Text;

                    // Appel de la méthode d'insertion
                    Connexion.InsertPersonnel(nomPers, prePers, nomRole, certifRole);

                    // Recharger les personnels (à implémenter)
                    LoadPersonnels();

                    // Vérification après rechargement
                    if (dataGridPersonnel.Items.Count > 0) // Ajustez selon votre logique
                    {
                        MessageBox.Show("Personnel ajouté avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Erreur : Le personnel n'a pas été ajouté.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez remplir tous les champs et entrer un ID valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
