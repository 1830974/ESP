using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entree_1830974.Ressources
{
    public static class LicensePlateGenerator
    {
        private static readonly Random random = new Random();

        public static string Generate()
        {
            Random random = new Random();
            string validFirstLetters = "EGHIJKMNPUWXYZ";
            string validLetters = "ABCDEFGHJKLMNPRSTUVWXYZ";

            char firstLetter = validFirstLetters[random.Next(validFirstLetters.Length)];
            int numbers = random.Next(100);
            string lastLetters = new string(Enumerable.Repeat(validLetters, 3)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"{firstLetter}{numbers:D2} {lastLetters}";
        }
    }
}
