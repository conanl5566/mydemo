using System;
using Volo.Abp.BackgroundJobs;

namespace demo3 {
    [BackgroundJobName ("YellowJob")]
    public class WriteToConsoleYellowJobArgs {
        public string Value { get; set; }

        public DateTime Time { get; set; }

        public WriteToConsoleYellowJobArgs () {
            Time = DateTime.Now;
        }
    }
}