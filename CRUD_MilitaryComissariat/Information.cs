using static System.Console;
using static System.Convert;
using System.Security;
class Information
{
    public static void Info()
    {
        Checkings ch = new();
        Write("Введите путь файла для сохранения информации: ");
        string path = ReadLine()!;
        if (File.Exists(path))
        {
            Write("Этот путь уже занят другим файлом!\n");
            Environment.Exit(0);
        }
        FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);
        StreamWriter swmain = new StreamWriter(fs, System.Text.Encoding.UTF8);
        while (true)
        {

            Write("Введите количество солдат: ");
            int n = ToInt32(ReadLine());
            if (n < 1)
            {
                Write("Некорректное количество солдат!\n");
                continue;
            }
            Dictionary<int, Soldier> SList = new(n);
            try
            {
                WriteLine("Солдаты:\n\n");
                WriteLine("----------------------------------\n");
                swmain.WriteLine("Солдаты\n");
                swmain.WriteLine("----------------------------------\n");
                for (int i = 0; i < n; i++)
                {
                    int Number = i;

                    Write($"\nСолдат {Number + 1}: \n");
                    Write("Введите ФИО солдата: ");
                    string FIO = ReadLine()!;
                    if (!ch.FIO(FIO))
                    {
                        throw new MyException("Неправильно введена фамилия!");
                    }
                    Write("Введите его категорию годности: ");
                    char VC = ToChar(ReadLine()!);
                    if (ch.ValidityCategory(VC))
                    {
                        throw new MyException("Неправильно введена категория годности!");
                    }
                    Write($"Введите род войск: ");
                    string MB = ReadLine()!;
                    if (!ch.MilitaryBranch(MB))
                    {
                        throw new MyException("Неправильно введен род войск!");
                    }
                    Write("Введите его адрес проживания: ");
                    string Address = ReadLine()!;
                    if (!ch.Address(Address))
                    {
                        throw new MyException("Неправильно введен адрес!");
                    }
                    Write("Введите номер его паспорта (4 цифры): ");
                    string PassportNumber = ReadLine()!;
                    if (!ch.PassportNumber(PassportNumber))
                    {
                        throw new MyException("Неправильно введен номер паспорта!");
                    }
                    Write("Введите серию его паспорта (6 цифр): ");
                    string PassportSeries = ReadLine()!;
                    if (!ch.PassportSeries(PassportSeries))
                    {
                        throw new MyException("Неправильно введена серия паспорта!");
                    }
                    Soldier soldier = new(FIO, VC,
                        MB, Address, PassportNumber, PassportSeries);
                    SList.Add(Number, soldier);
                    swmain.WriteLine($"Солдат {Number}:\n");
                    swmain.WriteLine($"ФИО: {FIO}\n");
                    swmain.WriteLine($"Категория годности: {VC}\n");
                    swmain.WriteLine($"Род войск: {MB}\n");
                    swmain.WriteLine($"Адрес проживания: {Address}\n");
                    swmain.WriteLine($"Номер паспорта: {PassportNumber}\n");
                    swmain.WriteLine($"Серия паспорта: {PassportSeries}\n");
                    swmain.WriteLine("----------------------------------\n");
                }
                Selection(SList, swmain);
                break;
            }
            catch (FileNotFoundException)
            {
                WriteLine("Файл не найден");
            }
            catch (DirectoryNotFoundException)
            {
                WriteLine("Директория не найдена");
            }
            catch (DriveNotFoundException)
            {
                WriteLine("Вы указали несуществующий диск\n");
            }
            catch (PathTooLongException)
            {
                WriteLine("Слишком длинное имя файла/пути\n");
            }
            catch (SecurityException)
            {
                WriteLine("Ошибка безопасности\n");
            }
            catch (NotSupportedException)
            {
                WriteLine("Путь имеет недопустимый формат\n");
            }
            catch (FormatException)
            {
                Write("Проверьте правильность формата ввода.\n");
                continue;
            }
            catch (OverflowException)
            {
                Write("Значение выходит за пределы целевого типа.\n");
                continue;
            }
            catch (MyException ex)
            {
                WriteLine($"Ошибка! {ex.Message}\n");
                continue;
            }
            break;
        }

    }

    public static void Selection(Dictionary<int, Soldier> SList, StreamWriter file)
    {
        while (true)
        {
            WriteLine("\nВыберите действие:\n");
            WriteLine("1 - вывод информации о всех солдатах");
            WriteLine("2 - вывод информации о конкретном солдате");
            WriteLine("3 - добавить солдата");
            WriteLine("4 - удалить всех солдат");
            WriteLine("5 - удалить конкретного солдата");
            WriteLine("6 - отсортировать солдат");
            WriteLine("7 - отправить в учебный центр");
            WriteLine("8 - отправить на СВО");
            WriteLine("9 - освободить от призыва");
            WriteLine("10 - отправить на обучение в военный ВУЗ");
            WriteLine("11 - выход\n");

            int Digit = ToInt32(ReadLine());
            WriteLine("");
            switch (Digit)
            {
                case 1:
                    file.WriteLine("Информация о всех солдатах: \n");
                    foreach (var x in SList)
                    {
                        SoldierInformation(x);
                        file.WriteLine($"\n\nФамилия: {x.Value.FIO}\n");
                        file.WriteLine($"Категория годности: {x.Value.FIO}\n");
                        file.WriteLine($"Род войск: {x.Value.FIO}\n");
                        file.WriteLine($"Адрес: {x.Value.FIO}\n");
                        file.WriteLine($"Номер паспорта: {x.Value.FIO}\n");
                        file.WriteLine($"Серия паспорта: {x.Value.FIO}\n\n");
                        file.WriteLine("----------------------------------\n");

                    }
                    continue;
                case 2:
                    WriteLine("Введите номер солдата, о котором хотите узнать: ");
                    int s1 = ToInt32(ReadLine());
                    s1--;
                    foreach (var x in SList)
                    {
                        if (s1 == x.Key)
                        {
                            WriteLine($"Фамилия: {x.Value.FIO}\n" +
                                $"Категория годности: {x.Value.ValidityCategory}\n" +
                                $"Род войск: {x.Value.MilitaryBranch}\n" +
                                $"Адрес: {x.Value.Address}\n" +
                                $"Номер паспорта: {x.Value.PassportNumber}\n" +
                                $"Серия паспорта: {x.Value.PassportSeries}");
                            file.WriteLine("Информация о конкретном солдате: \n");
                            file.WriteLine($"\n\nФамилия: {x.Value.FIO}\n");
                            file.WriteLine($"Категория годности: {x.Value.FIO}\n");
                            file.WriteLine($"Род войск: {x.Value.FIO}\n");
                            file.WriteLine($"Адрес: {x.Value.FIO}\n");
                            file.WriteLine($"Номер паспорта: {x.Value.FIO}\n");
                            file.WriteLine($"Серия паспорта: {x.Value.FIO}\n\n");
                            file.WriteLine("----------------------------------\n");
                        }
                    }
                    continue;
                case 3:
                    NewSoldier(SList);
                    continue;
                case 4:
                    SList.Clear();
                    continue;
                case 5:
                    WriteLine("Введите номер солдата, которого хотите удалить: ");
                    int s2 = ToInt32(ReadLine());
                    s2--;
                    foreach (var x in SList)
                    {
                        if (s2 == x.Key)
                        {
                            SList.Remove(s2);
                        }
                    }
                    file.WriteLine($"Солдат №{s2} удален\n");
                    continue;
                case 6:
                    SoldiersSorting(SList, file);
                    continue;
                case 7:
                    WriteLine("Введите номер солдата, которого хотите отправить в учебный центр: ");
                    int s3 = ToInt32(ReadLine());
                    s3--;
                    foreach (var x in SList)
                    {
                        if (s3 == x.Key)
                        {
                            WriteLine($"Солдат №{s3 + 1} отправлен в учебный центр");
                            file.WriteLine($"Солдат №{s3 + 1} отправлен в учебный центр");
                            file.WriteLine("----------------------------------\n");
                            SList.Remove(s3);
                        }
                    }
                    continue;
                case 8:
                    WriteLine("Введите номер солдата, которого хотите отправить на СВО: ");
                    int s4 = ToInt32(ReadLine());
                    s4--;
                    foreach (var x in SList)
                    {
                        if (s4 == x.Key)
                        {
                            WriteLine($"Солдат №{s4 + 1} отправлен на СВО");
                            file.WriteLine($"Солдат №{s4 + 1} отправлен на СВО");
                            file.WriteLine("----------------------------------\n");
                            SList.Remove(s4);
                        }
                    }
                    continue;
                case 9:
                    WriteLine("Введите номер солдата, которого хотите освободить от призыва на военную службу: ");
                    int s5 = ToInt32(ReadLine());
                    s5--;
                    foreach (var x in SList)
                    {
                        if (s5 == x.Key)
                        {
                            WriteLine($"Солдат №{s5 + 1} освобожден от призыва на военную службу");
                            file.WriteLine($"Солдат №{s5 + 1} освобожден от призыва на военную службу");
                            file.WriteLine("----------------------------------\n");
                            SList.Remove(s5);
                        }
                    }
                    continue;
                case 10:
                    WriteLine("Введите номер солдата, которого хотите отправить военный ВУЗ: ");
                    int s6 = ToInt32(ReadLine());
                    s6--;
                    foreach (var x in SList)
                    {
                        if (s6 == x.Key)
                        {
                            WriteLine($"Солдат №{s6 + 1} отправлен на обучение в военный ВУЗ");
                            file.WriteLine($"Солдат №{s6 + 1} отправлен на обучение в военный в ВУЗ");
                            file.WriteLine("----------------------------------\n");
                            SList.Remove(s6);
                        }
                    }
                    continue;
                case 11:
                    file.Close();
                    Environment.Exit(0);
                    break;
            }
            if (Digit == 7)
            {
                break;
            }
        }
    }

    public static void SoldierInformation(KeyValuePair<int, Soldier> SList)
    {
        WriteLine($"ФИО: {SList.Value.FIO}\n" +
            $"Категория годности: {SList.Value.ValidityCategory}\n" +
            $"Род войск: {SList.Value.MilitaryBranch}\n" +
            $"Адрес проживания: {SList.Value.Address}" +
            $"\nНомер паспорта: {SList.Value.PassportNumber}\n" +
            $"Серия паспорта: {SList.Value.PassportSeries}\n");
    }

    public static void NewSoldier(Dictionary<int, Soldier> SList)
    {
        while (true)
        {
            Checkings ch = new();
            try
            {
                int Number = SList.Count;
                Write("Введите ФИО солдата: ");
                string FIO = ReadLine()!;
                if (!ch.FIO(FIO))
                {
                    throw new MyException("Неправильно введена фамилия!");
                }
                Write("Введите его категорию годности: ");
                char VC = ToChar(ReadLine()!);
                if (ch.ValidityCategory(VC))
                {
                    throw new MyException("Неправильно введена категория годности!");
                }
                Write($"Введите род войск: ");
                string MB = ReadLine()!;
                if (!ch.MilitaryBranch(MB))
                {
                    throw new MyException("Неправильно введен род войск!");
                }
                Write("Введите его адрес проживания: ");
                string Address = ReadLine()!;
                if (!ch.Address(Address))
                {
                    throw new MyException("Неправильно введен адрес!");
                }
                Write("Введите номер его паспорта (4 цифры): ");
                string PassportNumber = ReadLine()!;
                if (!ch.PassportNumber(PassportNumber))
                {
                    throw new MyException("Неправильно введен номер паспорта!");
                }
                Write("Введите серию его паспорта (6 цифр): ");
                string PassportSeries = ReadLine()!;
                if (!ch.PassportSeries(PassportSeries))
                {
                    throw new MyException("Неправильно введена серия паспорта!");
                }
                Soldier soldier = new Soldier(FIO, VC,
                    MB, Address, PassportNumber, PassportSeries);
                SList.Add(Number, soldier);
            }
            catch (FormatException)
            {
                WriteLine("\nПроверьте правильность формата ввода.\n");
                continue;
            }
            catch (OverflowException)
            {
                WriteLine("\nЗначение выходит за пределы целевого типа.\n");
                continue;
            }
            catch (MyException ex)
            {
                WriteLine($"\nОшибка! {ex.Message}");
                continue;
            }
            break;
        }
    }

    public static void SoldiersSorting(Dictionary<int, Soldier> SList, StreamWriter file)
    {
        WriteLine("Выберите вид сортировки: ");
        WriteLine("1 - по ФИО (от А до Я)");
        WriteLine("2 - по номеру паспорта");
        int SortingSelect = ToInt32(ReadLine());
        switch (SortingSelect)
        {
            case 1:
                var SortedByAlphabet = SList.OrderBy(s => s.Value.FIO);
                file.WriteLine("Солдаты, отсортированные по ФИО\n");
                file.WriteLine("----------------------------------\n");
                foreach (var x in SortedByAlphabet)
                {
                    SoldierInformation(x);
                    file.WriteLine($"\n\nФамилия: {x.Value.FIO}\n");
                    file.WriteLine($"Категория годности: {x.Value.FIO}\n");
                    file.WriteLine($"Род войск: {x.Value.FIO}\n");
                    file.WriteLine($"Адрес: {x.Value.FIO}\n");
                    file.WriteLine($"Номер паспорта: {x.Value.FIO}\n");
                    file.WriteLine($"Серия паспорта: {x.Value.FIO}\n\n");
                    file.WriteLine("----------------------------------\n");
                }
                break;
            case 2:
                var SortedByPassportNumber = SList.OrderBy(s => s.Value.PassportNumber);
                file.WriteLine("Солдаты, отсортированные по номеру паспорта\n");
                file.WriteLine("----------------------------------\n");
                foreach (var x in SList)
                {
                    SoldierInformation(x);
                    file.WriteLine($"\n\nФамилия: {x.Value.FIO}\n");
                    file.WriteLine($"Категория годности: {x.Value.FIO}\n");
                    file.WriteLine($"Род войск: {x.Value.FIO}\n");
                    file.WriteLine($"Адрес: {x.Value.FIO}\n");
                    file.WriteLine($"Номер паспорта: {x.Value.FIO}\n");
                    file.WriteLine($"Серия паспорта: {x.Value.FIO}\n\n");
                }
                file.WriteLine("----------------------------------\n");
                break;
        }
    }
}