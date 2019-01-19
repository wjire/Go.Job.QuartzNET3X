using System;

namespace Go.Job.Model
{

    [Serializable]
    public class Result
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public string Data { get; set; }
    }
}
