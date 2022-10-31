namespace Android.Data
{
    public struct BleCharacteristicData
    {
        public string deviceAddr;
        public string serviceUuid;
        public string characteristicUuid;
        public int intData;
        public string stringData;
        public bool isNotify;
        public int property;
        public bool hasData;

        /*
        public BleCharacteristicData(string addr,string service, string ch,sbyte[] sbytes,bool isNot, int properties)
        {
            this.deviceAddr = addr;
            this.serviceUuid = service;
            this.characteristic = ch;
            this.isNotify = isNot;
            this.length = sbytes.Length;
            this.data = new byte[sbytes.Length];
            this.hasData = true;
            for( int i = 0; i < length; ++i)
            {
                data[i] = unchecked((byte)sbytes[i]);
            }
            this.property = properties;
        }
        */

        public BleCharacteristicData(string addr, string service, string charactericticUuid, int intData, string stringData, bool isNot, int properties, bool hasData)
        {

            this.deviceAddr = addr;
            this.serviceUuid = service;
            this.characteristicUuid = charactericticUuid;
            this.isNotify = isNot;
            this.intData = intData;
            this.stringData = stringData;
            this.hasData = hasData;
            this.property = properties;
        }
    }
}