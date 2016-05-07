﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Metropolis.Api.Core.Domain;
using Metropolis.Api.Extensions;

namespace Metropolis.Api.Core.Parsers.XmlParsers.MetricHandlers
{
    public class ClassAttributeParser : IJavaMetricParser
    {
        public int Order => 3;
        public string Id => "NOF";

        public void Parse(XElement metric, Dictionary<string, Class> classMap, XNamespace nameSpace)
        {
            metric.Descendants(nameSpace + "Values")
                  .Descendants(nameSpace + "Value")
                  .ForEach(each =>
                  {
                      var className = each.AttributeValue("source").Replace(".java", "").Replace(".java", "");
                      var numberOfFields = each.AttributeValue("value").AsInt();

                      classMap.DoWhenItemFound(className, item => item.LinesOfCode = item.Members.Sum(x => x.LinesOfCode) + numberOfFields);
                  });
        }
    }
}