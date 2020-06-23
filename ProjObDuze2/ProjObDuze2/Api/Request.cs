//This file should not be modified

using BigTask2.Interfaces;

namespace BigTask2.Api
{
    class Request
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Solver { get; set; }
        public string Problem { get; set; }
        public Filter Filter { get; set; }
    }
}
