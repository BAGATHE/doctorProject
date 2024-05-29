namespace Docteur.Models
{
    public class MedicamentTraitement : Medicament
    {
        double nombrePrise;

        public MedicamentTraitement()
        {
        }

        public MedicamentTraitement(int id, string nom, double prixUnitaire) : base(id, nom, prixUnitaire)
        {
        }

        public MedicamentTraitement(int id, string nom, double prixUnitaire, double nombrePrise) : base(id, nom, prixUnitaire)
        {
            NombrePrise = nombrePrise;
        }

        public double NombrePrise { get => nombrePrise; set => nombrePrise = value; }

        public double GetPrixTotal()
        {
            return this.PrixUnitaire * this.NombrePrise;
        }
    }
}