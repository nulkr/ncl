using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ncl.Equipment;
using ncl;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Bson;
using System.IO.Compression;


namespace ExRecipe
{
    class Program
    {
        static void Main(string[] args)
        {
            Recipe recipe = new Recipe();

            Console.WriteLine("Reading Pmac Schema");
            recipe.AddPmacSchema("DEFINE_VAR.h");

            #region Test JsonFile
            
            //Console.WriteLine("Generate Json, BSon, Compressed-Json");
            //JsonFile.Save(recipe, "1.json");
            //JsonFile.SaveBson(recipe, "1.bson");
            //JsonFile.SaveCompressed(recipe, "1.cjson");
            //Console.WriteLine();

            //Console.Write("Check Json......");
            //recipe._Items.Clear();
            //JsonFile.Load<Recipe>(ref recipe, "1.json");
            //JsonFile.Save(recipe, "json.json");
            //Console.WriteLine(Utils.FileCompare("1.json", "json.json"));

            //Console.Write("Check Bson......");
            //recipe._Items.Clear();
            //JsonFile.LoadBson<Recipe>(ref recipe, "1.bson");
            //JsonFile.Save(recipe, "bson.json");
            //Console.WriteLine(Utils.FileCompare("1.json", "bson.json"));

            //Console.Write("Check Compressed-Json......");
            //recipe._Items.Clear();
            //JsonFile.LoadCompressed<Recipe>(ref recipe, "1.cjson");
            //JsonFile.Save(recipe, "cjson.json");
            //Console.WriteLine(Utils.FileCompare("1.json", "cjson.json"));
            //Console.WriteLine();
            #endregion

            #region Test Recipe

            Console.WriteLine("Save Recipe Schema");
            recipe.SaveCsvSchema("recipe-schema.csv");
            recipe.Save("1.rcp");
            Console.WriteLine();

            Console.WriteLine("Load/Save Recipe File");
            Console.Write(recipe["t_MaxStage"].Value); Console.Write(" >>> ");
            recipe["t_MaxStage"].Value = 333.33333;
            Console.Write(recipe["t_MaxStage"].Value); Console.Write(" >>> ");
            recipe.Load("1.rcp");
            Console.Write(recipe["t_MaxStage"].Value);
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Backup/Restore Recipe File");
            recipe.Backup();
            Console.Write(recipe["t_MaxStage"].Value); Console.Write(" >>> ");
            recipe["t_MaxStage"].Value = 333.33333;
            Console.Write(recipe["t_MaxStage"].Value); Console.Write(" >>> ");
            recipe.Restore();
            Console.Write(recipe["t_MaxStage"].Value);
            Console.WriteLine();
            Console.WriteLine();
            #endregion

            #region Test alarms

            Console.Write("=====================================\n");
            Console.Write("Check Alarms......");
            Alarms alarms = new Alarms();
            alarms.SaveToCSV("alarms.csv");
            Console.Write(alarms[33].Text); Console.Write(" >>> ");
            alarms[33].Text = "Changed";
            Console.Write(alarms[33].Text); Console.Write(" >>> ");
            alarms.LoadFromCSV("alarms.csv");
            Console.Write(alarms[33].Text);
            Console.WriteLine();
            Console.WriteLine();
            #endregion

            #region Test DIOs

            Console.Write("=====================================\n");
            Console.Write("Check DIOList......");
            DIOList io = new DIOList(4, 16);
            io.SaveCsvSchema("io.csv");
            io.LoadCsvSchema("io.csv");
            io.SaveCsvSchema("io-1.csv");
            Console.WriteLine(Utils.FileCompare("io.csv", "io-1.csv"));

            #endregion

            #region Test Motors

            Console.Write("=====================================\n");
            Console.Write("Check DIOList......");
            MotorList motors = new MotorList(32);
            motors[1].Name = "01 Loader";
            motors.SaveToCsv("motors.csv");
            motors.LoadFromCsv("motors.csv");
//            Console.WriteLine(Utils.FileCompare("io.csv", "io-1.csv"));

            #endregion
        }
    }
}
