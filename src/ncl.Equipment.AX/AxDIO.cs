using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ncl.Equipment.Ax
{

    
    public class AxDIO : DIOList
    {
        private const int SUCCESS = 0;

        #region constructor

        public AxDIO(int modCount = 2, int subCount = 16, int bitCount = 16)
            : base(modCount, subCount, bitCount)
        {
        }
        #endregion

        public bool Init(string schemaFile)
        {
            uint upStatus = 0;
            if (CAXD.AxdInfoIsDIOModule(ref upStatus) != SUCCESS) 
                return false;

            if (upStatus != (uint)AXT_EXISTENCE.STATUS_EXIST) 
                return false;

            int modCount = 0;
            if (CAXD.AxdInfoGetModuleCount(ref modCount) != SUCCESS)
                return false;

            if (File.Exists(schemaFile))
                LoadCsvSchema(schemaFile);
            else
            {
                if (MsgBox.Query(schemaFile + " file not found!\n" + "created base schema file?"))
                    SaveCsvSchema(schemaFile);
                
                return false;
            }

            return true;
        }

        public bool CheckModules()
        {
            int modCount = 0;
            if (CAXD.AxdInfoGetModuleCount(ref modCount) != SUCCESS)
                return false;

            int boardNo = -1;
            int modPos = -1;
            uint modID = 0;

            int inCount = 0;
            int outCount = 0;
            for (int mod = 0; mod < modCount; mod++)
            {
                CAXD.AxdInfoGetModule(mod, ref boardNo, ref modPos, ref modID);

                switch ((AXT_MODULE)modID)
                {
                    case AXT_MODULE.AXT_SIO_RDI32:
                    case AXT_MODULE.AXT_SIO_DI32:
                        inCount++;
                        break;
                    case AXT_MODULE.AXT_SIO_DO32P:
                    case AXT_MODULE.AXT_SIO_RDO32:
                        outCount++;
                        break;
                }
            }

            MsgBox.Show(string.Format("DIO Moodule Count IN:{0} OUT:{1}", inCount, outCount));

            if (ModCount != modCount)
            {
                MsgBox.Error(string.Format("Module Count mismatch {0} -> {1}", ModCount, modCount));
                return false;
            }

            return true;
        }

        public void ReadAll(bool readOutput = false)
        {
            for (int mod = 0; mod < ModCount; mod++)
                for (int sub = 0; sub < SubCount; sub++)
                {
                    switch (BitCount)
                    {
                        case 16: 
                            switch (SchemaArray[mod, sub])
                            {
                                case DIOSubType.Input:
                                    CAXD.AxdiReadInportWord(mod, 0, ref DataArray[mod, sub]); 
                                    break;
                                case DIOSubType.Output:
                                    if (readOutput) CAXD.AxdoReadOutportWord(mod, 0, ref DataArray[mod, sub]); 
                                    break;
                            }
                            break;
                        case 32:
                            switch (SchemaArray[mod, sub])
                            {
                                case DIOSubType.Input:
                                    CAXD.AxdiReadInportDword(mod, 0, ref DataArray[mod, sub]); 
                                    break;
                                case DIOSubType.Output:
                                    if (readOutput) CAXD.AxdoReadOutportDword(mod, 0, ref DataArray[mod, sub]); 
                                    break;
                            }
                            break;
                        default:
                            MsgBox.Error("BitCount:" + BitCount.ToString() + " is not supported yet!");
                            return;
                    }
                }

            DataArrayToBits(readOutput);
        }

        public void WriteAll()
        {
            BitsToDataArray();

            for (int mod = 0; mod < ModCount; mod++)
                for (int sub = 0; sub < SubCount; sub++)
                {
                    if (SchemaArray[mod, sub] == DIOSubType.Output)
                        switch (BitCount)
                        {
                            case 16: 
                                CAXD.AxdoWriteOutportWord(mod, 0, DataArray[mod, sub]); 
                                break;
                            case 32: 
                                CAXD.AxdoWriteOutportDword(mod, 0, DataArray[mod, sub]); 
                                break;
                            default:
                                MsgBox.Error("BitCount:" + BitCount.ToString() + " is not supported yet!");
                                return;
                        }
                }
        }
    }
}
