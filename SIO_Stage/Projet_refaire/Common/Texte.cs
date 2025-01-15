using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Projet_refaire.Common
{
    public static class Texte
    {
        public static bool IsNumeric(this string text)
        {
            return Regex.IsMatch(text, "^[0-9]*$"); // Accepte les chaines vides ou contenant uniquement que des chiffres
        }
    }
}