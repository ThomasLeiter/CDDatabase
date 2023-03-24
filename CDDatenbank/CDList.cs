using System.Text;

namespace CDDatabase
{
    internal class CDList
    {
        private List<CD> list = new List<CD>();

        /// <summary>
        /// Create a new, empty CDList object
        /// </summary>
        public CDList() { }

        /// <summary>
        /// Read a CD list from semicolon separated csv-data.
        /// </summary>
        /// <param name="csvData"></param>
        public CDList(string[] csvData)
        {
            foreach (string line in csvData)
            {
                if (line.Length == 0) continue;
                try
                {
                    Add(new CD(line));
                }
                catch (CDException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Get a sublist of all CDs matching interpreter and/or title.
        /// Nulled values and empty strings will be ignored.
        /// e.g. Find(null,"Love") or Find("", "Love") will find all CDs titled "Love".
        /// e.g. Find("No","Love") will find all CDs by interpreter "No" titled "Love".
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="title"></param>
        /// <returns>The selected CD sublist</returns>
        public List<CD> Find(string? interpreter, string? title)
        {
            IEnumerable<CD> selection = list;
            if (interpreter != null && interpreter.Length > 0)
                selection = selection.Where(cd => cd.Interpreter.Equals(interpreter));
            if (title != null && title.Length > 0)
                selection = selection.Where(cd => cd.Title.Equals(title));
            return selection.ToList();
        }

        public List<CD> FindTitle(string title)
        {
            return Find(null, title);
        }

        public List<CD> FindInterpreter(string interpreter)
        {
            return Find(interpreter, null);
        }

        public void Add(CD cd)
        {
            if (list.Contains(cd))
            {
                throw new CDException($"CD {cd.ToString()} already exists.");
            }
            list.Add(cd);
        }

        public void DeleteCD(CD cd)
        {
            list.Remove(cd);
        }

        public void Sort()
        {
            list.Sort();
        }

        public CD First()
        {
            return list[0];
        }

        public CD Next(CD cd)
        {
            int idx = list.IndexOf(cd);
            if (idx < 0 || idx == list.Count() - 1)
                return First();
            return list[idx + 1];
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            list.ForEach(cd => builder.AppendLine(cd.ToString()));
            return builder.ToString();
        }

        public string ToCSV()
        {
            StringBuilder builder = new StringBuilder();
            list.ForEach(cd => builder.AppendLine(cd.ToCSV()));
            return builder.ToString();
        }
    }
}
