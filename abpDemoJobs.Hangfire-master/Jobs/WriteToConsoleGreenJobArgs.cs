using System;
using Volo.Abp.BackgroundJobs;

namespace demo3 {
    [BackgroundJobName ("GreenJob")]
    public class WriteToConsoleGreenJobArgs {
        public string Value { get; set; }

        public DateTime Time { get; set; }

        public WriteToConsoleGreenJobArgs () {
            Time = DateTime.Now;
        }
    }
}