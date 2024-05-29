namespace Docteur.Models
{
    public class MaladieDetails : BDDObject
    {
        int id;
        int idPartieCorps;
        double echelleMin;
        double echelleMax;

        public MaladieDetails()
        {
        }

        public MaladieDetails(int id, int idPartieCorps, double echelleMin, double echelleMax)
        {
            Id = id;
            IdPartieCorps = idPartieCorps;
            EchelleMin = echelleMin;
            EchelleMax = echelleMax;
        }

        [Default]
        public int Id { get => id; set => id = value; }
        public int IdPartieCorps { get => idPartieCorps; set => idPartieCorps = value; }
        public double EchelleMin { get => echelleMin; set => echelleMin = value; }
        public double EchelleMax { get => echelleMax; set => echelleMax = value; }

        public override string tableName()
        {
            return "maladieDetails";
        }
    }
}