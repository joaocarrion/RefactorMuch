using System.Security.Cryptography;

namespace RefactorMuch.Parse
{
  public class CRC64 : HashAlgorithm
  {
    public override void Initialize()
    {
      
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
      
    }

    protected override byte[] HashFinal()
    {
      throw new System.NotImplementedException();
    }
  }
}
