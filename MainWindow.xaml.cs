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
        Dictionary<int, string> roles;
        public MainWindow()
        {
            InitializeComponent();
            Connexion.Initialize();
            lesPersonnels = Connexion.SelectedPersonnel();
            dataGridPersonnel.ItemsSource = lesPersonnels;
            Dictionary<int, string> roles = Connexion.SelectedRole();
            var roleList = roles.Select(r => new { RoleID = r.Key, RoleName = r.Value }).ToList();

            // Lier la ComboBox aux rôles
            comboRolePersonnel.ItemsSource = roleList;
            comboRolePersonnel.DisplayMemberPath = "RoleName";
            comboRolePersonnel.SelectedValuePath = "RoleID";

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
                        int selectedRoleId = (int)comboRolePersonnel.SelectedValue;
                        Connexion.UpdatePersonnel(selectedPersonnel.IdPers, nom, prenom, selectedRoleId, certification);
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
            try
            {
                // Vérifie si un élément est sélectionné dans le DataGrid
                if (dataGridPersonnel.SelectedItem is Personnel selectedPersonnel)
                {
                    
                    // Affiche une boîte de dialogue pour confirmer la suppression
                    var result = MessageBox.Show(
                        $"Êtes-vous sûr de vouloir supprimer {selectedPersonnel.NomPers} {selectedPersonnel.PrePres} ?",
                        "Confirmation de suppression",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        int id = selectedPersonnel.IdPers;
                        // Appeler la méthode pour supprimer dans la base de données
                        Connexion.DeletePersonnel(id);
                        dataGridPersonnel.SelectedIndex = -1;
                        // Supprimer l'élément de la liste locale
                        lesPersonnels.Remove(selectedPersonnel);

                        // Rafraîchir le DataGrid
                        dataGridPersonnel.ItemsSource = null;
                        dataGridPersonnel.ItemsSource = lesPersonnels;

                        // Afficher un message de succès
                        MessageBox.Show("Personnel supprimé avec succès !");
                    }
                }
                else
                {
                    // Aucun élément n'est sélectionné
                    MessageBox.Show("Veuillez sélectionner un personnel à supprimer.");
                }
            }
            catch (Exception ex)
            {
                // Afficher un message d'erreur en cas de problème
                MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void AjouterPersonnel_Click(object sender, RoutedEventArgs e)
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
            lesPersonnels = Connexion.SelectedPersonnel();
            dataGridPersonnel.ItemsSource = lesPersonnels;
            dataGridPersonnel.Items.Refresh();

        }

        private void dataGridPersonnel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridPersonnel.SelectedIndex != -1)
            {

                Personnel selectedPersonnel = dataGridPersonnel.SelectedItem as Personnel;

                txtNomPersonnel.Text = selectedPersonnel.NomPers;
                txtPrenomPersonnel.Text = selectedPersonnel.PrePres;
                comboRolePersonnel.SelectedValue = selectedPersonnel.IdRole;
                txtCertificationPersonnel.Text = selectedPersonnel.CertifPers;
            }


        }

        private void txtsearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            dataGridPersonnel.ItemsSource = Connexion.SearchPersonnel(txtsearch.Text);
            dataGridPersonnel.Items.Refresh();
        }
    }
}
