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
        }

        private void AjouterPersonnel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validation des données d'entrée
                
               
                    string nomPers = txtNomPersonnel.Text;
                    string prePers = txtPrenomPersonnel.Text;
                    int nomRole = Convert.ToInt32(comboRolePerso.SelectedValue);
                    string certifRole = txtCertificationPersonnel.Text;

                    // Appel de la méthode d'insertion
                    Connexion.InsertPersonnel(nomPers, prePers, nomRole, certifRole);


                    // Vérification après rechargement
                    if (dataGridPersonnel.Items.Count > 0) // Ajustez selon votre logique
                    {
                        MessageBox.Show("Personnel ajouté avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Erreur : Le personnel n'a pas été ajouté.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                personnels = Connexion.SelectedPersonnel();
                dataGridPersonnel.ItemsSource= personnels;
                dataGridPersonnel.Items.Refresh();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
