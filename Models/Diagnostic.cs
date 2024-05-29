namespace Docteur.Models
{
    public class Diagnostic
    {
        List<Maladie> maladies;
        List<MedicamentTraitement> medicaments;

        public List<Maladie> Maladies { get => maladies; set => maladies = value; }
        public List<MedicamentTraitement> Medicaments { get => medicaments; set => medicaments = value; }

        public string ShowListMaladies()
        {
            return string.Join(", ", Maladies.Select(m => m.Nom));
        }
    }
}