using System;
using ncl;
using ncl.Equipment;
using System.IO;

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
            #endregion Test JsonFile

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
            #endregion Test Recipe

            #region Test alarms

            Console.Write("=====================================\n");
            Console.Write("Check Alarms......");
            AlarmList alarms = new AlarmList();
            alarms.SaveToCSV("alarms.csv");
            Console.Write(alarms[33].Text); Console.Write(" >>> ");
            alarms[33].Text = "Changed";
            Console.Write(alarms[33].Text); Console.Write(" >>> ");
            alarms.LoadFromCSV("alarms.csv");
            Console.Write(alarms[33].Text);
            Console.WriteLine();
            Console.WriteLine();
            #endregion Test alarms

            #region Test DIOs

            Console.Write("=====================================\n");
            Console.Write("Testing DIOList......");
            DIOList io = new DIOList(4, 16);

            if (!File.Exists("io.csv"))
                io.SaveCsvSchema("io.csv");

            io.LoadCsvSchema("io.csv");
            io.SaveCsvSchema("io-1.csv");
            Console.WriteLine(Utils.FileCompare("io.csv", "io-1.csv"));

            {
                Console.WriteLine("Testing IO.DataArray <-> IO.Bits......");
                Random r = new Random();

                for (int i = 0; i < 100000; i++)
                {
                    int mod = r.Next(io.ModCount);
                    int sub = r.Next(io.SubCount);
                    int bit = r.Next(io.BitCount);
                    int index = mod * io.SubCount * io.BitCount + sub * io.BitCount + bit;

                    io.DataArray[mod, sub] = (uint)r.Next(UInt16.MaxValue);

                    bool b1 = Utils.GetBit32(io.DataArray[mod, sub], bit);

                    io.DataArrayToBits(true);
                    bool b2 = io.Bits[index];

                    if (io.SchemaArray[mod, sub] != DIOSubType.None)
                    {
                        if (b1 != b2)
                        {
                            Console.WriteLine("DataArrayToBits " + b1.ToString() + " != " + b2.ToString());
                        }
                    }
                    if (io.SchemaArray[mod, sub] == DIOSubType.Output)
                    {
                        io.Bits[index] = Convert.ToBoolean(r.Next(2));
                        bool b3 = io.Bits[index];

                        io.BitsToDataArray();
                        bool b4 = Utils.GetBit32(io.DataArray[mod, sub], bit);

                        if (b3 != b4)
                        {
                            Console.WriteLine("BitsToDataArray " + b3.ToString() + " != " + b4.ToString());
                        }
                    }
                }
                Console.WriteLine("Finished");
            }
            Console.WriteLine();
            #endregion Test DIOs

            #region Test Motors

            Console.Write("=====================================\n");
            Console.Write("Check MotorList......");
            MotorList motors = new MotorList(32);
            motors[1].Name = "01 Loader";
            motors.SaveCsvSchema("motors.csv");
            motors.LoadCsvSchema("motors.csv");
            //            Console.WriteLine(Utils.FileCompare("io.csv", "io-1.csv"));

            #endregion Test Motors
        }
    }
}