using System;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace Smart_health_desktop_solution_WPF
{
    internal class MyDictionary : Dictionary<string, MyValue>
    {
        public void Add(string key, object value1, string value2)
        {
            MyValue val;
            val.EntryValue = value1;
            val.EntryType = value2;
            this.Add(key, val);
        }

        private struct MyValue
        {
            public object EntryValue;
            public double EntryType;
        }
    }

}
