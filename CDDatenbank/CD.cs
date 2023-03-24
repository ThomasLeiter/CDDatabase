namespace CDDatabase
{
    internal class CD : IComparable<CD>
    {
        public string Interpreter { get; set; }
        public string Title { get; set; }
        public int RunTime { get; set; }

        public CD(string interpreter, string title, int runTime)
        {
            Interpreter = interpreter;
            Title = title;
            RunTime = runTime;
        }

        public CD(string csv)
        {
            try
            {
                string[] items = csv.Split(";");
                Interpreter = items[0];
                Title = items[1];
                RunTime = int.Parse(items[2]);
            }
            catch (Exception e)
            {
                throw new CDException($"Cannot parse string {csv}");             
            }
        }

        public int CompareTo(CD? other)
        {
            if (null == other)
            {
                return -1;
            }
            int cmp = Interpreter.CompareTo(other.Interpreter);
            return (cmp != 0) ? cmp : Title.CompareTo(other.Title);
        }

        /// <summary>
        /// Build a semicolon separated CSV-Style representation of the CD object
        /// </summary>
        /// <returns>String representation of the CD object.</returns>
        public override string ToString()
        {
            return $"{Interpreter}:\t{Title}\t({RunTime})";
        }

        public string ToCSV()
        {
            return $"{Interpreter};{Title};{RunTime}";
        }


        public override bool Equals(object? obj)
        {
            var otherCd = obj as CD;
            if (null == otherCd)
                return false;
            return Interpreter.Equals(otherCd.Interpreter) && Title.Equals(otherCd.Title);
        }

        public override int GetHashCode()
        {
            return Interpreter.GetHashCode() + Title.GetHashCode();
        }

    }
}
