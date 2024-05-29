using Npgsql;
using System.Globalization;

namespace Docteur.Models
{
    public class Person
    {
        int age;
        CaracteristiqueCorps[] parametres;

        public Person()
        {
        }

        public Person(int age, CaracteristiqueCorps[] parametres)
        {
            Age = age;
            Parametres = parametres;
        }

        public int Age { get => age; set => age = value; }
        public CaracteristiqueCorps[] Parametres { get => parametres; set => parametres = value; }

        public CaracteristiqueCorps GetTemperature()
        {
            for (int i = 0; i < this.Parametres.Length; i++)
            {
                if (this.Parametres[i].Id == 2)
                {
                    return this.Parametres[i];
                }
            }
            throw new Exception("Temperature not found");
        }

        public void AddParameter(string[] idParameter, string[] parameterValue)
        {
            this.Parametres = new CaracteristiqueCorps[idParameter.Length];
            for (int i = 0; i < idParameter.Length; i++)
            {
                this.Parametres[i] = new CaracteristiqueCorps(idParameter[i], "", parameterValue[i]);
            }
        }

        public double GetEchelleTemperature(NpgsqlConnection con, double temperature)
        {
            bool isNewConnexion = false;
            if (con == null)
            {
                con = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }
            try
            {
                string sql = "SELECT echelle FROM temperatureCorporelle WHERE temperaturemin <= " + temperature.ToString(CultureInfo.InvariantCulture) + " AND " + temperature.ToString(CultureInfo.InvariantCulture) + " <= temperaturemax";
                Console.WriteLine(sql);
                using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetDouble(0);
                        }
                    }
                }
                throw new Exception("Echelle impossible a determiner");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (isNewConnexion == true)
                {
                    con.Close();
                }
            }
        }

        public List<CaracteristiqueCorps> GetSymptomes()
        {
            List<CaracteristiqueCorps> symptomes = new List<CaracteristiqueCorps>();
            for (int i = 0; i < this.Parametres.Length; i++)
            {
                if (this.Parametres[i].Echelle != 0)
                {
                    symptomes.Add(this.Parametres[i]);
                }
            }
            return symptomes;
        }
    }
}