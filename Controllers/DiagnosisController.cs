using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docteur.Models;
using Microsoft.AspNetCore.Mvc;

namespace Docteur.Controllers
{
    public class DiagnosisController : Controller
    {
        public IActionResult Index(string[] idParameter, string[] parameterValue, int age)
        {
            Person p = new Person();
            p.Age = age;
            p.AddParameter(idParameter, parameterValue);
            Doctor d = new Doctor();
            Diagnostic diagnostic = d.DiagnosePerson(p, null);
            return View(diagnostic);
        }
    }
}