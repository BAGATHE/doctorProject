namespace Docteur.Models
{
    public class Maladie : BDDObject
    {
        int id;
        string nom;
        CaracteristiqueCorps[] parametres;

        public Maladie()
        {
        }

        public Maladie(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }

        [Default]
        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public CaracteristiqueCorps[] Parametres { get => parametres; set => parametres = value; }

        public override string tableName()
        {
            return "maladie";
        }
    }
}