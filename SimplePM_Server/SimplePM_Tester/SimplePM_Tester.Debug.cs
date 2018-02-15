﻿/*
 * Copyright (C) 2017, Kadirov Yurij.
 * All rights are reserved.
 * Licensed under Apache License 2.0 with additional restrictions.
 * 
 * @Author: Kadirov Yurij
 * @Website: https://sirkadirov.com/
 * @Email: admin@sirkadirov.com
 * @Repo: https://github.com/SirkadirovTeam/SimplePM_Server
 */

using System;
using System.IO;
using MySql.Data.MySqlClient;

namespace SimplePM_Server.SimplePM_Tester
{
    
    internal partial class SimplePM_Tester
    {
        
        /*
         * Функция, ответственная за debug
         * тестирование   пользовательских
         * решений.
         */
        public ProgramTestingResult Debug()
        {

            /*
             * Переменная хранит полный путь
             * к запускаемому файлу авторского
             * решения задачи
             */
            var authorSolutionExePath = GetAuthorSolutionExePath(
                out var authorSolutionCodeLanguage
            );

            /*
             * Передаём новосозданным переменным
             * информацию о лимитах для пользовательского
             * процесса (пользовательского решения задачи)
             */
            GetDebugProgramLimits(
                out var memoryLimit, // переменная, хранящая значение лимита по памяти
                out var timeLimit // переменная, хранящая значение лимита по процессорному времени
            );
            
            /*
             * Проводим нетестовый запуск авторского решения
             * и получаем всё необходимое для тестирования
             * пользовательской программы
             */
            var authorTestingResult = new ProgramTester(
                ref _compilerPlugins,
                authorSolutionCodeLanguage,
                authorSolutionExePath,
                "-author-solution",
                memoryLimit,
                timeLimit,
                submissionInfo.CustomTest,
                0,
                submissionInfo.AdaptProgramOutput
            ).RunTesting();

            /*
             * Проверяем, успешно ли проведен запуск
             * авторского решения задачи. В случае
             * обнаружения каких-либо ошибок выбрасываем
             * исключение AuthorSolutionRunningException,
             * которое информирует улавливатель исключений
             * о необходимости предоставления информации
             * об ошибке в лог-файлах сервера и прочих
             * местах, где это важно и необходимо.
             */
            if (authorTestingResult.Result != Test.MiddleSuccessResult)
                throw new SimplePM_Exceptions.AuthorSolutionRunningException();
            
            /*
             * Проводим тестовый запуск пользовательского
             * решения поставленной задачи и получаем всё
             * необходимое для тестирования программы.
             */
            var userTestingResult = new ProgramTester(
                ref sConfig,
                ref sCompilersConfig,
                ref _compilerPlugins,
                submissionInfo.CodeLang,
                exeFileUrl,
                "-user-solution",
                memoryLimit,
                timeLimit,
                submissionInfo.CustomTest,
                authorTestingResult.Output.Length,
                submissionInfo.AdaptProgramOutput
            ).RunTesting();
            
            /*
             * Если результат тестирования не полностью
             * известен, осуществляем проверку по дополнительным
             * тестам и выдвигаем остаточный результат debug
             * тестирования пользовательского решения задачи.
             */
            if (userTestingResult.Result == Test.MiddleSuccessResult)
            {
                userTestingResult.Result = (
                    userTestingResult.Output == authorTestingResult.Output
                ) ? '+' : '-';
            }

            /*
             * Формируем результат тестирования пользовательского
             * решения поставленной задачи, добавляем информацию
             * о единственном тесте, который был проведен
             * непосредственно при тестировании данного
             * пользовательского решения данной задачи.
             */
            var programTestingResult = new ProgramTestingResult(1)
            {
                TestingResults =
                {
                    [0] = userTestingResult
                }
            };

            /*
             * Возвращаем сформированный результат
             * тестирования пользовательского
             * решения поставленной задачи.
             */
            return programTestingResult;

        }

