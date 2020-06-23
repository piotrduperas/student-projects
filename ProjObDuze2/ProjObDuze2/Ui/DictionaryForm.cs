//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System.Collections.Generic;

namespace BigTask2.Ui
{
    abstract class DictionaryForm : IForm
    {
        protected Dictionary<string, string> entries = new Dictionary<string, string>();
        public bool GetBoolValue(string name)
        {
            return bool.Parse(entries.GetValueOrDefault(name));
        }

        public int GetNumericValue(string name)
        {
            return int.Parse(entries.GetValueOrDefault(name));
        }

        public string GetTextValue(string name)
        {
            return entries.GetValueOrDefault(name);
        }

        abstract public void Insert(string command);
    }
}
