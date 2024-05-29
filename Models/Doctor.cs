using Npgsql;

namespace Docteur.Models
{
    public class Doctor
    {
        public Doctor()
        {
        }

        public List<Medicament> GetAllMedicaments(NpgsqlConnection con)
        {
            bool isNewConnexion = false;
            if (con == null)
            {
                con = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }
            try
            {
                string sql = "SELECT * FROM vw_medicament_efficacite";
                List<Medicament> medicaments = new List<Medicament>();
                int idCurrentMedicine = 0;
                using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (idCurrentMedicine != reader.GetInt32(reader.GetOrdinal("idmedicament")))
                            {
                                medicaments.Add(new Medicament(reader.GetInt32(reader.GetOrdinal("idmedicament")), reader.GetString(reader.GetOrdinal("nom")), reader.GetDouble(reader.GetOrdinal("prixunitaire"))));
                                idCurrentMedicine = medicaments[medicaments.Count - 1].Id;
                            }
                            medicaments[medicaments.Count - 1].AddTraitement(new CaracteristiqueCorps(reader.GetInt32(reader.GetOrdinal("idcaracteristiquecorps")), "", 0), reader.GetDouble(reader.GetOrdinal("variationechelle")));
                        }
                    }
                }
                return medicaments;
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

        public List<Maladie> GetMaladies(NpgsqlConnection con, List<CaracteristiqueCorps> symptomes, Person p)
        {
            bool isNewConnexion = false;
            if (con == null)
            {
                con = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }
            try
            {
                int currentIdMaladie = 0;
                string idSymptomes = string.Join(",", symptomes.Select(s => s.Id));
                string sql = "SELECT vw_maladie_details.idMaladie, vw_maladie_details.nom " +
                "FROM vw_maladie_details " +
                "WHERE vw_maladie_details.idCaracteristiqueCorps IN(" + idSymptomes + ") AND ageMin <= " + p.Age + " AND " + p.Age + " <= ageMax AND NOT EXISTS( " +
                    "SELECT idCaracteristiqueCorps " +
                    "FROM vw_maladie_details " +
                    "WHERE idCaracteristiqueCorps NOT IN(" + idSymptomes + ") AND vw_maladie_details.idMaladie = idMaladie " +
                ") " +
                "GROUP BY vw_maladie_details.idMaladie, vw_maladie_details.nom; ";
                Console.WriteLine(sql);
                List<Maladie> maladies = new List<Maladie>();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (currentIdMaladie != reader.GetInt32(reader.GetOrdinal("idmaladie")))
                            {
                                maladies.Add(new Maladie(reader.GetInt32(reader.GetOrdinal("idmaladie")), reader.GetString(reader.GetOrdinal("nom"))));
                                currentIdMaladie = maladies[maladies.Count - 1].Id;
                            }
                        }
                    }
                }
                if (maladies.Count == 0 && symptomes.Count > 0)
                {
                    maladies.Add(new Maladie(0, "Maladie inconnue"));
                }
                return maladies;
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



        public List<MedicamentTraitement> GetMedicaments(NpgsqlConnection con, List<CaracteristiqueCorps> symptomes)
        {
            bool isNewConnexion = false;
            if (con == null)
            {
                con = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }
            try
            {
                List<Medicament> medicaments = this.GetAllMedicaments(con);
                List<CaracteristiqueCorps> parametres = new CaracteristiqueCorps().Find(con, "").OfType<CaracteristiqueCorps>().ToList();
                double[] solutions = new ProgrammationLineaire().FindBestCombinaisonMedicament(medicaments, parametres, symptomes);
                List<MedicamentTraitement> traitements = new List<MedicamentTraitement>();
                for (int i = 0; i < solutions.Length; i++)
                {
                    if (solutions[i] > 0)
                    {
                        traitements.Add(new MedicamentTraitement(medicaments[i].Id, medicaments[i].Nom, medicaments[i].PrixUnitaire, solutions[i]));
                    }
                }
                return traitements;
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

        public Diagnostic DiagnosePerson(Person personne, NpgsqlConnection con)
        {
            bool isNewConnexion = false;
            if (con == null)
            {
                con = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }
            try
            {
                CaracteristiqueCorps temperature = personne.GetTemperature();
                temperature.Echelle = personne.GetEchelleTemperature(con, temperature.Echelle);
                List<CaracteristiqueCorps> symptomes = personne.GetSymptomes();
                List<Maladie> maladies = this.GetMaladies(con, symptomes, personne);
                Diagnostic diagnostic = new Diagnostic();
                diagnostic.Maladies = maladies;
                diagnostic.Medicaments = this.GetMedicaments(con, symptomes);
                return diagnostic;
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
    }
}