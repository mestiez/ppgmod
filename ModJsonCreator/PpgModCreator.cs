using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ppgmod
{
    public static class PpgModCreator
    {
        public const string Output = "mod.json";

        public static void Start(string[] arguments)
        {
            ModMetaData metadata = new ModMetaData();
            var questions = GetQuestions();

            Say($"Enter the requested values. Press enter without entering anything to use the default value.\nThe program will output a mod.json at the current location ({Environment.CurrentDirectory})\n");
            AskQuestions(metadata, ref questions);

            WriteJson(metadata);
        }

        private static void AskQuestions(ModMetaData metadata, ref List<Question> questions)
        {
            foreach (var question in questions)
            {
                var answer = Ask(question.Sentence);
                bool answeredNothing = string.IsNullOrWhiteSpace(answer);

                if (answeredNothing)
                    question.SetValue(metadata, question.Entry.DefaultValue);
                else
                {
                    try
                    {
                        question.SetValue(metadata, answer);
                    }
                    catch (Exception)
                    {
                        question.SetValue(metadata, question.Entry.DefaultValue);
                    }
                }
            }
        }

        private static void WriteJson(ModMetaData metadata)
        {
            // Ik gebruik deze chaos zodat de output executable klein blijft (met Newtonsoft was het meer dan 300kb geweest, gestoord)
            using (var stream = new FileStream(Output, FileMode.Create))
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true, "\t"))
                {
                    DataContractJsonSerializer d = new DataContractJsonSerializer(typeof(ModMetaData));
                    d.WriteObject(writer, metadata);
                }
            }
        }

        private static List<Question> GetQuestions()
        {
            var members = typeof(ModMetaData).GetFields();
            var questions = new List<Question>();
            foreach (var member in members)
            {
                var entry = member.GetCustomAttribute<EntryAttribute>();
                if (entry == null) continue;
                questions.Add(new Question(entry, member));
            }
            return questions;
        }

        private static void Say(string message)
        {
            Console.WriteLine(message);
        }

        private static string Ask(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        public struct Question
        {
            public Question(EntryAttribute entry, FieldInfo field)
            {
                Entry = entry;
                Field = field;
            }

            public EntryAttribute Entry { get; }
            public FieldInfo Field { get; }

            public string Sentence => $"Enter {Entry.Information}. Defaults to \'{Entry.DefaultValue}\'";

            public void SetValue(ModMetaData instance, string value)
            {
                var fieldType = Field.FieldType;
                if (fieldType.IsArray)
                {
                    string[] result = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim()).ToArray();
                    Field.SetValue(instance, result);
                }
                else
                {
                    Field.SetValue(instance, value);
                }
            }
        }
    }
}
