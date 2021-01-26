using System;

namespace scheduler_auth.api.Helpers
{
    public class Randomizer
    {
        public Randomizer()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static int RandomNumber(int LowerBound, int UpperBound)
        {
            byte[] guid = Guid.NewGuid().ToByteArray();
            int seed = guid[0] + (guid[1] << 8) +
                (guid[2] << 16) + (guid[3] << 24);
            Random rand = new Random(seed);
            return System.Convert.ToInt32(((UpperBound - LowerBound) * rand.NextDouble()) + LowerBound);
        }

        public static string RandomNumberOfDigits(int NumDigits)
        {
            string RandString = "";
            int NewRandom;
            for (int i = 0; i < NumDigits; i++)
            {
                NewRandom = RandomNumber(0, 9);
                RandString += NewRandom.ToString();
            }
            return RandString;
        }

        public static string RandomLetter()
        {
            string chrRand;
            int intRand = RandomNumber(2, 52);
            switch (intRand)
            {
                case 8:
                    chrRand = "A";
                    break;
                case 10:
                    chrRand = "C";
                    break;
                case 11:
                    chrRand = "D";
                    break;
                case 12:
                    chrRand = "E";
                    break;
                case 13:
                    chrRand = "F";
                    break;
                case 14:
                    chrRand = "G";
                    break;
                case 15:
                    chrRand = "H";
                    break;
                case 16:
                    chrRand = "J";
                    break;
                case 17:
                    chrRand = "K";
                    break;
                case 18:
                    chrRand = "L";
                    break;
                case 19:
                    chrRand = "M";
                    break;
                case 20:
                    chrRand = "N";
                    break;
                case 21:
                    chrRand = "P";
                    break;
                case 22:
                    chrRand = "Q";
                    break;
                case 23:
                    chrRand = "R";
                    break;
                case 24:
                    chrRand = "S";
                    break;
                case 25:
                    chrRand = "T";
                    break;
                case 26:
                    chrRand = "U";
                    break;
                case 27:
                    chrRand = "V";
                    break;
                case 28:
                    chrRand = "W";
                    break;
                case 29:
                    chrRand = "X";
                    break;
                case 30:
                    chrRand = "Y";
                    break;
                case 31:
                    chrRand = "Z";
                    break;
                case 32:
                    chrRand = "a";
                    break;
                case 33:
                    chrRand = "d";
                    break;
                case 34:
                    chrRand = "e";
                    break;
                case 35:
                    chrRand = "f";
                    break;
                case 36:
                    chrRand = "g";
                    break;
                case 37:
                    chrRand = "h";
                    break;
                case 38:
                    chrRand = "j";
                    break;
                case 39:
                    chrRand = "k";
                    break;
                case 40:
                    chrRand = "m";
                    break;
                case 41:
                    chrRand = "n";
                    break;
                case 42:
                    chrRand = "p";
                    break;
                case 43:
                    chrRand = "q";
                    break;
                case 44:
                    chrRand = "r";
                    break;
                case 45:
                    chrRand = "s";
                    break;
                case 46:
                    chrRand = "t";
                    break;
                case 47:
                    chrRand = "u";
                    break;
                case 48:
                    chrRand = "v";
                    break;
                case 49:
                    chrRand = "w";
                    break;
                case 50:
                    chrRand = "x";
                    break;
                case 51:
                    chrRand = "y";
                    break;
                case 52:
                    chrRand = "z";
                    break;
                default:
                    chrRand = intRand.ToString();
                    break;
            }
            return chrRand;
        }
        public static string RandomLetterAll()
        {
            string chrRand;
            int intRand = RandomNumber(0, 61);
            switch (intRand)
            {
                case 10:
                    chrRand = "A";
                    break;
                case 11:
                    chrRand = "B";
                    break;
                case 12:
                    chrRand = "C";
                    break;
                case 13:
                    chrRand = "D";
                    break;
                case 14:
                    chrRand = "E";
                    break;
                case 15:
                    chrRand = "F";
                    break;
                case 16:
                    chrRand = "G";
                    break;
                case 17:
                    chrRand = "H";
                    break;
                case 18:
                    chrRand = "I";
                    break;
                case 19:
                    chrRand = "J";
                    break;
                case 20:
                    chrRand = "K";
                    break;
                case 21:
                    chrRand = "L";
                    break;
                case 22:
                    chrRand = "M";
                    break;
                case 23:
                    chrRand = "N";
                    break;
                case 24:
                    chrRand = "O";
                    break;
                case 25:
                    chrRand = "P";
                    break;
                case 26:
                    chrRand = "Q";
                    break;
                case 27:
                    chrRand = "R";
                    break;
                case 28:
                    chrRand = "S";
                    break;
                case 29:
                    chrRand = "T";
                    break;
                case 30:
                    chrRand = "U";
                    break;
                case 31:
                    chrRand = "V";
                    break;
                case 32:
                    chrRand = "W";
                    break;
                case 33:
                    chrRand = "X";
                    break;
                case 34:
                    chrRand = "Y";
                    break;
                case 35:
                    chrRand = "Z";
                    break;
                case 36:
                    chrRand = "a";
                    break;
                case 37:
                    chrRand = "b";
                    break;
                case 38:
                    chrRand = "c";
                    break;
                case 39:
                    chrRand = "d";
                    break;
                case 40:
                    chrRand = "e";
                    break;
                case 41:
                    chrRand = "f";
                    break;
                case 42:
                    chrRand = "g";
                    break;
                case 43:
                    chrRand = "h";
                    break;
                case 44:
                    chrRand = "i";
                    break;
                case 45:
                    chrRand = "j";
                    break;
                case 46:
                    chrRand = "k";
                    break;
                case 47:
                    chrRand = "l";
                    break;
                case 48:
                    chrRand = "m";
                    break;
                case 49:
                    chrRand = "n";
                    break;
                case 50:
                    chrRand = "o";
                    break;
                case 51:
                    chrRand = "p";
                    break;
                case 52:
                    chrRand = "q";
                    break;
                case 53:
                    chrRand = "r";
                    break;
                case 54:
                    chrRand = "s";
                    break;
                case 55:
                    chrRand = "t";
                    break;
                case 56:
                    chrRand = "u";
                    break;
                case 57:
                    chrRand = "v";
                    break;
                case 58:
                    chrRand = "w";
                    break;
                case 59:
                    chrRand = "x";
                    break;
                case 60:
                    chrRand = "y";
                    break;
                case 61:
                    chrRand = "z";
                    break;
                default:
                    chrRand = intRand.ToString();
                    break;
            }
            return chrRand;
        }
        public static string RandomASCIICharacter()
        {
            return ((char)RandomNumber(32, 126)).ToString();
        }

        public static string RandomNumberOfASCIICharacters(int numASCII)
        {
            string strRand = "";
            for (int i = 0; i < numASCII; i++)
            {
                strRand += RandomASCIICharacter();
            }
            return strRand;
        }
        public static string RandomNumberOfLetters(int NumLetters)
        {
            string strRand = "";
            for (int i = 0; i < NumLetters; i++)
            {
                strRand += RandomLetter();
            }
            return strRand;
        }
        public static string RandomNumberOfLettersAll(int NumLetters)
        {
            string strRand = "";
            for (int i = 0; i < NumLetters; i++)
            {
                strRand += RandomLetterAll();
            }
            return strRand;
        }
    }
}
