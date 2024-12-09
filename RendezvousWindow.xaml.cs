using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Infotools;

namespace Infottols
{
    public partial class RendezvousWindow : Window
    {
        private ObservableCollection<RendezVous> rendezVousList = new ObservableCollection<RendezVous>();
        private ObservableCollection<Client> ClientsList = new ObservableCollection<Client>();
        private RendezVous selectedRendezVous;

        public RendezvousWindow()
        {
            InitializeComponent();
            RendezvousListBox.ItemsSource = rendezVousList;
            LoadAppointments();
            LoadClients();

            // Événement de sélection
            RendezvousListBox.SelectionChanged += RendezvousListBox_SelectionChanged;
        }

        private void LoadAppointments()
        {
            try
            {
                var appointments = Bdd.GetAllAppointments();
                rendezVousList.Clear();
                foreach (var appointment in appointments)
                {
                    rendezVousList.Add(appointment);
                }

                if (rendezVousList.Count == 0)
                {
                    MessageBox.Show("Aucun rendez-vous trouvé dans la base de données.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des rendez-vous : {ex.Message}");
            }
        }

        private void LoadClients()
        {
            try
            {
                var clients = Bdd.GetAllClients();
                ClientsList.Clear();

                foreach (var client in clients)
                {
                    ClientsList.Add(client);
                }

                ClientComboBox.ItemsSource = ClientsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des clients : {ex.Message}");
            }
        }

        private void ClearFields()
        {
            LocationTextBox.Text = string.Empty;
            DateDatePicker.SelectedDate = null;
            StatusCombobox.SelectedIndex = 0;
            ClientComboBox.SelectedIndex = -1;
            selectedRendezVous = null;
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            if (DateDatePicker.SelectedDate.HasValue &&
                !string.IsNullOrWhiteSpace(LocationTextBox.Text) &&
                ClientComboBox.SelectedValue != null)
            {
                try
                {
                    var rendezVous = new RendezVous
                    {
                        Date = DateDatePicker.SelectedDate.Value,
                        Location = LocationTextBox.Text,
                        Status = (Status)StatusCombobox.SelectedIndex,
                        ClientId = (int)ClientComboBox.SelectedValue
                    };

                    Bdd.InsertAppointment(rendezVous.ClientId, 1, rendezVous.Date, rendezVous.Location, rendezVous.Status.ToString());
                    rendezVousList.Add(rendezVous);
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'ajout du rendez-vous : {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
            }
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRendezVous != null &&
                DateDatePicker.SelectedDate.HasValue &&
                !string.IsNullOrWhiteSpace(LocationTextBox.Text) &&
                ClientComboBox.SelectedValue != null)
            {
                try
                {
                    // Récupérer le salespersonId (exemple avec un ComboBox, modifiez si nécessaire)
                    

                    // Mise à jour des propriétés du rendez-vous
                    selectedRendezVous.Date = DateDatePicker.SelectedDate.Value;
                    selectedRendezVous.Location = LocationTextBox.Text;
                    selectedRendezVous.Status = (Status)StatusCombobox.SelectedIndex;
                    selectedRendezVous.ClientId = (int)ClientComboBox.SelectedValue;

                    // Appel de la méthode pour mettre à jour la base de données
                    Bdd.UpdateAppointment(selectedRendezVous.Id, selectedRendezVous.ClientId, selectedRendezVous.Salesperson_Id, selectedRendezVous.Date, selectedRendezVous.Location, selectedRendezVous.Status.ToString());

                    // Recharge de la liste des rendez-vous
                    RendezvousListBox.Items.Clear(); // Vide la liste
                    var updatedAppointments = Bdd.GetAllAppointments(); // Méthode à implémenter pour récupérer tous les rendez-vous
                    foreach (var appointment in updatedAppointments)
                    {
                        RendezvousListBox.Items.Add(appointment);
                    }

                    // Optionnel: Remettre à jour l'affichage de la liste
                    RendezvousListBox.Items.Refresh();

                    // Effacer les champs de saisie
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la modification du rendez-vous : {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
            }
        }


        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRendezVous != null)
            {
                try
                {
                    Bdd.DeleteAppointment(selectedRendezVous.Id);
                    rendezVousList.Remove(selectedRendezVous);
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression du rendez-vous : {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un rendez-vous à supprimer.");
            }
        }

        private void RendezvousListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RendezvousListBox.SelectedItem is RendezVous rendezVous)
            {
                selectedRendezVous = rendezVous;

                // Remplir les champs avec les données sélectionnées
                LocationTextBox.Text = selectedRendezVous.Location;
                DateDatePicker.SelectedDate = selectedRendezVous.Date;
                StatusCombobox.SelectedIndex = (int)selectedRendezVous.Status;
                ClientComboBox.SelectedValue = selectedRendezVous.ClientId;
            }
            else
            {
                ClearFields();
            }
        }
    }
}
