using System;
using Google.OrTools.LinearSolver;
using Newtonsoft.Json;

namespace Docteur.Models
{
    public class ProgrammationLineaire
    {
        public double[] FindBestCombinaisonMedicament(List<Medicament> medicaments, List<CaracteristiqueCorps> parametres, List<CaracteristiqueCorps> symptomes)
        {
            using var solver = new Solver("LinearProgrammingExample", Solver.OptimizationProblemType.CBC_MIXED_INTEGER_PROGRAMMING);

            // Variables de décision
            // var x = solver.MakeIntVar(0, int.MaxValue, "x");
            // var y = solver.MakeIntVar(0, int.MaxValue, "y");
            var variables = this.GetDecisionVariable(medicaments.Count, solver);
            //Console.WriteLine(variables.Length);
            //Console.WriteLine(JsonConvert.SerializeObject(variables, Formatting.Indented));

            // Fonction objectif à minimiser
            var objective = solver.Objective();
            // objective.SetCoefficient(x, 1000);
            // objective.SetCoefficient(y, 500);
            this.SetObjectiveFunction(objective, medicaments, variables);
            //Console.WriteLine(JsonConvert.SerializeObject(objective, Formatting.Indented));
            objective.SetMinimization();

            // Contraintes
            // solver.Add(2 * x + y >= 5);
            // solver.Add(3 * x + 2 * y >= 3);
            this.AddConstraints(solver, parametres, medicaments, variables, symptomes);

            // Résolution du problème
            var resultStatus = solver.Solve();

            // Affichage des résultats
            if (resultStatus == Solver.ResultStatus.OPTIMAL)
            {
                Console.WriteLine("Optimal solution:");
                double[] optimal = new double[variables.Length];
                for (int i = 0; i < variables.Length; i++)
                {
                    optimal[i] = variables[i].SolutionValue();
                    Console.WriteLine($"x{i} = {variables[i].SolutionValue()}");
                }
                // Console.WriteLine($"x = {x.SolutionValue()}");
                // Console.WriteLine($"y = {y.SolutionValue()}");
                Console.WriteLine($"Optimal Value: {objective.Value()}");
                return optimal;
            }
            else
            {
                Console.WriteLine("No optimal solution found.");
                return null;
            }
        }

        public void AddConstraints(Solver solver, List<CaracteristiqueCorps> parametres, List<Medicament> medicaments, Variable[] variables, List<CaracteristiqueCorps> symptomes)
        {
            for (int i = 0; i < parametres.Count; i++)
            {
                double echelle = 0;
                foreach (var s in symptomes)
                {
                    if (parametres[i].Id == s.Id)
                    {
                        echelle = s.Echelle;
                    }
                }
                var constraint = solver.MakeConstraint(echelle, double.PositiveInfinity);
                for (int n = 0; n < medicaments.Count; n++)
                {
                    constraint.SetCoefficient(variables[n], medicaments[n].GetVariation(parametres[i]));
                }
                //solver.Add(constraint);
            }
        }

        public Variable[] GetDecisionVariable(int numVariables, Solver solver)
        {
            var variables = new Variable[numVariables];
            for (int i = 0; i < numVariables; i++)
            {
                variables[i] = solver.MakeIntVar(0, int.MaxValue, $"x{i}");
            }
            return variables;
        }

        public void SetObjectiveFunction(Objective objective, List<Medicament> medicaments, Variable[] decisions)
        {
            for (int i = 0; i < medicaments.Count; i++)
            {
                objective.SetCoefficient(decisions[i], medicaments[i].PrixUnitaire);
            }
        }
    }
}