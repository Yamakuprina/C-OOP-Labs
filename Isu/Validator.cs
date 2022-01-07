namespace Isu
{
    public class Validator
    {
        public static bool IsGroupNameCorrect(string name)
        {
            int courseAndNumber = int.Parse(name[2..]);
            return name[0] == 'M' && name[1] == '3' && courseAndNumber > 100 && courseAndNumber < 500;
        }
    }
}