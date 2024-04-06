using static System.Console;
/******************************************************************************
3. Implement a console application for text analysis. The application must have the following functionality:

 a) calculate the number of characters, words, sentences in the entered text;

 b) calculate the number of occurrences of each character;

 c) calculate the average length of words in the text;

 d) determine the position of the symbol in the text;

 e) determine the occurrence of a sentence in the text;

 f) defines and outputs palindrome words in the text.

All operations must be implemented as methods.
*******************************************************************************/


class Files
{
    private static string path = null;// путь к файлу будет храниться тут

    private static string tryAgainText = ". Попробуйте еще раз";
    private static string[] rules = {
        "Во время любого ввода вы можете ввести число \"0\" и программа завершится",
        "Во время любого ввода вы можете ввести слово \"п\" и будут показаны правила программы",
        "При завершении ввода данных нажимайте клавишу Enter",
        "При сообщении \"Введите: да/нет\" если вы согласны, введите слово \"да\", иначе введите любое другое слово или символ",
        "Программа не может хранить более 1-ого текста, имейте это ввиду"
    };

    // other
    public static void ExitMenuRules(string text)
    {
        if (text == "0")
        {
            Environment.Exit(0);
        }
        else if (text == "м")
        {
            Menu();
        }
        else if (text == "п")
        {
            TitleMsg("\nПРАВИЛА\n");
            foreach (string i in rules) // вывести все правила в консоль
            {
                WriteLine("\t" + i);
            }
            Menu();
        }
    }

    public static void ErrorMsg(string err)
    {
        ForegroundColor = ConsoleColor.Red;
        Write(err);
        ForegroundColor = ConsoleColor.Gray;
    }

    public static void ResultMsg(string text)
    {
        ForegroundColor = ConsoleColor.Green;
        Write(text);
        ForegroundColor = ConsoleColor.Gray;
    }

    public static void TitleMsg(string text)
    {
        ForegroundColor = ConsoleColor.Blue;
        Write(text);
        ForegroundColor = ConsoleColor.Gray;
    }

    private static int GetOption()
    {
        string str = ReadLine();

        ExitMenuRules(str);

        int num = 0;
        try
        {
            num = Convert.ToInt32(str);
        }
        catch (Exception ex) // проверка ввода (если не число, то вывести ошибку и попросить ввести данные снова
        {
            if (ex is FormatException)
            {
                ErrorMsg("Введено не число" + tryAgainText + "\n");
                Menu();
            }
        }

        if (num < 0) // проверка ввода (если число не положительное, то вывести ошибку и попросить ввести данные снова
        {
            ErrorMsg("Число должно быть положительным" + tryAgainText + "\n");
            Menu();
        }

        return num;
    }

