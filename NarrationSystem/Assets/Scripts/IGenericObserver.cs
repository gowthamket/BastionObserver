using System;

public interface IGenericObserver <T> where T : Enum
{
   public void OnNotify(T action);
}
