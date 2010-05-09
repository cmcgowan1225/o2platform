﻿using System.Reflection;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel.ExtensionMethods;
using O2.External.SharpDevelop.AST;
using O2.DotNetWrappers.Windows;
using O2.DotNetWrappers.DotNet;
using System;
using O2.DotNetWrappers.H2Scripts;

namespace O2.External.SharpDevelop.ExtensionMethods
{
    public static class FastCompiler_ExtensionMethods
    {
        public static Assembly compile(this string pathToFileToCompile)
        {
            var csharpCompiler = new CSharp_FastCompiler();
            var compileProcess = new System.Threading.AutoResetEvent(false);
            csharpCompiler.compileSourceCode(pathToFileToCompile.contents());
            csharpCompiler.onCompileFail = () => compileProcess.Set();
            csharpCompiler.onCompileOK = () => compileProcess.Set();
            compileProcess.WaitOne();
            return csharpCompiler.assembly();
        }

        public static Assembly compile_CodeSnippet(this string codeSnipptet)
        {
            var csharpCompiler = new CSharp_FastCompiler();
            var compileProcess = new System.Threading.AutoResetEvent(false);
            //csharpCompiler.compileSourceCode(pathToFileToCompile.contents());
            csharpCompiler.compileSnippet(codeSnipptet);
            csharpCompiler.onCompileFail = () => compileProcess.Set();
            csharpCompiler.onCompileOK = () => compileProcess.Set();
            compileProcess.WaitOne();
            return csharpCompiler.assembly();
        }

        public static Assembly assembly(this CSharp_FastCompiler csharpCompiler)
        {
            if (csharpCompiler != null)
                if (csharpCompiler.CompilerResults != null)
                    if (csharpCompiler.CompilerResults.Errors.HasErrors == false)
                    {
                        if (csharpCompiler.CompilerResults.CompiledAssembly != null)
                            return csharpCompiler.CompilerResults.CompiledAssembly;
                    }
                    else
                        "CompilationErrors:".line().add(csharpCompiler.CompilationErrors).error();

            return null;
        }

        public static Assembly compile(this string pathToFileToCompile, string targetAssembly)
        {
            var assembly = pathToFileToCompile.compile(true);
            Files.Copy(assembly.Location, targetAssembly);
            return assembly;
        }

        public static Assembly compile(this string pathToFileToCompile, bool compileToFileAndWithDebugSymbols)
        {
            string generateDebugSymbolsTag = @"//debugSymbols".line();
            if (pathToFileToCompile.fileContains(generateDebugSymbolsTag).isFalse())
                pathToFileToCompile.fileInsertAt(0, generateDebugSymbolsTag);
            return pathToFileToCompile.compile();

        }        

        public static object executeFirstMethod(this string pathToFileToCompileAndExecute)
        {
            return pathToFileToCompileAndExecute.executeFirstMethod(new object[] { });
        }

        public static object executeFirstMethod(this string pathToFileToCompileAndExecute, object[] parameters)
        {
            var assembly = pathToFileToCompileAndExecute.compile();
            return assembly.executeFirstMethod(parameters);
        }

        public static object executeFirstMethod(this Assembly assembly)
        {
            return assembly.executeFirstMethod(false, false, new object[] {});
        }

        public static object executeFirstMethod(this Assembly assembly ,  object[] parameters)
        {
            return assembly.executeFirstMethod(false, false, parameters);
        }

        public static object executeFirstMethod(this Assembly assembly ,  bool executeInStaThread, bool executeInMtaThread, object[] parameters)
        {            
            if (assembly != null)
            {
                var methods = assembly.methods();
                foreach (var method in methods)
                    if (method.IsSpecialName == false)  // we need to do this since Properties get_ and set_ also look like methods
                    //if (methods.Count >0)        		
                    //{
                    {
                        if (executeInStaThread)
                            return O2Thread.staThread(() => method.executeMethod(parameters));
                        if (executeInMtaThread)
                            return O2Thread.mtaThread(() => method.executeMethod(parameters));

                        return method.executeMethod(parameters);
                    }
            }
            return null;
        }

        public static object executeMethod(this MethodInfo method, params object[] parameters)
        {
            try
            {
                if (method.parameters().size() == parameters.size())
                    return method.invoke(parameters);
                return method.invoke();
            }
            catch (Exception ex)
            {
                ex.log("in CSharp_FastCompiler.executeMethod");
                return null;
            }
        }        

        public static object executeH2Script(this string h2ScriptFile)
        {
            try
            {
                if (h2ScriptFile.extension(".h2").isFalse())
                    "[in executeH2Script]: file to execute must be a *.h2 file, it was:{0}".error(h2ScriptFile);
                else
                {
                    var h2Script = H2.load(h2ScriptFile);
                    return h2Script.execute();                    
                }
            }
            catch (Exception ex)
            {
                ex.log("in CSharp_FastCompiler.executeH2Script");
            }
            return null;
        }

        public static object execute(this H2 h2Script)
        {
            try
            {                
                var assembly = h2Script.SourceCode.assembly();
                return assembly.executeFirstMethod();                
            }
            catch (Exception ex)
            {
                ex.log("in CSharp_FastCompiler.executeH2Script");
            }
            return null;
        }
    }
}
