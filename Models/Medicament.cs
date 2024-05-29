using Npgsql;

namespace Docteur.Models
{
    public class Medicament : BDDObject
    {
        int id;
        string nom;
        double prixUnitaire;
        List<TraitementSymptome> traitements;

        public Medicament()
        {
        }

        public Medicament(int id, string nom, double prixUnitaire)
        {
            Id = id;
            Nom = nom;
            PrixUnitaire = prixUnitaire;
            Traitements = new List<TraitementSymptome>();
        }

        [Default]
        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public double PrixUnitaire { get => prixUnitaire; set => prixUnitaire = value; }
        public List<TraitementSymptome> Traitements { get => traitements; set => traitements = value; }

        public override string tableName()
        {
            return "medicament";
        }

        // public List<MedicamentTraitement> GetOrdonnances(NpgsqlConnection con, CaracteristiqueCorps symptome)
        // {
        //     bool isNewConnexion = false;
        //     if (con == null)
        //     {
        //         con = new Connect().getConnectPostgres();
        //         isNewConnexion = true;
        //     }
        //     try
        //     {

        //         return null;
        //     }
        //     catch (Exception)
        //     {
        //         throw;
        //     }
        //     finally
        //     {
        //         if (isNewConnexion == true)
        //         {
        //             con.Close();
        //         }
        //     }
        // }

        // public List<CaracteristiqueCorps> TraiterSymptome(CaracteristiqueCorps symptome)
        // {
        //     List<CaracteristiqueCorps> contreIndication = new List<CaracteristiqueCorps>();
        //     for (int i = 0; i < this.Traitements.Count; i++)
        //     {
        //         if (this.Traitements[i].Symptome.Id == symptome.Id)
        //         {
        //             //symptome
        //         }
        //     }
        //     return null;
        // }

        public double GetVariation(CaracteristiqueCorps caracteristiqueCorps)
        {
            try
            {
                return this.Traitements.First(t => t.Symptome.Id == caracteristiqueCorps.Id).Variation;
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public void AddTraitement(CaracteristiqueCorps parametre, double variation)
        {
            this.Traitements.Add(new TraitementSymptome(parametre, variation));
        }
    }
}