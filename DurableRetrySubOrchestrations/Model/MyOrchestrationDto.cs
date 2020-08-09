using System;
using System.Collections.Generic;
using System.Text;

namespace DurableRetrySubOrchestrations.Model
{
    public class MyOrchestrationDto
    {
        public string InputStartData { get; set; }
        public string MyActivityOneResult { get; set; }
        public string ExternalInputData { get; set; }
        public string MyActivityTwoResult { get; set; }
    }
}
