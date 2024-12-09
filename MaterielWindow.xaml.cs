using System;
using System.Data;
using System.Linq;
using System.Windows;

namespace NemoApp
{
    public partial class MaterielWindow : Window
    {
        public MaterielWindow()
        {
            InitializeComponent();
            loadMateriels();
            loadTypes();
        }

        private void loadMateriels()
        {
            try
            {
                // Récupérer les matériels depuis la base de données
                List<Materiel> materiels = Connexion.SelectMateriel();

                // Convertir la liste en DataTable ou créer une DataTable compatible
                DataTable materielTable = new DataTable();
                materielTable.Columns.Add("idMat", typeof(int));
                materielTable.Columns.Add("TypeMat", typeof(string));
                materielTable.Columns.Add("prix", typeof(decimal));
                materielTable.Columns.Add("qte", typeof(int));

                foreach (var materiel in materiels)
                {
                    materielTable.Rows.Add(materiel.IdMat, materiel.TypeMat, materiel.PrixMat, materiel.Qte);
                }

                // Assigner la DataTable à la ListBox
                MaterielListBox.ItemsSource = materielTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des matériels : " + ex.Message);
            }
        }


        private void loadTypes()
        {
            try
            {
                // Récupérer les types de matériel depuis la base de données
                List<Materiel> types = Connexion.SelectMateriel();

                // Assigner les types au ComboBox
                TypeComboBox.ItemsSource = types;
                TypeComboBox.DisplayMemberPath = "NomTypeMat";  // Afficher le nom du type
                TypeComboBox.SelectedValuePath = "IdTypeMat";   // Utiliser l'ID comme valeur
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des types de matériel : " + ex.Message);
            }
        }


        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            if (MaterielListBox.SelectedItem is DataRowView selectedRow)
            {
                // Récupérer les informations du matériel sélectionné
                int idMat = Convert.ToInt32(selectedRow["idMat"]);
                string typeNom = selectedRow["nomTypeMat"].ToString();
                decimal prixLoc = Convert.ToDecimal(selectedRow["prixLoc"]);
                int quantite = Convert.ToInt32(selectedRow["qteDisp"]);

                // Trouver le bon élément dans le ComboBox en fonction du nom
                TypeComboBox.SelectedItem = TypeComboBox.Items.Cast<DataRowView>()
                    .FirstOrDefault(item => item["nomTypeMat"].ToString() == typeNom);

                // Remplir les TextBox avec les valeurs existantes
                PrixTextBox.Text = prixLoc.ToString("F2");
                QuantiteTextBox.Text = quantite.ToString();

                // Demander confirmation à l'utilisateur
                var result = MessageBox.Show("Voulez-vous modifier ce matériel?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Vérifier si un type est bien sélectionné dans le ComboBox
                    if (TypeComboBox.SelectedItem != null)
                    {
                        // Récupérer l'ID du type de matériel sélectionné
                        int idTypeMat = Convert.ToInt32(((DataRowView)TypeComboBox.SelectedItem)["idTypeMat"]);

                        // Appeler la méthode de mise à jour dans la classe Connexion
                        Connexion.UpdateMateriel(idMat, idTypeMat, (double)prixLoc, quantite);

                        // Recharger les matériels après modification
                        loadMateriels();
                        MessageBox.Show("Matériel mis à jour avec succès!");
                    }
                    else
                    {
                        MessageBox.Show("Veuillez sélectionner un type de matériel.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un matériel à modifier.");
            }
        }

    }
}
