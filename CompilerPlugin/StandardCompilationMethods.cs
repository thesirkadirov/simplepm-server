﻿/*
 * ███████╗██╗███╗   ███╗██████╗ ██╗     ███████╗██████╗ ███╗   ███╗
 * ██╔════╝██║████╗ ████║██╔══██╗██║     ██╔════╝██╔══██╗████╗ ████║
 * ███████╗██║██╔████╔██║██████╔╝██║     █████╗  ██████╔╝██╔████╔██║
 * ╚════██║██║██║╚██╔╝██║██╔═══╝ ██║     ██╔══╝  ██╔═══╝ ██║╚██╔╝██║
 * ███████║██║██║ ╚═╝ ██║██║     ███████╗███████╗██║     ██║ ╚═╝ ██║
 * ╚══════╝╚═╝╚═╝     ╚═╝╚═╝     ╚══════╝╚══════╝╚═╝     ╚═╝     ╚═╝
 * 
 * SimplePM Server is a part of software product "Automated
 * verification system for programming tasks "SimplePM".
 * 
 * Copyright (C) 2016-2018 Yurij Kadirov
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Affero General Public License for more details.
 * 
 * You should have received a copy of the GNU Affero General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
 * 
 * GNU Affero General Public License applied only to source code of
 * this program. More licensing information hosted on project's website.
 * 
 * Visit website for more details: https://spm.sirkadirov.com/
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;

namespace CompilerPlugin
{
    
    public class StandardCompilationMethods
    {
        
        public static string GenerateExeFileLocation(
            string srcFileLocation,
            string currentSubmissionId
        )
        {

            /*
             * В зависимости  от  текущей  платформы,
             * устанавливаем специфическое расширение
             * запускаемого файла.
             */

            var outFileExt = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? ".exe" : string.Empty;

            // Получаем путь родительской директории файла исходного кода
            var parentDirectoryFullName = new FileInfo(srcFileLocation).DirectoryName
                                          ?? throw new DirectoryNotFoundException(srcFileLocation + " parent");

            // Формируем начальный путь исполняемого файла
            var exePath = Path.Combine(
                parentDirectoryFullName,
                's' + currentSubmissionId
            );

            /*
             * В  случае,  если  расширение исполняемого  файла в данной
             * ОС не нулевое, добавляем его к имени файла.
             */

            if (!string.IsNullOrWhiteSpace(outFileExt))
                exePath += outFileExt;

            // Возвращаем сформированный путь
            return exePath;

        }

        public static CompilationResult RunCompiler(string compilerFullName, string compilerArgs, string workingDirectory = null)
        {

            //TODO: Реализовать возможность установки рабочей папки компилятора
            
            try
            {

                // Создаём новый экземпляр процесса компилятора
                var cplProc = new Process
                {
                    
                    // Устанавливаем стартовую информацию
                    StartInfo = new ProcessStartInfo(compilerFullName, compilerArgs)
                    {
                        
                        // Никаких ошибок, я сказал!
                        ErrorDialog = false,

                        // Минимизируем его, ибо не достоен он почестей!
                        WindowStyle = ProcessWindowStyle.Minimized,

                        // Перехватываем выходной поток
                        RedirectStandardOutput = true,

                        // Перехватываем поток ошибок
                        RedirectStandardError = true,

                        // Для перехвата делаем процесс демоном
                        UseShellExecute = false
                        
                    }
                    
                };

                // Если указана требуемая рабочая папка, устанавливаем её
                if (workingDirectory != null)
                    cplProc.StartInfo.WorkingDirectory = workingDirectory;
                
                // Запускаем процесс компилятора
                cplProc.Start();

                // Ожидаем завершения процесса компилятора
                cplProc.WaitForExit();

                /*
                 * Осуществляем чтение выходных потоков
                 * компилятора в специально созданные
                 * для этого переменные.
                 */

                var standartOutput = cplProc.StandardOutput.ReadToEnd();
                var standartError = cplProc.StandardError.ReadToEnd();

                // Стандартный выход компилятора
                if (standartOutput.Length == 0)
                    standartOutput = "SimplePM Server v" + Assembly.GetExecutingAssembly().GetName().Version + " on " +
                                     Environment.OSVersion;

                // Возвращаем результат компиляции
                return new CompilationResult
                {

                    // Записываем данные с выходного потока компилятора
                    CompilerOutput = HttpUtility.HtmlEncode(standartOutput + "\n" + standartError)

                };

            }
            catch (Exception ex)
            {
                
                // Возвращаем провальный результат компиляции
                return new CompilationResult
                {
                    
                    HasErrors = true,
                    CompilerOutput = ex.ToString()
                    
                };

            }

        }

        public static CompilationResult ReturnCompilerResult(CompilationResult temporaryResult)
        {

            // Проверка на предопределение наличия ошибок при компиляции
            if (!temporaryResult.HasErrors)
            {
                // Проверяем на наличие исполняемого файла
                temporaryResult.HasErrors = !File.Exists(temporaryResult.ExeFullname);
            }

            // Возвращаем результат компиляции
            return temporaryResult;

        }

        
    }
    
}