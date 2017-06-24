﻿/*
 * Copyright (C) 2017, Kadirov Yurij.
 * All rights are reserved.
 * Licensed under CC BY-NC-SA 4.0 license.
 * 
 * @Author: Kadirov Yurij
 * @Website: https://sirkadirov.com/
 * @Email: admin@sirkadirov.com
 * @Repo: https://github.com/SirkadirovTeam/SimplePM_Server
 */

namespace SimplePM_Server
{
    class SimplePM_Submission
    {
        /// <summary>
        /// Submission type
        /// </summary>
        public enum SubmissionType
        {
            unset = 0,
            syntax = 2,
            debug = 4,
            release = 8
        }

        /// <summary>
        /// Submission's programming language
        /// </summary>
        public enum SubmissionLanguage
        {
            unset,
            freepascal,
            csharp,
            cpp,
            c,
            python,
            lua,
            java
        }

        /// <summary>
        /// With this function you can get code language enum by string
        /// </summary>
        /// <param name="codeLang">name of programming language</param>
        /// <returns></returns>
        public static SubmissionLanguage getCodeLanguageByName(string codeLang)
        {
            switch (codeLang)
            {
                case "freepascal":
                    return SubmissionLanguage.freepascal;
                case "csharp":
                    return SubmissionLanguage.csharp;
                case "cpp":
                    return SubmissionLanguage.cpp;
                case "c":
                    return SubmissionLanguage.c;
                case "python":
                    return SubmissionLanguage.python;
                case "lua":
                    return SubmissionLanguage.lua;
                case "java":
                    return SubmissionLanguage.java;
                default:
                    return SubmissionLanguage.unset;
            }
        }

        /// <summary>
        /// With this function you can get file extension by submission's 
        /// programming language
        /// </summary>
        /// <param name="lang">Submission's programming language</param>
        /// <returns></returns>
        public static string getExtByLang(SubmissionLanguage lang)
        {
            switch (lang)
            {
                case SubmissionLanguage.freepascal:
                    return "pas";
                case SubmissionLanguage.csharp:
                    return "cs";
                case SubmissionLanguage.cpp:
                    return "cpp";
                case SubmissionLanguage.c:
                    return "c";
                case SubmissionLanguage.python:
                    return "py";
                case SubmissionLanguage.lua:
                    return "lua";
                case SubmissionLanguage.java:
                    return "java";
                default:
                    return "txt";
            }
        }
    }
}