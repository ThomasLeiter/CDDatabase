namespace CDDatabase
{
    /// <summary>
    /// Console base GUI for the CD Database.
    /// Run new ConsoleUI().Main();
    /// </summary>
    internal class ConsoleUI
    {
        CDList list = new CDList();
        FileInfo file = new FileInfo("dummy_file_name");

        public ConsoleUI() { }

        /// <summary>
        /// Main loop of the console program
        /// </summary>
        public void Main()
        {
            Console.WriteLine("[0]: Create new CD List");
            Console.WriteLine("[1]: Open existing CD List");
            Console.WriteLine("[9]: Exit");
            string input = GetUserInput("Please enter a valid selection");
            switch (input)
            {
                case "0":
                    HandleCreateCommand(); break;
                case "1":
                    HandleOpenCommand(); break;
                case "9": return;
                default: break;
            }
            Main();
        }

        private void HandleOpenCommand()
        {
            string name = GetUserInput("Please enter an existing list name:");
            if (OpenList(name))
            {
                EditMenu();
            }
        }

        private void HandleCreateCommand()
        {
            string name = GetUserInput("Please enter a list name:");
            if (CreateList(name))
            {
                EditMenu();
            }
        }

        private void EditMenu()
        {
            Console.WriteLine("[0] Add CD data");
            Console.WriteLine("[1] Delete CD");
            Console.WriteLine("[2] Search");
            Console.WriteLine("[3] List all CDs");
            Console.WriteLine("[4] Sort CD list");
            Console.WriteLine("[8] Discard changes and Exit");
            Console.WriteLine("[9] Save and Exit");


            string input = GetUserInput("Please enter a valid selection");
            switch (input)
            {
                case "0":
                    HandleAddCommand(); break;
                case "1":
                    HandleDeleteCommand(); break;
                case "2":
                    HandleSearchCommand(); break;
                case "3":
                    HandleListCommand(); break;
                case "4":
                    HandleSortCommand(); break;
                case "8": return;
                case "9": HandleSaveCommand(); return;
                default: break;
            }
            EditMenu();
        }

        private void HandleSortCommand()
        {
            list.Sort();
        }

        private void HandleListCommand()
        {
            Console.WriteLine(list.ToString());
        }

        private void HandleSaveCommand()
        {
            SaveList();
        }

        private void HandleAddCommand()
        {
            string interpreter = GetUserInput("Please input the interpreter:");
            string title = GetUserInput("Please input the title:");
            string runtime = GetUserInput("Please input the run time:");
            try
            {
                list.Add(new CD($"{interpreter};{title};{runtime}"));
            }
            catch (CDException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void HandleDeleteCommand()
        {
            string interpreter = GetUserInput("Please input the interpreter:");
            string title = GetUserInput("Please input the title:");
            try
            {
                list.DeleteCD(new CD($"{interpreter};{title};0"));
            }
            catch (CDException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void HandleSearchCommand()
        {
            string interpreter = GetUserInput("Please input the interpreter:");
            string title = GetUserInput("Please input the title:");
            foreach (CD cd in list.Find(interpreter, title))
            {
                Console.WriteLine(cd.ToString());
            }
        }

        private string GetUserInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        private bool FileExists(string path)
        {
            return new FileInfo(path).Exists;
        }

        private bool CreateList(string name)
        {
            if (name.Length == 0)
                return false;
            name = Path.ChangeExtension(name, ".CDB");
            if (FileExists(name))
            {
                Console.Error.WriteLine(string.Format("File {0} already exists", name));
                return false;
            }
            list = new CDList();
            file = new FileInfo(name);
            return true;
        }

        private bool OpenList(string name)
        {
            if (name.Length == 0)
                return false;
            name = Path.ChangeExtension(name, ".CDB");
            if (!FileExists(name))
            {
                Console.Error.WriteLine(string.Format("File {0} doesn't exist", name));
                return false;
            }
            try
            {
                file = new FileInfo(name);
                var streamReader = new StreamReader(file.OpenRead());
                string[] csvData = streamReader.ReadToEnd().Split("\n");
                streamReader.Close();
                list = new CDList(csvData);
                return true;
            }
            catch (IOException e)
            {
                Console.Error.WriteLine($"Something went wrong: {e}");
                return false;
            }
        }

        private void SaveList()
        {
            try
            {
                HandleBackup();
                var streamWriter = new StreamWriter(file.OpenWrite());
                streamWriter.Write(list.ToCSV());
                streamWriter.Close();
                Console.WriteLine($"List saved to {file.ToString()}");
            }
            catch (IOException e)
            {
                Console.Error.WriteLine($"Something went wrong: {e}");
            }
        }

        private void HandleBackup()
        {
            if (!file.Exists)
                return;
            string backup = Path.ChangeExtension(file.FullName, "CDBU");
            var backupFile = new FileInfo(backup);
            if (backupFile.Exists)
                backupFile.Delete();
            file.CopyTo(backup);
        }

    }
}
