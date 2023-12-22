
using System;
using System.Reflection;
using System.Security.AccessControl;
using System.Xml;
using System.Xml.Linq;
using Google.Apis.Auth.OAuth2;

namespace LibraryForSomeDich
{
    public class MyLibrary
    {
        const float pi = 3.14f;

        /// <summary>
        /// Method Fuck my brain
        /// </summary>
        public static void PloshadKruga(float radius)
        {
            var s = pi * (radius * 2);
            Console.WriteLine(s);
        }
        /// <summary>
        /// Hi! in Console
        /// </summary>
        public static void Privet(string flex,string huy)
        {
          
            Console.WriteLine("Hello World" + flex);
        }
       
        public static async void PrintAllSummaries()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var xmlFile = $"{assembly.GetName().Name}.xml";
            var xmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);

            if (File.Exists(xmlFilePath))
            {
                var doc = XDocument.Load(xmlFilePath);
                var methods = typeof(MyLibrary).GetMethods();

                foreach (var method in methods)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    List<ParameterInfo> parametruList = new List<ParameterInfo>();

                    foreach (ParameterInfo paramInfo in parameters)
                    {
                        parametruList.Add(paramInfo); // Добавление типа параметра в список
                        Console.WriteLine($"Имя параметра: {paramInfo.Name}");
                        Console.WriteLine($"Тип параметра: {paramInfo.ParameterType}");
                    }             
                        string properties = string.Join(",", parametruList.ConvertAll(x => x.ParameterType));
                        if (properties != null)
                        {
                            var memberNameOther = $"M:{typeof(MyLibrary).FullName}.{method.Name}({properties})";
                            var summaryother = doc.Descendants("member")
                                                  .FirstOrDefault(e => e.Attribute("name")?.Value == memberNameOther)?
                                                  .Element("summary")?.Value.Trim();
                            if (!string.IsNullOrEmpty(summaryother))
                            {
                              Console.WriteLine($"Summary для метода {method.Name}: {summaryother}");
                            string translationEndpoint = $"https://api.funtranslations.com/translate/pirate.json?text={summaryother}";

                            try
                            {
                                using (var client = new HttpClient())
                                {
                                    var response = await client.GetAsync(translationEndpoint);

                                    if (response.IsSuccessStatusCode)
                                    {
                                        var result = await response.Content.ReadAsStringAsync();
                                        Console.WriteLine($"Original text: {summaryother}");
                                        Console.WriteLine($"Translated text: {result}");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Failed to translate. Status code: {response.StatusCode}");
                                    }
                                }
                            }
                            catch (HttpRequestException ex)
                            {
                                Console.WriteLine($"HTTP request exception: {ex.Message}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Exception occurred: {ex.Message}");
                            }
                        }
                    }                                         
                    var memberName = $"M:{typeof(MyLibrary).FullName}.{method.Name}";                  
                    var summary = doc.Descendants("member")
                                     .FirstOrDefault(e => e.Attribute("name")?.Value == memberName)?
                                     .Element("summary")?.Value.Trim();

                    if (!string.IsNullOrEmpty(summary))
                    {
                        Console.WriteLine($"Summary для метода {method.Name}: {summary}");                   
                    }
                    else
                    {                     
                        Console.WriteLine();
                    }
                }
            }
           
        }


    }
}
