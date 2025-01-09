using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace NemoApp
{
    public partial class MainWindow : Window
    {

        List<Personnel> lesPersonnels = new List<Personnel>();
        Dictionary<int, string> roles;
        List<Plonge> lesPlonges = new List<Plonge>();
        List<Site> leSite = new List<Site>();
        public MainWindow()
        {
            InitializeComponent();
            Connexion.Initialize();
            lesPersonnels = Connexion.SelectedPersonnel();
            dataGridPersonnel.ItemsSource = lesPersonnels;
            Dictionary<int, string> roles = Connexion.SelectedRole();
            var roleList = roles.Select(r => new { RoleID = r.Key, RoleName = r.Value }).ToList();
            lesPlonges = Connexion.SelectedPlongees();
            dataGridPlonge.ItemsSource = lesPlonges;
            leSite = Connexion.SelectedSite();

            // Lier la ComboBox aux rôles
            comboRolePersonnel.ItemsSource = roleList;
            comboRolePersonnel.DisplayMemberPath = "RoleName";
            comboRolePersonnel.SelectedValuePath = "RoleID";

            // Lier la ComboBox aux sites
            comboSitePlong.ItemsSource = leSite;
            comboSitePlong.DisplayMemberPath = "NomSite";
            comboSitePlong.SelectedValuePath = "IdSite";



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

        private void AjouterPlongee_Click(object sender, RoutedEventArgs e)
        {
         
            try
            {
                if (datePickerPlong.SelectedDate.HasValue &&
                    comboSitePlong.SelectedValue != null &&
                    comboHeurePlong.SelectedItem != null &&
                    !string.IsNullOrWhiteSpace(txtDureePlong.Text) && int.TryParse(txtDureePlong.Text, out int duree))
                {
                    // Récupérer la date sélectionnée
                    DateTime datePlong = datePickerPlong.SelectedDate.Value;

                    // Récupérer l'horaire sélectionné
                    string? horairePlong = comboHeurePlong.SelectedValue as string;

                    // Combiner la date et l'horaire
                    MessageBox.Show(horairePlong);
                    DateTime dateHeurePlong = DateTime.Parse($"{datePlong:yyyy-MM-dd}  {horairePlong}");

                    // Récupérer les autres informations
                    int idSite = (int)comboSitePlong.SelectedValue;
                    string nomSite = ((Site)comboSitePlong.SelectedItem).NomSite;

                    // Créer une instance de Site
                    Site leSite = new Site(idSite, nomSite, 0); // 0 est une valeur par défaut pour profondMax

                    // Ajouter la plongée à la base de données
                    Connexion.InsertPlongees(dateHeurePlong, leSite, duree.ToString());

                    // Rafraîchir la liste des plongées
                    lesPlonges = Connexion.SelectedPlongees();
                    dataGridPlonge.ItemsSource = null;
                    dataGridPlonge.ItemsSource = lesPlonges;

                    MessageBox.Show("Plongée ajoutée avec succès !");
                }
                else
                {
                    MessageBox.Show("Veuillez remplir tous les champs correctement.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout de la plongée : {ex.Message}");
            }
        }
    }
}