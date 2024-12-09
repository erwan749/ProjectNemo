using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NemoApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadMaterials();
            LoadClients();
        }

        // Modèles de données (pour la démonstration)
        public class Material
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int AvailableQuantity { get; set; }
        }

        public class Client
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        // Charger les matériaux dans le DataGrid
        private void LoadMaterials()
        {
            var materials = new List<Material>
            {
                new Material { Name = "Palme, Masque, Tuba", Price = 20.00m, AvailableQuantity = 10 },
                new Material { Name = "Combinaison", Price = 15.00m, AvailableQuantity = 5 },
                new Material { Name = "Gilet de stabilisation", Price = 30.00m, AvailableQuantity = 8 },
                new Material { Name = "Bouteille", Price = 10.00m, AvailableQuantity = 12 },
                new Material { Name = "Ordinateur de plongée", Price = 50.00m, AvailableQuantity = 3 }
            };

            MaterialsDataGrid.ItemsSource = materials;
        }

        // Charger les clients dans le ComboBox
        private void LoadClients()
        {
            var clients = new List<Client>
            {
                new Client { Id = 1, Name = "Alice Dupont" },
                new Client { Id = 2, Name = "Bob Martin" },
                new Client { Id = 3, Name = "Claire Lefevre" }
            };

            ClientComboBox.ItemsSource = clients;
            ClientComboBox.DisplayMemberPath = "Name";
            ClientComboBox.SelectedValuePath = "Id";
        }

        // Gestion de la location (au clic sur le bouton)
        private void RentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaterial = (Material)MaterialsDataGrid.SelectedItem;
            var selectedClient = (Client)ClientComboBox.SelectedItem;
            var quantity = (int)QuantityNumericUpDown.Value;

            if (selectedMaterial != null && selectedClient != null)
            {
                // Vérifier si la quantité demandée est disponible
                if (selectedMaterial.AvailableQuantity >= quantity)
                {
                    // Mettre à jour la quantité du matériel
                    selectedMaterial.AvailableQuantity -= quantity;
                    MaterialsDataGrid.Items.Refresh(); // Rafraîchir le DataGrid

                    MessageBox.Show($"Location enregistrée pour {selectedClient.Name} : {quantity} x {selectedMaterial.Name}");
                }
                else
                {
                    MessageBox.Show("Quantité insuffisante");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un matériel et un client");
            }
        }
    }
}
