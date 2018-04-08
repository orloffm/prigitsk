namespace Prigitsk.Console.Verbs.Draw
{
    public class DrawRunnerOptions : IDrawRunnerOptions
    {
        public DrawRunnerOptions(
            string repository ,
            string target ,
            string output ,
            string format ,
            string remoteToUse ,
            bool forceTreatAsGitHub ,
            bool leaveTails )
        {
            Repository = repository;
            TargetDirectory = target;
            OutputFileName = output;
            Format = format;
            RemoteToUse = remoteToUse;
            ForceTreatAsGitHub = forceTreatAsGitHub;
            LeaveTails = leaveTails;
        }

        public string Format { get; }

        public string OutputFileName { get; }

        public string RemoteToUse { get; }

        public string Repository { get; }

        public string TargetDirectory { get; }

        public bool ForceTreatAsGitHub { get; }

        public bool LeaveTails { get; }
    }
}