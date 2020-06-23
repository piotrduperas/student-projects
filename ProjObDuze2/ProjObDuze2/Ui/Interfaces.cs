//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
//This file Can be modified

using BigTask2.Api;
using System.Collections.Generic;

namespace BigTask2.Ui
{
    interface IForm
    {
        void Insert(string command);
        bool GetBoolValue(string name);
        string GetTextValue(string name);
        int GetNumericValue(string name);
    }

    interface IDisplay
    {
        void Print(IEnumerable<Route> routes);
    }

    interface ISystem
    {
        IForm Form { get; }
        IDisplay Display { get; }
    }

    interface ISystemFactory
    {
        ISystem CreateSystem();
    }
}
