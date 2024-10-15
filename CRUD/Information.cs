using static System.Console;
using static System.Convert;
using System.Security;
class Information
{
    public async static void Info()
    {
        #region Info
        Checkings ch = new();
        Write("Введите путь файла для сохранения информации: ");
        string path = ReadLine()!;
        if (File.Exists(path))
        {
            Write("Этот путь уже занят другим файлом!\n");
            Environment.Exit(0);
        }
        FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);
        //StreamWriter swmain = new StreamWriter(fs, System.Text.Encoding.UTF8);
        await using (StreamWriter swmain = new StreamWriter(fs, System.Text.Encoding.UTF8)) {
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
                    WriteLine(@"
                        Солдаты:
                        ----------------------------------
                    ");
                    swmain.WriteLine(@"
                        Солдаты
                        ----------------------------------
                    ");
                    Parallel.For(0, n, (Number) =>
                    {
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

                        Soldier soldier = new(FIO, VC, MB, Address, PassportNumber, PassportSeries);
                        SList.Add(Number, soldier);

                        swmain.WriteLine(@$"
                    Солдат {Number}:
                    ФИО: {FIO}
                    Категория годности: {VC}
                    Род войск: {MB}
                    Адрес проживания: {Address}
                    Номер паспорта: {PassportNumber}
                    Серия паспорта: {PassportSeries}
                    ----------------------------------
                    ");
                    });
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
        #endregion
    }

    public static void Selection(Dictionary<int, Soldier> SList, StreamWriter file)
    {
        #region Selection
        while (true)
        {
            WriteLine(@"
            Выберите действие:
            1 Вывод информации о всех солдатах
            2 Вывод информации о конкретном солдате
            3 Добавить солдата
            4 Удалить всех солдат
            5 Удалить конкретного солдата
            6 Отсортировать солдат
            7 Отправить в учебный центр
            8 Отправить на СВО
            9 Освободить от призыва
            10 Отправить на обучение в военный ВУЗ
            11 Выход
            ");

            int Digit = ToInt32(ReadLine());
            WriteLine("");
            switch (Digit)
            {
                case 1:
                    file.WriteLine("Информация о всех солдатах: \n");
                    Parallel.ForEach(SList, (x) =>
                    {
                        SoldierInformation(x);
                        file.WriteLine($@"
                            Фамилия: {x.Value.FIO}
                            Категория годности: {x.Value.FIO}
                            Род войск: {x.Value.FIO}
                            Адрес: {x.Value.FIO}
                            Номер паспорта: {x.Value.FIO}
                            Серия паспорта: {x.Value.FIO}
                        ");

                    });
                    continue;
                case 2:
                    WriteLine("Введите номер солдата, о котором хотите узнать: ");
                    int s1 = ToInt32(ReadLine());
                    s1--;
                    Parallel.ForEach(SList, (x) =>
                    {
                        if (s1 == x.Key)
                        {
                            WriteLine($@"
                                Фамилия: {x.Value.FIO}
                                Категория годности: {x.Value.ValidityCategory}
                                Род войск: {x.Value.MilitaryBranch}
                                Адрес: {x.Value.Address}
                                Номер паспорта: {x.Value.PassportNumber}
                                Серия паспорта: {x.Value.PassportSeries}
                                ----------------------------------
                                ");

                            file.WriteLine($@"
                                Информация о конкретном солдате: 
                                Фамилия: {x.Value.FIO}
                                Категория годности: {x.Value.FIO}
                                Род войск: {x.Value.FIO}
                                Адрес: {x.Value.FIO}
                                Номер паспорта: {x.Value.FIO}
                                Серия паспорта: {x.Value.FIO}
                                ----------------------------------
                            ");
                        }
                    });
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
                    Parallel.ForEach(SList, (x) =>
                    {
                        if (s2 == x.Key)
                        {
                            SList.Remove(s2);
                        }
                    });
                    file.WriteLine($"Солдат №{s2} удален\n");
                    continue;
                case 6:
                    SoldiersSorting(SList, file);
                    continue;
                case 7:
                    WriteLine("Введите номер солдата, которого хотите отправить в учебный центр: ");
                    int s3 = ToInt32(ReadLine());
                    s3--;
                    Parallel.ForEach(SList, (x) =>
                    {
                        if (s3 == x.Key)
                        {
                            WriteLine($"Солдат №{s3 + 1} отправлен в учебный центр");
                            file.WriteLine(@$"
                            Солдат №{s3 + 1} отправлен в учебный центр
                            ----------------------------------
                            ");
                            SList.Remove(s3);
                        }
                    });
                    continue;
                case 8:
                    WriteLine("Введите номер солдата, которого хотите отправить на СВО: ");
                    int s4 = ToInt32(ReadLine());
                    s4--;
                    Parallel.ForEach(SList, (x) =>
                    {
                        if (s4 == x.Key)
                        {
                            WriteLine($"Солдат №{s4 + 1} отправлен на СВО");
                            file.WriteLine(@$"
                            Солдат №{s4 + 1} отправлен на СВО
                            ----------------------------------
                            ");
                            SList.Remove(s4);
                        }
                    });
                    continue;
                case 9:
                    WriteLine("Введите номер солдата, которого хотите освободить от призыва на военную службу: ");
                    int s5 = ToInt32(ReadLine());
                    s5--;
                    Parallel.ForEach(SList, (x) =>
                    {
                        if (s5 == x.Key)
                        {
                            WriteLine($"Солдат №{s5 + 1} освобожден от призыва на военную службу");
                            file.WriteLine(@$"
                            Солдат №{s5 + 1} освобожден от призыва на военную службу
                            ----------------------------------
                            ");
                            SList.Remove(s5);
                        }
                    });
                    continue;
                case 10:
                    WriteLine("Введите номер солдата, которого хотите отправить военный ВУЗ: ");
                    int s6 = ToInt32(ReadLine());
                    s6--;
                    Parallel.ForEach(SList, (x) =>
                    {
                        if (s6 == x.Key)
                        {
                            WriteLine($"Солдат №{s6 + 1} отправлен на обучение в военный ВУЗ");
                            file.WriteLine(@$"
                            Солдат №{s6 + 1} отправлен на обучение в военный в ВУЗ
                            ----------------------------------
                            ");
                            SList.Remove(s6);
                        }
                    });
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
        #endregion
    }

    public static void SoldierInformation(KeyValuePair<int, Soldier> SList)
    {
        #region SoldierInformation
        WriteLine(@$"
        ФИО: {SList.Value.FIO}\n
        Категория годности: {SList.Value.ValidityCategory}\n
        Род войск: {SList.Value.MilitaryBranch}\n
        Адрес проживания: {SList.Value.Address}\n
        Номер паспорта: {SList.Value.PassportNumber}\n
        Серия паспорта: {SList.Value.PassportSeries}\n
        ");
        #endregion
    }

    public static void NewSoldier(Dictionary<int, Soldier> SList)
    {
        #region NewSoldier
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

                var soldier = new Soldier(FIO, VC,
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
        #endregion
    }

    public static void SoldiersSorting(Dictionary<int, Soldier> SList, StreamWriter file)
    {
        #region SoldiersSorting
        WriteLine(@"Выберите вид сортировки: 
        1 - по ФИО (от А до Я)
        2 - по номеру паспорта
        ");
        int SortingSelect = ToInt32(ReadLine());
        switch (SortingSelect)
        {
            case 1:
                var SortedByAlphabet = SList.AsParallel().OrderBy(s => s.Value.FIO);
                file.WriteLine(@"
                Солдаты, отсортированные по ФИО\n
                ----------------------------------\n
                ");
                Parallel.ForEach(SortedByAlphabet, (x) =>
                {
                    SoldierInformation(x);
                    file.WriteLine(@$"
                    \n\nФамилия: {x.Value.FIO}\n
                    Категория годности: {x.Value.ValidityCategory}\n
                    Род войск: {x.Value.MilitaryBranch}\n
                    Адрес: {x.Value.Address}\n
                    Номер паспорта: {x.Value.PassportNumber}\n
                    Серия паспорта: {x.Value.PassportSeries}\n\n
                    ----------------------------------\n
                    ");
                });
                break;
            case 2:
                var SortedByPassportNumber = SList.AsParallel().OrderBy(s => s.Value.PassportNumber);
                file.WriteLine(@"
                Солдаты, отсортированные по номеру паспорта\n
                ----------------------------------
                ");
                Parallel.ForEach(SortedByPassportNumber, (x) =>
                {
                    SoldierInformation(x);
                    file.WriteLine($@"
                    \n\nФамилия: {x.Value.FIO}\n
                    Категория годности: {x.Value.ValidityCategory}\n
                    Род войск: {x.Value.MilitaryBranch}\n
                    Адрес: {x.Value.Address}\n
                    Номер паспорта: {x.Value.PassportNumber}\n
                    Серия паспорта: {x.Value.PassportSeries}\n\n
                    ");

                });
                file.WriteLine("----------------------------------\n");
                break;
        }
        #endregion SoldiersSorting
    }
}