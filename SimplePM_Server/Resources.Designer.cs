﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimplePM_Server {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SimplePM_Server.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///        DELETE FROM
        ///            `spm_supported_languages`
        ///        WHERE
        ///            `owner_server_id` = @owner_server_id
        ///        ;
        ///    .
        /// </summary>
        public static string delete_outdated_languages_query {
            get {
                return ResourceManager.GetString("delete_outdated_languages_query", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///        SELECT 
        ///            `memoryLimit`, 
        ///            `timeLimit` 
        ///        FROM 
        ///            `spm_problems_tests` 
        ///        WHERE 
        ///            `problemId` = @problemId 
        ///        ORDER BY 
        ///            `id` ASC 
        ///        LIMIT 
        ///            1
        ///        ;
        ///    .
        /// </summary>
        public static string get_debug_limits {
            get {
                return ResourceManager.GetString("get_debug_limits", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///        INSERT INTO
        ///            `spm_supported_languages`
        ///        SET
        ///            `title` = @title,
        ///            `name` = @name,
        ///            `syntax_name` = @syntax_name,
        ///            `owner_server_id` = @owner_server_id
        ///        ON DUPLICATE KEY UPDATE
        ///            `title` = @title,
        ///            `syntax_name` = @syntax_name,
        ///            `owner_server_id` = @owner_server_id
        ///        ;
        ///    .
        /// </summary>
        public static string send_supported_languages_query {
            get {
                return ResourceManager.GetString("send_supported_languages_query", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///        START TRANSACTION; 
        ///        SELECT 
        ///            `spm_problems`.`difficulty`, 
        ///        
        ///            `spm_problems`.`authorSolution`, 
        ///            `spm_problems`.`authorSolutionLanguage`, 
        ///        
        ///            `spm_problems`.`adaptProgramOutput`, 
        ///        
        ///            `spm_submissions`.`submissionId`, 
        ///        
        ///            `spm_submissions`.`olympId`, 
        ///        
        ///            `spm_submissions`.`time`, 
        ///        
        ///            `spm_submissions`.`userId`, 
        ///            `spm_submissions`.`prob [rest of string was truncated]&quot;;.
        /// </summary>
        public static string submission_query {
            get {
                return ResourceManager.GetString("submission_query", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///        UPDATE 
        ///	        `spm_submissions` 
        ///        SET 
        ///            `status` = &apos;ready&apos;, 
        ///            `testType` = @param_testType, 
        ///            `hasError` = @param_hasError, 
        ///            `compiler_text` = @param_compiler_text, 
        ///            `errorOutput` = @param_errorOutput, 
        ///            `output` = @param_output, 
        ///            `exitcodes` = @param_exitcodes, 
        ///            `tests_result` = @param_result, 
        ///            `b` = @param_rating 
        ///        WHERE 
        ///            `submissionId` = @param_submis [rest of string was truncated]&quot;;.
        /// </summary>
        public static string submission_result_query {
            get {
                return ResourceManager.GetString("submission_result_query", resourceCulture);
            }
        }
    }
}
