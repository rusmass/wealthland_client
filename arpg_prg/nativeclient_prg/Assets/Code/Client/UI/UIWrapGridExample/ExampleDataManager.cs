using System;
using System.Collections;
using System.Collections.Generic;

namespace DataManager
{
    public class ExampleDataManager
    {
        protected ExampleDataManager()
        {
            _textValues = new List<string>();

            for (int i = 0; i < 10; ++i)
            {
                var value = _textPrefix + i;
                _textValues.Add(value);
            }
        }

        private List<string> _textValues;
        private const string _textPrefix = "I am test wrapgrid id = ";

        public List<string> TextValues { get { return _textValues; } }
        public static readonly ExampleDataManager Instance = new ExampleDataManager();
    }
}
