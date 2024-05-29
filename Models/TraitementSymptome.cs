namespace Docteur.Models
{
    public class TraitementSymptome
    {
        CaracteristiqueCorps symptome;
        double variation;

        public TraitementSymptome()
        {
        }

        public TraitementSymptome(CaracteristiqueCorps symptome, double variation)
        {
            Symptome = symptome;
            Variation = variation;
        }

        public CaracteristiqueCorps Symptome { get => symptome; set => symptome = value; }
        public double Variation { get => variation; set => variation = value; }
    }
}