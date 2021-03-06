using Metropolis.Api.Collection.PowerShell;
using Metropolis.Common.Extensions;
using Metropolis.Common.Models;

namespace Metropolis.Api.Collection.Steps.Java
{
    /// <summary>
    ///     Java Checkstyle parser automation
    ///     TODO: check if Java is installed or not
    ///     For more info checkout: http://checkstyle.sourceforge.net/cmdline.html#Usage_by_Classpath_update
    /// </summary>
    public class PuppyCrawlerCheckstyleCollectionStep : BaseCollectionStep
    {
        public const string CheckstyleCommand = @"java -cp '{0}' com.puppycrawl.tools.checkstyle.Main -c '{1}' -f xml -o '{2}' '{3}'";

        public PuppyCrawlerCheckstyleCollectionStep() : this(new RunPowerShell())
        {
        }

        public PuppyCrawlerCheckstyleCollectionStep(IRunPowerShell powerShell) : base(powerShell)
        {
        }

        public override string MetricsType => "Java Checkstyle";
        public override string Extension => ".xml";
        public override ParseType ParseType => ParseType.PuppyCrawler;

        public override string PrepareCommand(MetricsCommandArguments args, MetricsResult result)
        {
            var cmd = CheckstyleCommand.FormatWith(
                LocateBinaries("checkstyle-6.18-all.jar"), // include all jars into the class path
                LocateSettings("metropolis_checkstyle_metrics.xml"), // metropolis collection settings for checkstyle
                result.MetricsFile, // output xml file
                args.SourceDirectory // source directory to scan
                );

            return cmd;
        }

        public override string ValidateMetricResults(string fileNametoValidate)
        {
            //TODO: validate checkstyle output somehow...this usually works
            return string.Empty;
        }
    }
}