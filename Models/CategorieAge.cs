namespace Docteur.Models
{
    public class CategorieAge : BDDObject
    {
        int id;
        string nom;
        double ageMin;
        double ageMax;

        public CategorieAge()
        {
        }

        public CategorieAge(int id, string nom, double ageMin, double ageMax)
        {
            Id = id;
            Nom = nom;
            AgeMin = ageMin;
            AgeMax = ageMax;
        }

        [Default]
        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public double AgeMin { get => ageMin; set => ageMin = value; }
        public double AgeMax { get => ageMax; set => ageMax = value; }

        public override string tableName()
        {
            return "categorieAge";
        }
    }
}