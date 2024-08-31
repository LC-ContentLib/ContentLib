namespace ContentLib.Core.Utils;

public interface IFactory<out T>
{
    T Create();
}