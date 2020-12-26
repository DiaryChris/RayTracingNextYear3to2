namespace EventSys
{
    /// <summary>
    /// 定义委托
    /// 类比Action类
    /// 比Action有更丰富的委托类型
    /// </summary>
     
    public delegate void CallBack();
    public delegate void CallBack<T>(T arg);
    public delegate void CallBack<T, X>(T arg1, X arg2);
    public delegate void CallBack<T, X, Y>(T arg1, X arg2, Y arg3);
    public delegate void CallBack<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);
    public delegate void CallBack<T, X, Y, Z, W>(T arg1, X arg2, Y arg3, Z arg4, W arg5);
}