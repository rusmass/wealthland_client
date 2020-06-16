using System;
using System.Collections;
using System.Collections.Generic;

namespace Metadata
{
    public class TemplateTable : Dictionary<int, Template>
    {
        public TemplateTable()
        {

        }

        public TemplateTable(int capacity) : base(capacity)
        {

        }
    }
}