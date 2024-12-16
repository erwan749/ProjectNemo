using Org.BouncyCastle.Crypto;
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

        List<Personnel> lesPersonnels = new List<Personnel>();
        public MainWindow()
        {
            InitializeComponent();
            Connexion.Initialize();
            lesPersonnels = Connexion.SelectedPersonnel();
            dataGridPersonnel.ItemsSource = lesPersonnels;
            Dictionary<int, string> lesRoles = Connexion.SelectedRole();
            comboRolePersonnel.ItemsSource = lesRoles;
            
        }

       

        private void AjoutezPersonnel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ModifierPersonnel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridPersonnel.SelectedItem is Personnel selectedPersonnel)
            {
                try
                {
                    // Récupérer les informations saisies
                    string nom = txtNomPersonnel.Text.Trim();
                    string prenom = txtPrenomPersonnel.Text.Trim();
                    string role = comboRolePersonnel.Text.Trim();
                    string certification = txtCertificationPersonnel.Text.Trim();

                    // Validation des entrées
                    if (!string.IsNullOrWhiteSpace(nom) &&
                        !string.IsNullOrWhiteSpace(prenom) &&
                        !string.IsNullOrWhiteSpace(role) &&
                        !string.IsNullOrWhiteSpace(certification))
                    {
                        // Mettre à jour l'objet sélectionné dans la liste
                        selectedPersonnel.NomPers = nom;
                        selectedPersonnel.PrePres = prenom;
                        selectedPersonnel.NomRole = role;
                        selectedPersonnel.CertifPers = certification;

                        // Mettre à jour la source de données
                        int index = lesPersonnels.FindIndex(p => p.IdPers == selectedPersonnel.IdPers);
                        if (index >= 0)
                        {
                            lesPersonnels[index] = selectedPersonnel;
                        }

                        // Rafraîchir l'affichage du DataGrid
                        dataGridPersonnel.ItemsSource = null;
                        dataGridPersonnel.ItemsSource = lesPersonnels;

                        MessageBox.Show("Personnel modifié avec succès !");
                    }
                    else
                    {
                        MessageBox.Show("Veuillez remplir tous les champs correctement.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la modification : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un personnel à modifier.");
            }
        }


        private void SupprimerPersonnel_Click(object sender, RoutedEventArgs e)
        {
        }
           

        private void AjouterPersonnel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGridPersonnel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Personnel selectedPersonnel = dataGridPersonnel.SelectedItem as Personnel;

            txtNomPersonnel.Text = selectedPersonnel.NomPers;
            txtPrenomPersonnel.Text = selectedPersonnel.PrePres;
            comboRolePersonnel.SelectedItem = selectedPersonnel.NomRole;
            txtCertificationPersonnel.Text = selectedPersonnel.CertifPers;


        }
    }
}
