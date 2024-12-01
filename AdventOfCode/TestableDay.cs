using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public abstract class TestableDay : BaseDay
    {
        protected override string InputFileDirPath => this.GetType().CustomAttributes.Any(n => n.AttributeType == (typeof(RunTestAttribute)))
            ? "TestInputs"
            : "Inputs";
    }
}
