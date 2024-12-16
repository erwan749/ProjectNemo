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
        List<Personnel> personnels;
        public MainWindow()
        {
            InitializeComponent();
            Connexion.Initialize();
            Dictionary<int, string> roles = Connexion.SelectedRole();
            comboRolePerso.Items.Clear();
            comboRolePerso.ItemsSource = roles.Values;
            personnels = Connexion.SelectedPersonnel();
            dataGridPersonnel.ItemsSource = personnels;
            LoadPersonnels();
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
                    comboRolePerso.SelectedItem is ComboBoxItem selectedRole &&
                    !string.IsNullOrWhiteSpace(txtCertificationPersonnel.Text)
                   )
                {
                    string nomPers = txtNomPersonnel.Text;
                    string prePers = txtPrenomPersonnel.Text;
                    int nomRole = Convert.ToInt32(comboRolePerso);
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