    private static void FileExistanceCheck(string path)
    {
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)// обработка исключения, когда пользователь вводит путь снова, но не закончив ввод переходит в меню с помощью кодового слова "м"
        {
            ErrorMsg("Путь к файлу был утрачен. Введите путь снова");
        }
        else
        {
            ErrorMsg("Файл пустой");
        }
    }

    // 1
    private static void UserDialogWrap1()
    {
        WriteLine("Введите путь к файлу: ");
        string path = GetPathFromConsole();

        if (path == null || path == "")
        {
            ErrorMsg("Путь не может быть пустым!" + tryAgainText);
            UserDialogWrap1();
        }
        else
        {
            FileInfo fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                ErrorMsg("Такого пути к файлу нет" + tryAgainText);
                UserDialogWrap1();
            }
            else
            {
                ResultMsg("Путь принят!");
            }
        }
    }

    private static string GetPathFromConsole()
    {
        path = ReadLine();
        ExitMenuRules(path);
        return path;
    }

    // 2
    private static void UserDialogWrap2()// ввод/вывод отдельно от функций по выполнению задач
    {
        ResultMsg("Сведения о файле: \n");
        string[] info = GetFileInfo();
        
        if (info != null)
        {
            foreach (string i in info)
            {
                WriteLine(i);
            }
        }
        else
        {
            FileExistanceCheck(path);
        }
    }

    // чистые функции (почти)
    private static string[] GetFileInfo()
    {
        string[] info = { "пусто", "пусто", "пусто", "пусто" };//информация о файле будет храниться тут

        FileInfo fileInfo = new FileInfo(path);
        string[] lines = GetLines(path);

        if (lines != null)
        {
            info[0] = "\tРазмер файла: " + fileInfo.Length;// размер файла

            int count = 0, count2 = 0, wordCount = 0, sentenceCount = 0;
            foreach (string line in lines)// кол-во символов во всем файле
            {
                count++;// кол-во строк
                wordCount += GetNumberOfWords(line);// кол-во слов в строке (и цикл foreach поможет сделать это для всех строк файла)
                sentenceCount += GetNumberOfSentences(line);// кол-во слов в строке (и цикл foreach поможет сделать это для всех строк файла)
                for (int i = 0; i < line.Length; i++)// кол-во символов в строке
                {
                    count2++;
                }
            }
            info[1] = "\tКол-во символов: " + count2;

            info[2] = "\tКол-во слов: " + wordCount;
            info[3] = "\tКол-во предложений: " + sentenceCount;
        }
        else
        {
            info = null;
        }

        return info;
    }

    private static string[] GetLines(string path)
    {
        try
        {
            return File.ReadAllLines(path);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    private static int GetNumberOfWords(string str)
    {
        int count = 0; // создаем счетчик, задаем ему значение 0
        if (str.Length > 0)
        {
            count = 1; // задаем счетчику значение 1, если длина текста/строки больше 0 (т.е. если есть хотя бы 1 символ, значит есть 1 слово)
        }

        for (int i = 0; i < str.Length; i++) // задействуем все элементы/символы строки
        {
            if (str[i] == 32)
            {
                count++;  // увеличиваем показание счетчика на один при каждом нахождении пробела
            }
        }
        return count;
    }

    private static int GetNumberOfSentences(string str)
    {
        int count = 0; // создаем счетчик, задаем ему значение 0

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == 33 || str[i] == 46 || str[i] == 63 || str[i] == 59)
            {
                count++; // увеличиваем показание счетчика на один при каждом нахождении символов '.' '!' '?' ';'
            }
        }
        return count;
    }

    // 3
    private static void UserDialogWrap3()
    {
        Write("Введите номер строки: ");
        string[] lines = GetLines(path);

        int option = GetOption() - 1;

        if (lines != null)
        {
            if (option >= lines.Length)
            {
                ErrorMsg("Введенный вами номер строки превышает кол-во строк в файле / такой строки в файле не существует\n");
                Menu();
            }
            else
            {
                ResultMsg($"\nСтрока под номером {option+1}:\n");
                WriteLine(lines[option]);
            }
        }
        else
        {
            FileExistanceCheck(path);
        }
    }

    // 4
    private static void UserDialogWrap4()
    {
        Write("Введите номер предложения: ");
        string[] sentences = a();

        int option = GetOption() - 1;

        if (sentences != null)
        {
            if (option >= sentences.Length - 1)
            {
                ErrorMsg("Введенный вами номер предложения превышает кол-во предложений в файле / такого предложения в файле не существует\n");
                Menu();
            }
            else
            {
                ResultMsg($"\nСтрока под номером {option + 1}:\n");
                WriteLine(sentences[option]);
            }
        }
        else
        {
            FileExistanceCheck(path);
        }
    }

    private static string[] a()
    {
        string[] str = File.ReadAllLines(path); // получить массив строк

        string strNew = ""; // создать переменную строка

        foreach (string line in str) // перевести массив строк в одну строку
        {
            strNew += line; // добавить строку из массива к новой переменной
        }

        string[] newNewStr = strNew.Split(new string[] { ".", "!", "?", ";" }, StringSplitOptions.None); // 1. создать новый массив строк 2. разделить старую строку на предложения (т.е. на те части, у которых последний знак это точка или другой знак конца предложения. Options.None - никаких доп.опций  

        return newNewStr;
    }

    public static void Menu()
    {
        string[] exercises =
        {
            "Добавить/Сменить путь к файлу", // 1
            "Вывести статистику файла:" +
            "\n\t\t- размер файла" +
            "\n\t\t- кол-во символов" +
            "\n\t\t- кол-во предложений" +
            "\n\t\t- кол-во слов" +
            "\n\t\t- кол-во строк", // 2
            "Вывести строку по номеру", // 3
            "Вывести предложение по номеру" // 4
        };


        int option = 1; // выбор пользователя

        while (option != 0)
        {
            TitleMsg("\n\nГЛАВНОЕ МЕНЮ");
            WriteLine("\nВыберите опцию:");
            WriteLine("\t0 - Завершить программу");
            for (int i = 0; i < exercises.Length; i++)
            {
                WriteLine($"\t{i + 1} - {exercises[i]}");
            }
            Write("Ваш выбор: ");
            option = GetOption(); // выбор пользователя меняется и теперь от него зависят дальнейшие действия программы

            if (option != 0)
            { // если выбрана опция 0, то код просто заканчивается
                if (option > 1 && option < 5 && path == null) // если выбрана опция 2-9 (т.е. опция для работы с текстом) и текста нет, то выводится предупреждение и происходит возврат в меню
                {
                    ErrorMsg("Нет файла для работы. Возможно вы хотели добавить новый путь к файлу?\n");
                    Menu();
                }
                else
                {
                    switch (option)
                    {
                        case 1:
                            UserDialogWrap1();
                            break;

                        case 2:
                            UserDialogWrap2();
                            break;

                        case 3:
                            UserDialogWrap3();
                            break;

                        case 4:
                            UserDialogWrap4();
                            break;

                        default:
                            ErrorMsg("Такой опции нет" + tryAgainText + "\n");
                            break;
                    }
                }
            }
        }
    }

    static void Main()
    {
        WriteLine("Практикум по программированию. Практическая 2, задание 1. Работа с файлами");
        ExitMenuRules("п");
        Menu();
    }
}