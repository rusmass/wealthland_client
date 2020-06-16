using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Coding<T>
{
    /// <summary>
    /// litJson的类，把数据转换成json
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static string encode(T model){
		return JsonMapper.ToJson(model);


	}
		
//	public static T decode(string message){
//		return JsonMapper.ToObject<T>(message);
//	}
}


