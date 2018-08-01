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

using NLog;
using System.Text;
using MySql.Data.MySqlClient;
using ProgramTestingAdditions;

namespace SimplePM_Server.ProgramTesting.STester
{
    
    public class SyntaxTesting : TestingType
    {
        
        private static readonly Logger logger = LogManager.GetLogger("SimplePM_Server.ProgramTesting.STester.SyntaxTesting");

        public SyntaxTesting(
            ref MySqlConnection conn,
            string exeFilePath,
            ref SubmissionInfo.SubmissionInfo submissionInfo
        ) : base(ref conn, exeFilePath, ref submissionInfo) {  }

        public override ProgramTestingResult RunTesting()
        {
            
            logger.Trace("#" + submissionInfo.SubmissionId + ": SyntaxTesting.RunTesting() [started|finished]");
            
            return new ProgramTestingResult(1)
            {

                TestingResults =
                {

                    [0] = new SingleTestResult
                    {

                        // Выходные данные заполняем кракозябрами
                        Output = Encoding.UTF8.GetBytes("NULL"),

                        // Выходные данные исключений устанавливаем в null
                        ErrorOutput = null,

                        // Код выхода - стандартный
                        ExitCode = 0,

                        // Результатом будет промежуточный успешный
                        Result = SingleTestResult.PossibleResult.MiddleSuccessResult,

                        // Использованная память
                        UsedMemory = 0,

                        // Использованное процессорное время
                        UsedProcessorTime = 0

                    }

                }

            };
            
        }
        
    }
    
}