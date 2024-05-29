using Npgsql;

namespace Docteur.Models
{
    public class CaracteristiqueCorps : BDDObject
    {
        int id;
        string nom;
        double echelle;

        public CaracteristiqueCorps()
        {
        }

        public CaracteristiqueCorps(int id, string nom, double echelle)
        {
            Id = id;
            Nom = nom;
            Echelle = echelle;
        }

        public CaracteristiqueCorps(string id, string nom, string echelle)
        {
            Id = Int32.Parse(id);
            Nom = nom;
            EchelleString = echelle;
        }

        [Default]
        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public double Echelle { get => echelle; set => echelle = value; }

        public string EchelleString { set => echelle = Double.Parse(value); }

        public override string tableName()
        {
            return "caracteristiqueCorps";
        }

        public List<Medicament> GetMedicaments(NpgsqlConnection con)
        {
            bool isNewConnexion = false;
            if (con == null)
            {
                con = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }
            try
            {
                string sql = "SELECT * " +
                "FROM vw_medicament_efficacite " +
                "WHERE idmedicament IN( " +
                    "SELECT idmedicament " +
                "FROM vw_medicament_efficacite " +
                "WHERE idcaracteristiquecorps = " + this.Id +
                " AND variationEchelle > 0)";
                Console.WriteLine(sql);
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
                            medicaments[medicaments.Count - 1].AddTraitement(this, reader.GetDouble(reader.GetOrdinal("variationechelle")));
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

        public List<MedicamentTraitement> GetCheaperMedicine(NpgsqlConnection con)
        {
            bool isNewConnexion = false;
            if (con == null)
            {
                con = new Connect().getConnectPostgres();
                isNewConnexion = true;
            }
            try
            {
                List<Medicament> medicaments = this.GetMedicaments(con);
                Console.WriteLine(medicaments.Count);
                Medicament cheaper = medicaments.OrderBy(m => Math.Ceiling(this.CalculateNbrPrises(m)) * m.PrixUnitaire).ToArray()[0];
                MedicamentTraitement mt = new MedicamentTraitement(cheaper.Id, cheaper.Nom, cheaper.PrixUnitaire);
                mt.Traitements = cheaper.Traitements;
                mt.NombrePrise = Math.Ceiling(this.CalculateNbrPrises(mt));
                return null;
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

        public double CalculateNbrPrises(Medicament medicament)
        {
            return this.Echelle / medicament.GetVariation(this);
        }
    }
}