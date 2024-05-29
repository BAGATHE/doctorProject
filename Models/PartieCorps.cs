namespace Docteur.Models
{
    public class PartieCorps : BDDObject
    {
        int id;
        string nom;

        public PartieCorps()
        {
        }

        public PartieCorps(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }

        [Default]
        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }

        public override string tableName()
        {
            return "partieCorps";
        }
    }
}