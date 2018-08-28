using System;

namespace Bios.Communication.ConnectionManager
{
    public interface IDataParser : IDisposable, ICloneable
    {
        void handlePacketData(byte[] packet);
    }
}