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
        List<Personnel> lesPersonnel;
        public MainWindow()
        {
            InitializeComponent();
            Connexion.Initialize();
            LoadMateriels();
            Dictionary<int, string> roles = Connexion.SelectedRole();
            comboRolePersonnel.Items.Clear();
            comboRolePersonnel.ItemsSource = roles.Values;
            LoadTypes();
            lesPersonnel = Connexion.SelectedPersonnel() ;
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
       

        private void AjouterPersonnel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validation des données d'entrée
                if (!string.IsNullOrWhiteSpace(txtNomPersonnel.Text) &&
                    !string.IsNullOrWhiteSpace(txtPrenomPersonnel.Text) &&
                    comboRolePersonnel.SelectedItem is ComboBoxItem selectedRole &&
                    !string.IsNullOrWhiteSpace(txtCertificationPersonnel.Text)
                   )
                {
                    string nomPers = txtNomPersonnel.Text;
                    string prePers = txtPrenomPersonnel.Text;
                    int nomRole = Convert.ToInt32(comboRolePersonnel);
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

        private void ModifierPersonnel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridPersonnel.SelectedItem is Personnel selectedPersonnel)
            {
                try
                {
                    // Récupérer les informations saisies
                    string nom = txtNomPersonnel.Text;
                    string prenom = txtPrenomPersonnel.Text;
                    int role = Convert.ToInt32(comboRolePersonnel.Text);
                    string certification = txtCertificationPersonnel.Text;

                    // Validation des entrées
                    if (!string.IsNullOrWhiteSpace(nom) &&
                        !string.IsNullOrWhiteSpace(prenom) &&
                        comboRolePersonnel.SelectedItem is ComboBoxItem selectedRole &&
                        !string.IsNullOrWhiteSpace(certification))
                    {
                        // Appeler la méthode de mise à jour
                        Connexion.UpdatePersonnel(selectedPersonnel.IdPers, nom, prenom, role, certification);

                        // Recharger les personnels
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
            Personnel selectedPersonnel = dataGridPersonnel.SelectedItem as Personnel;
            try {
                Connexion.DeletePersonnel(selectedPersonnel.IdPers);
                    
                    }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    }
    
