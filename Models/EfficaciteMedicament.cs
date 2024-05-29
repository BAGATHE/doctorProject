namespace Docteur.Models
{
    public class EfficaciteMedicament : BDDObject
    {
        int id;
        int idPartieCorps;
        double variationEchelle;

        public EfficaciteMedicament()
        {
        }

        public EfficaciteMedicament(int id, int idPartieCorps, double variationEchelle)
        {
            Id = id;
            IdPartieCorps = idPartieCorps;
            VariationEchelle = variationEchelle;
        }

        [Default]
        public int Id { get => id; set => id = value; }
        public int IdPartieCorps { get => idPartieCorps; set => idPartieCorps = value; }
        public double VariationEchelle { get => variationEchelle; set => variationEchelle = value; }

        public override string tableName()
        {
            return "efficaciteMedicament";
        }
    }
}