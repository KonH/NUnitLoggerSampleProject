# NUnit Logger Sample Project

This repository reproduce issue with [NUnitXml.TestLogger](https://github.com/spekt/nunit.testlogger) package.
It leads to XML output with unknown values for 'name' and 'fullname'.
The issue happens in case of Tuple collection usage in [TestCaseSource] attribute.

## Minimal reproduction setup

Code ([full](Tests.cs)):
```C#
namespace NUnitLoggerSampleProject {
    public class Tests {
        [TestCaseSource(nameof(GetInvalidTestCases))]
        public void InvalidCase((int, int) argument) {
            Assert.Pass();
        }

        static IEnumerable<(int, int)> GetInvalidTestCases() {
            return new[] {
                (0, 1)
            };
        }
    }
}
```

Execution ([script](test.sh)):
```Shell
dotnet test --logger:"nunit;LogFilePath=test-result.xml"
```

## Actual result

Result file contains:
```Xml
<test-suite type="TestSuite" name="UnknownNamespace" fullname="UnknownNamespace" total="1" passed="1" failed="0" inconclusive="0" skipped="0" result="Passed" duration="0.031035">
    <test-suite type="TestFixture" name="UnknownType" fullname="UnknownNamespace.UnknownType" total="1" passed="1" failed="0" inconclusive="0" skipped="0" result="Passed" duration="0.031035">
        <test-case name="InvalidCase((0, 1))" fullname="UnknownNamespace.UnknownType.NUnitLoggerSampleProject.Tests.InvalidCase((0, 1))" methodname="NUnitLoggerSampleProject.Tests.InvalidCase((0, 1))" classname="UnknownType" result="Passed" duration="0.031035" asserts="0" />
    </test-suite>
</test-suite>
```

Console output:
```
nunitXml Logger: Unable to parse the test name 'NUnitLoggerSampleProject.Tests.InvalidCase((0, 1))' into a namespace type and method. Using Namespace='UnknownNamespace', Type='UnknownType' and Method='NUnitLoggerSampleProject.Tests.InvalidCase((0, 1))'
```

## Expected result

No additional output in console and result file should contains:
```Xml
<test-suite type="TestSuite" name="NUnitLoggerSampleProject" fullname="NUnitLoggerSampleProject" total="1" passed="1" failed="0" inconclusive="0" skipped="0" result="Passed" duration="0.00062">
    <test-suite type="TestFixture" name="Tests" fullname="NUnitLoggerSampleProject.Tests" total="1" passed="1" failed="0" inconclusive="0" skipped="0" result="Passed" duration="0.00062">
        <test-case name="InvalidCase((0,1))" fullname="NUnitLoggerSampleProject.Tests.InvalidCase((0,1))" methodname="ValidCase((0,1))" classname="Tests" result="Passed" duration="0.00062" asserts="0" />
    </test-suite>
</test-suite>
```