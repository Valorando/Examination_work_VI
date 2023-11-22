using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination_work_19_11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string certified_words = "certified_words.txt";
            string userFilePath = null;

            Task main_file = Task.Run(() =>
            {
                if (!File.Exists(certified_words))
                {
                    using (StreamWriter sw = File.CreateText(certified_words))
                    {

                    }
                }
            });
            main_file.Wait();


            void SearchFiles(string directory, string certifiedFilePath, string userFile)
            {
                try
                {
                    string[] files = Directory.GetFiles(directory, "*.txt");

                    foreach (string file in files)
                    {
                        if (file == userFile)
                        {
                            continue;
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Проверка файла: {file}");
                        Console.ResetColor();

                        if (ContainsCertifiedWords(file, certifiedFilePath))
                        {
                            List<string> forbiddenWords = GetForbiddenWordsInFile(file, certifiedFilePath);
                        }
                    }

                    string[] subdirectories = Directory.GetDirectories(directory);
                    foreach (string subdirectory in subdirectories)
                    {
                        SearchFiles(subdirectory, certifiedFilePath, userFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{ex.Message}");
                    Console.ResetColor();
                }
            }



            bool ContainsCertifiedWords(string filePath, string certifiedFilePath)
            {
                try
                {
                    if (Path.GetFileName(filePath) == certified_words)
                    {
                        return false;
                    }


                    string fileContent = File.ReadAllText(filePath);


                    string[] certifiedWords = File.ReadAllLines(certifiedFilePath);


                    foreach (string word in certifiedWords)
                    {
                        if (fileContent.Contains(word))
                        {

                            fileContent = fileContent.Replace(word, "*******");
                        }
                    }


                    File.WriteAllText(filePath, fileContent);


                    return fileContent != File.ReadAllText(filePath);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{ex.Message}");
                    Console.ResetColor();
                }
                return false;
            }

            List<string> GetForbiddenWordsInFile(string filePath, string certifiedFilePath)
            {
                try
                {
                    List<string> forbiddenWords = File.ReadAllLines(certifiedFilePath).ToList();
                    string fileContent = File.ReadAllText(filePath);

                    return forbiddenWords.Where(word => fileContent.Contains(word)).ToList();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{ex.Message}");
                    Console.ResetColor();
                }
                return new List<string>();
            }


            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("Для выбора соответствующей опции нажмите нужную клавишу");
                    Console.WriteLine("1 - Импортировать запрещенные слова из файла.");
                    Console.WriteLine("2 - Начать сканирование.");
                    Console.WriteLine("3 - Выйти.");
                    Console.WriteLine();
                    ConsoleKeyInfo key = Console.ReadKey();

                    if (key.Key == ConsoleKey.D1)
                    {
                        try
                        {
                            Task t_loader = Task.Run(() =>
                            {
                                try
                                {
                                    Console.WriteLine();
                                    Console.Write("Введите путь к файлу с запрещенными словами: ");
                                    userFilePath = Console.ReadLine();

                                    if (File.Exists(userFilePath))
                                    {
                                        File.WriteAllText(certified_words, File.ReadAllText(userFilePath));
                                        Console.WriteLine("Содержимое файла успешно импортировано.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Файл {userFilePath} не найден.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"{ex.Message}");
                                    Console.ResetColor();
                                }

                            });
                            t_loader.Wait();
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{ex.Message}");
                            Console.ResetColor();
                        }
                    }

                    if (key.Key == ConsoleKey.D2)
                    {
                        try
                        {
                            Task t_chek = Task.Run(() =>
                            {
                                try
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Сканирование начато.");

                                    Stopwatch timer = Stopwatch.StartNew();

                                    DriveInfo[] allDrives = DriveInfo.GetDrives();

                                    //Сканирование на всех носителях
                                    //foreach (DriveInfo drive in allDrives)
                                    //{
                                    //    if (drive.DriveType == DriveType.Fixed)
                                    //    {
                                    //        SearchFiles(drive.RootDirectory.FullName, certified_words, userFilePath);
                                    //    }
                                    //}


                                    //Сканирование только на диске С.
                                    foreach (DriveInfo drive in allDrives)
                                    {
                                        if (drive.DriveType == DriveType.Fixed && drive.Name.StartsWith("C"))
                                        {
                                            SearchFiles(drive.RootDirectory.FullName, certified_words, userFilePath);
                                        }
                                    }

                                    Console.WriteLine("Сканирование завершено.");
                                    timer.Stop();
                                    Console.WriteLine($"Время сканирования: {timer.Elapsed}");
                                }
                                catch (Exception ex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"{ex.Message}");
                                    Console.ResetColor();
                                }
                            });
                            t_chek.Wait();
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{ex.Message}");
                            Console.ResetColor();
                        }
                    }

                    if (key.Key == ConsoleKey.D3)
                    {
                        Environment.Exit(0);
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{ex.Message}");
                    Console.ResetColor();
                }
            }
        }
    }
}