        /*
         * Функция, ответственная за скачивание,
         * компиляцию и получение  полного  пути
         * к  авторскому   решению  поставленной
         * задачи.
         */
        private string GetAuthorSolutionExePath(out string authorSolutionCodeLanguage)
        {
            
            /*
             * Выборка информации об
             * авторском решении из
             * базы данных SimplePM.
             */
            
            // Формируем SQL запрос
            const string querySelect = @"
                SELECT 
                    `codeLang`, 
                    `code` 
                FROM 
                    `spm_problems_ready` 
                WHERE 
                    `problemId` = @problemId 
                ORDER BY 
                    `problemId` ASC 
                LIMIT 
                    1
                ;
            ";
            
            var cmdSelect = new MySqlCommand(querySelect, connection);
            cmdSelect.Parameters.AddWithValue("@problemId", submissionInfo.ProblemId);

            // Чтение результатов запроса
            var dataReader = cmdSelect.ExecuteReader();

            // Объявляем необходимые переменные
            byte[] authorProblemCode;

            // Читаем полученные данные
            if (dataReader.Read())
            {

                // Исходный код авторского решения
                authorProblemCode = (byte[]) dataReader["code"];

                // Язык авторского решения
                authorSolutionCodeLanguage = dataReader["codeLang"].ToString();

                // Закрываем data reader
                dataReader.Close();

            }
            else
            {

                // Закрываем data reader
                dataReader.Close();

                // Авторское решение не найдено
                throw new SimplePM_Exceptions.AuthorSolutionNotFoundException();

            }
            
            /*
             * Компиляция авторского решения
             * поставленной задачи с последующим
             * возвращением результатов компиляции.
             */

            // Определяем расширение файла
            var authorFileExt = "." + SimplePM_Submission.GetExtByLang(
                authorSolutionCodeLanguage,
                ref _compilerPlugins
            );

            // Получаем случайный путь к директории авторского решения
            var tmpAuthorDir = sConfig["Program"]["temp_path"] + 
                @"\author\" + Guid.NewGuid() + @"\";

            // Создаём папку текущего авторского решения задачи
            Directory.CreateDirectory(tmpAuthorDir);

            // Определяем путь хранения файла исходного кода вторского решения
            var tmpAuthorSrcLocation = 
                tmpAuthorDir + "sa" + 
                submissionInfo.SubmissionId + 
                authorFileExt;

            // Записываем исходный код авторского решения в заданный временный файл
            File.WriteAllBytes(
                tmpAuthorSrcLocation,
                authorProblemCode
            );

            // Устанавливаем его аттрибуты
            File.SetAttributes(
                tmpAuthorSrcLocation,
                FileAttributes.Temporary | FileAttributes.NotContentIndexed
            );

            // Инициализируем экземпляр класса компилятора
            var compiler = new SimplePM_Compiler(
                ref sConfig,
                ref sCompilersConfig,
                ref _compilerPlugins,
                "a" + submissionInfo.SubmissionId,
                tmpAuthorSrcLocation,
                authorSolutionCodeLanguage
            );

            // Получаем структуру результата компиляции
            var cResult = compiler.ChooseCompilerAndRun();

            /*
             * В случае возникновения ошибки при компиляции
             * авторского решения аварийно завершаем работу
             * функции и выбрасываем исключение, содержащее
             * информацию о файле и причине ошибки при  его
             * открытии.
             */
            if (cResult.HasErrors)
                throw new FileLoadException(cResult.ExeFullname);
            
            return cResult.ExeFullname;

        }

        /*
         * Функция, ответственная за получение
         * информации  о  лимитах   для  debug
         * тестирования       пользовательских
         * решений  задач по программированию.
         */
        private void GetDebugProgramLimits(out int memoryLimit, out int timeLimit)
        {

            /*
             * Формируем и выполняем запрос к
             * базе  данных  системы, который
             * позволит нам узнать информацию
             * о лимитах для пользовательской
             * программы.
             */

            // Формируем SQL запрос
            const string querySelect = @"
                SELECT 
                    `memoryLimit`, 
                    `timeLimit` 
                FROM 
                    `spm_problems_tests` 
                WHERE 
                    `problemId` = @problemId 
                ORDER BY 
                    `timeLimit` DESC,
                    `memoryLimit` DESC,
                    `id` ASC 
                LIMIT 
                    1
                ;
            ";

            var cmdSelect = new MySqlCommand(querySelect, connection);
            cmdSelect.Parameters.AddWithValue("@problemId", submissionInfo.ProblemId);

            // Чтение результатов запроса
            var dataReader = cmdSelect.ExecuteReader();
            
            // Читаем полученные данные
            if (dataReader.Read())
            {

                // Получаем лимит по используемой программой памяти
                memoryLimit = int.Parse(dataReader["memoryLimit"].ToString());

                // Получаем лимит по используемом программой процессорному времени
                timeLimit = int.Parse(dataReader["timeLimit"].ToString());

                // Закрываем data reader
                dataReader.Close();

            }
            else
            {

                // Закрываем data reader
                dataReader.Close();

                /*
                 * Выбрасываем исключение, которое означает
                 * присутствие ошибки при попытке получения
                 * информации с базы данных системы.
                 */
                throw new SimplePM_Exceptions.DatabaseQueryException();

            }

        }

    }

}